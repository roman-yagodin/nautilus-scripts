# About nautilus-scripts

**nautilus-scripts** is a collection of various C#, PowerShell and Shell scripts, designed to use with *Nautilus* and *Nemo* and *Caja* filemanagers.

## System requirements

The C# scripts require *Mono Framework* installed. Most of them use Mono C# Shell (`csharp` utility), some use [csexec](https://github.com/roman-yagodin/csexec) script (which should be in `/usr/bin`). Also, most scripts depends on [R7.Scripting](https://github.com/roman-yagodin/R7.Scripting) library, which  should be in `~/.config/csharp`. Follow the links to get those components and more info about them.

The PowerShell scripts require [PowerShell](https://github.com/PowerShell/PowerShell) in order to run. Scripts,
which designed to run in a terminal window, also require [pwshx](https://github.com/roman-yagodin/pwshx) wrapper. 

## Major dependencies

- `pdf-compress` require *GhostScript*
- `pdf-combine`, `pdf-fix`, `pdf-rotate` require *PDFTK*
- `image-*`, `pdf-to` require *ImageMagick*
- `convert` require *Unoconv* from *LibreOffice* / *OpenOffice* 
- `video` require *FFMpeg*
- `compare` require *Meld*

## License

All scripts are published under the terms of the GNU General Public License, either version 3 of the License, or (at your option) any later version.
