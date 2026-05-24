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
$processName = [System.IO.Path]::GetFileNameWithoutExtension($ExeName)

if ($null -ne $task) {
    Stop-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
}

Stop-Process -Name $processName -Force -ErrorAction SilentlyContinue

$deadline = (Get-Date).AddSeconds(30)
while ((Get-Process -Name $processName -ErrorAction SilentlyContinue) -and (Get-Date) -lt $deadline) {
    Start-Sleep -Seconds 1
}

if (Get-Process -Name $processName -ErrorAction SilentlyContinue) {
    throw "The process $processName is still running and is locking files in $DeployPath."
}

function Remove-DeploymentContents {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    $items = Get-ChildItem -Path $Path -Force -ErrorAction SilentlyContinue
    foreach ($item in $items) {
        Remove-Item -Path $item.FullName -Recurse -Force -ErrorAction Stop
    }
}

for ($attempt = 1; $attempt -le 5; $attempt++) {
    try {
        # Clear the deployment folder so old files do not survive a new release.
        Remove-DeploymentContents -Path $DeployPath
        break
    } catch {
        if ($attempt -eq 5) {
            throw
        }

        Start-Sleep -Seconds 2
    }
}

Copy-Item -Path (Join-Path $PublishPath "*") -Destination $DeployPath -Recurse -Force

$exePath = Join-Path $DeployPath $ExeName
if (-not (Test-Path $exePath)) {
    throw "Expected executable was not found after deployment: $exePath"
}

if ($null -ne $task) {
    Start-ScheduledTask -TaskName $TaskName
}
