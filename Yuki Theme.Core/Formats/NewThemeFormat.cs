using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Formats
{
	public static class NewThemeFormat
	{
		public static Dictionary <string, ThemeField> GetFieldsFromDictionary (Dictionary <string, Dictionary <string, string>> attributes)
		{
			Dictionary <string, ThemeField> fields = new Dictionary <string, ThemeField> ();
			foreach (KeyValuePair <string, Dictionary <string, string>> attribute in attributes)
			{
				if (attribute.Key != "Wallpaper" && attribute.Key != "Sticker")
				{
					ThemeField field = ThemeField.GetFieldFromDictionary (attribute.Value);
					string shadowName = ShadowNames.GetShadowName (attribute.Key, SyntaxType.Pascal); // Convert to Shadow Name
					if (shadowName == null) shadowName = attribute.Key;
					if (!fields.ContainsKey (shadowName))
						fields.Add (shadowName, field);
				}
			}

			return fields;
		}

		/// <summary>
		/// Save current theme in new format. It is mainly used in new versions of Yuki Theme. Smaller than version 6 won't be able to open the format
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="wantToKeep"></param>
		/// <param name="themeToSave"></param>
		public static void saveList (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			string json = JsonConvert.SerializeObject (themeToSave, Formatting.Indented);
			bool iszip = Helper.IsZip (themeToSave.fullPath);


			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				File.WriteAllText (themeToSave.fullPath, json);
			else
			{
				if (iszip)
				{
					Helper.UpdateZip (themeToSave.fullPath, json, img2, wantToKeep, img3, wantToKeep, "", false);
				} else
				{
					Helper.Zip (themeToSave.fullPath, json, img2, img3, "", false);
				}
			}
		}

		public static string loadThemeToPopulate (string pathToTheme, bool needToGetImages, bool isDefault, string extension)
		{
			string json = "";
			if (isDefault)
			{
				string pathToLoad = Helper.ConvertNameToPath (pathToTheme);
				IThemeHeader header = DefaultThemes.headers [pathToTheme];
				Assembly a = header.Location;
				string pathToMemory = $"{header.ResourceHeader}.{pathToLoad}{extension}";
				// var a = CLI.GetCore ();

				Tuple <bool, string> content = Helper.GetThemeFromMemory (pathToMemory, a);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathToMemory, a);

						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasImage != null)
							{
								CLI_Actions.ifHasImage (iag.Item2);
							}

							if (CLI_Actions.ifHasImage2 != null)
							{
								CLI_Actions.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHave != null)
								CLI_Actions.ifDoesntHave ();
							if (CLI_Actions.ifDoesntHave2 != null)
								CLI_Actions.ifDoesntHave2 ();
						}

						iag = null;
						iag = Helper.GetStickerFromMemory (pathToMemory, a);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasSticker != null)
							{
								CLI_Actions.ifHasSticker (iag.Item2);
							}

							if (CLI_Actions.ifHasSticker2 != null)
							{
								CLI_Actions.ifHasSticker2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHaveSticker != null)
								CLI_Actions.ifDoesntHaveSticker ();
							if (CLI_Actions.ifDoesntHaveSticker2 != null)
								CLI_Actions.ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (CLI_Actions.ifDoesntHave != null)
							CLI_Actions.ifDoesntHave ();

						if (CLI_Actions.ifDoesntHaveSticker != null)
							CLI_Actions.ifDoesntHaveSticker ();

						if (CLI_Actions.ifDoesntHave2 != null)
							CLI_Actions.ifDoesntHave2 ();

						if (CLI_Actions.ifDoesntHaveSticker2 != null)
							CLI_Actions.ifDoesntHaveSticker2 ();
					}

					StreamReader reader = new StreamReader (a.GetManifestResourceStream (pathToMemory));
					json = reader.ReadToEnd ();
				}
			} else
			{
				Tuple <bool, string> content = Helper.GetTheme (pathToTheme);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						Tuple <bool, Image> iag = Helper.GetImage (pathToTheme);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasImage != null)
							{
								CLI_Actions.ifHasImage (iag.Item2);
							}

							if (CLI_Actions.ifHasImage2 != null)
							{
								CLI_Actions.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHave != null)
								CLI_Actions.ifDoesntHave ();

							if (CLI_Actions.ifDoesntHave2 != null)
								CLI_Actions.ifDoesntHave2 ();
						}

						iag = Helper.GetSticker (pathToTheme);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasSticker != null)
							{
								CLI_Actions.ifHasSticker (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHaveSticker != null)
								CLI_Actions.ifDoesntHaveSticker ();

							if (CLI_Actions.ifDoesntHaveSticker2 != null)
								CLI_Actions.ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (CLI_Actions.ifDoesntHave != null)
							CLI_Actions.ifDoesntHave ();
						if (CLI_Actions.ifDoesntHaveSticker != null)
							CLI_Actions.ifDoesntHaveSticker ();

						if (CLI_Actions.ifDoesntHave2 != null)
							CLI_Actions.ifDoesntHave2 ();
						if (CLI_Actions.ifDoesntHaveSticker2 != null)
							CLI_Actions.ifDoesntHaveSticker2 ();
					}

					json = File.ReadAllText (pathToTheme);
				}
			}

			return json;
		}

		public static string GetNameOfTheme (string path)
		{
			string nm = "";
			var theme = LoadTheme (path);
			nm = theme.Name;
			return nm;
		}

		private static Theme LoadTheme (string path)
		{
			string json = loadThemeToPopulate (path, false, false, "");
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			return theme;
		}

		public static void WriteName (string path, string name)
		{
			string json = "";

			Tuple <bool, string> content = Helper.GetTheme (path);
			bool iszip = content.Item1;

			if (content.Item1)
				json = content.Item2;
			else
				json = File.ReadAllText (path);
			Console.WriteLine (json);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.Name = name;
			json = JsonConvert.SerializeObject (theme, Formatting.Indented);


			if (!iszip)
				File.WriteAllText (path, json);
			else
			{
				Helper.UpdateZip (path, json, null, true, null, true, "", false);
			}
		}

		public static void PopulateDictionaryFromTheme (Theme             theme, ref Dictionary <string, ThemeField> attributes,
		                                                ref List <string> namesExtended)
		{
			foreach (KeyValuePair <string, ThemeField> field in theme.Fields)
			{
				if (!namesExtended.Contains (field.Key))
				{
					namesExtended.Add (field.Key);
					attributes.Add (field.Key, field.Value);
				}
			}
		}

		/// <summary>
		/// Load Theme by name.
		/// </summary>
		/// <param name="name">Theme's name. It's mandatory for loading theme properly</param>
		/// <returns>Parsed theme</returns>
		public static Theme populateList (string name, bool loadImages)
		{
			Console.WriteLine (name);
			string path = Helper.ConvertNameToPath (name);
			bool isDef = CLI.ThemeInfos [name].isDefault;
			string json = loadThemeToPopulate (isDef ? name : CLI.pathToFile(path, false), loadImages, isDef, Helper.FILE_EXTENSTION_NEW);

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.isDefault = isDef;
			theme.fullPath = isDef ? CLI.pathToMemory (path) : CLI.pathToFile (path, false);
			theme.path = path;
			return theme;
		}

		/// <summary>
		/// Load Theme directly to the CLI
		/// </summary>
		public static void LoadThemeToCLI ()
		{
			Theme theme = populateList (CLI.nameToLoad, true);
			CLI.currentTheme = theme;
			if (theme == null)
			{
				MessageBox.Show (CLI.Translate ("messages.theme.invalid.full"), CLI.Translate ("messages.theme.invalid.short"),
				                 MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else
			{
				CLI.names.AddRange (theme.Fields.Keys);
				CLI.names.InsertRange (1, ShadowNames.imageNames);
			}
		}

		public static Tuple <bool, string> VerifyToken (string path)
		{
			bool valid = false;
			string json = loadThemeToPopulate (path, false, false, Helper.FILE_EXTENSTION_NEW);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			string group = "";
			if (theme != null)
			{
				valid = Helper.VerifyToken (theme);
				group = theme.Group;
			}
			return new Tuple <bool, string> (valid, group);
		}
	}
}