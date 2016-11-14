#!/usr/bin/csharp

using System;
using System.Text;
using System.IO;
using R7.Scripting;

var compLevel = "/default";
// other: /default, /screen (lowest), /ebook, /printer, /prepress (highest)
var outfilePrefix = "combined_";

Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
var log = new Log ("combine-pdf");

try
{
   var filenames = string.Empty;
   var files = FileHelper.GetFiles(FileSource.Nautilus);
   var filteredFiles = new List<string> ();
   var pdfFiles = 0;

   foreach (var file in files)
   {
      var ext = Path.GetExtension(file).ToLowerInvariant();
      if ((ext == ".pdf" || ext == ".ps") && !FileHelper.IsDirectory (file))
      {
         filenames += string.Format("\"{0}\" ", Path.GetFileName(file));
		 filteredFiles.Add (file);
         pdfFiles++;
      }
   }

   if (pdfFiles > 0)
   {
      // extract pdfmarks from first file
      Command.Run (Path.Combine (Nautilus.ScriptDirectory, "common", "compress-PDF-pdfmarks.sh"), "\"" + files[0] + "\"");

      // make combined pdf
      var outfile = outfilePrefix + Path.GetFileName(files[0]);
      Command.Run("gs", string.Format("-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS={0} -dNOPAUSE -dQUIET -dBATCH "+
				                                "-sOutputFile=\"{1}\" {2} .pdfmarks ", compLevel, outfile, filenames));

      // remove pdfmarks file
		File.Delete (".pdfmarks");

		// backup combined files
		foreach (var file in filteredFiles) {
            FileHelper.Backup (file, "~backup", BackupType.Numbered);
			File.Delete (file);
		}
   }
   else
   {
      log.WriteLine ("Info: Not enougth PDF/PS files to combine");
   }
}
catch (Exception ex)
{
   log.WriteLine ("Error: " + ex.Message);
}

log.Close();
quit;
