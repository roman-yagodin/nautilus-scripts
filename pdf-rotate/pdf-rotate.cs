#!/usr/bin/csexec -t -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
		Console.WriteLine ("Possible page rotation values (in degrees):");
		Console.WriteLine ("\tnorth: 0, east: 90, south: 180, west: 270");
		Console.WriteLine ("\tleft: -90, right: +90, down: +180");
		Console.WriteLine ();
		Console.Write ("Enter page rotation: (hit [Enter] for \"right\") ");
		var rotation = Console.ReadLine ();

		var script = new PdfRotateScript ();
		if (!string.IsNullOrEmpty (rotation)) {
			script.Run (rotation);
		}
		else {
			script.Run ("right");
		}

		return 0;
    }
}

public class PdfRotateScript
{
	public void Run (string rotation)
	{
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
		Directory.CreateDirectory ("~backup");
		var log = new Log ("pdf-rotate");

		try {
			foreach (var file in FileHelper.GetFiles (FileSource.Nautilus)) {
				try {
					if (Path.GetExtension (file).ToLowerInvariant () == ".pdf") {
						Command.Run ("pdftk", string.Format ("\"{0}\" rotate 1-end{1} output \"{0}.rotated\"", file, rotation));

						if (File.Exists (file + ".rotated")) {
							var backupFile = Path.Combine ("~backup", Path.GetFileName (file));
							File.Copy (file, backupFile, true);
							File.Copy (file + ".rotated", file, true);
							File.Delete (file + ".rotated");
						}
					}
				}
				catch (Exception ex)
				{
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
