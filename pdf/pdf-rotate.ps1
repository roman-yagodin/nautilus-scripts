#!/usr/bin/pwshx -t

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"
Import-Module "$PSScriptRoot/../.modules/Pdf/Pdf.psm1"
Import-Module "$PSScriptRoot/.modules/Pwshx/Pwshx.psm1"

Get-NautilusSelectedFiles | Get-FilteredFiles -Extension ".pdf" `
    | Invoke-PdfRotate

Wait-IfTerminal
