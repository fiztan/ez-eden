# EzYuzu to EzEden Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Adapt EzYuzu (Yuzu updater) to become EzEden (Eden emulator updater) targeting Windows amd64-clang-pgo nightly builds from `git.eden-emu.dev`.

**Architecture:** Port the existing C# WinForms application, replacing GitHub API calls with Gitea API calls, switching from 7-Zip extraction to native `System.IO.Compression.ZipFile`, removing the Mainline/EarlyAccess channel system (Eden only has nightly), and updating all UI strings.

**Tech Stack:** .NET 7.0 Windows Forms, System.Text.Json, System.IO.Compression, Microsoft.Extensions.DependencyInjection, CommandLineParser

## Global Constraints

- Target build: `Eden-Windows-{commit}-amd64-clang-pgo.zip`
- API: Gitea at `https://git.eden-emu.dev/api/v1/repos/eden-ci/nightly/releases`
- Eden tags format: `v{unix_timestamp}.{commit_hash}` (e.g., `v1789157782.8a22f1845b`)
- Version stored as string (not int) since tags contain hashes
- No 7-Zip dependency - use `ZipFile.ExtractToDirectory` or `ZipFile.OpenRead`
- No subdirectory extraction (Eden zips extract flat)

---

### Task 1: Update Program.cs - API URLs and DI

**Files:**
- Modify: `source/EzYuzu/Program.cs`

**Changes:**
- Rename namespace `EzYuzu` → keep as-is (internal only)
- Change `"GitHub-Api"` client to `"Gitea-Api"` with base address `https://git.eden-emu.dev/api/v1/`
- Change headers to `accept: application/json`, `user-agent: EzEden`
- Remove `"GitHub-EzYuzu"` client (no longer needed for self-update or 7zip)
- Update CLI help text references from EzYuzu to EzEden
- Remove branch detection from CLI args (no Mainline/EarlyAccess)

---

### Task 2: Update GitHubRepo.cs - Gitea DTO

**Files:**
- Modify: `source/EzYuzu/Classes/Entities/GitHubRepo.cs`

**Changes:**
- Gitea API response is very similar to GitHub API. Keep `Repo`, `Asset` classes
- Remove `Reactions`, `Uploader` classes (not needed, Gitea doesn't have them in same format)
- Simplify `Author` class
- Keep `tag_name`, `assets`, `browser_download_url` fields

---

### Task 3: Rewrite YuzuBranchDetector → EdenBranchDetector

**Files:**
- Modify: `source/EzYuzu/Classes/Yuzu/Detectors/YuzuBranchDetector.cs`

**Changes:**
- Rename class to `EdenBranchDetector`
- Remove `YuzuBranch` enum (only Nightly)
- Remove `GetCurrentlyInstalledBranchAsync()` (no branch detection needed)
- Change `GetDetectedBranchAvailableUpdateVersionsAsync()` to:
  - Hit `repos/eden-ci/nightly/releases` via Gitea API
  - Filter assets ending with `-amd64-clang-pgo.zip`
  - Return `List<KeyValuePair<string, string>>` of tagName → downloadUrl

---

### Task 4: Update YuzuInstallationStateDetector → EdenInstallationStateDetector

**Files:**
- Modify: `source/EzYuzu/Classes/Yuzu/Detectors/YuzuInstallationStateDetector.cs`

**Changes:**
- Rename class to `EdenInstallationStateDetector`
- Change version comparison from `int` to `string` (Eden tags are `v{timestamp}.{hash}`)
- Read version file as string, compare with tag_name
- Check for `eden.exe` instead of `yuzu.exe`
- Remove `YuzuBranch` dependency

---

### Task 5: Rewrite YuzuManager → EdenManager (base)

**Files:**
- Modify: `source/EzYuzu/Classes/Yuzu/Managers/YuzuManager.cs`

**Changes:**
- Rename to `EdenManager`
- Remove `SevenZipExeFilePath` property
- Remove `IsSevenZipInstalled()`, `DownloadSevenZipAsync()` methods
- Remove `DownloadInstallVisualCppRedistAsync()` method
- Remove `UpdateVisualCppRedistAsync` property
- Update `CloseYuzu()` → `CloseEden()` - kill `eden` processes
- Update `CleanUpDirectories()` - remove `yuzu-windows-msvc*` pattern, just delete TempUpdate
- Update `ProcessYuzuNewInstallationAsync()` → `ProcessEdenNewInstallationAsync()` - remove yuzu config dirs
- Update `GetGPUConfigAsync()` - keep GPU detection but remove "GitHub-EzYuzu" client, use direct URL or local configs
- Keep `CopyFiles()` and `PrepareTempUpdateFolder()` as-is

---

### Task 6: Rewrite MainlineYuzuManager → EdenManager (download+extract)

**Files:**
- Modify: `source/EzYuzu/Classes/Yuzu/Managers/MainlineYuzuManager.cs`

**Changes:**
- Rename to `EdenNightlyManager` (or merge into EdenManager)
- Download zip file from `downloadUrl`
- Extract using `ZipFile.ExtractToDirectory()` instead of 7-Zip
- No subdirectory copy needed (Eden zips are flat)
- Write version file with full tag name (not just number)
- Progress reporting for download + extraction

---

### Task 7: Delete EarlyAccessYuzuManager.cs

**Files:**
- Delete: `source/EzYuzu/Classes/Yuzu/Managers/EarlyAccessYuzuManager.cs`

**Changes:**
- Eden has no Early Access channel, this file is unnecessary

---

### Task 8: Update frmMain.cs - UI for Eden

**Files:**
- Modify: `source/EzYuzu/frmMain.cs`

**Changes:**
- Remove `cboUpdateChannel` dropdown and all Mainline/EarlyAccess logic
- Update all text strings: "Yuzu" → "Eden", "yuzu.exe" → "eden.exe"
- Remove HDR/cemu.exe renaming logic
- Update `LaunchYuzu()` → `LaunchEden()` - look for `eden.exe`
- Update folder browser description to "Browse to the folder containing eden.exe"
- Update settings file name to `EzEden.settings.json`
- Update installation state UI text
- Remove channel override menu items

---

### Task 9: Update CLI Options and CommandLineUpdater

**Files:**
- Modify: `source/EzYuzu/Classes/CLOptions/CommandLineInstallOptions.cs`
- Modify: `source/EzYuzu/Classes/CLOptions/YuzuCommandLineUpdater.cs`

**Changes:**
- Remove `-m/--mainline` and `-e/--early-access` options
- Remove `--enable-hdr` option
- Update help text references
- Simplify updater to always use nightly channel
- Update `ProcessYuzuDirectory` → `ProcessEdenDirectory`

---

### Task 10: Update Settings and remaining files

**Files:**
- Modify: `source/EzYuzu/Classes/Settings/EzYuzuSettings.cs`
- Modify: `source/EzYuzu/Classes/Updaters/AppUpdater.cs`

**Changes:**
- Rename settings properties (YuzuLocation → EdenLocation, etc.)
- Remove self-update check (or point to Eden releases)
- Update settings file serialization

---

### Task 11: Update frmAbout.cs and resources

**Files:**
- Modify: `source/EzYuzu/frmAbout.cs`
- Modify: `source/EzYuzu/Properties/AssemblyInfo.cs`

**Changes:**
- Update title, description, credits
- Point to Eden project instead of Yuzu
