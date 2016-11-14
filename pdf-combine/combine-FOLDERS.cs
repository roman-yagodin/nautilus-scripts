#!/usr/bin/csharp

using System;
using System.Text;
using System.IO;
using R7.Scripting;

var log = new Log("combine-lists-pdf.log");

try 
{
   var compLevel = "/screen";
   // other: /default, /screen (lowest), /ebook, /printer, /prepress (highest)

   Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
   var scriptDirectory = Nautilus.ScriptDirectory;

   // For testing:
   // Directory.SetCurrentDirectory("/media/Data/Сайт/На сайт/140709 1112 Приемная/09-07-2014");

   var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());

   foreach (var dir in dirs)
   {
      var filenames = string.Empty;
      var pdfFiles = 0;   
      var outfile = FileHelper.TranslitMachine(dir.Substring(dir.LastIndexOf("/")+1)).ToLowerInvariant() + ".pdf";

      var files = Directory.GetFiles(dir);

      foreach (var file in files)
      {
         var ext = Path.GetExtension(file).ToLowerInvariant();
         if ((ext == ".pdf" || ext == ".ps") && !FileHelper.IsDirectory (file))
         {  
            filenames += string.Format("\"{0}\" ", Path.GetFileName(file));
            pdfFiles++;         
         }
      }

      Directory.SetCurrentDirectory(dir);

      if (pdfFiles > 0)
      {
         // extract pdfmarks from first file
         Command.Run (Path.Combine (scriptDirectory, "common", "compress-PDF-pdfmarks.sh"), "\"" + files[0] + "\"");
         
         // make combined pdf
    
         Command.Run("gs", string.Format("-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS={0} -dNOPAUSE -dQUIET -dBATCH "+
			                                      "-sOutputFile=\"{1}\" {2} .pdfmarks ", compLevel, outfile, filenames));
         
         // remove pdfmarks file  
	      File.Delete (".pdfmarks");

         File.Move(outfile, Path.Combine("..", outfile));
      }
      else 
      {
         log.WriteLine("Info: Not enougth PDF/PS files to combine");
      }  
   }
}
catch (Exception ex)
{
   log.WriteLine ("Error: " + ex.Message);
}

log.Close();
quit;
