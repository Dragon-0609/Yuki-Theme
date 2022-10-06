using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
namespace AppAggregator
{
	internal class Aggregator
	{
		public static  string CurrentPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ()?.Location);
		private static bool   _isPlugin;
		private static bool   _fullAggregate;
		private static string _baseDirectory;
		private static bool   _clean;

		public static void Main (string [] args)
		{
			_isPlugin = false;
			if (args.Length > 0)
			{
				IsAggregate (args [0]);
			} else
			{
				_fullAggregate = true;
			}
			_baseDirectory = GetAppBaseDirectoryPath ();
			if (!_clean)
			{
				CreateOutputDirectory ();

				if (_fullAggregate)
				{
					GenerateZip ();
					_isPlugin = true;
					GenerateZip ();
				} else
				{
					GenerateZip ();
				}
			} else
			{
				DeleteOutputDirectory ();
			}
		}
		private static void IsAggregate (string arg)
		{
			if (IsClean (arg))
				_clean = true;
			else if (IsFullAggregate (arg))
				_fullAggregate = true;
			else
				_isPlugin = IsPlugin (arg);
		}
		private static void GenerateZip ()
		{
			string [] paths = GetNecessaryFilePaths ();
			string outputName = GenerateOutputName ();


			string fileName = Path.Combine (_baseDirectory, "zip_output", outputName);
			if (File.Exists (fileName))
				File.Delete (fileName);
			using (ZipArchive zip = ZipFile.Open (fileName, ZipArchiveMode.Create))
			{
				foreach (string path in paths)
				{
					zip.CreateEntryFromFile (path, Path.GetFileName (path));
				}
				zip.CreateEntry ("Themes/");
			}
		}
		private static string GenerateOutputName ()
		{

			string outputName = "Yuki_Theme";
			if (_isPlugin)
				outputName += "_Plugin";
			outputName += ".zip";
			return outputName;
		}

		private static bool IsPlugin (string s)
		{
			return s.ToLower () == "plugin";
		}

		private static bool IsFullAggregate (string s)
		{
			return s.ToLower () == "full";
		}

		private static bool IsClean (string s)
		{
			return s.ToLower () == "clean";
		}

		private static void CreateOutputDirectory ()
		{
			if (!Directory.Exists (Path.Combine (_baseDirectory, "zip_output")))
				Directory.CreateDirectory (Path.Combine (_baseDirectory, "zip_output"));
		}

		private static void DeleteOutputDirectory ()
		{
			string path = Path.Combine (_baseDirectory, "zip_output");
			if (Directory.Exists (path))
			{
				foreach (string file in Directory.GetFiles (path))
				{
					File.Delete (file);
				}
				Directory.Delete (path);
			}
		}

		private static string GetAppBaseDirectoryPath ()
		{
			string location = Assembly.GetEntryAssembly ()?.Location;
			if (location != null)
			{
				const string appDirectory = "App Aggregator";
				int intLocation = location.IndexOf (appDirectory, StringComparison.Ordinal);
				return location.Substring (0, intLocation);
			} else
			{
				return null;
			}
		}

		private static string [] GetNecessaryFilePaths ()
		{
			List <string> res = new List <string> ();

			string [] filePaths = Directory.GetFiles (_baseDirectory, "files*.txt", SearchOption.AllDirectories);
			res.AddRange (GetFiles (filePaths, "files.txt"));

			string targetFile = _isPlugin ? "files_plugin.txt" : "files_program.txt";
			res.AddRange (GetFiles (filePaths, targetFile));

			return res.ToArray ();
		}
		private static string [] GetFiles (string [] filePaths, string target)
		{

			string file = filePaths.First (i => i.ToLower ().Contains (target));
			string [] baseFiles = File.ReadAllText (file).Split (
				new string [] { Environment.NewLine },
				StringSplitOptions.None
			);

			string [] files = GetFilesByArray (baseFiles);
			return files;
		}
		private static string [] GetFilesByArray (string [] baseFiles)
		{
			string [] files = Directory.GetFiles (_baseDirectory, "*", SearchOption.AllDirectories).Where (f => baseFiles.Contains (Path.GetFileName (f))).ToArray ();
			FilePath [] paths = FilePath.Convert (files);
			files = paths.DistinctBy (f => f.FileName).Select (path => path.FullPath).ToArray ();
			return files;
		}
	}

	internal class FilePath
	{
		public string FileName;
		public string FullPath;
		public FilePath (string fullPath)
		{
			FullPath = fullPath;
			FileName = Path.GetFileName (fullPath);
		}

		public static FilePath [] Convert (string [] files)
		{
			List <FilePath> paths = new List <FilePath> ();
			foreach (string file in files)
			{
				paths.Add (new FilePath (file));
			}
			return paths.ToArray ();
		}
	}

	internal static class Extensions
	{
		public static IEnumerable <TSource> DistinctBy <TSource, TKey>
			(this IEnumerable <TSource> source, Func <TSource, TKey> keySelector)
		{
			HashSet <TKey> seenKeys = new HashSet <TKey> ();
			foreach (TSource element in source)
			{
				if (seenKeys.Add (keySelector (element)))
				{
					yield return element;
				}
			}
		}
	}
}