# Tasty Adrenaline Development

## Requirements

- .NET SDK capable of building SDK-style .NET Framework projects
- Valheim with BepInEx and Jotunn installed for testing

The project restores JotunnLib from NuGet. It does not include Valheim, BepInEx, Jotunn runtime files, or Unity assets.

## Build

```sh
dotnet restore
dotnet build -c Debug
```

The Debug DLL is created at `bin/Debug/net48/TastyAdrenaline.dll`.

On Unix systems, Debug builds deploy the DLL and PDB to the first available location:

1. `MOD_DEPLOYPATH`
2. `$BEPINEX_PATH/plugins`
3. `~/Library/Application Support/Steam/steamapps/common/Valheim/BepInEx/plugins`

## Release Package

On Windows, run the packaging script from PowerShell:

```powershell
.\scripts\package-release.ps1
```

If PowerShell blocks local scripts, run it for the current terminal session with:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
.\scripts\package-release.ps1
```

The Release package contains:

```text
Package/
├── manifest.json
├── README.md
├── icon.png
└── plugins/
    └── TastyAdrenaline.dll
```

The script creates `TastyAdrenaline.zip` in the repository root with the contents of `Package` at the archive root, ready to upload to Thunderstore. On Unix systems, the existing Release build also creates the archive when the `zip` command is available.

This project has no custom Unity asset bundles. Add a Unity project only when the mod needs custom prefabs, textures, or other assets.