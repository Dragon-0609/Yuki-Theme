using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Parsers;

namespace Yuki_Theme.CLI
{
	class MainClass
	{
		private static bool quit = false;
		
		#region Other Methods

		private static void AskPath (string content, string title)
		{
			Console.WriteLine ($"{title}:\n {content}");
			bool isPath = false;
			string pth = "";
			while(!isPath)
			{
				string path = Console.ReadLine ();
				if (path.ToLower () == "exit")
				{
					break;
				}else if (Directory.Exists (path))
				{
					if (Directory.Exists (System.IO.Path.Combine (path, "Highlighting")))
					{
						pth = path;
						isPath = true;
					} else
					{
						ShowError (
							$"{path} isn't PascalABC.NET directory. Select Path to the PascalABC.NET directory");
					}
				} else
				{
					ShowError ("The directory isn't exist");
				}
			}

			if (isPath)
			{
				Core.CLI.pascalPath = pth;
				Core.CLI.saveData ();
			}
		}
		
		private static void GiveMessage (string content, string title)
		{
			Console.WriteLine ($"{title}:\n {content}");
		}
		
		private static void ShowSuccess (string content, string title)
		{
			ConsoleColor clr = Console.ForegroundColor; 
			Console.ForegroundColor = ConsoleColor.Green;
			GiveMessage (content, title);
			Console.ForegroundColor = clr;
		}
		
		private static void ShowInvertSuccess (string content, string title)
		{
			ShowSuccess (title, content);
		}

		private static Image LoadImage ()
		{
			Image res = null;
			if(Core.CLI.isDefault ())
			{
				Tuple <bool, string> content = Helper.getThemeFromMemory (Core.CLI.gp, Core.CLI.GetCore ());
				if (content.Item1)
				{
					Tuple <bool, Image> iag = Helper.getImageFromMemory (Core.CLI.gp, Core.CLI.GetCore ());
					if (iag.Item1)
					{
						res = iag.Item2;
					}


				}
			} else
			{
				Tuple <bool, string> contents = Helper.getTheme (Core.CLI.getPath);
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.getImage (Core.CLI.getPath);
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			}

			return res;
		}

