#!/usr/bin/csharp

using System;
using System.IO;
using System.Diagnostics;
using R7.Scripting;

try 
{
	string[] files = null;
	        	
	Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);

	if (NauHelper.IsNothingSelected)
		files = Directory.GetFiles (Directory.GetCurrentDirectory ());
	else
		files = NauHelper.SelectedFiles;
	
	if (files.Length <= 0)
	{
		Console.WriteLine ("No files found!..");
	}
	else
	{
		foreach (string file in files)
		{
			if ((Path.GetExtension (file).ToUpper () == ".SVG")			
				&& !FileHelper.IsDirectory(file))
			{
				
				Command.Run ("inkscape", string.Format ("\"{0}\" --export-plain-svg=\"{0}\"", file));
				Console.WriteLine ("Converted {0}", file);
			}
		}
	}
		// Console.WriteLine("done!");
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
