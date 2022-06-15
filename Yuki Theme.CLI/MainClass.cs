using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CLITools;
using CommandLine;
using CommandLine.Text;
using Microsoft.Win32;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.CLI
{
	class MainClass
	{
		internal      bool quit      = false;
		internal      bool loop      = false;
		private const int  MAX_WIDTH = 80;

		private Dictionary <string, string> ToReplaceTranslationWithVariable = new ();

		internal Completion completion;

		/// <summary>
		/// Load CLI, to work with CLI. For example, get settings and load themes. After that, you can process the themes
		/// </summary>
		internal void LoadCLI (bool refreshSchemes)
		{
			if (Helper.mode != ProductMode.CLI) // Check if we loaded CLI early. If not load it
			{
				Helper.mode = ProductMode.CLI;
				CLI_Actions.setPath = AskPath;
				CLI_Actions.showSuccess = ShowSuccess;
				CLI_Actions.showError = ShowError;
				CLI_Actions.showError = ShowError;
				CLI_Actions.onRename = ShowInvertSuccess;
				CLI_Actions.SaveInExport = AskToDelete;
				Settings.connectAndGet ();
				Settings.settingMode = SettingMode.Light;
				Settings.saveAsOld = true;
				Core.CLI.load_schemes ();
				AddThemeCompletion ();
				AddDefinitionCompletion ();
				AddLanguageCompletion ();
			} else if (refreshSchemes)
			{
				Core.CLI.load_schemes ();
				AddThemeCompletion ();
			}
		}

		private void AddThemeCompletion ()
		{
			completion.themes.Clear ();
			string [] themes = Core.CLI.schemes.ToArray ();
			string [] commands = new [] { "copy", "export", "delete", "rename", "edit" };
			foreach (string command in commands)
			{
				completion.themes.Add (command, themes);
			}
		}

		private void AddDefinitionCompletion ()
		{
			List <string> list = new List <string> ();
			foreach (KeyValuePair <string, string> pair in Populater.changedNames)
			{
				list.Add (pair.Value);
			}

			completion.definitions = list.ToArray ();
		}

		private void AddLanguageCompletion ()
		{
			completion.commands = new Dictionary <string, string []>
			                      {
				                      { "settings --language", Settings.translation.GetShortLanguageNames },
				                      { "settings -l", Settings.translation.GetShortLanguageNames }
			                      }
			                      .Concat (completion.commands).ToDictionary (k => k.Key, v => v.Value);
			string valueToReplace = string.Join (", ", Settings.translation.GetShortLanguageNames);
			ToReplaceTranslationWithVariable.Add ("cli.help.settings.language", valueToReplace);
		}

		/// <summary>
		/// Set current theme to process it. For example export it.
		/// </summary>
		/// <param name="theme">Theme name</param>
		public void SetFile (string theme)
		{
			Core.CLI.SelectTheme (theme);
		}

		internal void ShowLoopMessage ()
		{
			if (loop)
				ShowError (Core.CLI.Translate ("cli.errors.loop").ToUpper ());
		}

		internal string [] ParseArguments (string commandLine)
		{
			char [] parmChars = commandLine.ToCharArray ();
			bool inQuote = false;
			for (int index = 0; index < parmChars.Length; index++)
			{
				if (parmChars [index] == '"')
					inQuote = !inQuote;
				if (!inQuote && parmChars [index] == ' ')
					parmChars [index] = '\n';
			}

			string[] arguments = (new string (parmChars)).Split ('\n');
			arguments [0] = arguments [0].ToLower ();
			return arguments;
		}

		/// <summary>
		/// Parse commands.
		/// </summary>
		/// <param name="parser">Parser</param>
		/// <param name="args">Arguments of commands</param>
		internal void Parse (Parser parser, string [] args)
		{
			var res = parser
				.ParseArguments <CopyCommand, ListCommand, ClearCommand, FieldsCommand, AllFieldsCommand, ExportCommand, ImportCommand,
					DeleteCommand,
					RenameCommand, SettingsCommand, EditCommand, FeatureCommand> (args);

			res.WithParsed <CopyCommand> (o =>
			   {
				   CopyC (o);
			   }).WithParsed <ListCommand> (o =>
			   {
				   ListC (o.ShowGroups);
			   }).WithParsed <ClearCommand> (o =>
			   {
				   ClearC ();
			   }).WithParsed <FieldsCommand> (o =>
			   {
				   FieldsC ();
			   }).WithParsed <AllFieldsCommand> (o =>
			   {
				   AllFieldsC ();
			   }).WithParsed <ExportCommand> (o =>
			   {
				   ExportC (o);
			   }).WithParsed <ImportCommand> (o =>
			   {
				   ImportC (o);
			   }).WithParsed <DeleteCommand> (o =>
			   {
				   DeleteC (o);
			   }).WithParsed <RenameCommand> (o =>
			   {
				   RenameC (o);
			   }).WithParsed <SettingsCommand> (o =>
			   {
				   SettingC (o);
			   }).WithParsed <EditCommand> (o =>
			   {
				   EditC (o);
			   }).WithParsed <FeatureCommand> (o =>
			   {
				   FeaturesC (o);
			   })
			   .WithNotParsed (errors =>
			   {
				   HelpText helpText = null;
				   if (errors.IsHelp () || errors.IsVersion ())
				   {
					   helpText = HelpText.AutoBuild (res);
					   Console.WriteLine (Translation (helpText.ToString ()));
					   // Console.WriteLine ( helpText.ToString ());
				   } else
				   {
					   foreach (var error in errors)
						   switch (error)
						   {
							   case BadVerbSelectedError badVerbSelectedError :
								   ShowError (Core.CLI.Translate ("cli.errors.nocommand", badVerbSelectedError.Token));
								   break;

							   case UnknownOptionError unknownOptionError :
								   ShowError (Core.CLI.Translate ("cli.errors.nooption", unknownOptionError.Token));
								   break;

							   case SequenceOutOfRangeError sequenceOutOfRange :
								   ShowError (sequenceOutOfRange.ToString ());
								   // ShowError (Core.CLI.Translate ("cli.errors.length"));
								   break;

							   case MissingValueOptionError missingValueOptionError :
								   helpText = HelpText.AutoBuild (res);
								   Console.WriteLine (Translation (helpText.ToString ()));
								   break;
							   // Handler other appropriate exceptions downhere.
							   default :
								   ShowError (Core.CLI.Translate ("cli.errors.happened", error.ToString ()));
								   break;
						   }
				   }
			   });
		}

		#region Commands

		private void CopyC (CopyCommand o)
		{
			string fr = ConvertToText (o.Names.ElementAt (0));
			string to = ConvertToText (o.Names.ElementAt (1));

			if (fr.Length > 1 && to.Length > 1)
			{
				if (fr != to)
				{
					LoadCLI (true);
					if (Contains (fr))
					{
						if (!Contains (to))
						{
							Core.CLI.add (fr, to);
						} else
						{
							ShowError (Core.CLI.Translate ("messages.name.exist.full"));
						}
					} else
					{
						ShowError (Core.CLI.Translate ("cli.errors.notinthemes", fr));
					}
				} else
				{
					ShowError (Core.CLI.Translate ("messages.name.notchanged"), Core.CLI.Translate ("download.canceled.title"));
				}
			} else
			{
				ShowError (Core.CLI.Translate ("messages.name.invalid"), Core.CLI.Translate ("messages.name.invalid.short"));
			}
		}

		private void ListC (bool groups)
		{
			LoadCLI (true);
			Console.WriteLine ("Theme list:");
			if (groups)
			{
				foreach (string scheme in Core.CLI.schemes)
				{
					Console.WriteLine ($"{Core.CLI.ThemeInfos [scheme].group}:  {scheme}");
				}
			}else
			{
				foreach (string scheme in Core.CLI.schemes)
				{
					Console.WriteLine (scheme);
				}
			}
		}

		private void ClearC ()
		{
			Console.Clear ();
			ShowLoopMessage ();
		}

		private void FieldsC ()
		{
			LoadCLI (true);
			SetFile ("Darcula");
			Core.CLI.restore ();
			Console.WriteLine ($"There're {Core.CLI.names.Count} fields:");
			foreach (string name in Core.CLI.names)
			{
				Console.WriteLine ("\t" + Populater.getChangedName (name));
			}

			SetFile ("N|L"); // Set to the default value
		}

		private void AllFieldsC ()
		{
			LoadCLI (true);
			Settings.settingMode = SettingMode.Advanced;
			SetFile ("Darcula");
			Core.CLI.restore ();
			Console.WriteLine ($"There're {Core.CLI.currentTheme.Fields.Keys.Count} fields:");
			foreach (string name in Core.CLI.currentTheme.Fields.Keys)
			{
				Console.WriteLine ("\t" + name);
			}

			SetFile ("N|L"); // Set to the default value
		}

		private void ExportC (ExportCommand o)
		{
			bool showerror = false;
			if (o.Name != null)
			{
				o.Name = ConvertToText (o.Name);
				LoadCLI (true);
				if (Contains (o.Name))
				{
					SetFile (o.Name);

					Core.CLI.export (null, null, null, null, true);
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.CLI.Translate ("cli.errors.cantexport", o.Name));
			}
		}

		private void ImportC (ImportCommand o)
		{
			bool showerror = false;
			if (o.Path != null)
			{
				if (o.Path.Contains (".yukitheme") || o.Path.Contains (".icls") || o.Path.Contains (".json"))
				{
					LoadCLI (true);
					Core.CLI.import (o.Path, AskToDelete);
				} else
					showerror = true;
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.CLI.Translate ("cli.errors.cantimport", o.Path));
			}
		}

		private void DeleteC (DeleteCommand o)
		{
			bool showerror = false;
			if (o.Name != null)
			{
				LoadCLI (true);
				o.Name = ConvertToText (o.Name);
				if (Contains (o.Name))
					Core.CLI.remove (o.Name, AskToDelete, null, AfterDelete);
				else
				{
					ShowError (Core.CLI.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.CLI.Translate ("cli.errors.cantdelete", o.Name));
			}
		}

		private void RenameC (RenameCommand o)
		{
			LoadCLI (true);
			string fr = ConvertToText (o.Names.ElementAt (0));
			string to = ConvertToText (o.Names.ElementAt (1));
			if (fr.Length > 1 && to.Length > 1)
			{
				if (fr != to)
				{
					Core.CLI.rename (fr, to);
				} else
				{
					ShowError (Core.CLI.Translate ("messages.name.notchanged"), Core.CLI.Translate ("download.canceled.title"));
				}
			} else
			{
				ShowError (Core.CLI.Translate ("messages.name.invalid"), Core.CLI.Translate ("messages.name.invalid.short"));
			}
		}

		private void SettingC (SettingsCommand o)
		{
			LoadCLI (true);
			bool changed = false;
			if (!isNull (o.Path))
			{
				if (Directory.Exists (Path.Combine (o.Path, "Highlighting")))
				{
					Settings.pascalPath = o.Path;
					changed = true;
				} else
				{
					ShowError (Core.CLI.Translate ("messages.path.wrong"));
				}
			}

			if (!isNull (o.Quiet))
			{
				bool resa = false;
				if (bool.TryParse (o.Quiet, out resa))
				{
					Settings.askChoice = resa;
					changed = true;
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.quiet.wrong.full"), Core.CLI.Translate ("cli.errors.quiet.wrong.short"));
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
					Settings.settingMode = o.Mode.ToLower () == "light" ? SettingMode.Light : SettingMode.Advanced;
					changed = true;
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.mode.invalid"));
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
					Settings.actionChoice = act;
					changed = true;
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.action.invalid"));
				}
			}
			
			if (!isNull (o.Language))
			{
				string lower = o.Language.ToLower();
				if (Settings.translation.ContainsLanguageISO2 (lower))
				{
					Settings.localization = lower;
					Settings.translation.LoadLocale (lower);
					changed = true;
				} else
				{
					string languages = string.Join (", ", Settings.translation.GetShortLanguageNames);
					ShowError (Core.CLI.Translate ("cli.errors.language.invalid", languages));
				}
			}
			

			if (changed)
			{
				Settings.SaveData ();
				ShowSuccess ("Settings are saved!", "Saved");
			} else
			{
				ShowError (Core.CLI.Translate ("cli.errors.settings.notsaved.full"),
				           Core.CLI.Translate ("cli.errors.settings.notsaved.short"));
			}
		}

		private void EditC (EditCommand o)
		{
			bool showerror = false;
			if (o.Name != null)
			{
				o.Name = ConvertToText (o.Name);
				LoadCLI (true);
				if (Contains (o.Name))
				{
					if (!Core.CLI.ThemeInfos [o.Name].isDefault)
					{
						SetFile (o.Name);
						Core.CLI.restore ();
						if (o.Definition != null)
						{
							bool color = false;
							bool error = false;
							string err_txt = "";
							try
							{
								color = isColor (o.Definition);
							} catch (ArgumentException e)
							{
								error = true;
								err_txt = e.Message;
							}

							if (!error)
							{
								if (color)
								{
									bool bg = false;
									bool txt = false;
									Color bgcolor = Color.Empty;
									Color txtcolor = Color.Empty;

									if (o.Background != null)
									{
										bgcolor = ColorTranslator.FromHtml (o.Background);
										bg = true;
									}

									if (o.Text != null)
									{
										txtcolor = ColorTranslator.FromHtml (o.Text);
										txt = true;
									}

									if (bg || txt)
									{
										o.Definition = Populater.getNormalizedName (o.Definition);
										if (Core.CLI.currentTheme.Fields.ContainsKey (o.Definition))
										{
											ThemeField dic = Core.CLI.currentTheme.Fields [o.Definition];
											SetColorsToField (ref dic, txt, txtcolor, bg, bgcolor);
											foreach (KeyValuePair <string, string> keyValuePair in dic.GetAttributes ())
											{
												GiveMessage (keyValuePair.Value, keyValuePair.Key);
											}
										}

										var sttr = Populater.getDependencies (o.Definition);
										if (sttr != null)
											foreach (var sr in sttr)
											{
												ThemeField dic = Core.CLI.currentTheme.Fields [sr];
												SetColorsToField (ref dic, txt, txtcolor, bg, bgcolor);
											}

										Core.CLI.save (null, null, true);
									} else
									{
										ShowError (Core.CLI.Translate ("cli.errors.parameter"));
									}
								} else
								{
									bool changed = false;
									if (o.Opacity != null)
									{
										if (o.Definition.ToLower () == "sticker")
											Core.CLI.currentTheme.StickerOpacity = int.Parse (o.Opacity);
										else
											Core.CLI.currentTheme.WallpaperOpacity = int.Parse (o.Opacity);
										changed = true;
									}

									if (o.Align != null && o.Definition.ToLower () == "image")
									{
										Alignment align = Alignment.Center;
										if (o.Align.ToLower () == "left") align = Alignment.Left;
										else if (o.Align.ToLower () == "center") align = Alignment.Center;
										else align = Alignment.Right;
										Core.CLI.currentTheme.WallpaperAlign = (int)align;
										changed = true;
									}

									if (changed)
										Core.CLI.save (null, null, true);
									SetImageLocation (o);
								}
							} else
							{
								ShowError (err_txt);
							}
						}
					} else
					{
						ShowError (Core.CLI.Translate ("colors.default.error.full"), Core.CLI.Translate ("colors.default.error.short"));
					}
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.CLI.Translate ("cli.errors.cantedit", o.Name));
			}
		}

		private void FeaturesC (FeatureCommand o)
		{
			string command = o.Commands.ElementAt (0).ToLower ();
			string path = "null";
			if (o.Commands.Count () == 2)
				path = o.Commands.ElementAt (1);

			if (command == "update")
				UpdateC (path);
			else if (command == "export")
			{
				SettingEditor editor = new SettingEditor();
				editor.ExportSettings (path);
			} else if (command == "import")
			{
				SettingEditor editor = new SettingEditor();
				editor.ImportSettings (path);
			} else if (command == "print")
			{
				SettingEditor editor = new SettingEditor();
				editor.PrintSettings ();
			} else if (command == "edit")
			{
				ConditionalEdition edition = new ConditionalEdition ();
				edition.Edit (o.Commands);
			}
		}

		#endregion


		#region CLI Features

		private void UpdateC (string path)
		{
			// Update from path
			if (path != "null")
			{
				if (path.StartsWith ("\"") && path.EndsWith ("\"")) path = path.Substring (1, path.Length - 2);
				if (path.Exist ())
				{
					File.Copy (
						path,
						Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
						              "yuki_theme.zip"), true);

					Assembly a = Core.CLI.GetCore ();
					int md = 0;
					bool found = false;
					while (!found)
					{
						string pth = Preparer.FileNamespace +
						             (md == 0 ? "files_program.txt" : "files_plugin.txt");
						Stream str = a.GetManifestResourceStream (pth);
						if (str != null)
						{
							using StreamReader reader = new StreamReader (str);
							string cx = reader.ReadLine ();
							string ph = Path.Combine (Core.CLI.currentPath, cx);
							Console.WriteLine (ph);
							if (ph.Exist ())
							{
								found = true;
								break;
							}
						}

						md++;
						if (md >= 2)
						{
							ShowError ("cli.errors.mode.notrecognized");
							string type = Console.ReadLine ();
							if (type.ToLower () == "program")
							{
								md = 0;
								found = true;
							} else if (type.ToLower () == "plugin")
							{
								md = 1;
								found = true;
							} else
							{
								md = 2;
								break;
							}
						}
					}

					if (found)
					{
						Helper.mode = md == 0 ? ProductMode.Program : ProductMode.Plugin;
						Preparer prep = new Preparer ();
						prep.prepare (false);
						RegistryKey ke =
							Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
						if (ke != null) ke.SetValue ("cli_update", "true");

						quit = true;
					} else
					{
						ShowError (Core.CLI.Translate ("cli.errors.find.app"));
					}
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.find.file"));
				}
			} else // Check Update
			{
			}
		}

		#endregion


		#region Other Methods

		private void AskPath (string content, string title)
		{
			Console.WriteLine ($"{title}:\n {content}");
			bool isPath = false;
			string pth = "";
			while (!isPath)
			{
				string path = Console.ReadLine ();
				if (path.ToLower () == "exit")
				{
					break;
				} else if (Directory.Exists (path))
				{
					if (Directory.Exists (System.IO.Path.Combine (path, "Highlighting")))
					{
						pth = path;
						isPath = true;
					} else
					{
						ShowError (Core.CLI.Translate ("messages.path.wrong"));
					}
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.directory.notexist"));
				}
			}

			if (isPath)
			{
				Settings.pascalPath = pth;
				Core.Settings.SaveData ();
			}
		}

		private void GiveMessage (string content, string title)
		{
			Console.WriteLine ($"{title}:\n {content}");
		}

		private void ShowSuccess (string content, string title)
		{
			ConsoleColor clr = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			GiveMessage (content, title);
			Console.ForegroundColor = clr;
		}

		private void ShowInvertSuccess (string content, string title)
		{
			ShowSuccess (title, content);
		}

		private bool Contains (string st)
		{
			return Core.CLI.schemes.Contains (st);
		}

		private string ConvertToText (string str)
		{
			string st = str;
			if ((st.StartsWith ("\"") && st.EndsWith ("\"")) || (st.StartsWith ("'") && st.EndsWith ("'")))
			{
				st = str.Substring (1, str.Length - 2);
			}

			return st;
		}

		private bool AskToDelete (string st, string st2)
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
				ShowError (Core.CLI.Translate ("cli.errors.input.wrong"));
			}

			return ans;
		}

		private void AfterDelete (string content, object obj)
		{
			ShowError (Core.CLI.Translate ("cli.message.deleted", content));
		}

		private bool isNull (string st)
		{
			return st == null || st.Length == 0;
		}

		private Image LoadImage ()
		{
			Image res = null;
			if (Core.CLI.isDefault ())
			{
				Assembly location;
				string pathToMemory;
				Tuple <bool, string> content = GetThemeFromMemory (out location, out pathToMemory);
				if (content.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathToMemory, location);
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			} else
			{
				Tuple <bool, string> contents = Helper.GetTheme (Core.CLI.pathToFile (Core.CLI.pathToLoad, true));
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetImage (Core.CLI.pathToFile (Core.CLI.pathToLoad, true));
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			}

			return res;
		}

		private Image LoadSticker ()
		{
			Image res = null;
			if (Core.CLI.isDefault ())
			{
				Assembly location;
				string pathToMemory;
				Tuple <bool, string> content = GetThemeFromMemory (out location, out pathToMemory);
				if (content.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetStickerFromMemory (pathToMemory, location);
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			} else
			{
				Tuple <bool, string> contents = Helper.GetTheme (Core.CLI.pathToFile (Core.CLI.pathToLoad, true));
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetSticker (Core.CLI.pathToFile (Core.CLI.pathToLoad, true));
					if (iag.Item1)
					{
						res = iag.Item2;
					}
				}
			}

			return res;
		}

		private Tuple <bool, string> GetThemeFromMemory (out Assembly location, out string pathToMemory)
		{
			IThemeHeader header = DefaultThemes.headers [Core.CLI.nameToLoad];
			string ext = Helper.GetThemeFormat (true, Core.CLI.pathToLoad, Core.CLI.nameToLoad) == ThemeFormat.Old
				? Helper.FILE_EXTENSTION_OLD
				: Helper.FILE_EXTENSTION_NEW;
			pathToMemory = $"{header}.{Core.CLI.pathToLoad}{ext}";
			location = header.Location;
			Tuple <bool, string> content = Helper.GetThemeFromMemory (pathToMemory, location);
			return content;
		}


		/// <summary>
		/// If something went wrong, here we can Write it to console with Red Color.
		/// </summary>
		/// <param name="str">Content</param>
		/// <param name="str2">Title</param>
		public void ShowError (string str, string str2)
		{
			ShowError ($"{str2}:\n {str}");
		}

		/// <summary>
		/// If something went wrong, here we can Write it to console with Red Color.
		/// </summary>
		/// <param name="str">Content</param>
		public void ShowError (string str)
		{
			ConsoleColor clr = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine (str);
			Console.ForegroundColor = clr;
		}


		private void SetImageLocation (EditCommand o)
		{
			if (o.Path != null)
			{
				bool img = false;
				bool stick = false;
				Image image = null;
				Image sticker = null;
				if (!File.Exists (o.Path))
				{
					ShowError (Core.CLI.Translate ("cli.errors.file.notexist"));
					return;
				}

				if (o.Definition.ToLower () == "image")
				{
					image = Image.FromFile (o.Path);
					sticker = LoadSticker ();
					img = true;
				} else
				{
					sticker = Image.FromFile (o.Path);
					image = LoadImage ();
					stick = true;
				}

				if (img || stick)
				{
					Core.CLI.save (image, sticker, false);
				} else
				{
					ShowError (Core.CLI.Translate ("cli.errors.strange"));
				}
			}
		}

		private void SetColorsToField (ref ThemeField dic, bool txt, Color txtcolor, bool bg, Color bgcolor)
		{
			if (dic.Foreground != null && txt)
				dic.Foreground = ColorTranslator.ToHtml (txtcolor);
			if (dic.Background != null && bg)
				dic.Background = ColorTranslator.ToHtml (bgcolor);
		}

		private bool isColor (string definition)
		{
			bool result = false;
			definition = definition.ToLower ();
			switch (definition)
			{
				case "default" :
				case "linenumber" :
				case "fold" :
				case "foldmarker" :
				case "selectedfold" :
				case "digit" :
				case "comment" :
				case "string" :
				case "keyword" :
				case "beginend" :
				case "punctuation" :
				case "operator" :
				case "constant" :
				case "selection" :
				case "vruler" :
				case "invalidlines" :
				case "caretmarker" :
				case "linenumbers" :
				case "foldline" :
				case "method" :
				case "markprevious" :
				case "selectedfoldline" :
				case "eolmarkers" :
				case "spacemarkers" :
				case "tabmarkers" :
				case "digits" :
				case "linebigcomment" :
				case "linecomment" :
				case "blockcomment" :
				case "blockcomment2" :
				case "keywords" :
				case "programsections" :
				case "special" :
				case "async" :
				case "accesskeywords1" :
				case "nonreserved1" :
				case "operatorkeywords" :
				case "selectionstatements" :
				case "iterationstatements" :
				case "exceptionhandlingstatements" :
				case "raisestatement" :
				case "jumpstatements" :
				case "jumpprocedures" :
				case "internalconstant" :
				case "internaltypes" :
				case "referencetypes" :
				case "modifiers" :
				case "accessmodifiers" :
				case "errorwords" :
				case "warningwords" :
				case "direcivenames" :
				case "specialdirecivenames" :
				case "direcivevalues" :
				{
					result = true;
				}
					break;

				case "image" :
				case "sticker" :
				{
					result = false;
				}
					break;

				default :
				{
					throw new ArgumentException ("Wrong value. To see acceptable value please write: \"yuki help edit\"");
				}
				break;
			}

			return result;
		}

		private string Translation (string help)
		{
			string res = help;
			Regex regex = new Regex ("cli.help(\n|\\S)*", RegexOptions.Multiline);
			MatchCollection matches = regex.Matches (res);
			
			
			string whitespace = "";

			foreach (Match match2 in matches)
			{
				Match matched2 = Regex.Match (res, "^.*?(?=" + match2.Value + ")", RegexOptions.Multiline);
				whitespace = new string (' ', matched2.Value.Length);
			}
			
			foreach (Match match in matches)
			{
				string translation = Core.CLI.Translate (match.Value);
				if (translation.Contains ("{0}") && ToReplaceTranslationWithVariable.ContainsKey (match.Value))
					translation = translation.Replace ("{0}", ToReplaceTranslationWithVariable [match.Value]);
				string [] cv = translation.Split ('\n');
				if (cv.Length > 1)
				{
					translation = "";
					int cvLength = cv.Length - 1;
					for (var i = 0; i < cv.Length; i++)
					{
						translation += cv [i];
						if (i != cvLength)
						{
							translation += "\n" + whitespace;
						}
					}
				}

				int cnt = 0;
				string [] cc = translation.Split (' ');
				if (cc.Length > 1)
				{
					translation = "";
					foreach (string ccv in cc)
					{
						if (cnt >= MAX_WIDTH)
						{
							translation += "\n" + whitespace;
							cnt = 0;
						}

						translation += ccv + " ";
						cnt += ccv.Length;
					}
				}

				res = Regex.Replace (res, match.Value, translation);
			}

			return res;
		}

		internal void CheckUpdateInslattaion ()
		{
			RegistryKey ke =
				Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);

			int inst = ke?.GetValue ("install") != null ? 1 : 0;
			if (inst == 1)
			{
				ShowSuccess (Core.CLI.Translate ("cli.success.update.full"), Core.CLI.Translate ("cli.success.update.short"));
				ke?.DeleteValue ("install");
				if ((string)ke?.GetValue ("cli_update", "null") != "null")
					ke.DeleteValue ("cli_update");
			}
		}

		#endregion
	}
}