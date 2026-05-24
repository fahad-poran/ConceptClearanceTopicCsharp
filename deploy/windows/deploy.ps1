param(
    [Parameter(Mandatory = $true)]
    [string]$PublishPath,

    [Parameter(Mandatory = $true)]
    [string]$DeployPath,

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

if (-not (Test-Path $DeployPath)) {
    New-Item -ItemType Directory -Path $DeployPath -Force | Out-Null
}

$task = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
if ($null -ne $task) {
    $processName = [System.IO.Path]::GetFileNameWithoutExtension($ExeName)
    Stop-Process -Name $processName -Force -ErrorAction SilentlyContinue
}

# Clear the deployment folder so old files do not survive a new release.
Get-ChildItem -Path $DeployPath -Force | Remove-Item -Recurse -Force

Copy-Item -Path (Join-Path $PublishPath "*") -Destination $DeployPath -Recurse -Force

$exePath = Join-Path $DeployPath $ExeName
if (-not (Test-Path $exePath)) {
    throw "Expected executable was not found after deployment: $exePath"
}

if ($null -ne $task) {
    Start-ScheduledTask -TaskName $TaskName
}
