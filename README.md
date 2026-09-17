# EzEden

A Portable Eden Updater for Standalone versions of Eden Nightly builds.

Perfect for those who run Eden off an External HDD or through (but not limited to) frontends such as LaunchBox, Steam, EmulationStation and HyperSpin.

## Based on

This project is a port of [EzYuzu](https://github.com/amakvana/EzYuzu) by [amakvana](https://github.com/amakvana), adapted to work with the [Eden](https://eden-emu.dev/) emulator and its Gitea-based nightly build system. All credit for the original architecture, UI design, and codebase goes to the EzYuzu author.

## Table of Contents

- [Overview](#overview)
  - [Methodology](#methodology)
  - [Basic Usage](#basic-usage)
- [Downloads](#downloads)
- [Installation](#installation)
- [User Guide](#user-guide)
- [Acknowledgements](#acknowledgements)

## Overview

### Methodology

- Reads [Eden Nightly](https://git.eden-emu.dev/eden-ci/nightly/releases) Gitea API JSON data
- Extracts the latest `-amd64-clang-pgo.zip` archive URL from the release body
- Downloads & extracts it into your Eden Root Folder

### Basic Usage

1. Browse and locate your Eden Root Folder, this is the folder containing `eden.exe`
2. EzEden will automatically detect the version of `eden.exe`
3. (Optional): Change Update Version
4. Click on `New Install` or `Update Eden`

- Downloads the latest copy of Eden & extracts it into your Eden Root Folder.
- Automatically checks if your standalone copy of Eden is up-to-date.
- Shows changelog for the selected version.
- Post-Update options can be found under `Options` > `General` > `Update Eden`
- Safe Mode can be launched via holding `Ctrl` then launching EzEden.exe
- Temp files are stored within `TempUpdate` and are deleted upon completion.

## Downloads

https://github.com/fiztan/ez-eden/releases/latest

Requires:

- Latest [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.

## Installation

1. Download the latest `EzEden.zip` from [Releases](https://github.com/fiztan/ez-eden/releases/latest)
2. Extract the entire contents into a folder and run `EzEden.exe`

EzEden is 100% portable - it can be run from any location.

EzEden does not require Administrator privileges to update Eden.

## User Guide

1. Click `...` and browse to the folder containing `eden.exe`
2. EzEden will detect your installed version and available updates
3. Select the version you want to install from the dropdown
4. Click the main button to download and install

## Acknowledgements

Thanks:

- [amakvana](https://github.com/amakvana) - Author of [EzYuzu](https://github.com/amakvana/EzYuzu), the original project this was ported from
- [Eden Team](https://eden-emu.dev/) - Nintendo Switch Emulator Developers
- [Agus Raharjo](https://www.iconfinder.com/agusraharj) - Icons

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
