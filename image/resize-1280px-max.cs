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

					if (ext == ".jpg" || ext == ".jpeg")
					{
                         // backup original files
                        FileHelper.Backup (file, "~backup", BackupType.Numbered);

                        // run convert
                        Command.Run ("convert", string.Format (
                            "\"{0}\" -auto-orient -interpolate filter -filter lanczos " +
                            "-resize {2} -quality 92 -sampling-factor 1:1:1 " +
                            "-interlace Line +repage \"{1}\"", file, file, "1280x1280>")
                        );
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
