param(
    [Parameter(Mandatory = $true)]
    [string]$TaskName,

    [Parameter(Mandatory = $true)]
    [string]$DisplayName,

    [Parameter(Mandatory = $true)]
    [string]$AppRoot,

    [Parameter(Mandatory = $true)]
    [string]$ExeName,

    [Parameter(Mandatory = $true)]
    [string]$AppUrl
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $AppRoot)) {
    New-Item -ItemType Directory -Path $AppRoot -Force | Out-Null
}

$existing = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
if ($null -ne $existing) {
    Stop-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
    Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
}

$launcherSource = Join-Path $PSScriptRoot "run-app.ps1"
if (-not (Test-Path $launcherSource)) {
    throw "Launcher script not found in repo: $launcherSource"
}

$launcher = Join-Path $AppRoot "run-app.ps1"
Copy-Item -Path $launcherSource -Destination $launcher -Force

$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$launcher`" -AppRoot `"$AppRoot`" -ExeName `"$ExeName`" -AppUrl `"$AppUrl`""
$trigger = New-ScheduledTaskTrigger -AtStartup
$principal = New-ScheduledTaskPrincipal -UserId "SYSTEM" -LogonType ServiceAccount -RunLevel Highest
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -AllowStartIfOnBatteries

Register-ScheduledTask `
    -TaskName $TaskName `
    -Action $action `
    -Trigger $trigger `
    -Principal $principal `
    -Settings $settings `
    -Description $DisplayName | Out-Null

Start-ScheduledTask -TaskName $TaskName
