#!/usr/bin/pwshx -t

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pdf

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension ".pdf" | Backup-File -BackupDir "~backup" | Compress-Pdf -Verbose

Wait-IfTerminal