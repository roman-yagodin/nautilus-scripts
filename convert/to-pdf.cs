#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        new Script (args [0]).Run ();
        return 0;
    }
}

public class Script
{
    public string ScriptName { get; protected set; }

    public Script (string scriptName)
    {
        ScriptName = Path.GetFileNameWithoutExtension (scriptName);
    }

	public void Run ()
	{
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
		var log = new Log (ScriptName);
        var backupDirectory = "./~backup";

		try
		{
			var files = (NauHelper.IsNothingSelected)? Directory.GetFiles (Directory.GetCurrentDirectory ()) : NauHelper.SelectedFiles;

            if (files.Length > 0) {
                Directory.CreateDirectory (backupDirectory);
            }

			foreach (string file in files)
			{
				try
				{
					var ext = Path.GetExtension (file).ToLowerInvariant ();

                    // TODO: Filter by extension (list may be very large)

                    // backup files, overwrite existing
                    File.Copy (file, Path.Combine (backupDirectory, Path.GetFileName (file)), true);

                    // run unoconv
                    Command.Run ("unoconv", string.Format ("-f pdf \"{0}\"", file));
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
		finally
		{
			log.Close();
		}
	}
}
