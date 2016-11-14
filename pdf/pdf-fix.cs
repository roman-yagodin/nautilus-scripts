#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
		new PdfFixScript ().Run ();
		return 0;
    }
}

public class PdfFixScript
{
	public void Run ()
	{
		Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
		var log = new Log ("pdf-fix");

		try {
			foreach (var file in FileHelper.GetFiles (FileSource.Nautilus)) {
				try {
					if (Path.GetExtension (file).ToLowerInvariant () == ".pdf") {
						// repairs a PDF’s corrupted XREF table and stream lengths, if possible
						Command.Run ("pdftk", string.Format ("\"{0}\" output \"{0}.fixed\"", file));
						
						if (File.Exists (file + ".fixed")) {
							FileHelper.Backup (file, "~backup", BackupType.Numbered);
							FileHelper.Move (file + ".fixed", file, true);
						}
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
			log.Close();
		}
	}
}
