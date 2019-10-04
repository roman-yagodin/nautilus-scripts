#!/usr/bin/pwsh

Add-Type –Path "$PSScriptRoot/../../.nuget/Unidecode.NET.1.4.0/lib/netstandard2.0/Unidecode.NET.dll"
$Unidecoder = [Unidecode.NET.Unidecoder]

function PreUnidecodeCyrillic {
    # Make it more bank-style
    param ([string] $text)

    $text = $text -creplace "Ъ", "'"
    $text = $text -creplace "ъ", "'"
    $text = $text -creplace "ь", ""
    $text = $text -creplace "Ь", ""

    $text = $text -creplace "Ц", "C"
    $text = $text -creplace "ц", "c"

    $text = $text -creplace "Х", "H"
    $text = $text -creplace "х", "h"

    $text = $text -creplace "Ш", "Sh"
    $text = $text -creplace "ш", "sh"

    $text = $text -creplace "Щ", "Sch"
    $text = $text -creplace "щ", "sch"

    $text = $text -creplace "Ё", "E"
    $text = $text -creplace "ё", "e"

    $text = $text -creplace "ЫЙ", "Y"
    $text = $text -creplace "ый", "y"
    
    return $text
}

function Test-Unidecode {
    $Unidecoder::Unidecode("Съешь ещё этих мягких французских булок, да выпей же чаю...")
    $Unidecoder::Unidecode((PreUnidecodeRuText "Съешь ещё этих мягких французских булок, да выпей же чаю..."))

    $Unidecoder::Unidecode("Холодный зимний вечер")
    $Unidecoder::Unidecode((PreUnidecodeRuText "Холодный зимний вечер"))
}

Export-ModuleMember -Function * -Variable *