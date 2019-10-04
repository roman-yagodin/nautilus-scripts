#!/usr/bin/pwshx -t

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pwshx

$selectedFiles = Get-NautilusSelectedFiles
$filesCount = ($selectedFiles | Measure-Object).Count
$numOfDigits = [System.Math]::Floor([System.Math]::Log($filesCount,10) + 1)
$selectedFiles | Rename-FileSequentally -NumOfDigits $numOfDigits

Wait-IfTerminal
