param(
    [Parameter(Mandatory = $false)]
    [int]$Port = 5157,

    [Parameter(Mandatory = $false)]
    [string]$RuleName = "InterviewPrepWeb LAN"
)

$ErrorActionPreference = "Stop"

$existing = Get-NetFirewallRule -DisplayName $RuleName -ErrorAction SilentlyContinue
if ($null -ne $existing) {
    Write-Host "Firewall rule already exists: $RuleName"
    exit 0
}

New-NetFirewallRule `
    -DisplayName $RuleName `
    -Direction Inbound `
    -Action Allow `
    -Protocol TCP `
    -LocalPort $Port `
    -Profile Private
