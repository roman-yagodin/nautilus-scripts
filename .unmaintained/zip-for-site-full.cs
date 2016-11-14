#!/usr/bin/csharp

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using R7.Scripting;

Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
var log = new Log("zip-for-site.log");

try
{
	string tmpDirName = Path.Combine(DateTime.Now.ToString("yy"),  DateTimeHelper.IsoToday);
	string tmpDirName2 = DateTimeHelper.IsoToday;

	// some addition to path in case, when we use existing dir
	string subTmpDirName = string.Empty;

	string[] files = FileHelper.GetFiles (FileSource.NautilusSelection);

	if (files.Length == 0)
		files = Directory.GetFiles (Directory.GetCurrentDirectory (), "*", SearchOption.AllDirectories);
	else
	{
		// files = Nautilus.SelectedFiles;

		if (files.Length == 1 && FileHelper.IsDirectory (files[0])) // && DateTimeHelper.IsIsoDate (Path.GetFileName (files[0])))
		{
			tmpDirName = Path.GetFileName (files[0]);
			subTmpDirName = tmpDirName;
		}

		// TODO: must be all files and files in dirs


		List<string> combined = new List<string> (files.Length * 2);
		combined.AddRange (files);

		foreach (string file in files)
			if (FileHelper.IsDirectory (file))
				combined.AddRange (Directory.GetFiles (file, "*", SearchOption.AllDirectories));

		files = combined.ToArray ();
	}

   #region CREATE TEMP DIR

   string tmp = Path.GetTempPath ();
	   string tmpDir = Path.Combine (Path.GetTempPath (), tmpDirName);
	   string zipFileName = tmpDirName2 + ".zip";



   if (Directory.Exists (tmpDir))
		   Directory.Delete (tmpDir, true);

   Directory.CreateDirectory (tmpDir);

   #endregion




   string zipFile = Path.Combine (Directory.GetCurrentDirectory (), zipFileName);

	   Console.WriteLine (Directory.GetCurrentDirectory ());
	   foreach (string file in files)
   {
		   // skipping directories (they will be recreated) and [old] zip archive
		   if (!FileHelper.IsDirectory (file))
	   {
			   if (file != zipFile)
		   {
				   Console.WriteLine (file);

			   string destFile =
               Path.Combine (tmpDir, file.Remove (0, Directory.GetCurrentDirectory ().Length + 1));

			   string destDir = Path.GetDirectoryName (destFile);
				   if (!Directory.Exists (destDir))
					   Directory.CreateDirectory (destDir);

			   File.Copy (file, destFile, true);
				   //Console.WriteLine("File " + file + " copied to " + Path.Combine(zipDir, Path.GetFileName(file)));
			   }
		   }
	   }

   // deleting old archive after copy
	   if (File.Exists (zipFile))
   {
		   File.Delete (zipFile);
		   Console.WriteLine ("Delete old file " + Path.GetFileName (zipFile));
	   }

   //zip -r -0 -UN=UTF8 "${SOURCE_FILE}.zip" "${SOURCE_FILE}"

   // usual result is "/tmp"
	   Directory.SetCurrentDirectory (Path.Combine (tmp, subTmpDirName));

   Console.WriteLine ("Starting zip...");
	   Process zip = new Process ();
	   zip.StartInfo.FileName = "zip";
	   zip.StartInfo.Arguments = string.Format ("-r -6 -UN=UTF8 \"{0}\" \"{1}\"", zipFileName, tmpDirName);
	   zip.Start ();

   if (zip.WaitForExit (Command.DefaultWaitTime))
   {
	   File.Move (Path.Combine( Path.Combine (tmp, subTmpDirName), zipFileName), zipFile);
	   Directory.Delete (tmpDir, true);
	   Console.WriteLine ("Temp directory deleted");
   }
}
catch (Exception ex)
{
		log.WriteLine ("Error: " + ex.Message + "\n" + ex.StackTrace);
}
quit;
