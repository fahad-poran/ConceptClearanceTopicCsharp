param(
    [Parameter(Mandatory = $false)]
    [int]$Port = 5157,

    [Parameter(Mandatory = $false)]
    [string]$RuleName = "InterviewPrepWeb LAN"
)

$ErrorActionPreference = "Stop"

$existing = Get-NetFirewallRule -DisplayName $RuleName -ErrorAction SilentlyContinue
if ($null -ne $existing) {
    Set-NetFirewallRule -DisplayName $RuleName -Profile Any
    Write-Host "Firewall rule already exists and was updated: $RuleName"
    exit 0
}

New-NetFirewallRule `
    -DisplayName $RuleName `
    -Direction Inbound `
    -Action Allow `
    -Protocol TCP `
    -LocalPort $Port `
    -Profile Any
