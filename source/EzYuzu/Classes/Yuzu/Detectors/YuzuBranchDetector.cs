using EzYuzu.Classes.Entities;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EzYuzu.Classes.Yuzu.Detectors
{
    public sealed class EdenBranchDetector
    {
        private readonly IHttpClientFactory clientFactory;

        public EdenBranchDetector(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public string EdenDirectoryPath { get; set; } = "";

        public async Task<List<KeyValuePair<string, string>>> GetAvailableUpdateVersionsAsync()
        {
            var result = await GetAvailableUpdateVersionsWithChangelogAsync();
            return result.Select(x => new KeyValuePair<string, string>(x.tagName, x.url)).ToList();
        }

        public async Task<List<(string tagName, string url, string changelog)>> GetAvailableUpdateVersionsWithChangelogAsync()
        {
            try
            {
                using var client = clientFactory.CreateClient("Gitea-Api");
                client.Timeout = TimeSpan.FromSeconds(20);
                using var response = await client.GetAsync("repos/eden-ci/nightly/releases");
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();
                var repoData = await JsonSerializer.DeserializeAsync<IEnumerable<Repo>>(stream)!;

                var results = new List<(string tagName, string url, string changelog)>();

                foreach (var repo in repoData!)
                {
                    if (string.IsNullOrEmpty(repo.TagName))
                        continue;

                    string? downloadUrl = null;

                    if (repo.Assets is not null && repo.Assets.Count > 0)
                    {
                        var asset = repo.Assets.FirstOrDefault(a =>
                            a.BrowserDownloadUrl!.EndsWith("-amd64-clang-pgo.zip", StringComparison.Ordinal));
                        if (asset is not null)
                            downloadUrl = asset.BrowserDownloadUrl;
                    }

                    if (downloadUrl is null && !string.IsNullOrEmpty(repo.Body))
                    {
                        downloadUrl = ExtractDownloadUrlFromMarkdown(repo.Body);
                    }

                    if (downloadUrl is not null)
                    {
                        string changelog = ExtractChangelogFromMarkdown(repo.Body);
                        results.Add((repo.TagName, downloadUrl, changelog));
                    }
                }

                return results;
            }
            catch
            {
                return new List<(string, string, string)>();
            }
        }

        private static string ExtractChangelogFromMarkdown(string? body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return "";

            // Match content between "## Changelog" and "# Packages" (or end of string)
            var match = Regex.Match(body, @"## Changelog\s*\n(.*?)(?=#\s|\z)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string changelog = match.Groups[1].Value.Trim();
                return changelog;
            }

            return body.Trim();
        }

        private static string? ExtractDownloadUrlFromMarkdown(string body)
        {
            var matches = Regex.Matches(body, @"\((https?://[^)]*amd64-clang-pgo\.zip)\)");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value;
            }

            var plainMatches = Regex.Matches(body, @"(https?://[^\s\)]*amd64-clang-pgo\.zip)");
            if (plainMatches.Count > 0)
            {
                return plainMatches[0].Groups[1].Value;
            }

            return null;
        }
    }
}
