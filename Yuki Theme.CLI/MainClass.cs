﻿using System;
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

		private const string SettingModeLight    = "light";
		private const string SettingModeAdvanced = "advanced";
		private const string ActionDelete        = "delete";
		private const string ActionImport        = "import";
		private const string ActionIgnore        = "ignore";

		private Dictionary <string, string> ToReplaceTranslationWithVariable = new ();

		internal Completion completion;

		/// <summary>
		/// Load API, to work with API. For example, get settings and load themes. After that, you can process the themes
		/// </summary>
		internal void LoadCLI (bool refreshSchemes)
		{
			if (Helper.mode != ProductMode.CLI) // Check if we loaded API early. If not load it
			{
				Helper.mode = ProductMode.CLI;
				API_Actions.setPath = AskPath;
				API_Actions.showSuccess = ShowSuccess;
				API_Actions.showError = ShowError;
				API_Actions.showError = ShowError;
				API_Actions.onRename = ShowInvertSuccess;
				API_Actions.SaveInExport = AskToDelete;
				Settings.connectAndGet ();
				Settings.settingMode = SettingMode.Light;
				Settings.saveAsOld = true;
				Core.API.LoadSchemes ();
				AddThemeCompletion ();
				AddDefinitionCompletion ();
				AddLanguageCompletion ();
			} else if (refreshSchemes)
			{
				Core.API.LoadSchemes ();
				AddThemeCompletion ();
			}
		}

		private void AddThemeCompletion ()
		{
			completion.themes.Clear ();
			string [] themes = Core.API.schemes.ToArray ();
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
			Core.API.SelectTheme (theme);
		}

		internal void ShowLoopMessage ()
		{
			if (loop)
				ShowError (Core.API.Translate ("cli.errors.loop").ToUpper ());
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
								   ShowError (Core.API.Translate ("cli.errors.nocommand", badVerbSelectedError.Token));
								   break;

							   case UnknownOptionError unknownOptionError :
								   ShowError (Core.API.Translate ("cli.errors.nooption", unknownOptionError.Token));
								   break;

							   case SequenceOutOfRangeError sequenceOutOfRange :
								   ShowError (sequenceOutOfRange.ToString ());
								   // ShowError (Core.API.Translate ("cli.errors.length"));
								   break;

							   case MissingValueOptionError missingValueOptionError :
								   helpText = HelpText.AutoBuild (res);
								   Console.WriteLine (Translation (helpText.ToString ()));
								   break;
							   // Handler other appropriate exceptions downhere.
							   default :
								   ShowError (Core.API.Translate ("cli.errors.happened", error.ToString ()));
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
							Core.API.Add (fr, to);
						} else
						{
							ShowError (Core.API.Translate ("messages.name.exist.full"));
						}
					} else
					{
						ShowError (Core.API.Translate ("cli.errors.notinthemes", fr));
					}
				} else
				{
					ShowError (Core.API.Translate ("messages.name.notchanged"), Core.API.Translate ("download.canceled.title"));
				}
			} else
			{
				ShowError (Core.API.Translate ("messages.name.invalid"), Core.API.Translate ("messages.name.invalid.short"));
			}
		}

		private void ListC (bool groups)
		{
			LoadCLI (true);
			Console.WriteLine ("Theme list:");
			if (groups)
			{
				foreach (string scheme in Core.API.schemes)
				{
					Console.WriteLine ($"{Core.API.themeInfos [scheme].group}:  {scheme}");
				}
			}else
			{
				foreach (string scheme in Core.API.schemes)
				{
					Console.WriteLine ("\t{0}", scheme);
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
			Core.API.Restore ();
			Console.WriteLine ($"There're {Core.API.names.Count} fields:");
			foreach (string name in Core.API.names)
			{
				Console.WriteLine ("\t" + Populater.GetChangedName (name));
			}

			SetFile ("N|L"); // Set to the default value
		}

		private void AllFieldsC ()
		{
			LoadCLI (true);
			Settings.settingMode = SettingMode.Advanced;
			SetFile ("Darcula");
			Core.API.Restore ();
			Console.WriteLine ($"There're {Core.API.currentTheme.Fields.Keys.Count} fields:");
			foreach (string name in Core.API.currentTheme.Fields.Keys)
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

					Core.API.Export (null, null, null, null, true);
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.API.Translate ("cli.errors.cantexport", o.Name));
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
					Core.API.Import (o.Path, AskToDelete);
				} else
					showerror = true;
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.API.Translate ("cli.errors.cantimport", o.Path));
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
					Core.API.Remove (o.Name, AskToDelete, null, AfterDelete);
				else
				{
					ShowError (Core.API.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.API.Translate ("cli.errors.cantdelete", o.Name));
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
					Core.API.Rename (fr, to);
				} else
				{
					ShowError (Core.API.Translate ("messages.name.notchanged"), Core.API.Translate ("download.canceled.title"));
				}
			} else
			{
				ShowError (Core.API.Translate ("messages.name.invalid"), Core.API.Translate ("messages.name.invalid.short"));
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
					ShowError (Core.API.Translate ("messages.path.wrong"));
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
					ShowError (Core.API.Translate ("cli.errors.quiet.wrong.full"), Core.API.Translate ("cli.errors.quiet.wrong.short"));
				}
			}

			if (!isNull (o.Mode))
			{
				bool isValid = false;
				string modeInLower = o.Mode.ToLower();
				isValid = modeInLower == SettingModeLight || modeInLower == SettingModeAdvanced;

				if (isValid)
				{
					Settings.settingMode = modeInLower == SettingModeLight ? SettingMode.Light : SettingMode.Advanced;
					changed = true;
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.mode.invalid"));
				}
			}

			if (!isNull (o.Action))
			{
				string actionLower = o.Action.ToLower();
				bool isValid = actionLower is ActionDelete or ActionImport or ActionIgnore;
				int actionNumber = GetThemeFoundActionNumber (actionLower);

				if (isValid)
				{
					Settings.actionChoice = actionNumber;
					changed = true;
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.action.invalid"));
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
					ShowError (Core.API.Translate ("cli.errors.language.invalid", languages));
				}
			}
			

			if (changed)
			{
				Settings.SaveData ();
				ShowSuccess ("Settings are saved!", "Saved");
			} else
			{
				ShowError (Core.API.Translate ("cli.errors.settings.notsaved.full"),
				           Core.API.Translate ("cli.errors.settings.notsaved.short"));
			}
		}

		private static int GetThemeFoundActionNumber (string actionLower)
		{
			return actionLower == ActionDelete ? 0 : actionLower == ActionImport ? 1 : 2;
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
					if (!Core.API.themeInfos [o.Name].isDefault)
					{
						SetFile (o.Name);
						Core.API.Restore ();
						if (o.Definition != null)
						{
							bool color = false;
							bool error = false;
							string err_txt = "";
							try
							{
								color = IsColor (o.Definition);
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
										o.Definition = Populater.GetNormalizedName (o.Definition);
										if (Core.API.currentTheme.Fields.ContainsKey (o.Definition))
										{
											ThemeField dic = Core.API.currentTheme.Fields [o.Definition];
											SetColorsToField (ref dic, txt, txtcolor, bg, bgcolor);
											foreach (KeyValuePair <string, string> keyValuePair in dic.GetAttributes ())
											{
												GiveMessage (keyValuePair.Value, keyValuePair.Key);
											}
										}

										var sttr = Populater.GetDependencies (o.Definition);
										if (sttr != null)
											foreach (var sr in sttr)
											{
												ThemeField dic = Core.API.currentTheme.Fields [sr];
												SetColorsToField (ref dic, txt, txtcolor, bg, bgcolor);
											}

										Core.API.Save (null, null, true);
									} else
									{
										ShowError (Core.API.Translate ("cli.errors.parameter"));
									}
								} else
								{
									bool changed = false;
									if (o.Opacity != null)
									{
										if (o.Definition.ToLower () == "sticker")
											Core.API.currentTheme.StickerOpacity = int.Parse (o.Opacity);
										else
											Core.API.currentTheme.WallpaperOpacity = int.Parse (o.Opacity);
										changed = true;
									}

									if (o.Align != null && o.Definition.ToLower () == "image")
									{
										Alignment align = Alignment.Center;
										if (o.Align.ToLower () == "left") align = Alignment.Left;
										else if (o.Align.ToLower () == "center") align = Alignment.Center;
										else align = Alignment.Right;
										Core.API.currentTheme.WallpaperAlign = (int)align;
										changed = true;
									}

									if (changed)
										Core.API.Save (null, null, true);
									SetImageLocation (o);
								}
							} else
							{
								ShowError (err_txt);
							}
						}
					} else
					{
						ShowError (Core.API.Translate ("colors.default.error.full"), Core.API.Translate ("colors.default.error.short"));
					}
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.notinthemes", o.Name));
				}
			} else
			{
				showerror = true;
			}

			if (showerror)
			{
				ShowError (Core.API.Translate ("cli.errors.cantedit", o.Name));
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


		#region API Features

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

					Assembly a = Core.API_Actions.GetCore ();
					int md = 0;
					bool found = false;
					while (!found)
					{
						string pth = InstallationPreparer.FileNamespace +
						             (md == 0 ? "files_program.txt" : "files_plugin.txt");
						Stream str = a.GetManifestResourceStream (pth);
						if (str != null)
						{
							using StreamReader reader = new StreamReader (str);
							string cx = reader.ReadLine ();
							string ph = Path.Combine (Core.API.currentPath, cx);
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
						InstallationPreparer prep = new InstallationPreparer ();
						prep.Prepare (false);
						RegistryKey ke =
							Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
						if (ke != null) ke.SetValue ("cli_update", "true");

						quit = true;
					} else
					{
						ShowError (Core.API.Translate ("cli.errors.find.app"));
					}
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.find.file"));
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
						ShowError (Core.API.Translate ("messages.path.wrong"));
					}
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.directory.notexist"));
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
			return Core.API.schemes.Contains (st);
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
				ShowError (Core.API.Translate ("cli.errors.input.wrong"));
			}

			return ans;
		}

		private void AfterDelete (string content, object obj)
		{
			ShowError (Core.API.Translate ("cli.message.deleted", content));
		}

		private bool isNull (string st)
		{
			return st == null || st.Length == 0;
		}

		private Image LoadImage ()
		{
			Image res = null;
			if (Core.API_Actions.IsDefault ())
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
				Tuple <bool, string> contents = Helper.GetTheme (Core.PathGenerator.PathToFile (Core.API.pathToLoad, true));
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetImage (Core.PathGenerator.PathToFile (Core.API.pathToLoad, true));
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
			if (Core.API_Actions.IsDefault ())
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
				Tuple <bool, string> contents = Helper.GetTheme (Core.PathGenerator.PathToFile (Core.API.pathToLoad, true));
				if (contents.Item1)
				{
					Tuple <bool, Image> iag = Helper.GetSticker (Core.PathGenerator.PathToFile (Core.API.pathToLoad, true));
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
			IThemeHeader header = DefaultThemes.headers [Core.API.nameToLoad];
			string ext = Helper.GetThemeFormat (true, Core.API.pathToLoad, Core.API.nameToLoad) == ThemeFormat.Old
				? Helper.FILE_EXTENSTION_OLD
				: Helper.FILE_EXTENSTION_NEW;
			pathToMemory = $"{header}.{Core.API.pathToLoad}{ext}";
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
					ShowError (Core.API.Translate ("cli.errors.file.notexist"));
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
					Core.API.Save (image, sticker, false);
				} else
				{
					ShowError (Core.API.Translate ("cli.errors.strange"));
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

		private bool IsColor (string definition)
		{
			bool result = Highlighter.IsInColors (definition);
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
				string translation = Core.API.Translate (match.Value);
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
				ShowSuccess (Core.API.Translate ("cli.success.update.full"), Core.API.Translate ("cli.success.update.short"));
				ke?.DeleteValue ("install");
				if ((string)ke?.GetValue ("cli_update", "null") != "null")
					ke.DeleteValue ("cli_update");
			}
		}

		#endregion
	}
}