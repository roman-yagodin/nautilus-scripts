#!/usr/bin/csexec -t -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

///<summary>
/// Compress scanned PDF files using ImageMagick
///</summary>
public static class Program
{
    public static int Main (string [] args)
    {
		Console.WriteLine ("!!! ONE-PAGE PDFs ONLY !!!");
		Console.WriteLine ();

		Console.Write ("Enter image DPI (e.g. 72-51200): (hit [Enter] for \"96\") ");
		
		int density;
		if (!int.TryParse (Console.ReadLine (), out density)) {
			density = 100;
		}
		Console.WriteLine ();

		Console.Write ("Enter JPEG quality (0-100)): (hit [Enter] for \"75\") ");
		
		
		int quality;
		if (!int.TryParse (Console.ReadLine (), out quality)) {
			quality = 75;
		}
		else if (quality < 0) {
			quality = 0;
		}
		else if (quality > 100) {
			quality = 100;
		}
		Console.WriteLine ();

        var script = new PdfCompressScannedScript (args);
		script.Density = density;
		script.Files = FileHelper.GetFiles (FileSource.NautilusSelection);
		var scriptResult = script.Run ();

		Console.WriteLine ();
		Console.Write ("Press any key to quit...");
		Console.ReadKey (true);

        return scriptResult;
    }
}

public class PdfCompressScannedScript: FileScriptBase
{
	public int Density { get; set; }

	public int Quality { get; set; }

	public PdfCompressScannedScript (string [] args): base (args)
	{
		AllowedExtensions.Add (".pdf");
		ContinueOnErrors = true;
	}

	public override int ProcessFile (string file)
	{
		// skip directories
		if (FileHelper.IsDirectory (file)) {
			return 0;
		}

		var outFile = file + ".compressed";
		
		var result = Command.Run ("convert",
			$"\"{file}\" -density {Density}x{Density} -quality {Quality} -compress jpeg \"{outFile}\""
		);

		if (result == 0) {
			var fi1 = new FileInfo (file);
			var fi2 = new FileInfo (outFile);

			if (fi1.Length > fi2.Length) {
				// compression succeded, compressed file size is less than original file size
				FileHelper.Backup (file, "~backup", BackupType.Numbered);
				FileHelper.Move (outFile, file, true);
				Console.WriteLine ($"{Path.GetFileName (file)} - compressed.");
			}
			else {
				// compression failed, size of compressed file is greater or equal than original
				File.Delete(outFile);
				Console.WriteLine ($"{Path.GetFileName (file)} - not compressed.");
			}
		}
		else {
			Console.WriteLine ($"{Path.GetFileName (file)} - error.");
		}

		return result;
	}
}
