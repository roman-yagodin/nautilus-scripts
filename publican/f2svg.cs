#!/usr/bin/csharp

using System;
using System.IO;
using System.Diagnostics;
using R7.Scripting;

// class A { void Main () {

try 
{   
   string F2SVG_PATH = Environment.GetEnvironmentVariable ("F2SVG_PATH");
   bool F2SVG_FORCE_REGEN = Environment.GetEnvironmentVariable ("F2SVG_FORCE_REGEN") == "true" || 
                            Environment.GetEnvironmentVariable ("F2SVG_FORCE_REGEN") == "on" || 
                            Environment.GetEnvironmentVariable ("F2SVG_FORCE_REGEN") == "yes";

   // FIXME: Some defaults - must get them from publican.cfg, for all RUN TYPES
   string F2SVG_FONT_SERIF = "Liberation Serif"; 
   string F2SVG_FONT_SANS = "Liberation Sans"; 
   string F2SVG_FONT_MONO = "Liberation Mono";
   float F2SVG_FONT_SIZE = 18.0f; 

   // THINK: Internal params?
   float F2SVG_SCRIPT_MINSIZE = F2SVG_FONT_SIZE / 3f * 2f; 
   float F2SVG_SCRIPT_SIZEMULT = 0.8f;

	string [] files = null;
	bool filesReady = false;

   // RUN TYPE 1 - from Publican build helper script
	if (!string.IsNullOrEmpty (F2SVG_PATH))
	{
		Directory.SetCurrentDirectory (F2SVG_PATH);
		files = Directory.GetFiles (Directory.GetCurrentDirectory ());
		filesReady = true;
      
      F2SVG_FONT_SERIF = Environment.GetEnvironmentVariable ("F2SVG_FONT_SERIF");
      F2SVG_FONT_SANS = Environment.GetEnvironmentVariable ("F2SVG_FONT_SANS"); 
      F2SVG_FONT_MONO = Environment.GetEnvironmentVariable ("F2SVG_FONT_MONO");
      float.TryParse (Environment.GetEnvironmentVariable ("F2SVG_FONT_SIZE"), out F2SVG_FONT_SIZE);
      F2SVG_SCRIPT_MINSIZE = F2SVG_FONT_SIZE / 3f * 2f;
  }
	
	// RUN TYPE 2 - from Nautilus
	if (!filesReady)
	{
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
	
		if (NauHelper.IsNothingSelected)
			files = Directory.GetFiles (Directory.GetCurrentDirectory ());
		else
			files = NauHelper.SelectedFiles;
	}
	
	Console.WriteLine ("Converting MML or ODF formulas to SVG with jeuclid-cli...");

	if (files.Length <= 0)
	{
		Console.WriteLine ("No formulas found!..");
	}
	else
	{
      // Perform conversion
   	foreach (string file in files)
		{
			if ((Path.GetExtension (file).ToUpperInvariant () == ".ODF" 
				|| Path.GetExtension (file).ToUpperInvariant () == ".MML")			
				&& !FileHelper.IsDirectory(file))
			{
				string svgFile = Path.GetFileNameWithoutExtension (file) + ".svg";
	
            bool needRegen = true;

         	if (File.Exists (svgFile))
			{
					// if last change time of original file is before of change time of svg, 
					// then we do not need to regen svg
               		needRegen = File.GetLastWriteTime (file).ToFileTime() >= File.GetLastWriteTime (svgFile).ToFileTime();
			}
		
			if (needRegen || F2SVG_FORCE_REGEN)
			{
				Process convert = new Process ();
				convert.StartInfo.FileName = "jeuclid-cli";
				convert.StartInfo.Arguments = 
					string.Format ("\"{0}\" \"{1}\" -fontSize {2} -fontsMonospaced \"{3}\" " +
			 			"-fontsSansSerif \"{4}\" -fontsSerif \"{5}\" -scriptMinSize {6} -scriptSizeMult {7}", 
						file, // 0 
                 svgFile, // 1
				 F2SVG_FONT_SIZE.ToString("##.#", System.Globalization.CultureInfo.InvariantCulture), // 2
                 F2SVG_FONT_MONO, // 3
                 F2SVG_FONT_SANS, // 4
                 F2SVG_FONT_SERIF, // 5
				 F2SVG_SCRIPT_MINSIZE.ToString("##.#", System.Globalization.CultureInfo.InvariantCulture), // 6  12.0
				 F2SVG_SCRIPT_SIZEMULT.ToString("##.#", System.Globalization.CultureInfo.InvariantCulture) // 7  0.8
              );
				convert.Start ();
				convert.WaitForExit ();

				// FIXME: This throws an exception
				// File.SetLastWriteTime (svgFile, File.GetLastWriteTime(file));
				
				Console.WriteLine ("Converted {0}", file);
			} 
			else
			{
				Console.WriteLine ("Skipped {0}", file);
			}
       		}
		}
	}
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
quit;

// }}
