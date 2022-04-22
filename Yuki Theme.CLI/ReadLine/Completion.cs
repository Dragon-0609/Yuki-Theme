using System;
using System.Collections.Generic;
using System.Linq;

namespace CLITools
{
	public class Completion : IAutoCompleteHandler
	{
		// characters to start completion from
		public char [] Separators { get; set; }

		private  Dictionary <string, string []> commands;
		internal Dictionary <string, string []> themes;

		private string [] defaultCompletion;

		// text - The current text entered in the console
		// index - The index of the terminal cursor within {text}
		public string [] GetSuggestions (string text, int index)
		{
			string [] clx;
			if (CheckDictionary (text, commands, false, out string [] strings)) return strings;
			if (CheckDictionary (text, themes, true, out string [] strinv)) return strinv;

			clx = ChangeCase (text, SortBySimilarity (text, defaultCompletion, false));
			return clx;
		}

		private bool CheckDictionary (string text, Dictionary <string, string []> comms, bool keepCase, out string [] strings)
		{
			foreach (KeyValuePair <string, string []> pair in comms)
			{
				if (text.ToLower ().StartsWith (pair.Key + " "))
				{
					string [] clx;
					if (keepCase)
						clx = SortBySimilarity (text.Substring (pair.Key.Length + 1), pair.Value, true);
					else
						clx = ChangeCase (text, SortBySimilarity (text.Substring (pair.Key.Length + 1), pair.Value, false));
					
					{
						strings = clx;
						return true;
					}
				}
			}

			strings = null;
			return false;
		}

		private static string [] ChangeCase (string key, string [] value)
		{
			string [] clx = value;
			if (key.Length >= 1)
			{
				if (key.All (char.IsUpper))
				{
					clx = new string[value.Length];
					for (var i = 0; i < value.Length; i++)
					{
						clx [i] = value [i].ToUpper ();
					}
				} else if (key.Length >= 2 && char.IsLetter (key [0]) && char.IsUpper (key [0]) && char.IsLower (key [1]) &&
				           char.IsLetter (key [1]))
				{
					clx = new string[value.Length];
					clx [0] = value [0].ToUpper ();
					for (var i = 1; i < value.Length; i++)
					{
						clx [i] = value [i];
					}
				}
			}

			return clx;
		}


		public Completion ()
		{
			Separators = new [] { ' ', '.', '/' };
			commands = new Dictionary <string, string []>
			{
				{ "features", new [] { "export", "import", "print", "update" } },
				{ "settings", new [] { "--path", "--quiet", "--mode", "--action" } }
			};
			defaultCompletion = new []
			{
				"copy", "list", "clear", "fields", "allfields", "export", "import", "delete", "rename", "settings", "edit", "features"
			};
			themes = new Dictionary <string, string []> ();
		}

		private string [] SortBySimilarity (string target, string [] reference, bool keepCase)
		{
			if (target.Length == 0) return reference;
			if (!keepCase)
				target = target.ToLower ();
			string [] result = reference.OrderBy (m => m.StartsWith (target) ? 0 : 1).ThenBy (m => m).ToArray ();
			return result;
		}
	}
}