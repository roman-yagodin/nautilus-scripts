#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        var maxSize = 1024;

        var script = new FileScript (args, (file) => {
            FileHelper.Backup (file, "~backup", BackupType.Numbered);
            return Command.Run ("convert", string.Format (
                "\"{0}\" -auto-orient -interpolate filter -filter lanczos " +
                "-resize {2}x{2}> -quality 92 -sampling-factor 1:1:1 " +
                "-interlace Line +repage \"{1}\"", file, file, maxSize)
            );
        });

        script.Files = FileHelper.GetFiles (FileSource.Nautilus);
        script.AllowedExtensions.Add (".jpg");
        script.AllowedExtensions.Add (".jpeg");

        var result = script.Run ();        

        return result;
    }
}
