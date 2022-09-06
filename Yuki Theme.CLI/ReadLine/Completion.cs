using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CLITools
{
	public class Completion : IAutoCompleteHandler
	{
		// characters to start completion from
		public char [] Separators { get; set; }

		internal Dictionary <string, string []> commands;
		internal Dictionary <string, string []> themes;

		internal string [] definitions;

		private string [] defaultCompletion;

		// text - The current text entered in the console
		// index - The index of the terminal cursor within {text}
		public string [] GetSuggestions (string text, int index)
		{
			if (text.StartsWith (" ")) text = text.TrimStart ();
			
			string [] clx = null;
			
			if (CheckDictionary (text, commands, false, out string [] strings)) return strings;
			
			if (CheckDictionary (text, themes, true, out string [] strinv)) return strinv;

			if (definitions.Length != 0)
			{
				if (CheckByRegex (text, definitions, @"^edit\s\w*\s-d\s", out string [] strinr)) return strinr;
			}

			if (CheckByRegex (text, new [] { "-d" }, @"^edit\s\w*\s", out string [] strinh)) return strinh;
			
			if (text.Length == 0 || !text.Contains (" "))
				clx = ChangeCase (text, SortBySimilarity (text, defaultCompletion, false));

			return clx;
		}

		private bool CheckDictionary (string text, Dictionary <string, string []> comms, bool keepCase, out string [] strings)
		{
			foreach (KeyValuePair <string, string []> pair in comms)
			{
				string lower = text.ToLower ();
				if (lower.StartsWith (pair.Key + " "))
				{
					string [] clx;
					if (keepCase)
						clx = SortBySimilarity (text.Substring (pair.Key.Length + 1), pair.Value, true);
					else
						clx = ChangeCase (text, SortBySimilarity (text.Substring (pair.Key.Length + 1), pair.Value, false));
					
					if (clx.All(cx => Regex.Matches(lower, cx, RegexOptions.IgnoreCase).Count == 0))
					{
						strings = clx;
						return true;
					}

					break;
				}
			}

			strings = null;
			return false;
		}

		private bool CheckByRegex (string text, string[] values, string regex, out string[] strinr)
		{
			Match match = Regex.Match (text, regex, RegexOptions.Singleline);
			if (match.Success)
			{
				if (values.All(cx => Regex.Matches(text, cx, RegexOptions.IgnoreCase).Count == 0))
				{
					strinr = SortBySimilarity (text.Substring (match.Value.Length), values, false);
					return true;
				}				
			}
			strinr = null;
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
				{ "features edit if token", new [] { "true", "false" } },
				{ "features edit if group", new [] { "null" } },
				{ "features edit if", new [] { "group", "token" } },
				{ "features edit", new [] { "if" } },
				{ "features", new [] { "export", "import", "print", "update", "edit" } },
				{ "settings", new [] { "--path", "--quiet", "--mode", "--action", "--language" } },
				{ "list", new [] { "--group" } }
			};
			
			defaultCompletion = new []
			{
				"copy", "list", "clear", "fields", "allfields", "export", "import", "delete", "rename", "settings", "edit", "features"
			};
			
			themes = new Dictionary <string, string []> ();
			definitions = new string[0];
		}

		private string [] SortBySimilarity (string target, string [] reference, bool keepCase)
		{
			if (target.Length == 0) return reference;
			if (!keepCase)
				target = target.ToLower ();
			string[] result;
			
			if (!keepCase)
				result = reference.OrderBy (m => m.ToLower().StartsWith (target) ? 0 : 1).ThenBy (m => m.ToLower().Contains (target) ? 0 : 1)
			                            .ThenBy (m => m).ToArray ();
			else
				result = reference.OrderBy (m => m.StartsWith (target) ? 0 : 1).ThenBy (m => m.Contains (target) ? 0 : 1)
						 .ThenBy (m => m).ToArray ();
			return result;
		}
	}
}