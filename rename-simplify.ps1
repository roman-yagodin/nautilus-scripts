#!/usr/bin/pwsh

Import-Module "$PSScriptRoot/.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/.modules/Files/Files.psm1"

Get-NautilusSelectedFiles | Rename-FileSimplify
