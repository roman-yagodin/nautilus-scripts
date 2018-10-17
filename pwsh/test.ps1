#!/usr/bin/pwshx -t

#"NEMO_SCRIPT_SELECTED_FILE_PATHS: $Env:NEMO_SCRIPT_SELECTED_FILE_PATHS" | Out-File env.txt
#"NEMO_SCRIPT_SELECTED_URIS: $Env:NEMO_SCRIPT_SELECTED_URIS" | Out-File env.txt -Append
#"NEMO_SCRIPT_CURRENT_URI: $Env:NEMO_SCRIPT_CURRENT_URI" | Out-File env.txt -Append
#"NEMO_SCRIPT_WINDOW_GEOMETRY: $Env:NEMO_SCRIPT_WINDOW_GEOMETRY" | Out-File env.txt -Append
#"Script Directory: $PSScriptRoot" | Out-File env.txt -Append

#Get-ChildItem Env: | Out-File env.txt

Import-Module "$PSScriptRoot/../.modules/Nautilus/Nautilus.psm1"
Import-Module "$PSScriptRoot/../.modules/Pdf/Pdf.psm1"
Import-Module "$PSScriptRoot/../.modules/Files/Files.psm1"


