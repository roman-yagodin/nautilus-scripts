#!/usr/bin/pwsh

$Source = "nuget.org"
$Location = "https://api.nuget.org/v3/index.json"
$Destination = "$PWD"

if ($(Get-PackageSource -Name $Source | Measure-Object).Count -eq 0) {
    Register-PackageSource -Name $Source -Location $Location -ProviderName NuGet
}

Install-Package -Source $Source -Name Unidecode.NET -MinimumVersion 1.4.0 -Destination $Destination -SkipDependencies