		private static Image LoadSticker ()
		{
			Image res = null;
			if(Core.CLI.isDefault ())
			{
				Tuple <bool, string> content = Helper.getThemeFromMemory (Core.CLI.gp, Core.CLI.GetCore ());
				if (content.Item1)
				{
					Tuple <bool, Image> iag = Helper.getStickerFromMemory (Core.CLI.gp, Core.CLI.GetCore ());
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			} else
			{
				Tuple <bool, string> contents = Helper.getTheme (Core.CLI.getPath);
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.getSticker (Core.CLI.getPath);
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			}
			return res;
		}

		private static bool Contains (string st)
		{
			return Core.CLI.schemes.Contains (st);
		}

		private static string ConvertToText (string str)
		{
			string st = str;
			if ((st.StartsWith ("\"") && st.EndsWith ("\"")) || (st.StartsWith ("'") && st.EndsWith ("'")))
			{
				st = str.Substring (1, str.Length - 2);
			}

			return st;
		}

		private static bool AskToDelete (string st, string st2)
		{
			GiveMessage (st + " { yes|y or no|n }", st2);
			string res = Console.ReadLine ().ToLower ();
			bool ans = false;
			bool err = false;
			if (res.StartsWith ("y"))
			{
				if (res.Length == 1) ans = true;
				else if (res.Length == 3)
				{
					err = res != "yes";
					ans = !err;
				} else
					err = true;
			} else if (res.StartsWith ("n"))
			{
				if (res.Length == 1) { } else if (res.Length == 2)
				{
					err = res != "no";
				} else
					err = true;
			} else
			{
				err = true;
			}

			if (err)
			{
				ShowError ("Wrong input. Acceptable values: yes or no");
			}

			return ans;
		}
		
		private static void AfterDelete (string content, object obj)
		{
			ShowError ($@"{content} has been successfully deleted.");
		}

		private static bool isNull (string st)
		{
			return st == null || st.Length == 0;
		}

		private static bool TryToParseMode (string st, out bool ret)
		{
			ret = false;
			return false;
		}
		
		#endregion

		/// <summary>
		/// Load CLI, to work with CLI. For example, get settings and load themes. After that, you can process the themes
		/// </summary>
		public static void LoadCLI ()
		{
			if (Helper.mode != ProductMode.CLI) // Check if we loaded CLI early. If not load it
			{
				Helper.mode = ProductMode.CLI;
				Core.CLI.setPath = AskPath;
				Core.CLI.showSuccess = ShowSuccess;
				Core.CLI.showError = ShowError;
				Core.CLI.showError = ShowError;
				Core.CLI.onRename = ShowInvertSuccess;
				Core.CLI.SaveInExport = AskToDelete;
				Core.CLI.connectAndGet ();
				Core.CLI.load_schemes ();
			}
		}

		/// <summary>
		/// Set current theme to process it. For example export it.
		/// </summary>
		/// <param name="theme">Theme name</param>
		public static void SetFile (string theme)
		{
			Core.CLI.currentoFile = theme;
			Core.CLI.currentFile = Helper.ConvertNameToPath (theme);
		}

		/// <summary>
		/// If something went wrong, here we can Write it to console with Red Color.
		/// </summary>
		/// <param name="str">Content</param>
		/// <param name="str2">Title</param>
		public static void ShowError (string str, string str2)
		{
			ShowError ($"{str2}:\n {str}");
		}

		/// <summary>
		/// If something went wrong, here we can Write it to console with Red Color.
		/// </summary>
		/// <param name="str">Content</param>
		public static void ShowError (string str)
		{
			ConsoleColor clr = Console.ForegroundColor; 
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine (str);
			Console.ForegroundColor = clr;
		}

		public static void Main (string[] args)
		{
			
			var parser = new Parser (parserSettings =>
			{
				parserSettings.AutoHelp = true;
				parserSettings.AutoVersion = true;
				parserSettings.CaseInsensitiveEnumValues = false;
				parserSettings.CaseSensitive = true;
				parserSettings.EnableDashDash = true;
				parserSettings.IgnoreUnknownArguments = false;
			});

			if (args != null && args.Length > 0)
			{
				Parse (parser, args);
			} else
			{
				ShowError ("Loop mode is activated. To exit write 'QUIT'.\n".ToUpper ());
				while (!quit)
				{
					Console.Write ("yuki >");
					string command = Console.ReadLine ();
					if (command.ToLower ().Contains ("quit")) break;
					Regex argReg = new Regex (@"\w+|""[\w\s]*""");
					string [] cmds = new string[argReg.Matches (command).Count];
					int i = 0;
					foreach (var enumer in argReg.Matches (command))
					{
						cmds [i] = (string) enumer.ToString ();
						i++;
					}

					Parse (parser, cmds);
				}
			}
		}

		/// <summary>
		/// Parse commands.
		/// </summary>
		/// <param name="parser">Parser</param>
		/// <param name="args">Arguments of commands</param>
		private static void Parse (Parser parser, string[] args)
		{
			var res = parser
				.ParseArguments <CopyCommand, ListCommand, ExportCommand, ImportCommand, DeleteCommand,
					RenameCommand,
					SettingsCommand> (args);

			res.WithParsed <CopyCommand> (o =>
			   {
				   string fr = ConvertToText (o.Names.ElementAt (0));
				   string to = ConvertToText (o.Names.ElementAt (1));
				   
				   if (fr.Length > 0 && to.Length > 0)
				   {
					   if (fr != to)
					   {
						   LoadCLI ();
						   Core.CLI.add (fr, to);
					   } else
					   {
						   ShowError ("You didn't change the name", "Canceled");
					   }
				   } else
				   {
					   ShowError ("Invalid name. You must enter at least 1 character", "Invalid name");
				   }
			   }).WithParsed <ListCommand> (o =>
			   {
				   LoadCLI ();
				   Console.WriteLine ("Theme list:");
				   foreach (string scheme in Core.CLI.schemes)
				   {
					   Console.WriteLine (scheme);
				   }
			   }).WithParsed <ExportCommand> (o =>
			   {
				   bool showerror = false;
				   if(o.Name !=null)
				   {
					   o.Name = ConvertToText (o.Name);
					   LoadCLI ();
					   if (Contains (o.Name))
					   {
						   SetFile (o.Name);
						   Image img = LoadImage ();
						   Image stick = LoadSticker ();

						   Core.CLI.export (img, stick);
						   img = null;
						   stick = null;
						   GC.Collect ();
					   } else
					   {
						   ShowError (
							   $"'{o.Name}' isn't in the themes. Please, write 'yuki list' to get all themes' names");
					   }
				   }
				   else
				   {
					   showerror = true;
				   }

				   if (showerror)
				   {
					   ShowError (
						   $"Can't export '{o.Name}'.");
				   }
			   }).WithParsed <ImportCommand> (o =>
			   {
				   bool showerror = false;
				   if(o.Path !=null)
				   {
					   if (o.Path.Contains (".yukitheme") || o.Path.Contains (".icls") || o.Path.Contains (".json"))
					   {
						   LoadCLI ();
						   Core.CLI.import (o.Path);
					   }
					   else
						   showerror = true;
				   }
				   else
				   {
					   showerror = true;
				   }

				   if (showerror)
				   {
					   ShowError (
						   $"Can't import '{o.Path}'. The acceptable formats: '.yukitheme', '.icls', '.json'");
				   }
			   }).WithParsed <DeleteCommand> (o =>
			   {
				   bool showerror = false;
				   if (o.Name != null)
				   {
					   LoadCLI ();
					   o.Name = ConvertToText (o.Name);
					   if (Contains (o.Name))
						   Core.CLI.remove (o.Name, AskToDelete, null, AfterDelete);
					   else
					   {
						   ShowError (
							   $"'{o.Name}' isn't in the themes. Please, write 'yuki list' to get all themes' names");
					   }
				   } else
				   {
					   showerror = true;
				   }

				   if (showerror)
				   {
					   ShowError (
						   $"Can't delete '{o.Name}'.");
				   }
			   }).WithParsed <RenameCommand> (o =>
			   {
				   LoadCLI ();
				   string fr = ConvertToText (o.Names.ElementAt (0));
				   string to = ConvertToText (o.Names.ElementAt (1));

				   if (fr.Length > 0 && to.Length > 0)
				   {
					   if (fr != to)
					   {
						   Core.CLI.rename (fr, to);
					   } else
					   {
						   ShowError ("You didn't change the name", "Canceled");
					   }
				   } else
				   {
					   ShowError ("Invalid name. You must enter at least 1 character", "Invalid name");
				   }
			   }).WithParsed <SettingsCommand> (o =>
			   {
				   LoadCLI ();
				   bool changed = false;
				   if (!isNull (o.Path))
				   {
					   if (Directory.Exists (System.IO.Path.Combine (o.Path, "Highlighting")))
					   {
						   Core.CLI.pascalPath = o.Path;
						   changed = true;
					   } else
					   {
						   ShowError (
							   $"{o.Path} isn't PascalABC.NET directory. Select Path to the PascalABC.NET directory");
					   }   
				   }
				   if (!isNull (o.Quiet))
				   {
					   bool resa = false;
					   if (bool.TryParse (o.Quiet, out resa))
					   {
						   Core.CLI.askChoice = resa;
						   changed = true;
					   } else
					   {
						   ShowError ("Wrong input. Acceptable values: 'true' or 'false'", "Quiet: ");
					   }
				   }
				   if (!isNull (o.Mode))
				   {
					   bool isValid = false;
					   switch (o.Mode.ToLower ())
					   {
						   case "light" :
						   {
							   isValid = true;
						   }
							   break;
						   case "advanced" :
						   {
							   isValid = true;
						   }
							   break;

					   }

					   if (isValid)
					   {
						   Core.CLI.settingMode = o.Mode.ToLower () == "light" ? 0 : 1;
						   changed = true;
					   } else
					   {
						   ShowError ("Invalid input! Acceptable values: 'LIGHT' or 'ADVANCED'");
					   }
				   }
				   if (!isNull (o.Action))
				   {
					   bool isValid = false;
					   int act = 0;
					   switch (o.Action.ToLower ())
					   {
						   case "delete" :
						   {
							   isValid = true;
							   act = 0;
						   }
							   break;
						   case "import" :
						   {
							   isValid = true;
							   act = 1;
						   }
							   break;
						   case "ignore" :
						   {
							   isValid = true;
							   act = 2;
						   }
							   break;

					   }

					   if (isValid)
					   {
						   Core.CLI.settingMode = act;
						   changed = true;
					   } else
					   {
						   ShowError ("Invalid input! Acceptable values: 'DELETE', 'IMPORT' or 'IGNORE'");
					   }
				   }

				   if (changed)
				   {
					   Core.CLI.saveData ();
					   ShowSuccess ("Settings are saved!", "Saved");
				   } else
				   {
					   ShowError ("Settings aren't saved!", "Not saved");
				   }
			   })
			   .WithNotParsed (errors =>
			   {
				   HelpText helpText = null;
				   if (errors.IsHelp () || errors.IsVersion ())
				   {
					   helpText = HelpText.AutoBuild (res);
					   Console.WriteLine (helpText);
				   } else
				   {
					   foreach (var error in errors)
						   switch (error)
						   {
							   case BadVerbSelectedError badVerbSelectedError :
								   ShowError ($"There is no \"{badVerbSelectedError.Token}\" subcommand.");
								   break;

							   case UnknownOptionError unknownOptionError :
								   ShowError ($"There is no \"{unknownOptionError.Token}\" option.");
								   break;

							   case SequenceOutOfRangeError sequenceOutOfRange :
								   ShowError ("Length of values are invalid. There must be 2 values");
								   break;

							   case MissingValueOptionError missingValueOptionError :
								   helpText = HelpText.AutoBuild (res);
								   Console.WriteLine (helpText);
								   break;
							   // Handler other appropriate exceptions downhere.
							   default :
								   ShowError ($"The {error} happened.");
								   break;
						   }
				   }
			   });
		}
	}
}
