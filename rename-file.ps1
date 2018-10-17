#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/.modules/Nautilus/Nautilus.psm1"

$Prefix = Read-Host -Prompt "Enter filename prefix"

$selectedFiles = Get-NautilusSelectedFiles

$filesCount = ($selectedFiles | Measure-Object).Count
$numOfDigits = [System.Math]::Floor([System.Math]::Log($filesCount,10) + 1)

$index = 1
$selectedFiles | ForEach-Object -Process {
    Rename-Item $_ -NewName "$($Prefix)_$($index.ToString().PadLeft($numOfDigits,'0'))$(Split-Path -Path $_ -Extension)"
    $index++
}