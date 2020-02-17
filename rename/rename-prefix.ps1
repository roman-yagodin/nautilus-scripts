#!/usr/bin/pwshx -t

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pwshx

Get-NautilusSelectedFiles | Rename-FilePrefix

Wait-IfTerminal
