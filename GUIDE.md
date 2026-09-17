# EzEden Guide

## Requirements

- Latest [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.

## Methodology

- Reads [Eden Nightly](https://git.eden-emu.dev/eden-ci/nightly/releases) Gitea API JSON data
- Extracts the latest `-amd64-clang-pgo.zip` archive URL from the release body
- Downloads & extracts it into your Eden Root Folder

## Usage

1. Browse and locate your Eden Root Folder, this is the folder containing `eden.exe`
2. EzEden will automatically detect the version of `eden.exe`
3. (Optional): Change Update Version
4. Click on `New Install` or `Update Eden`

- Downloads the latest copy of Eden & extracts it into your Eden Root Folder.
- Automatically checks if your standalone copy of Eden is up-to-date.
- Shows changelog for the selected version.
- Launch Eden after update. Go to `Options` > `General` > `Update Eden` and check `Launch Eden After Update`
- It shouldn't overwrite configs unless `New Install` is displayed. However, backup beforehand.
- Temp files are stored within `TempUpdate` and are deleted upon completion.
- [GUIDE](https://github.com/fiztan/ez-eden/blob/master/GUIDE.md) for detailed instructions

## Download Options

| Option         | Description                                                             |
| -------------- | ----------------------------------------------------------------------- |
| `New Install`  | Installs Eden Nightly. Resets configs.                                  |
| `Update Eden`  | Updates Eden to the latest version.                                     |

## Graphical User Interface Options

### Safe Mode

To launch EzEden in Safe Mode and reset user preferences:

1. Ensure all instances of EzEden are closed.
2. Hold `Ctrl` down, then launch EzEden.
3. When done correctly, EzEden's title bar will display "EzEden - Eden Portable Updater - Safe Mode"

### New Install

To install Eden Portable for the first time:

1. Create an empty folder on your device and give it a name.
2. Select your newly created empty folder.
3. (Optional) Launch Eden after update. Go to `Options` > `General` > `Update Eden` and check `Launch Eden After Update`
4. The button should now change to `New Install`
5. Click on `New Install`
6. Done

### Update Eden

1. Select your Eden root folder (the folder containing `eden.exe`)
2. The button should now change to `Update Eden`
3. (Optional) Launch Eden after update. Go to `Options` > `General` > `Update Eden` and check `Launch Eden After Update`
4. Click on `Update Eden`
5. Done

### Checking Eden is up-to-date

1. Select your Eden root folder (the folder containing `eden.exe`)
2. EzEden will automatically check if the current copy of Eden is up-to-date
3. If Eden is up-to-date, the Update button will be disabled and will state `Eden is currently Up-To-Date!`
4. Done

## Command Line Interface Options

### Switches

```
-p, --path              Required. Set the Eden Location Directory Path, this is the path where eden.exe resides. Must wrap path in double quotes.
-v                      Set a specific version tag to update/rollback Eden to. Must wrap version tag in double quotes.
-l, --launch-eden       Launch Eden after successful New Install/Update

--help                  Displays the EzEden help screen
--version               Displays EzEden's version information
```

### Usage Examples

New Install/Update to latest Eden:

```
start "" /wait "EzEden.exe" -p "D:\Eden"
```

New Install/Update to latest Eden, then launch it:

```
start "" /wait "EzEden.exe" -p "D:\Eden" -l
```

Update/rollback to specific version of Eden (e.g. v1789157782.8a22f1845b):

```
start "" /wait "EzEden.exe" -p "D:\Eden" -v "v1789157782.8a22f1845b"
```

Launch Eden after EzEden has completed an update:

```
start "" /wait "EzEden.exe" -p "D:\Eden" -l
```
