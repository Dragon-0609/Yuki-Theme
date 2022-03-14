using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
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
		public static void saveList (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			string json = JsonConvert.SerializeObject (CLI.currentTheme, Formatting.Indented);
			bool iszip = Helper.IsZip (CLI.currentTheme.fullPath);


			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				File.WriteAllText (CLI.currentTheme.fullPath, json);
			else
			{
				if (iszip)
				{
					Helper.UpdateZip (CLI.currentTheme.fullPath, json, img2, wantToKeep, img3, wantToKeep, "", false);
				} else
				{
					Helper.Zip (CLI.currentTheme.fullPath, json, img2, img3, "", false);
				}
			}
		}

		public static string loadThemeToPopulate (string pathToFile, bool needToGetImages, bool isDefault, string nameToLoadForMemory,
		                                          string extension)
		{
			string json = "";
			if (isDefault)
			{
				string pathToLoad = Helper.ConvertNameToPath (nameToLoadForMemory);
				IThemeHeader header = DefaultThemes.headers [nameToLoadForMemory];
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
				Tuple <bool, string> content = Helper.GetTheme (pathToFile);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						Tuple <bool, Image> iag = Helper.GetImage (pathToFile);
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

						iag = Helper.GetSticker (pathToFile);
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

					json = File.ReadAllText (pathToFile);
				}
			}

			return json;
		}

		public static string GetNameOfTheme (string path)
		{
			string nm = "";
			if (path.Contains ("__"))
			{
			}

			string json = loadThemeToPopulate (path, false, false, "", "");
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			nm = theme.Name;
			return nm;
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
				File.WriteAllText (CLI.pathToFileNew, json);
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

		public static void populateList ()
		{
			Console.WriteLine (CLI.nameToLoad);
			bool isDef = CLI.isDefaultTheme [CLI.nameToLoad];
			string json = loadThemeToPopulate (CLI.pathToFileNew, true, isDef, CLI.nameToLoad, Helper.FILE_EXTENSTION_NEW);

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.isDefault = isDef;
			theme.fullPath = isDef ? CLI.pathToMemoryNew : CLI.pathToFileNew;
			theme.path = CLI.pathToLoad;
			CLI.names.AddRange (theme.Fields.Keys);
			CLI.names.InsertRange (1, ShadowNames.imageNames);
			CLI.currentTheme = theme;
		}
	}
}