# Tasty Adrenaline

Tasty Adrenaline adds a small adrenaline gain while Tasty Mead is active. It preserves the mead's original consume status effect.

## Requirements

- .NET SDK capable of building SDK-style .NET Framework projects
- Valheim with BepInEx and Jotunn installed

The project restores JotunnLib from NuGet. It does not include Valheim, BepInEx, Jotunn runtime files, or Unity assets.

## Build

```sh
dotnet restore
dotnet build -c Debug
```

Debug builds deploy the DLL and PDB to the first available location:

1. `MOD_DEPLOYPATH`
2. `$BEPINEX_PATH/plugins`
3. `~/Library/Application Support/Steam/steamapps/common/Valheim/BepInEx/plugins`

Before BepInEx is installed, the Debug build succeeds and reports that deployment was skipped.

Create a distributable package with:

```sh
dotnet build -c Release
```

The release build stages `Package/plugins/TastyAdrenaline.dll` and `README.md`, then creates `TastyAdrenaline.zip` when the `zip` command is available.

## Development

This initial project has no custom Unity asset bundles. Add a Unity project only when the mod needs custom prefabs, textures, or other assets.
