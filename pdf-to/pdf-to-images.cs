#!/usr/bin/csharp

using System;
using System.IO;
using R7.Scripting;

Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
var log = new Log("pdf-to-images.log");

try 
{
	foreach (var fileName in FileHelper.GetFiles (FileSource.Nautilus))
	{
		try 
		{
			var baseFileName = Path.GetFileNameWithoutExtension (fileName);

			Command.Run ("convert", string.Format ("-density 100 -quality 92 " +
				"\"{0}\" \"{1}\"_%03d.jpg", fileName, baseFileName));
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
