#!/usr/bin/pwshx -t

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pwshx

$selectedFiles = Get-NautilusSelectedFiles
$file1 = $selectedFiles | Select-Object -Index 0
$file2 = $selectedFiles | Select-Object -Index 1

diff @("--color", "-w", "-U", "3", """$file1""", """$file2""")

Wait-IfTerminal



