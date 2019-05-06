#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        new Script ().Run ();
        return 0;
    }
}

public class Script
{
	public void Run ()
	{
		Directory.SetCurrentDirectory (Nautilus.CurrentDirectory);
		var log = new Log ("pdf-to-pages-pdftk");

		try
		{
			foreach (var fileName in FileHelper.GetFiles (FileSource.NautilusSelection))
			{
				try
				{
					Command.Run ("pdftk", string.Format ("\"{0}\" burst", fileName));
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
