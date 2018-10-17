# About nautilus-scripts

**nautilus-scripts** is a collection of various C# and Shell scripts, designed to use with *Nautilus* and *Nemo* filemanagers (*Caja* support is planned).

## Common dependencies

All C# scripts require *Mono Framework* installed. Most of them use Mono C# Shell (`csharp` utility), some use [csexec](https://github.com/roman-yagodin/csexec) script (which should be in `/usr/bin`). Also, most scripts depends on [R7.Scripting](https://github.com/roman-yagodin/R7.Scripting) library, which  should be in `~/.config/csharp`. Follow the links to get those components and more info about them.

## Script-specific dependencies

- `pdf-combine/`, `pdf-compress/` require `gs` utility from *GhostScript*
- `image-*/`, `pdf-to/` require `convert` utility from *ImageMagick*
- `convert/` require `unoconv` utility from *LibreOffice* / *OpenOffice*
- `video/` require `ffmpeg` utility from *FFMpeg*
- `compare/` require `meld` application from *Meld*
-  `zip/` require `7z` utility from *7Zip*

## License

All scripts are published under the terms of the GNU General Public License, either version 3 of the License, or (at your option) any later version.
