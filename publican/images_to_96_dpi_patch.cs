#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

try 
{
    // FOP need such a density set to all raster pictures
	string density = "96";
		
	//Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
	
	string [] files;
	
	// Script may be started from Nautilus
	if (Nautilus.FromNau)
	{
		
		if (Nautilus.IsSomethingSelected)
			files = Nautilus.SelectedFiles;
		else
			files = Directory.GetFiles (Directory.GetCurrentDirectory());
	}
	else
    {
        // TODO: Embed in build script with env. variable !!!

     	// ... or from another script
		// in this case, args[1] is directory name
	    string [] args  = Environment.GetCommandLineArgs();
	    Console.WriteLine(args[0] + " " + args[1] + " " + args[2]);
		files = new string[0];
		Console.WriteLine(args.Length);
		files = Directory.GetFiles (args[1]);
	}
	
/*
    // 1) string.Format ("\"{0}\" -units PixelsPerInch -density {1}x{1} +repage \"{0}\"", file, density);				
    // 2) string.Format ("\"{0}\" -units PixelsPerInch -density {1}x{1} \"{0}\"", file, density));
    // FIX: set units first!!!     
    // http://www.imagemagick.org/discourse-server/viewtopic.php?f=2&t=18241           
    //string.Format ("\"{0}\" -set units PixelsPerInch -density {1} \"{0}\"", file, density));
*/

    foreach (string file in files) 
	{
		if ((Path.GetExtension (file).ToUpper () == ".PNG" || 
             Path.GetExtension (file).ToUpper () == ".JPG") && 
            (File.GetAttributes (file) & FileAttributes.Directory) == 0) 
		{  
            Console.WriteLine(file);

            // CHECK: Use mogrify instead of convert?
			Command.Run ("convert",
   		        string.Format ("-units PixelsPerInch \"{0}\" -density {1} \"{0}__\"", file, density));
 			
            // replace old file with new
            File.Delete (file);
            File.Move (file + "__", file);
		}
	}
         
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
	ShowVars();
}
quit;
