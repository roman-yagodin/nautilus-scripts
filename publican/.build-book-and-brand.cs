#!/usr/bin/csharp

// TODO: Make an application

using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using R7.Scripting;

try 
{
	string BOOK_PATH = Directory.GetCurrentDirectory();
	
	if (!File.Exists("publican.cfg"))
	{
		Directory.SetCurrentDirectory("..");
		BOOK_PATH = Directory.GetCurrentDirectory();
		
		if (!File.Exists("publican.cfg"))
		{
			throw new Exception("File \"publican.cfg\" not found. Please invoke build script from book directory.");		
		}
	}
   
   // Read publican.cfg
   string CONFIG = File.ReadAllText(Path.Combine(BOOK_PATH, "publican.cfg"));
   string BRAND = Regex.Match(CONFIG, @"^brand:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
   string CONDITION = Regex.Match(CONFIG, @"^condition:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
   string XFORMATS = Regex.Match(CONFIG, @"^x-formats:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();

   // force regen of derivate resources
   string XFORCE_REGEN = Regex.Match(CONFIG, @"^x-force-regen:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();

   // TODO: set font params (need to be forwarded in PDF XSLT and other parts!)
   string XFONT_SIZE = Regex.Match(CONFIG, @"^x-font-size:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value.ToLowerInvariant();
   string XFONT_SERIF = Regex.Match(CONFIG, @"^x-font-serif:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;  
   string XFONT_SANS = Regex.Match(CONFIG, @"^x-font-sans:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
   string XFONT_MONO = Regex.Match(CONFIG, @"^x-font-mono:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
  
   // string PAPER_TYPE = Regex.Match(CONFIG, @"^#\s*x_paper_type:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
   // string PAGE_ORIENTATION = Regex.Match(CONFIG, @"^#\s*x_page_orientation:\s*(.+)$", RegexOptions.Multiline).Groups[1].Value;
  
   // THINK: X-OPTION to choose highlighting mode?
 
   Console.WriteLine("Brand: " + BRAND);
   Console.WriteLine("Condition: " + CONDITION);
   Console.WriteLine("X-Formats: " + XFORMATS);
   Console.WriteLine("X-Force-Regen: " + XFORCE_REGEN);

    // Console.WriteLine("Paper type: " + PAPER_TYPE);
    // Console.WriteLine("Page orientation: " + PAGE_ORIENTATION);
  
   string BRAND_PATH = "/home/redhound/Publican/Brands/publican-" + BRAND;
   string HOME = Environment.GetEnvironmentVariable("HOME");
	 
	Environment.SetEnvironmentVariable("FOP_HYPHENATION_PATH", "/usr/share/publican/fop/hyph/fop-hyph.jar");
		
	// re-build and re-install brand
	Directory.SetCurrentDirectory(BRAND_PATH);
	
	// publican build --formats xml --langs all --publish
	Command.Run ("publican", "build --formats xml --langs all --publish");
	// publican install_brand --path "/usr/share/publican/Common_Content"
	Command.Run ("publican", "install_brand --path \"/usr/share/publican/Common_Content\"");
			
	// Convert ODF and MML formulas to SVG	
	Environment.SetEnvironmentVariable("F2SVG_PATH", BOOK_PATH + "/ru-RU/images");
	Environment.SetEnvironmentVariable("F2SVG_FORCE_REGEN", XFORCE_REGEN);
	Environment.SetEnvironmentVariable("F2SVG_FONT_SIZE", XFONT_SIZE);
   Environment.SetEnvironmentVariable("F2SVG_FONT_MONO", XFONT_MONO);
   Environment.SetEnvironmentVariable("F2SVG_FONT_SANS", XFONT_SANS);
   Environment.SetEnvironmentVariable("F2SVG_FONT_SERIF", XFONT_SERIF);
  
   Command.Run (HOME + "/.local/share/nautilus/scripts/publican/f2svg.cs", "");

	Directory.SetCurrentDirectory(BOOK_PATH);
  	// Ready all Chapter*.xml 	
	if (CONDITION == "slides") 
	{
      Directory.CreateDirectory("ru-RU/.chapters");
     
    	string [] chapters = Directory.GetFiles("ru-RU", "Chapter*.xml");
      	foreach (string chapter in chapters)
      	{
      	Console.WriteLine (chapter);
      	if (File.Exists("ru-RU/.chapters/" + Path.GetFileName(chapter) + "t"))
      	   File.Delete("ru-RU/.chapters/" + Path.GetFileName(chapter) + "t");
        
	   File.Copy(chapter, "ru-RU/.chapters/" + Path.GetFileName(chapter) + "t");
   	   
   	   //Console.WriteLine(string.Format("--output \"{0}t\" --xinclude --novalid \"{1}\" \"{0}\"", 
         //   chapter, HOME + "/.gnome2/nautilus-scripts/publican/set_condition.xslt"));
   	   
   	   Command.Run("xsltproc", string.Format("--output \"{0}t\" --xinclude --novalid \"{1}\" \"{0}\"", 
   	    chapter, HOME + "/.local/share/nautilus/scripts/publican/xslt/slides.xslt"));
   
   	   File.Delete (chapter);
     	   File.Move (chapter+"t", chapter);
	   }
	}
  
 	if (File.Exists("/usr/share/publican/xsl/pdf.xsl"))
	    File.Delete("/usr/share/publican/xsl/pdf.xsl");

    File.Copy("/usr/share/publican/xsl/pdf-" + CONDITION + ".xsl","/usr/share/publican/xsl/pdf.xsl");

	Directory.SetCurrentDirectory(BOOK_PATH);
	// publican build --formats pdf --langs ru-RU
	//Command.Run("publican", "build --formats pdf,html-single --langs ru-RU");
    Command.Run ("publican", "clean");
	Command.Run ("publican", string.Format("build --formats {0} --langs ru-RU", XFORMATS));

	//Command.Run("publican", "build --formats pdf,html-single --langs ru-RU");

	// Restore original Chapter*xml files 
	if (CONDITION == "slides") 
   {
      string [] chapters = Directory.GetFiles("ru-RU", "Chapter*.xml");
      foreach (string chapter in chapters)
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
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(ex.Message);
}

Console.Write("Press any key to exit...");
Console.ReadKey(true);
