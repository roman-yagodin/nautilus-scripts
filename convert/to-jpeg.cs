#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

///<summary>
/// Converts any graphic file to JPEG using ImageMagick
///</summary>
public static class Program
{
    public static int Main (string [] args)
    {
        var script = new FileScript (args, (file) => {
			var ext = Path.GetExtension (file).ToLowerInvariant ();
			var result = 0;
			if (ext != ".jpg" && ext != ".jpeg") {
				var outFile = Path.Combine (
					Path.GetDirectoryName (file),
					Path.GetFileNameWithoutExtension (file) + ".jpg"
				);

				FileHelper.Backup (file, "~backup", BackupType.Numbered);
				result = Command.Run ("convert", $"\"{file}\" -quality 100 \"{outFile}\"");
				if (result == 0) {
					File.Delete (file);
				}
			}

			return result;
        });


        // TODO: Filter by extension (list may be very large)
        script.Files = FileHelper.GetFiles (FileSource.Nautilus);

        var scriptResult = script.Run ();
        return scriptResult;
    }
}
