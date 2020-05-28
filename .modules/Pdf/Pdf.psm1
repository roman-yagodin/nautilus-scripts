#!/usr/bin/pwsh

function Invoke-PdfRotate {
    [CmdletBinding()]
    param (
        # TODO: Add Path parameter
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo]$PdfFile,

        [Parameter(
            Mandatory=$true,
            HelpMessage="Possible page rotation values (in degrees): north (0), east (90), south (180), west (270), left (-90), right (+90), down (+180)"
        )]
        [AllowEmptyString()]
        [ValidateSet("", "north", "east", "south", "west", "left", "right", "down")]
        [string]$Rotation
    )
    begin {
        if ($Rotation.Length -eq 0) { $Rotation = "right" }

        Write-Host "Rotating PDFs using ""$Rotation"" rotation." 
    }
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName + ".rotated"

        pdftk @("""$inFile""", "rotate",  "1-end$Rotation", "output", """$outFile""")
        
        if (Test-Path -Path $outFile) {
            Import-Module "$PSScriptRoot/../Files/Files.psm1"
            Backup-File $_ "~backup" | Out-Null
            Move-Item -Path $outFile -Destination $inFile -Force
                       
            Get-Item $inFile | Write-Output
        }
        else {
            Write-Warning "$($_.Name) rotation failed!";
        }
    }
}

function Invoke-PdfFix {
    [CmdletBinding()]
    param (
        # TODO: Add Path parameter
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo]$PdfFile
    )
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName + ".fixed"
        pdftk @("""$inFile""", "output", """$outFile""")
        
        if (Test-Path -Path $outFile) {
            Import-Module "$PSScriptRoot/../Files/Files.psm1"
            Backup-File $_ "~backup" | Out-Null
            Move-Item -Path $outFile -Destination $inFile -Force
            
            Get-Item $inFile | Write-Output
        }
        else {
            Write-Warning "$($_.Name) fixup failed!";
        }
    }
}

function Compress-Pdf {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo]$PdfFile,

        [Parameter(
            Mandatory=$true,
            HelpMessage="Possible compression levels: default, screen (low-res), ebook (medium-res), printer, prepress (high-res)"
        )]
        [AllowEmptyString()]
        [ValidateSet("", "default", "screen", "ebook", "printer", "prepress")]
        [string]$CompressionLevel
    )
    begin {
        if ($CompressionLevel.Length -eq 0) { $CompressionLevel = "default" }

        Write-Host "Compressing PDFs with Ghostscript using ""$CompressionLevel"" compression." 
    }
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName + ".compressed"

        ghostscript @("-sDEVICE=pdfwrite", "-dCompatibilityLevel=1.4", "-dPDFSETTINGS=/$CompressionLevel",
            "-dNOPAUSE", "-dQUIET", "-dBATCH", "-sOutputFile=""$outFile""", """$inFile""") 

        if (!(Test-Path -Path $outFile)) {
            Write-Warning "$($_.Name) compression failed!";
            exit
        }

        $inFileLength = (Get-Item $inFile).length
        $outFileLength = (Get-Item $outFile).length

        if ($outFileLength -lt $inFileLength) {
            Move-Item -Path $outFile -Destination $inFile -Force
            Get-Item $inFile | Write-Output
        }
        else {
            Remove-Item -Path $outFile
            Write-Verbose "$($_.Name) is not compressed.";
        }
    }
}