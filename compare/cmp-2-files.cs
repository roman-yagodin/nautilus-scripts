#!/usr/bin/csexec -t -r:R7.Scripting.dll

using System;
using System.IO;
using System.Diagnostics;
using R7.Scripting;

public class Program
{
	public static void Main (string [] args)
	{
		try
		{
			var files = FileHelper.GetFiles (FileSource.Nautilus);
			if (files.Length != 2)
				throw new Exception("You must select exactly 2 files");

			Command.Run("cmp", string.Format("-l \"{0}\" \"{1}\"", files[0], files[1]));

			Console.ReadKey (true);
		}
		catch  (Exception ex)
		{
			Console.WriteLine ("Error: " + ex.Message);
		}
	}
}
