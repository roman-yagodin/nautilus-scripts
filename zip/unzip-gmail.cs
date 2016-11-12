#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args) {
        new UnzipGmailScript ().Run ();
        return 0;
    }
}

public class UnzipGmailScript
{
	public void Run () {

        var log = new Log ("unzip-gmail");
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
        
        try {
            var files = NauHelper.SelectedFiles;
			foreach (string file in files) {
				try {
                    var ext = Path.GetExtension (file).ToLowerInvariant ();
					if (ext == ".zip") {
                        UnzipGmail (file);
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

    protected void UnzipGmail (string file) {
        // invoke 7-Zip
        Environment.SetEnvironmentVariable ("LANG", "C");
        Command.Run ("7z", string.Format ("x \"{0}\"", file));

        // backup original zip archive
        FileHelper.Backup (file, "~backup");
        File.Delete (file);
    }
}
