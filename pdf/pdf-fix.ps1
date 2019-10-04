#!/usr/bin/pwsh

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pdf

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension ".pdf" | Invoke-PdfFix
