#!/usr/bin/pwshx -t

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Images
Import-Module -Name Pwshx

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension @(".jpg", ".jpeg") `
    | Backup-File -BackupDir "~backup" | Resize-Image

Wait-IfTerminal
