#!/usr/bin/csharp

using System;
using System.IO;
using System.Diagnostics;
using R7.Scripting;

try 
{
	var files = FileHelper.GetFiles (FileSource.Nautilus);
	
	if (files.Length != 2)	
		throw new Exception("You must select exactly 2 files");

	Command.Run("meld", string.Format("\"{0}\" \"{1}\"", files[0], files[1]));			
}
catch (Exception ex)
{
	Console.WriteLine ("Error: " + ex.Message);
}
quit;
