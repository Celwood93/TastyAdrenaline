#!/bin/sh
set -eu

target="Debug"
target_path=""
target_assembly=""
project_path=""

while [ "$#" -gt 0 ]; do
    case "$1" in
        --target) target="$2"; shift 2 ;;
        --target-path) target_path="$2"; shift 2 ;;
        --target-assembly) target_assembly="$2"; shift 2 ;;
        --project-path) project_path="$2"; shift 2 ;;
        *) echo "Unknown argument: $1" >&2; exit 1 ;;
    esac
done

if [ -z "$target_path" ] || [ -z "$target_assembly" ] || [ -z "$project_path" ]; then
    echo "Missing required publish arguments." >&2
    exit 1
fi

plugin_name=${target_assembly%.dll}

if [ "$target" = "Debug" ]; then
    deploy_path=${MOD_DEPLOYPATH:-${BEPINEX_PATH:+$BEPINEX_PATH/plugins}}
    if [ -z "${deploy_path:-}" ]; then
        deploy_path="$HOME/Library/Application Support/Steam/steamapps/common/Valheim/BepInEx/plugins"
    fi

    if [ ! -d "$deploy_path" ]; then
        echo "Debug deployment skipped: BepInEx plugins directory not found at $deploy_path"
        exit 0
    fi

    plugin_path="$deploy_path/$plugin_name"
    mkdir -p "$plugin_path"
    cp "$target_path/$target_assembly" "$plugin_path/"
    [ -f "$target_path/$plugin_name.pdb" ] && cp "$target_path/$plugin_name.pdb" "$plugin_path/"
    echo "Published $target_assembly to $plugin_path"
    exit 0
fi

if [ "$target" = "Release" ]; then
    package_path="$project_path/Package"
    mkdir -p "$package_path/plugins"
    rm -f "$package_path/plugins/$target_assembly"
    cp "$target_path/$target_assembly" "$package_path/plugins/"

    if command -v zip >/dev/null 2>&1; then
        archive_path="$project_path/$plugin_name.zip"
        rm -f "$archive_path"
        (
            cd "$package_path"
            zip -qr "$archive_path" .
        )
        echo "Created release archive: $archive_path"
    else
        echo "Release package staged at $package_path; install zip to create an archive."
    fi
fi