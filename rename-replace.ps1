#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/.modules/Nautilus/Nautilus.psm1"

$match = Read-Host -Prompt "Match pattern"
$replacement = Read-Host -Prompt "Replacement string"

Get-NautilusSelectedFiles | ForEach-Object -Process {
    Rename-Item $_ -NewName "$($_.BaseName -Replace $match, $replacement)$($_.Extension)"
}