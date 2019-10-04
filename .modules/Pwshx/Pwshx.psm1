function Wait-IfTerminal
{
    [CmdletBinding ()]
    param (
        [string] $Prompt = "Press [Enter] to continue"
    )
    begin {
        if ($Env:PWSHX_IN_TERMINAL -eq "true") {
            Read-Host -Prompt $Prompt
        }
    }
}