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
		Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
		var log = new Log (ScriptName);
        
		try
		{
			var files = (Nautilus.IsNothingSelected)? Directory.GetFiles (Directory.GetCurrentDirectory ()) : Nautilus.SelectedFiles;

			foreach (string file in files)
			{
				try
				{
					var ext = Path.GetExtension (file).ToLowerInvariant ();

                    // TODO: Filter by extension (list may be very large)

                    var tryCount = 2;
                    var result = 0;
                    while (tryCount > 0) {
                        // run unoconv
                        result = Command.Run ("unoconv", string.Format ("-f pdf \"{0}\"", file));
                        if (result == 0) {
                            FileHelper.Backup (file, "~backup", BackupType.Numbered);
                            File.Delete (file);
                            break;
                        }
                        tryCount--;
                    }

                    if (result != 0) {
                        throw new Exception (string.Format ("Cannot convert \"{0}\" file.", file));
                    }
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
