#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
var log = new Log("translit-lowercase.log");

try 
{
	foreach (var file in FileHelper.GetFiles (FileSource.Nautilus))
	{
		try 
		{
			var newFileName = FileHelper.TranslitMachine (Path.GetFileNameWithoutExtension (file)).ToLower ();
			newFileName = newFileName + Path.GetExtension (file).ToLower ();
					
			if (!FileHelper.IsDirectory (file))
				File.Move (file, newFileName);
			else 
				Directory.Move (file, newFileName);
			
			log.WriteLine ("Renamed: " + Path.GetFileName (file));
		}
		catch (Exception ex)
		{
			log.WriteLine ("Error: " + ex.Message);
		}
	}
}
catch (Exception ex)
{
	log.WriteLine ("Error: " + ex.Message);
}

log.Close();
quit;
