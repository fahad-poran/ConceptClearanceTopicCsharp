param(
    [Parameter(Mandatory = $true)]
    [string]$AppRoot,

    [Parameter(Mandatory = $true)]
    [string]$ExeName,

    [Parameter(Mandatory = $true)]
    [string]$AppUrl
)

$ErrorActionPreference = "Stop"

$activeReleaseFile = Join-Path $AppRoot "active-release.txt"
if (-not (Test-Path $activeReleaseFile)) {
    throw "Active release file not found: $activeReleaseFile"
}

$releaseName = (Get-Content $activeReleaseFile -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($releaseName)) {
    throw "Active release file is empty: $activeReleaseFile"
}

$releasePath = Join-Path (Join-Path $AppRoot "releases") $releaseName
$exePath = Join-Path $releasePath $ExeName
if (-not (Test-Path $exePath)) {
    throw "Executable not found for active release: $exePath"
}

Push-Location $releasePath
try {
    $process = Start-Process -FilePath $exePath -ArgumentList "--urls `"$AppUrl`"" -WorkingDirectory $releasePath -PassThru
    Wait-Process -Id $process.Id
} finally {
    Pop-Location
}
