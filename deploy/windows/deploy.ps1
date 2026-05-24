param(
    [Parameter(Mandatory = $true)]
    [string]$PublishPath,

    [Parameter(Mandatory = $true)]
    [string]$AppRoot,

    [Parameter(Mandatory = $true)]
    [string]$TaskName,

    [Parameter(Mandatory = $true)]
    [string]$ExeName,

    [Parameter(Mandatory = $true)]
    [string]$AppUrl
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $PublishPath)) {
    throw "Publish path not found: $PublishPath"
}

if (-not (Test-Path $AppRoot)) {
    New-Item -ItemType Directory -Path $AppRoot -Force | Out-Null
}

$task = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
$processName = [System.IO.Path]::GetFileNameWithoutExtension($ExeName)
$releasesRoot = Join-Path $AppRoot "releases"
$releaseName = "$($env:GITHUB_RUN_ID)-$($env:GITHUB_RUN_ATTEMPT)"
if ($releaseName -eq "-") {
    $releaseName = (Get-Date).ToString("yyyyMMddHHmmss")
}
$releasePath = Join-Path $releasesRoot $releaseName
$activeReleaseFile = Join-Path $AppRoot "active-release.txt"

if ($null -ne $task) {
    Stop-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
}

Stop-Process -Name $processName -Force -ErrorAction SilentlyContinue

$deadline = (Get-Date).AddSeconds(30)
while ((Get-Process -Name $processName -ErrorAction SilentlyContinue) -and (Get-Date) -lt $deadline) {
    Start-Sleep -Seconds 1
}

if (Get-Process -Name $processName -ErrorAction SilentlyContinue) {
    throw "The process $processName is still running and is locking files in $AppRoot."
}

if (-not (Test-Path $releasesRoot)) {
    New-Item -ItemType Directory -Path $releasesRoot -Force | Out-Null
}

if (Test-Path $releasePath) {
    Remove-Item -Path $releasePath -Recurse -Force
}

New-Item -ItemType Directory -Path $releasePath -Force | Out-Null

Copy-Item -Path (Join-Path $PublishPath "*") -Destination $releasePath -Recurse -Force

$exePath = Join-Path $releasePath $ExeName
if (-not (Test-Path $exePath)) {
    throw "Expected executable was not found after deployment: $exePath"
}

Set-Content -Path $activeReleaseFile -Value $releaseName -NoNewline

if ($null -ne $task) {
    Start-ScheduledTask -TaskName $TaskName
}

function Wait-ForPort {
    param(
        [Parameter(Mandatory = $true)]
        [int]$Port,

        [Parameter(Mandatory = $false)]
        [int]$TimeoutSeconds = 60
    )

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    do {
        $listener = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
        if ($null -ne $listener) {
            return $true
        }

        Start-Sleep -Seconds 1
    } while ((Get-Date) -lt $deadline)

    return $false
}

if (-not (Wait-ForPort -Port 5157 -TimeoutSeconds 60)) {
    throw "The app did not start listening on port 5157 after deployment."
}
