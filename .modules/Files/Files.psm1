#!/usr/bin/pwsh

# TODO: Add Restore-File cmdlet
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
                New-Item -Path $BackupDir -ItemType Directory -Force 
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
    }
}

function Get-FilteredFiles
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [System.IO.FileInfo]$File,

        [Parameter(Mandatory=$true)]
        [string]$Extensions
    )
    process {
        if (($Extensions.ToLowerInvariant() -split ";") -contains (Split-Path -Path $File.FullName -Extension).ToLowerInvariant()) {
            Write-Output -InputObject $File
        }
    }
}
