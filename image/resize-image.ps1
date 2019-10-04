#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"
Import-Module "$PSScriptRoot/../.modules/Images/Images.psm1"
Import-Module "$PSScriptRoot/../.modules/Pwshx/Pwshx.psm1"

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension @(".jpg", ".jpeg") `
    | Backup-File -BackupDir "~backup" | Resize-Image

Wait-IfTerminal
