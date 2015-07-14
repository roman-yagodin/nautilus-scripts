#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
Directory.CreateDirectory("~compress-pdf");

var log = new Log("compress-pdf.log");

try 
{
	var compLevel = "/screen";
	// /default, /screen (lowest), /ebook, /printer, /prepress (highest)
	
	foreach (var file in FileHelper.GetFiles(FileSource.Nautilus))
	{
		try 
		{
			if (Path.GetExtension(file).ToLowerInvariant() == ".pdf" && !FileHelper.IsDirectory (file))
			{
				//Console.WriteLine(file);

				var outfile = ".compressed_" + Path.GetFileName(file);
				Console.WriteLine (outfile);

				//TODO: make indep. from concrete user, maybe implement NauHelper.ScriptDirectory 
				//Command.Run(Path.Combine(NauHelper.ScriptDirectory, "common", "compress-PDF-pdfmarks.sh"), "\"" + file + "\"");

Command.Run("/home/redhound/.local/share/nemo/scripts/common/compress-PDF-pdfmarks.sh", "\"" + file + "\"");
				
				
				Command.Run("gs", string.Format("-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS={0} -dNOPAUSE -dQUIET -dBATCH "+
				                                "-sOutputFile=\"{1}\" \"{2}\" .pdfmarks", compLevel, outfile, Path.GetFileName(file)));

				var fi1 = new FileInfo (file);
				var fi2 = new FileInfo (outfile);

				if (fi1.Length > fi2.Length)
				{
					// compression succeded, compressed file size 
					// less than original file size
					
					var backup = "./~compress-pdf/" + Path.GetFileName(file);
										
					if (File.Exists(backup))
						File.Delete(backup);
					
					File.Copy (file, backup);
					File.Delete(file);
					File.Move (outfile, file);
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
