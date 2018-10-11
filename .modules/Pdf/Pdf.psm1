#!/usr/bin/pwsh

function Invoke-PdfRotate {
    [CmdletBinding()]
    param (
        # TODO: Add Path parameter
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo]$PdfFile,

        [Parameter(Mandatory=$true)]
        [ValidateSet("right","left","down","north","east","west")]
        [string]$Rotation
    )
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName + ".rotated"
        pdftk @("""$inFile""", "rotate",  "1-end$Rotation", "output", """$outFile""")
        
        if (Test-Path -Path $outFile) {
            Import-Module "$PSScriptRoot/../Files/Files.psm1"
            Backup-File $_ "~backup"
            Move-Item -Path $outFile -Destination $inFile -Force
            Write-Verbose "$($_.Name) rotated";
        }
        else {
            Write-Warning "$($_.Name) rotation failed";
        }
    }
}