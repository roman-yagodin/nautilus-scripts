#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        var script = new FileScript (args, (file) => {
            var tryCount = 2;
            var result = 0;

            while (tryCount > 0) {
                // run unoconv
                result = Command.Run ("unoconv", string.Format ("-f pdf \"{0}\"", file));
                if (result == 0) {
                    FileHelper.Backup (file, "~backup", BackupType.Numbered);
                    File.Delete (file);
                    break;
                }
                tryCount--;
            }

            if (result != 0) {
                throw new Exception (string.Format ("Cannot convert \"{0}\" file.", file));
            }

            return result;
        });

        // TODO: Filter by extension (list may be very large)
        script.Files = FileHelper.GetFiles (FileSource.Nautilus);

        var scriptResult = script.Run ();
        return scriptResult;
    }
}
