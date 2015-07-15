#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
Log log = new Log("translit-lowercase.log");

try 
{
	
	string[] files;
	if (NauHelper.IsNothingSelected)
		files = Directory.GetFiles (Directory.GetCurrentDirectory ());
	else
		files = NauHelper.SelectedFiles;

	foreach (string file in files)
	{
		try 
		{
			string newFileName = FileHelper.TranslitMachine (Path.GetFileNameWithoutExtension (file)).ToLower ();
			newFileName = newFileName.Trim(new char [] {' ', '_', '-'});
				
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
