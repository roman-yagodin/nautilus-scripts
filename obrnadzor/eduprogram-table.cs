#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        new Script ().Run (Path.GetDirectoryName (args [0]));

        return 0;
    }
}

public class Script
{
	public void Run (string workingDir)
	{
		Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
		var log = new Log ("eduprogram-table");
        string [] tags = {"oop", "ucheb_plan", "annot", "graf", "metod", "chislen", "perevod" };

		try
		{
			var files = (NauHelper.IsNothingSelected)? Directory.GetFiles (Directory.GetCurrentDirectory ()) : NauHelper.SelectedFiles;

            var baseDir = "/portals/0/eduprograms/{folder}/";
            var template = File.ReadAllText (Path.Combine (workingDir, "template.html"));

            var oop_title = File.ReadAllText ("oop.txt");
            var folder = Path.GetFileName (Directory.GetCurrentDirectory()).Split (' ') [0];
            var code = oop_title.Split (' ') [0];

            var outFile = "output.html";
            var praktika_count = 1;

			foreach (string file in files)
			{
				try
				{
					var ext = Path.GetExtension (file);

					if (ext == ".pdf")
					{
						foreach (var tag in tags)
                        {
    						if (StartsWith (Path.GetFileName(file), tag))
    						{
                                template = template.Replace ("{" + tag + "}", Path.Combine  (baseDir, Path.GetFileName(file)) );
    						}
                        }

                        if (StartsWith (Path.GetFileName(file), "rp_"))
                        {
                            template = template.Replace ("{rp_praktika}",
                            "<a href=\"" + Path.Combine  (baseDir, Path.GetFileName(file)) + "\" itemprop=\"EduPr\">" + (praktika_count++) + "</a> {rp_praktika}");
                        }
                    }
				}
				catch (Exception ex)
				{
					log.WriteLine ("Error: " + ex.Message);
				}
			}

            template = template.Replace ("{code}", code);
            template = template.Replace ("{folder}", folder);
            template = template.Replace ("{oop_title}", oop_title);

            // clean tags
            template = template.Replace ("{rp_praktika}", string.Empty);

            File.WriteAllText (outFile, template);

            Command.Run ("atom", outFile);
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

	protected bool StartsWith (string value, string start)
	{
		return value.StartsWith (start, StringComparison.InvariantCultureIgnoreCase);
	}
}
