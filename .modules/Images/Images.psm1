#!/usr/bin/pwsh

function Resize-Image
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory, ValueFromPipeline)]
        [System.IO.FileInfo]$File,

        [Parameter(Mandatory, HelpMessage="Image size or resize geometry: https://imagemagick.org/script/command-line-options.php#resize")]
        [AllowEmptyString()]
        [string]$ResizeGeometry,

        [Parameter(HelpMessage="JPEG compression level (1-100): https://imagemagick.org/script/command-line-options.php#quality")]
        [ValidateRange(1,100)] 
        [int]$Quality = 92
    )
    begin {
        if ($ResizeGeometry.Length -eq 0) { $ResizeGeometry = "1280" }
        if ($ResizeGeometry -Match "\d+") { $ResizeGeometry = "$($ResizeGeometry)x$($ResizeGeometry)>" }
    
        Write-Host "Resizing images using $ResizeGeometry geometry and $Quality% quality."
    }
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName
        $convertArgs = @("""$inFile""", "-auto-orient", "-interpolate filter", "-filter lanczos",
                        "-resize $ResizeGeometry", "-quality $Quality", "-sampling-factor 1:1:1",
                        "-interlace Line", "+repage", """$outFile""")

        Start-Process -FilePath "convert" -ArgumentList $convertArgs -Wait

        Get-Item $outFile | Write-Output
    }
}