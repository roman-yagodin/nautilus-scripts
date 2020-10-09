#!/usr/bin/pwsh

[Environment]::SetEnvironmentVariable("PSModulePath", $Env:PSModulePath + [System.IO.Path]::PathSeparator + "$PSScriptRoot/../.modules")

Import-Module -Name Nautilus
Import-Module -Name Files
Import-Module -Name Pwshx

$selectedFiles = Get-NautilusSelectedFiles
if (($selectedFiles | Measure-Object).Count -lt 2) {
    Write-Error -Message "You must select at least 2 files to compare!"
    Wait-IfTerminal
    exit 1
}

$file1 = $selectedFiles | Select-Object -Index 0
$file2 = $selectedFiles | Select-Object -Index 1

diff @("--color", "-w", "-U", "3", """$file1""", """$file2""") | Out-File "$file1.patch"

