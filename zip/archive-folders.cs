#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args) {
        new Script ().Run ();
        return 0;
    }
}

public class Script
{
	public void Run () {

        var log = new Log ("archive-folders");
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);

        try {

            var directories = NauHelper.SelectedFiles;
			foreach (string directory in directories) {
				try {
					if (FileHelper.IsDirectory (directory)) {
                        CleanDirectory (directory);
                        ArchiveDirectory (directory);
                    }
				}
				catch (Exception ex) {
					log.WriteLine ("Error: " + ex.Message);
				}
			}
		}
		catch (Exception ex) {
			log.WriteLine ("Error: " + ex.Message);
		}
		finally {
			log.Close ();
		}
	}

    protected void CleanDirectory (string directory) {
        var subdirs = Directory.GetDirectories (directory);
        foreach (var subdir in subdirs) {
            // remove backup folders
            if (Path.GetFileNameWithoutExtension (subdir).StartsWith ("~")) {
                Directory.Delete (subdir, true);
            }
        }
    }

    protected void ArchiveDirectory (string directory) {
        var archiveName = Path.GetFileNameWithoutExtension (directory);
        Command.Run ("7z", string.Format ("a -r -t7z \"{0}.7z\" \"{0}/*\"", archiveName));
        Directory.Delete (directory, true);
    }
}
