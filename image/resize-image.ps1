#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"
Import-Module "$PSScriptRoot/../.modules/Images/Images.psm1"

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension @(".jpg", ".jpeg") `
    | Backup-File -BackupDir "~backup" | Resize-Image

if ($Env:PWSHX_IN_TERMINAL -eq "true") {
    Read-Host -Prompt "Press [Enter] to continue"
}