# About nautilus-scripts

**nautilus-scripts** is a collection of various C# and Shell scripts, designed to use with *Nautilus* and *Nemo* filemanagers (*Caja* support is planned).

## Common dependencies

All C# scripts require *Mono Framework* installed. Most of them use Mono C# Shell (`csharp` utility), some use [csexec](https://github.com/roman-yagodin/csexec) script (which should be in `/usr/bin`). Also, most scripts depends on [R7.Scripting](https://github.com/roman-yagodin/R7.Scripting) library, which  should be in `~/.config/csharp`. Follow the links to get those components and more info about them.

## Script-specific dependencies

- `pdf-combine/`, `pdf-compress/` require `gs` from *GhostScript*
- `image-*/`, `pdf-to/` require `convert` from *ImageMagick*
- `convert/` require `unoconv` from *LibreOffice* / *OpenOffice*
- `video/` require `ffmpeg`
- `compare/` require `meld`

## License

All scripts code licensed under GNU GPL v3 or any later version.
