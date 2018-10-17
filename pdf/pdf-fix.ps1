#!/usr/bin/pwsh

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"
Import-Module "$PSScriptRoot/../.modules/Pdf/Pdf.psm1"

Get-NautilusSelectedFiles | Get-FilteredFiles -Extensions ".pdf" | Invoke-PdfFix
