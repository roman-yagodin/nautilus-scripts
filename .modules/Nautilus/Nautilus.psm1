#!/usr/bin/pwsh

function Get-NautilusSelectedFiles
{
    [CmdletBinding()]
    param ()
    process {
        Get-NautilusSelectedItems | Where-Object { $_ -is [System.IO.FileInfo] }
    }
}

function Get-NautilusSelectedItems
{
    [CmdletBinding()]
    param ()
    process {
        $brand = Get-NautilusBrand
        $selectedFiles = (NautilusEnv $brand "SELECTED_FILE_PATHS") -split "\r?\n" | Where-Object { $_.length -gt 0 }
        foreach ($file in $selectedFiles) {
            Get-Item -Path $file -Force | Write-Output
        }
    }
}


function Get-NautilusBrand
{
    [CmdletBinding()]
    param ()
    process {
        $brands = @("NAUTILUS", "NEMO", "CAJA")
        foreach ($brand in $brands) {
            if ((NautilusEnv $brand "CURRENT_URI").Length -gt 0) {
                Write-Output $brand
            }
        }
    }
}

function NautilusEnv {
    param ([string]$brand, [string]$varname)
    return [System.Environment]::GetEnvironmentVariable("$($brand)_SCRIPT_$($varname)")
    #return $(Get-Item -Path Env:* | where {$_.Name -eq "$($brand)_SCRIPT_$($varname)"}).Value
}
