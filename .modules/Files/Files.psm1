#!/usr/bin/pwsh

Import-Module "$PSScriptRoot/../Unidecode/Unidecode.psm1"

function Backup-File
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File,

        # TODO: Add Path parameter
        [Parameter(Mandatory=$true)]
        [string]$BackupDir
    )
    process {
        $Path = [System.IO.Path]
        if (Test-Path -Path $File.FullName) {
            if (!(Test-Path -Path $BackupDir -PathType Container)) {
                New-Item -Path $BackupDir -ItemType Directory -Force | Out-Null
            }
            $backupDirAbs = $Path::Combine($Path::GetDirectoryName($File.FullName), $BackupDir);
            $backupNumber = 0;
            do {
                if ($backupNumber -gt 0) { $backupSuffix = ".~$backupNumber~" } else { $backupSuffix = ".~" }
                $backupFileName = $Path::Combine($backupDirAbs, "$($File.Name)$backupSuffix")
                $backupNumber++;
            } while (Test-Path -Path $backupFileName)
            Copy-Item $File -Destination $backupFileName
            Write-Verbose "$($File.Name) was backed up";
        }
        Write-Output -InputObject $File
    }
}

function Restore-File
{
    [CmdletBinding()]
    param (
        # TODO: Add Path parameter
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File,
        
        [Parameter(Mandatory=$true)]
        [ValidateScript({Test-Path -Path $_ -PathType Container})]
        [string]$BackupDir
    )
    process {
        $backupFiles = Get-ChildItem -Path $BackupDir -Filter "*~" -File |
            Where-Object { $_.Name.StartsWith($File.Name) }
        
        $numOfBackupFiles = ($backupFiles | Measure-Object).Count    
        if ($numOfBackupFiles -gt 0) {
            $lastBackupFile = $backupFiles[0]
            Move-Item -Path $lastBackupFile.FullName -Destination $File.FullName -Force
        }
        else {
            Write-Warning "No backups found for the ""$($File.Name)"" file."
        }
        
        for ($i = 1; $i -lt $numOfBackupFiles; $i++) {
            Move-Item -Path $backupFiles[$i].FullName -Destination $backupFiles[$i-1].FullName -Force
        }    
    }
}

function IsExtensionMatch
{
    param ([System.IO.FileInfo]$File, [string[]]$Extension)
    return $Extension -contains (Split-Path -Path $File.FullName -Extension).ToLowerInvariant()
}

function Get-FilteredFiles
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File,

        [Parameter(Mandatory=$true)]
        [string[]]$Extension
    )
    process {
        if (IsExtensionMatch -File $File -Extension $Extension) {
            Write-Output -InputObject $File
        }
    }
}

function Get-FilteredImages
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File
    )
    process {
        if (IsExtensionMatch -File $File -Extension @(".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff", ".gif")) {
            Write-Output -InputObject $File
        }
    }
}


function Format-FilenameToAlpha {
    param ([string] $filename)

    $filename = $filename -replace "[^0-9a-zA-Z-_\.]", "_"
    $filename = $filename -replace "-_", "_"
    $filename = $filename -replace "_-", "_"
    $filename = $filename -replace "_+", "_"
    $filename = $filename -replace "-+", "-"
    $filename = $filename -replace "\.+", "."
    $filename = $filename -replace "^\.", "" 
    $filename = $filename -replace "\.$", ""
    
    return $filename;
}

function Format-FileExtensionToAlpha {
    param ([string] $fileExt)
    
    $fileExt = $fileExt -replace "^\.", ""

    return "." + $(Format-FilenameToAlpha $fileExt);
}

function Rename-FileSimplify
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo] $File
        # TODO: Allow to apply unidecode, simplify and tolower independently
    )
    process {
        $newName = "$(Format-FilenameToAlpha ($Unidecoder::Unidecode((PreUnidecodeCyrillic $_.BaseName))))$(Format-FileExtensionToAlpha $_.Extension)".ToLowerInvariant()
        if ($newName -cne $_.Name) {
            Rename-Item $_ -NewName $newName 
        }
        Get-Item $newName | Write-Output
    }
}

function Rename-FileReplace
{
    # TODO: Allow to replace also in extension
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo] $File,
        [Parameter(Mandatory=$true, HelpMessage="Match pattern")]
        [string] $Match,
        [Parameter(Mandatory=$true, HelpMessage="Replacement string")]
        [AllowEmptyString()]
        [string] $Replacement
    )
    begin {
        Write-Host "Renaming files by replacing ""$Match"" pattern with ""$Replacement""."
    }
    process {
        $newName = "$($_.BaseName -replace $Match, $Replacement)$($_.Extension)"

        if ($_.Name -cne $newName) {
            Rename-Item $_ -NewName $newName
        }
        else {
            Write-Verbose "No match found in $($_.Name)"
        }
        Get-Item $newName | Write-Output
    }
}

function Rename-FileSequentally
{
    # TODO: Calculate number of files
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [System.IO.FileInfo] $File,
        [Parameter(Mandatory=$true, HelpMessage="Filename prefix")]
        [string] $Prefix,
        [Parameter(Mandatory=$true, HelpMessage="Number of decimal digits")]
        [int] $NumOfDigits
    )
    begin {
        Write-Host "Renaming files sequentally using ""$Prefix"" as prefix."
        $index = 1
    }
    process {
        $newName = "$($Prefix)_$($index.ToString().PadLeft($NumOfDigits,'0'))$($_.Extension)"
        $index++

        Rename-Item $_ -NewName $newName
        Get-Item $newName | Write-Output
    }
}

Export-ModuleMember -Cmdlet * -Function *
