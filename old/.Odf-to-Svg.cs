#!/usr/bin/csrun

using System;
using System.Web;
using System.IO;
using System.Diagnostics;
using Redhound.Scripting;

namespace Redhound.Scripting.Odf2Svg
{
    public class Program 
	{
        public static void Main (string[] args)
        {
        	string[] files = null;
        	
			bool filesReady = false;
        	
			// run from Publican build helper script
        	if (!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("PUBLICAN_ODF_PATH")))
			{
        		string odf_path = Environment.GetEnvironmentVariable ("PUBLICAN_ODF_PATH");
        		Directory.SetCurrentDirectory (odf_path);
        		files = Directory.GetFiles (Directory.GetCurrentDirectory ());
        		filesReady = true;
        	}
  
			// run from Nautilus
        	if (!filesReady)
			{
        		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
   
				if (NauHelper.SelectedFiles.Length == 0)
        			files = Directory.GetFiles (Directory.GetCurrentDirectory ());
        		else
        			files = NauHelper.SelectedFiles;
        	}
   
			Console.WriteLine ("Converting ODF formulas to SVG with jeuclid-cli...");
        	if (files.Length <= 0)
			{
        		Console.WriteLine ("No formulas found!..");
			}
			else
			{
        
        		foreach (string file in files)
				{
        			if (Path.GetExtension (file).ToUpper () == ".ODF" &&
						(File.GetAttributes (file) & FileAttributes.Directory) == 0)
					{
        				string svgFile = Path.GetFileNameWithoutExtension (file) + ".svg";
     
						bool needRegen = true;
        				if (File.Exists (svgFile))
						{
        					if (File.GetLastWriteTime (file) == File.GetLastWriteTime (svgFile))
							{
        						Console.WriteLine ("Skipped {0}", file);
        						needRegen = false;
        					}
        				}
     
						if (needRegen)
						{
        					Process convert = new Process ();
        					convert.StartInfo.FileName = "jeuclid-cli";
        					convert.StartInfo.Arguments = 
								string.Format ("\"{0}\" \"{1}\" -fontSize 18.0 -fontsMonospaced \"DejaVu Sans Mono\" " +
						 			"-fontsSansSerif \"Liberation Sans\" -fontsSerif \"Liberation Serif\" -scriptMinSize 12.0 -scriptSizeMult 0.8", 
									file, svgFile);
        					convert.Start ();
        					convert.WaitForExit ();
     
							// FIXME: This throws an exception
							// File.SetLastWriteTime (svgFile, File.GetLastWriteTime(file));
							
							Console.WriteLine ("Converted {0}", file);
        				}
        			}
        		}
       			// Console.WriteLine("done!");
        	}
		}
    }
}
