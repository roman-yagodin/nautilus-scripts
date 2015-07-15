using System;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Redhound.Scripting
{
	/*
	NAUTILUS_SCRIPT_SELECTED_FILE_PATHS: список выделенных файлов, разделённых переводом строки (только в локальном случае)
	NAUTILUS_SCRIPT_SELECTED_URIS: список адресов (URI) выделенных файлов, разделённых переводом строки
	NAUTILUS_SCRIPT_CURRENT_URI: текущий адрес URI
	NAUTILUS_SCRIPT_WINDOW_GEOMETRY: положение и размер текущего окна 
	NAUTILUS_SCRIPT_NEXT_PANE_SELECTED_FILE_PATHS: список выделенных файлов, разделённых переводом строки, в неактивной панели окна раздельного вида (только в локальном случае)	
	NAUTILUS_SCRIPT_NEXT_PANE_SELECTED_URIS: список адресов (URI) выделенных файлов, разделённых переводом строки, в неактивной панели окна раздельного вида
	NAUTILUS_SCRIPT_NEXT_PANE_CURRENT_URI: текущий адрес URI в неактивной панели окна раздельного вида
	*/

	/// <summary>
	/// Helper for Nautilus 
	/// </summary>
	public class NauHelper
	{
		public static string CurrentDirectory {
			get { return HttpUtility.UrlDecode (Environment.GetEnvironmentVariable ("NAUTILUS_SCRIPT_CURRENT_URI")).Remove (0, "file://".Length); }
		}

		private static string[] selectedFiles = null;
		public static string[] SelectedFiles {
			get {
				if (selectedFiles == null) {
					string filesVar = Environment.GetEnvironmentVariable ("NAUTILUS_SCRIPT_SELECTED_URIS");
					selectedFiles = filesVar.Split (new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					
					for (var i = 0; i < selectedFiles.Length; i++)
						selectedFiles[i] = HttpUtility.UrlDecode (selectedFiles[i]).Remove (0, "file://".Length);
				}
				return selectedFiles;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class DateTimeHelper
	{
		public static string IsoToday {
			get { return DateTime.Now.ToString ("yyMMdd"); }
		}
		
		public static bool IsIsoDate (string date)
		{
			if (date.Length != 6)
				return false;
			
			int a;
			if (!int.TryParse (date, out a))
				return false;
			
			int day = int.Parse (date.Substring (4, 2));
			if (day > 31 || day < 1)
				return false;
			
			int month = int.Parse (date.Substring (2, 2));
			if (month > 12 || month < 1)
				return false;
			
			return true;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class FileHelper
	{
		public static bool IsDirectory (string file)
		{
			return (File.GetAttributes (file) & FileAttributes.Directory) != 0;
		}
		
		 public static string Translit(string s)
            {
                // TODO: 
                // выбор режима транслитерации:
                //      й -> j, й - y и др.
                // выбор режима регистра:
                //      Ж -> ZH, Ж -> Zh
                // выбор направления транслитерации


                // http://www.translit.ru/


                string[] ru = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", 
                             "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф",
                             "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                             "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", 
                             "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф",
                             "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"
                           };


                string[] lat = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y",
                           "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f",
                           "h", "c", "ch", "sh", "sch", "", "y", "", "e", "yu", "ya",
                           "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y",
                           "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F",
                           "H", "C", "Ch", "Sh", "Sch", "", "Y", "", "E", "Yu", "Ya",
                            };

                // mode = LatY;
                // mode = LatJ;
                // mode = Visual (Lineage2);

                // ч -> 4
                // й - j
                // солнышко = COJlНblLLjKO

                string translitted = String.Empty;

                // FIXME: Эти операции не относятся к транслиту!

                // фильтруем "лишние" символы, заменяя на _
                // остаются только буквы, цифры, _, - и пробел
                s = Regex.Replace(s, @"[^0-9^a-z^а-я^A-Z^А-Я^-]", "_");

                //s = Regex.Replace(s, @",+", "_");

                // удаляем лишние '_'
                s = Regex.Replace(s, @"_+", "_");
				// TODO: удаление лишних '_' в конце строки
				
			   
			 

                // х в начале слова -> kh
                s = Regex.Replace(s, @"\bх", "kh");
                s = Regex.Replace(s, @"\bХ", "Kh");

                // ый в конце слова -> y
           		s = Regex.Replace(s, @"\Bый", "y");
			   	
                s = Regex.Replace(s, "ье", "ie");
                s = Regex.Replace(s, "ья", "iya");
                s = Regex.Replace(s, "ью", "iyu");
                //fname = Regex.Replace(fname, @"\W", "_");

                foreach (char ch in s)
                {
                    bool found = false;
                    for (int i = 0; i < ru.Length; i++)
                        if (ru[i] == ch.ToString())
                        {
                            translitted += lat[i];
                            found = true;
                            break;
                        }

                    if (!found)
                        translitted += ch.ToString();
                }

                // заменяем все лишние пробелы и подчеркивания
                translitted = Regex.Replace(translitted, @"[\s_]+", "_");

                return translitted;

            }
		
		    private static bool CopyDirectory (string SourcePath, string DestinationPath, bool overwriteexisting)
		    {
		    	bool ret = false;
		    	try 
               {
		    		SourcePath = SourcePath.EndsWith (@"\") ? SourcePath : SourcePath + @"\";
		    		DestinationPath = DestinationPath.EndsWith (@"\") ? DestinationPath : DestinationPath + @"\";
		    
                   if (Directory.Exists (SourcePath)) 
                   {
		    			if (Directory.Exists (DestinationPath) == false)
		    				Directory.CreateDirectory (DestinationPath);
		    
                       foreach (string fls in Directory.GetFiles (SourcePath)) 
                       {
		    				FileInfo flinfo = new FileInfo (fls);
		    				flinfo.CopyTo (DestinationPath + flinfo.Name, overwriteexisting);
		    			}
		    			foreach (string drs in Directory.GetDirectories (SourcePath)) 
                       {
		    				DirectoryInfo drinfo = new DirectoryInfo (drs);
		    				if (CopyDirectory (drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
		    					ret = false;
		    			}
		    		}
		    		ret = true;
		    	} 
               catch (Exception ex) 
               {
		    		ret = false;
		    	}
		    	return ret;
		    }  
		
		// CHECK: need testing
		public static void CopyDirectory (string source, string target)
		{
			CopyDirectoryWave (new DirectoryInfo (source), new DirectoryInfo (target));
		}
		
		// CHECK: need testing
		private static void CopyDirectoryWave (DirectoryInfo source, DirectoryInfo target)
		{
			foreach (DirectoryInfo dir in source.GetDirectories ())
				CopyDirectoryWave (dir, target.CreateSubdirectory (dir.Name));
			
			foreach (FileInfo file in source.GetFiles ())
				file.CopyTo (Path.Combine (target.FullName, file.Name), true);
		}
		
		
		/*
		public enum TranslitMode
        {
            TranslitRu, 
            DigitLetters, 
            GOST_7_79_2000, 
            GOST_16876_71, 
            SEV_1362_78,
            Visual_MMORPG,
            MvdRf, 
            LC,
            BGN,
            BSI
        }
		*/
		
		
		
	}
	
	public class Command {
		
		public static void Run (string command, string arguments)
		{
			Process ps = new Process ();
			ps.StartInfo.FileName = command;
			ps.StartInfo.Arguments = arguments;
			ps.Start ();
			ps.WaitForExit ();
		}
		
		public static void RunNoWait (string command, string arguments)
		{
			Process ps = new Process ();
			ps.StartInfo.FileName = command;
			ps.StartInfo.Arguments = arguments;
         	ps.Start ();
      	}
	}
}
