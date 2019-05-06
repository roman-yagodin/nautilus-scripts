#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using System.Linq;
using R7.Scripting;

///<summary>
/// Combine PDF files using PDFTK
///</summary>
public static class Program
{
    public static int Main (string [] args)
    {
			var script = new PdfCombineScript (args);
			script.Files = FileHelper.GetFiles (FileSource.NautilusSelection);
			
			var result = 0;
			if (script.Files.Length >= 2) {
				result = script.Run2 ();
			}

			return result;
    }
}

public class PdfCombineScript: FileScriptBase
{
	public PdfCombineScript (string [] args): base (args)
	{
		AllowedExtensions.Add (".pdf");
		ContinueOnErrors = false;
	}
	
	// TODO: Move to base library
	private string AddToFilename (string addition, string path)
	{
		return Path.Combine (Path.GetDirectoryName (path), addition + Path.GetFileName (path));
	}

	public int Run2 ()
	{
		var outfile = AddToFilename ("combined_", Files [0]);
		var infiles = string.Join (" ", Files.Select (f => $"\"{f}\""));

		var result = Command.Run ("pdftk", $"{infiles} cat output \"{outfile}\"");	

		foreach (var file in Files) {
			FileHelper.Backup (file, "~backup");
        	File.Delete (file);
		}

		return result;
	}

	public override int ProcessFile (string file)
	{
		return 0;
	}
}
