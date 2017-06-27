#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        var script = new FileScript (args, (file) => {
            var result = 0;
            
            var outFile = Path.Combine (
				Path.GetDirectoryName (file),
				Path.GetFileNameWithoutExtension (file) + ".pdf"
			);

            var ext = Path.GetExtension (file);
            if (ext == ".jpg" || ext == ".jpeg"
             || ext == ".png" || ext == ".bmp"
             || ext == ".tif" || ext == ".tiff"
             || ext == ".gif" || ext == ".pdf" || ext == ".ps")
            {
                // run imagemagick
                result = Command.Run ("convert", $"\"{file}\" \"{outFile}\"");
            }
            else {
                // run unoconv
                var tryCount = 2;
                while (tryCount > 0) {
                    result = Command.Run ("unoconv", $"-f pdf \"{file}\"");
                    if (result == 0) {
                        break;
                    }
                    tryCount--;
                }
            }
            
            if (result == 0) {
                FileHelper.Backup (file, "~backup", BackupType.Numbered);
                File.Delete (file);
            }
            else {
                throw new Exception (string.Format ("Cannot convert \"{0}\" file.", file));
            }

            return result;
        });

        script.Files = FileHelper.GetFiles (FileSource.Nautilus);

        var scriptResult = script.Run ();
        return scriptResult;
    }
}
