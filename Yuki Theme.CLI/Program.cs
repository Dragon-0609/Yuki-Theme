using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Parsers;

namespace Yuki_Theme.CLI
{
	class MainClass
	{
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
						Console.WriteLine (
							"It isn't PascalABC.NET directory. Select Path to the PascalABC.NET directory");
					}
				} else
				{
					Console.WriteLine ("The directory isn't exist");
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

		public static void LoadCLI ()
		{
			if (Helper.mode != ProductMode.CLI)
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

		public static void SetFile (string file)
		{
			Core.CLI.currentoFile = file;
			Core.CLI.currentFile = Helper.ConvertNameToPath (file);
		}

		public static void ShowError (string str, string str2)
		{
			ShowError ($"{str2}:\n {str}");
		}

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

			var res = parser
				.ParseArguments <DuplicateCommand, ListCommand, ExportCommand, ImportCommand, DeleteCommand,
					RenameCommand,
					SettingsCommand> (args);

			res.WithParsed <DuplicateCommand> (o =>
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
					   if (Contains (o.Name))
					   {
						   LoadCLI ();
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
				   if (!isNull (o.Path))
				   {
					   Console.WriteLine ("Path: " + o.Path);
				   }
				   else if (!isNull (o.Quiet))
				   {
					   bool resa = false;
					   if (bool.TryParse (o.Quiet, out resa))
					   {
						   Console.WriteLine ("Quiet: " + resa);
					   } else
					   {
						   ShowError ("Wrong input. Acceptable values: 'true' or 'false'", "Quiet: ");
					   }
				   } else if (!isNull (o.Mode))
				   {
					   Console.WriteLine ("Mode: " + o.Mode);
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
