#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        new CompressScannedPdfScript ().Run ();
        return 0;
    }
}

public class CompressScannedPdfScript
{
	public void Run ()
    {
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
        Directory.CreateDirectory ("~backup");
        var log = new Log ("scanned-85");

		try {
			foreach (var file in FileHelper.GetFiles (FileSource.Nautilus)) {
                var ext = Path.GetExtension (file).ToLowerInvariant ();
                if (ext == ".pdf" && !FileHelper.IsDirectory (file)) {
                    var outFile = ".compressed_" +   Path.GetFileName (file);
    				try {
    					Command.Run ("convert",
                            string.Format ("-density 150x150 -quality 85 -compress jpeg \"{0}\" \"{1}\"",
                                file, outFile));

        				if (new FileInfo (file).Length > new FileInfo (outFile).Length) {
        					// compression succeded, the compressed file size is less than original file size
        					var backupFile = Path.Combine ("~backup", Path.GetFileName (file));
        					if (File.Exists (backupFile)) {
        						File.Delete (backupFile);
                            }

                            // backup original file
        					File.Copy (file, backupFile);
        					File.Delete (file);

                            // substitute original file
        					File.Move (outFile, file);
        				}
        				else {
        					// compression failed, size of the compressed file is greater or equal than original
        					File.Delete (outFile);
        				}
    				}
    				catch (Exception ex) {
    					log.WriteLine ("Error: " + ex.Message);
    				}
                }
			}
		}
		catch (Exception ex) {
			log.WriteLine ("Error: " + ex.Message);
		}
		finally {
			log.Close();
		}
	}
}
