#!/usr/bin/csexec -t -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

///<summary>
/// Compress PDF files using Ghostscript
///</summary>
public static class Program
{
    public static int Main (string [] args)
    {
		Console.WriteLine ("Possible compression levels:");
		Console.WriteLine ();
		Console.WriteLine ("\tdefault -  output intended to be useful across a wide variety of uses,");
		Console.WriteLine ("\t           possibly at the expense of a larger output file.");
		Console.WriteLine ("\tscreen -   low-resolution output similar to the Acrobat Distiller \"Screen Optimized\" setting.");
		Console.WriteLine ("\tebook -    medium-resolution output similar to the Acrobat Distiller \"eBook\" setting.");
		Console.WriteLine ("\tprinter -  output similar to the Acrobat Distiller \"Print Optimized\" setting.");
		Console.WriteLine ("\tprepress - output similar to Acrobat Distiller \"Prepress Optimized\" setting.");
		Console.WriteLine ();
		Console.Write ("Enter compression level: (hit [Enter] for \"default\") ");
		var compLevel = Console.ReadLine ();
		Console.WriteLine ();

		if (string.IsNullOrWhiteSpace (compLevel)) {
			compLevel = "default";
		}

        var script = new PdfCompressScript (args);
		script.CompLevel = compLevel;
		script.Files = FileHelper.GetFiles (FileSource.Nautilus);
		var scriptResult = script.Run ();

		Console.WriteLine ();
		Console.Write ("Press any key to quit...");
		Console.ReadKey (true);

        return scriptResult;
    }
}

public class PdfCompressScript: FileScriptBase
{
	public string CompLevel { get; set; }

	public PdfCompressScript (string [] args): base (args)
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
		
		var result = Command.Run ("ghostscript",
			$"-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS=/{CompLevel} -dNOPAUSE -dQUIET -dBATCH -sOutputFile=\"{outFile}\" \"{file}\""
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
