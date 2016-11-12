#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
var log = new Log ("compress-pdf");

try
{
	var compLevel = "/default";
	// other: /default, /screen (lowest), /ebook, /printer, /prepress (highest)

	foreach (var file in FileHelper.GetFiles(FileSource.Nautilus))
	{
		try
		{
			if (Path.GetExtension(file).ToLowerInvariant() == ".pdf" && !FileHelper.IsDirectory (file))
			{
				var outfile = ".compressed_" + Path.GetFileName(file);
				Console.WriteLine (outfile);

				// make pdfmarks file
				Command.Run (Path.Combine (NauHelper.ScriptDirectory, "common", "compress-PDF-pdfmarks.sh"),
					"\"" + file + "\"");

				Command.Run("gs", string.Format("-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS={0} -dNOPAUSE -dQUIET -dBATCH "+
				                                "-sOutputFile=\"{1}\" \"{2}\" .pdfmarks", compLevel, outfile, Path.GetFileName(file)), 240000 );

				var fi1 = new FileInfo (file);
				var fi2 = new FileInfo (outfile);

				if (fi1.Length > fi2.Length)
				{
					// compression succeded, compressed file size is less than original file size
					FileHelper.Backup (file, "~backup", BackupType.Numbered);
        			FileHelper.Move (outFile, file, true);
				}
				else
				{
					// compression failed, size of compressed
					// file is greater or equal than original
					File.Delete(outfile);
					//THINK: Use better compression (once)?
				}

				File.Delete (".pdfmarks");

				/*
				var fs = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None);
				var sw = new StreamWriter(fs);
				sw.Write("  /DOCINFO pdfmark");
				sw.Close();
				fs.Close();
				*/

			}
		}
		catch (Exception ex)
		{
			log.WriteLine ("Error: " + ex.Message);
		}
	} // foreach
}
catch (Exception ex)
{
	log.WriteLine ("Error: " + ex.Message);
}

log.Close();
quit;
