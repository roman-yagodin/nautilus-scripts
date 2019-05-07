#!/usr/bin/pwsh

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
