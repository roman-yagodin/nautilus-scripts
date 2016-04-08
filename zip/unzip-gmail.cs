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
        Directory.CreateDirectory ("~backup");

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
        // tell to 7Zip to use
        Environment.SetEnvironmentVariable ("LANG", "C");
        Command.Run ("7z", string.Format ("x \"{0}\"", file));
        // move zip archive to ~backup
        File.Move (file, Path.Combine ("~backup", Path.GetFileName (file)));
    }
}
