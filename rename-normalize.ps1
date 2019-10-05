#!/usr/bin/pwsh

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/.modules")

Import-Module -Name Nautilus
Import-Module -Name Files

Get-NautilusSelectedFiles | Rename-FileNormalize
