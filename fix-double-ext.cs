#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args) {
        new FixDoubeExtScript ().Run ();
        return 0;
    }
}

public class FixDoubeExtScript
{
    public void Run () {

        var log = new Log ("fix-double-ext");
        Directory.SetCurrentDirectory (NauHelper.CurrentDirectory);
        Directory.CreateDirectory ("~backup");

        try {
            var files = NauHelper.SelectedFiles;
            foreach (string file in files) {
                try {
                    var ext1 = Path.GetExtension (file);
                    var fileName = Path.GetFileNameWithoutExtension (file);
                    var ext2 = Path.GetExtension (fileName);

                    if (!string.IsNullOrEmpty (ext2)) {
                        if (string.Compare (ext1, ext2, StringComparison.CurrentCultureIgnoreCase) == 0) {
                            var newFile = Path.Combine (
                                Path.GetDirectoryName (file),
                                Path.GetFileNameWithoutExtension (fileName)
                            ) + ext1;
                            
                            if (!File.Exists (newFile)) {
                                File.Move (file, newFile);
                            }
                            else {
                                log.WriteLine ("File already exists: " + newFile);
                            }                   
                        }
                    }  
                }
                catch (Exception ex) {
                    log.WriteLine ("Error: " + ex.Message);
                }
            }
	    }
        catch (Exception ex) {
            log.WriteLine ("Error: " + ex.Message);
        }
        finally {
            log.Close ();
        }
    }
}
