$ErrorActionPreference = 'Stop'

$projectPath = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $projectPath 'TastyAdrenaline.sln'
$packagePath = Join-Path $projectPath 'Package'
$archivePath = Join-Path $projectPath 'TastyAdrenaline.zip'

Write-Host 'Building Release configuration...'
dotnet build $solutionPath -c Release

if (-not (Test-Path -LiteralPath $packagePath -PathType Container)) {
    throw "Package directory not found: $packagePath"
}

if (Test-Path -LiteralPath $archivePath) {
    Remove-Item -LiteralPath $archivePath -Force
}

Write-Host 'Creating package archive...'
Compress-Archive -Path (Join-Path $packagePath '*') -DestinationPath $archivePath -Force

Write-Host "Created $archivePath"
