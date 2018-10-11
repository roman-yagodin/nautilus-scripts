#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"
Import-Module "$PSScriptRoot/../.modules/Pdf/Pdf.psm1"

Write-Host "Possible page rotation values (in degrees):"
Write-Host "`tnorth: 0, east: 90, south: 180, west: 270"
Write-Host "`tleft: -90, right: +90, down: +180"
$Rotation = Read-Host -Prompt "`nEnter page rotation: (hit [Enter] for ""right"")"

if ($Rotation.Length -eq 0) {
    $Rotation = "right"
}

Get-NautilusSelectedFiles | Select-FilesByExt -Extensions ".pdf" | Invoke-PdfRotate -Rotation $Rotation

if ($Env:PWSHX_IN_TERMINAL -eq "true") {
    Read-Host -Prompt "Press [Enter] to continue"
}