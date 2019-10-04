#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/.modules/Files/Files.psm1"

$selectedFiles = Get-NautilusSelectedFiles
$filesCount = ($selectedFiles | Measure-Object).Count
$numOfDigits = [System.Math]::Floor([System.Math]::Log($filesCount,10) + 1)
$selectedFiles | Rename-FileSequentally -NumOfDigits $numOfDigits

if ($Env:PWSHX_IN_TERMINAL -eq "true") {
    Read-Host -Prompt "Press [Enter] to continue"
}