using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yuki_Theme.Core;

namespace Yuki_Theme.CLI
{
	public class SettingEditor
	{
		internal void ExportSettings (string path)
		{
			Settings.connectAndGet ();
			string destination;
			if (path != "null" && Path.HasExtension (path))
			{
				destination = path;
			} else
			{
				destination = Path.Combine (Core.CLI.currentPath, "settings.syuki");
				if (!Path.HasExtension (path)) ShowError (Core.CLI.Translate ("cli.errors.export.extension", path, destination));
			}
			SortedDictionary <int, string> dict = Settings.PrepareAll;
			string output = "";
			int o = dict.Count - 1;
			for (var i = 0; i < dict.Count; i++)
			{
				output += $"{dict.Keys.ElementAt (i)} => {dict.Values.ElementAt (i)}";
				if (i != o) output += "\n";
			}

			string outdir = Path.GetDirectoryName (destination);
			if (outdir != null) Directory.CreateDirectory (outdir);
			File.WriteAllText (destination, output, Encoding.UTF8);
			ShowSuccess (Core.CLI.Translate ("cli.success.settings.export.full"), Core.CLI.Translate ("cli.success.settings.export.short"));
		}

		internal void ImportSettings (string path)
		{
			Settings.connectAndGet ();
			if (path != "null" && File.Exists (path))
			{
				try
				{
					string input = File.ReadAllText (path, Encoding.UTF8);
					string [] lines = input.Split ('\n');
					Dictionary <int, string> dict = new Dictionary <int, string> ();
					foreach (string line in lines)
					{
						string [] param = line.Split (new string [] { " => " }, StringSplitOptions.None);
						if (param.Length == 2)
						{
							int key = int.Parse (param [0]);
							dict.Add (key, param [1]);
						}
					}

					Settings.database.UpdateData (dict);
					Settings.connectAndGet ();
					ShowSuccess (Core.CLI.Translate ("cli.success.settings.import.full"),
					             Core.CLI.Translate ("cli.success.settings.import.short"));
				} catch (Exception e)
				{
					ShowError (Core.CLI.Translate ("cli.errors.happened", e.ToString ()));
				}
			} else
			{
				ShowError (path == "null"
					           ? Core.CLI.Translate ("cli.errors.export.null")
					           : Core.CLI.Translate ("messages.file.notexist.full2"));
			}
		}

		internal void PrintSettings ()
		{
			Settings.connectAndGet ();
			SortedDictionary <int, string> dict = Settings.PrepareAll;
			Console.WriteLine ();
			foreach (KeyValuePair <int, string> pair in dict)
			{
				Console.WriteLine ($"{pair.Key} => {pair.Value}");
			}

			Console.WriteLine ();
		}

		private void ShowError (string   text) => Program.program.ShowError (text);
		private void ShowSuccess (string text) => Program.program.ShowError (text);
		private void ShowSuccess (string body, string title) => Program.program.ShowError (body, title);
	}
}