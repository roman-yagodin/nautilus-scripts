#!/usr/bin/csexec -t -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        new Script ().Run ();

        Console.ReadKey (true);
        return 0;
    }
}

public class Script
{
	public void Run ()
	{
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
		var log = new Log ("ffmpeg-360p");

		try
		{
			var files = (NauHelper.IsNothingSelected)? Directory.GetFiles (Directory.GetCurrentDirectory ()) : NauHelper.SelectedFiles;

			foreach (string file in files)
			{
				try
				{
					var ext = Path.GetExtension (file);

					if (ext == ".wmv" || ext == ".mpeg" || ext == ".ogv" || ext == ".mkv" || ext == ".avi" || ext == ".mp4" || ext == ".flv" || ext == ".mpg")
					{
						// Console.WriteLine (OutputFileName (file, ".webm", 360));

						EncodeToWebm (file, "1M", "128k", 360);
						EncodeToMp4 (file, "1M", "128k", 360);
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

	protected string OutputFileName (string filename, string extension, int pvalue)
	{
		return Path.Combine (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename) +  "_" + pvalue + "p" + extension);
	}

	///<summary></summary>
	///<param name="videobitrate">Target video bitrate (1M)</param>
	///<param name="audiobitrate">Target audio bitrate (128k)</param>
	///<param name="pvalue">e.g. 360 for 360p</param>
	protected void EncodeToWebm (string file, string videobitrate, string audiobitrate, int pvalue)
	{
		var ffmpegParams = string.Format ("-i \"{0}\" -c:v libvpx -b:v {2} -b:a {3} -c:a libvorbis -vf scale=-1:{4} \"{1}\"", file, OutputFileName (file, ".webm", pvalue), videobitrate, audiobitrate, pvalue);
		Command.Run ("ffmpeg", ffmpegParams);
	}

	protected void EncodeToMp4 (string file, string videobitrate, string audiobitrate, int pvalue)
	{
		var ffmpegParams = string.Format ("-i \"{0}\" -c:v libx264 -preset slow -b:v {2} -b:a {3} -c:a libfdk_aac -vf scale=-1:{4} \"{1}\"", file, OutputFileName (file, ".mp4", pvalue), videobitrate, audiobitrate, pvalue);
		Command.Run ("ffmpeg", ffmpegParams);
	}
}
