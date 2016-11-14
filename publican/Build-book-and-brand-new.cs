#!/usr/bin/csharp

// TODO: Make an application

// using System; // already used by default
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using R7.Scripting;

//if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("R7_SCRIPTING_FORK")))
if (!Env.IsEmpty("R7_SCRIPTING_FORK"))
{
   var log = new Log ("publican.log");

   try
   {
      var BOOK_PATH = Directory.GetCurrentDirectory();


      var files = FileHelper.GetFiles(FileSource.Nautilus);

      if (files.Length == 0)
         throw new Exception ("Please select publican.cfg file.");

      if (files.Length > 1)
         throw new Exception ("Please select just one publican.cfg file.");

      var PUBLICAN_CFG = files[0];


      /*
	   if (!File.Exists("publican.cfg"))
	   {
		   Directory.SetCurrentDirectory("..");
		   BOOK_PATH = Directory.GetCurrentDirectory();

		   if (!File.Exists("publican.cfg"))
		   {
			   throw new Exception("File \"publican.cfg\" not found. Please invoke build script from book directory.");
		   }
	   }
      */

      // Read publican.cfg
      var CONFIG = File.ReadAllText(PUBLICAN_CFG);
      var BRAND = Regex.Match(CONFIG, @"^brand:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
      var XREBUILD_BRAND = Regex.Match(CONFIG, @"^x-rebuild-brand:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
      var CONDITION = Regex.Match(CONFIG, @"^condition:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
      //var XCONDITION = Regex.Match(CONFIG, @"^x-condition:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
      var XFORMATS = Regex.Match(CONFIG, @"^x-formats:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();

      // force regen of derivate resources, like SVG (from MathML, flatten, etc.)
      var XFORCE_REGEN = Regex.Match(CONFIG, @"^x-force-regen:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();

      // TODO: set font params (need to be forwarded in other parts!)
      var XFONT_SIZE = Regex.Match(CONFIG, @"^x-font-size:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
      var XFONT_SERIF = Regex.Match(CONFIG, @"^x-font-serif:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
      var XFONT_SANS = Regex.Match(CONFIG, @"^x-font-sans:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
      var XFONT_MONO = Regex.Match(CONFIG, @"^x-font-mono:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;

      // var XPAPER_TYPE = Regex.Match(CONFIG, @"^#\s*x-paper_type:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
      // var XPAGE_ORIENTATION = Regex.Match(CONFIG, @"^#\s*x-page_orientation:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;

      // THINK: X-OPTION to choose highlighting mode?

      Console.WriteLine("Brand: " + BRAND);
      Console.WriteLine("Condition: " + CONDITION);
      Console.WriteLine("X-Formats: " + XFORMATS);
      Console.WriteLine("X-Rebuild-Brand: " + XREBUILD_BRAND);
      Console.WriteLine("X-Force-Regen: " + XFORCE_REGEN);

      var HOME = Environment.GetEnvironmentVariable("HOME");
      var BRAND_PATH = HOME + "/Publican/Brands/publican-" + BRAND;
      // not working somehow: var BRAND_PATH = Path.Combine(HOME, "/Publican/Brands/publican-" + BRAND);

      // needed only if we use FOP to generate PDF
	   // Environment.SetEnvironmentVariable("FOP_HYPHENATION_PATH", "/usr/share/publican/fop/hyph/fop-hyph.jar");

      if (XREBUILD_BRAND == "true" || XREBUILD_BRAND == "yes" || XREBUILD_BRAND == "1")
      {
         Console.WriteLine("Rebuilding brand...");

          // re-build and re-install brand
	      Directory.SetCurrentDirectory(BRAND_PATH);

	      // publican build --formats xml --langs all --publish
	      Command.Run ("publican", "build --formats xml --langs all --publish");
	      // publican install_brand --path "/usr/share/publican/Common_Content"
	      Command.Run ("publican", "install_brand --path \"/usr/share/publican/Common_Content\"");
      }

	   // Convert ODF and MML formulas to SVG
	   Environment.SetEnvironmentVariable("F2SVG_PATH", BOOK_PATH + "/ru-RU/images");
	   Environment.SetEnvironmentVariable("F2SVG_FORCE_REGEN", XFORCE_REGEN);
	   Environment.SetEnvironmentVariable("F2SVG_FONT_SIZE", XFONT_SIZE);
      Environment.SetEnvironmentVariable("F2SVG_FONT_MONO", XFONT_MONO);
      Environment.SetEnvironmentVariable("F2SVG_FONT_SANS", XFONT_SANS);
      Environment.SetEnvironmentVariable("F2SVG_FONT_SERIF", XFONT_SERIF);

      Command.Run (Nautilus.ScriptDirectory + "/publican/f2svg.cs");

	   Directory.SetCurrentDirectory(BOOK_PATH);

      /* SLIDES!!! */
     	// Ready all Chapter*.xml
	   if (CONDITION == "slides")
	   {
         Directory.CreateDirectory("ru-RU/.chapters");

       	var chapters = Directory.GetFiles("ru-RU", "Chapter*.xml");
      	foreach (var chapter in chapters)
      	{
      	   Console.WriteLine (chapter);
      	   if (File.Exists("ru-RU/.chapters/" + Path.GetFileName(chapter) + "t"))
      	      File.Delete("ru-RU/.chapters/" + Path.GetFileName(chapter) + "t");

            File.Copy(chapter, "ru-RU/.chapters/" + Path.GetFileName(chapter) + "t");

      	   //Console.WriteLine(string.Format("--output \"{0}t\" --xinclude --novalid \"{1}\" \"{0}\"",
            //   chapter, HOME + "/.gnome2/nautilus-scripts/publican/set_condition.xslt"));

   	      Command.Run("xsltproc", string.Format("--output \"{0}t\" --xinclude --novalid \"{1}\" \"{0}\"",
   	          chapter, Nautilus.ScriptDirectory + "/publican/xslt/slides.xslt"));

   	      File.Delete (chapter);
     	    File.Move (chapter+"t", chapter);
         }
	   }

      /* SLIDES FOP!!!
    	if (File.Exists("/usr/share/publican/xsl/pdf.xsl"))
	       File.Delete("/usr/share/publican/xsl/pdf.xsl");

       File.Copy("/usr/share/publican/xsl/pdf-" + CONDITION + ".xsl","/usr/share/publican/xsl/pdf.xsl");
       */

	   Directory.SetCurrentDirectory(BOOK_PATH);
	   // publican build --formats pdf --langs ru-RU
	   //Command.Run("publican", "build --formats pdf,html-single --langs ru-RU");


      Console.WriteLine ("Building document...");

      var exitCode = 0;
      do
      {

         // clean book
         Command.Run ("publican", string.Format("clean --config \"{0}\"", PUBLICAN_CFG));

         exitCode = Command.Run ("publican", string.Format("build --config \"{0}\" --formats {1} --langs ru-RU", PUBLICAN_CFG, XFORMATS));

         if (exitCode != 0)
         {
            var fgColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Publican exited with code {0}", exitCode);
            if (exitCode == 139)
               Console.WriteLine("Press ESC to cancel rebuid cycle.");

            Console.ForegroundColor = fgColor;
         }

         if (Console.KeyAvailable)
            if (Console.ReadKey (true).Key == ConsoleKey.Escape)
               throw new Exception ("Build failed. Rebuild cycle canceled by user.");

         // publican exit codes:
         // 255 - parser error
         // 2 - DTD validation failed
         // 139 - random crush? version-dependant?

      } while (exitCode == 139);

	   //Command.Run("publican", "build --formats pdf,html-single --langs ru-RU");


      /* SLIDES!!! */
	   // Restore original Chapter*xml files
	   if (CONDITION == "slides")
      {
         var chapters = Directory.GetFiles("ru-RU", "Chapter*.xml");
         foreach (var chapter in chapters)
      	{
      	   if (File.Exists(chapter))
               File.Delete(chapter);
            File.Copy("ru-RU/.chapters/" + Path.GetFileName(chapter)+"t", chapter);
         }
      }

       // DEPRECATED: Copy /js brand directory to output dir, if format is one of HTML's
       // We use integrated Kate highlight + some jQuery for HTML
       /*string [] xformats = XFORMATS.Split(',');

       foreach (string xformat in xformats)
       {
           if (xformat.Contains("html"))
           {
               Command.Run("cp",
                   string.Format("-f -R \"{0}/ru-RU/js\" \"{1}/tmp/ru-RU/{2}\"",
			                 BRAND_PATH, BOOK_PATH, xformat));
           }
       }*/


       /*
	   string [] pdfFiles = Directory.GetFiles(Path.Combine(BOOK_PATH,"tmp/ru-RU/pdf"), "*.pdf", SearchOption.TopDirectoryOnly);


	   // TODO: Correct file name
	   if (pdfFiles.Length > 0)
		   System.IO.File.Move(pdfFiles[0], pdfFiles[0]+".pdf");

	   File.Delete("/usr/share/publican/xsl/pdf.xsl");
       File.Copy("/usr/share/publican/xsl/pdf-slides.xsl","/usr/share/publican/xsl/pdf.xsl");

	   Directory.SetCurrentDirectory(BOOK_PATH);
	   Command.Run("publican", "build --formats pdf --langs ru-RU");
	   */

   }
   catch (Exception ex)
   {
	   log.WriteException (ex);
   }

   log.Close();

   Console.Write("Press any key to exit...");
   Console.ReadKey(true);
}
else
{
   Env.Set ("R7_SCRIPTING_FORK","1");
   Command.RunNoWait("x-terminal-emulator", string.Format("-e {0}/publican/Build-book-and-brand-new.cs", Nautilus.ScriptDirectory));
}
quit;
