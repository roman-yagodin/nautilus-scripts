#!/usr/bin/pwsh

function Resize-Image
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory, ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File,

        [Parameter(Mandatory, HelpMessage="Image size or resize geometry: http://www.imagemagick.org/Usage/resize/")]
        [AllowEmptyString()]
        [string]$ResizeGeometry,

        [Parameter(Mandatory, HelpMessage="JPEG quality")]
        [System.Nullable[int]]$Quality
    )
    begin {
        if ($ResizeGeometry.Length -eq 0) { $ResizeGeometry = "1280" }
        if ($ResizeGeometry -Match "\d+") { $ResizeGeometry = "$($ResizeGeometry)x$($ResizeGeometry)>" }
        if (-not $Quality.HasValue) { $Quality = 90 }

        Write-Verbose "Resizing images using $ResizeGeometry geometry and $Quality% quality."
    }
    process {
        $inFile = $_.FullName
        $outFile = $_.FullName
        $convertArgs = @("""$inFile""", "-auto-orient", "-interpolate filter", "-filter lanczos",
                        "-resize $ResizeGeometry", "-quality $Quality", "-sampling-factor 1:1:1",
                        "-interlace Line", "+repage", """$outFile""")

        Start-Process -FilePath "convert" -ArgumentList $convertArgs -Wait
    }
}