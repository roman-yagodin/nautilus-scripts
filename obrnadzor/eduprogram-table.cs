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
        string [] tags = {"oop", "ucheb_plan", "graf", "annot", "metod", "chislen", "perevod" };
        var outFile = "output.html";
        var baseDir = "/portals/0/eduprograms/{folder}/";
        var counter = 0;

        FileStream fs = null;
        StreamWriter sw = null;

		try
		{
            // create output file
            fs = new FileStream (outFile, FileMode.Create);
            sw = new StreamWriter (fs);

            var directories = (NauHelper.IsNothingSelected)? Directory.GetDirectories (Directory.GetCurrentDirectory ()) : NauHelper.SelectedFiles;

            Array.Sort (directories);

            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName (directory);
                if (dirName.StartsWith ("__") || dirName.StartsWith ("~")) {
                    continue;
                }

                var template = File.ReadAllText (Path.Combine (workingDir, "template.html"));

                Directory.SetCurrentDirectory (directory);

                var oop_title = File.ReadAllText ("oop.txt");
                var folder = Path.GetFileName (Directory.GetCurrentDirectory()).Split (' ') [0];
                var code = oop_title.Split (' ') [0];

                var praktika_count = 1;

                var files = Directory.GetFiles (Directory.GetCurrentDirectory ());
                Array.Sort (files);

    			foreach (string file in files)
    			{
    				try
    				{
    					var ext = Path.GetExtension (file);

    					if ((ext == ".pdf" || ext == ".xls") && !Path.GetFileName (file).StartsWith ("__"))
    					{
                            var fileUrl = Path.Combine  (baseDir, Path.GetFileName(file));

    						foreach (var tag in tags)
                            {
        						if (StartsWith (Path.GetFileName(file), tag))
        						{
                                    template = template.Replace ("{" + tag + "}", fileUrl);
        						}
                            }

                            if (StartsWith (Path.GetFileName(file), "rp_"))
                            {
                                template = template.Replace ("{rp_praktika}",
                                "<a href=\"" + fileUrl + "\" itemprop=\"EduPr\">" + (praktika_count++) + "</a> {rp_praktika}");
                            }

                            if (StartsWith (Path.GetFileName(file), "graf_z"))
                            {
                                template = template.Replace ("{graf_z}",
                                "<br /><a href=\"" + fileUrl + "\" itemprop=\"education_shedule\">+&nbsp;заочники</a>");
                            }

                            if (StartsWith (Path.GetFileName(file), "ucheb_plan_z"))
                            {
                                template = template.Replace ("{ucheb_plan_z}",
                                "<br /><a href=\"" + fileUrl + "\" itemprop=\"education_plan\">+&nbsp;заочники</a>");
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
                template = template.Replace ("{#}", (++counter).ToString ());
                template = template.Replace ("{graf_z}", string.Empty);
                template = template.Replace ("{ucheb_plan_z}", string.Empty);

                // clean tags
                template = template.Replace ("{rp_praktika}", string.Empty);

                sw.Write (template);
            }
		}
		catch (Exception ex)
		{
			log.WriteLine ("Error: " + ex.Message);
		}
		finally
		{
            if (sw != null) {
                sw.Close ();
            }

            if (fs != null) {
                fs.Close ();
            }
			log.Close();
		}
	}

	protected bool StartsWith (string value, string start)
	{
		return value.StartsWith (start, StringComparison.InvariantCultureIgnoreCase);
	}
}
