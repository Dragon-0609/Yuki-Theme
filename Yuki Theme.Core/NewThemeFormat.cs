using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core
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

		public static Theme PrepareToSave (Image img2, Image img3)
		{
			Theme theme = new Theme ();
			theme.Name = CLI.currentoFile;
			theme.Group = CLI.groupName;
			theme.Version = Convert.ToInt32 (SettingsForm.current_version);
			theme.HasWallpaper = img2 != null;
			theme.HasSticker = img3 != null;
			theme.WallpaperOpacity = CLI.opacity;
			theme.StickerOpacity = CLI.sopacity;
			theme.WallpaperAlign = (int) CLI.align;
			theme.Fields = GetFieldsFromDictionary (CLI.localAttributes);
			return theme;
		}

		/// <summary>
		/// Save current theme in new format. It is mainly used in new versions of Yuki Theme. Smaller than version 6 won't be able to open the format
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		public static void saveList (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			Theme theme = PrepareToSave (img2, img3);

			string json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			bool iszip = Helper.IsZip (CLI.getPathNew);


			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				File.WriteAllText (CLI.getPathNew, json);
			else
			{
				if (iszip)
				{
					Helper.UpdateZip (CLI.getPathNew, json, img2, wantToKeep, img3, wantToKeep);
				} else
				{
					Helper.Zip (CLI.getPathNew, json, img2, img3);
				}
			}
		}

		public static string loadThemeToPopulate (string pathToMemory, string pathToFile, bool needToGetImages, bool isDefault)
		{
			string json = "";
			if (isDefault)
			{
				var a = CLI.GetCore ();
				
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
							if (CLI.ifHasImage != null)
							{
								CLI.ifHasImage (iag.Item2);
							}

							if (CLI.ifHasImage2 != null)
							{
								CLI.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHave != null)
								CLI.ifDoesntHave ();
							if (CLI.ifDoesntHave2 != null)
								CLI.ifDoesntHave2 ();
						}

						iag = null;
						iag = Helper.GetStickerFromMemory (pathToMemory, a);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasSticker != null)
							{
								CLI.ifHasSticker (iag.Item2);
							}

							if (CLI.ifHasSticker2 != null)
							{
								CLI.ifHasSticker2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHaveSticker != null)
								CLI.ifDoesntHaveSticker ();
							if (CLI.ifDoesntHaveSticker2 != null)
								CLI.ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (CLI.ifDoesntHave != null)
							CLI.ifDoesntHave ();

						if (CLI.ifDoesntHaveSticker != null)
							CLI.ifDoesntHaveSticker ();

						if (CLI.ifDoesntHave2 != null)
							CLI.ifDoesntHave2 ();

						if (CLI.ifDoesntHaveSticker2 != null)
							CLI.ifDoesntHaveSticker2 ();
					}
					StreamReader reader = new StreamReader (a.GetManifestResourceStream (pathToMemory));
					json = reader.ReadToEnd ();
				}
			} else
			{
				CLI.imagePath = "";
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
							if (CLI.ifHasImage != null)
							{
								CLI.ifHasImage (iag.Item2);
							}

							if (CLI.ifHasImage2 != null)
							{
								CLI.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHave != null)
								CLI.ifDoesntHave ();

							if (CLI.ifDoesntHave2 != null)
								CLI.ifDoesntHave2 ();
						}

						iag = Helper.GetSticker (pathToFile);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasSticker != null)
							{
								CLI.ifHasSticker (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHaveSticker != null)
								CLI.ifDoesntHaveSticker ();

							if (CLI.ifDoesntHaveSticker2 != null)
								CLI.ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (CLI.ifDoesntHave != null)
							CLI.ifDoesntHave ();
						if (CLI.ifDoesntHaveSticker != null)
							CLI.ifDoesntHaveSticker ();

						if (CLI.ifDoesntHave2 != null)
							CLI.ifDoesntHave2 ();
						if (CLI.ifDoesntHaveSticker2 != null)
							CLI.ifDoesntHaveSticker2 ();
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

			string json = loadThemeToPopulate (path, path, false, false);
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
				File.WriteAllText (CLI.getPathNew, json);
			else
			{
				Helper.UpdateZip (path, json, null, true, null, true);
			}
		}

		public static void PopulateDictionaryFromTheme (Theme theme, ref Dictionary <string, Dictionary <string, string>> attributes, ref List <string> namesExtended)
		{
			foreach (KeyValuePair <string, ThemeField> field in theme.Fields)
			{
				if (!namesExtended.Contains (field.Key))
				{
					Dictionary <string, string> attrs = field.Value.ConvertToDictionary ();
					namesExtended.Add (field.Key);
					attributes.Add (field.Key, attrs);

					if (field.Key.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtended.Contains ("Wallpaper"))
					{
						namesExtended.Remove ("Selection");
						namesExtended.Add ("Wallpaper");
						namesExtended.Add ("Selection");
					}

					if (field.Key.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtended.Contains ("Sticker"))
					{
						namesExtended.Remove ("Selection");
						namesExtended.Add ("Sticker");
						namesExtended.Add ("Selection");
					}
				}
			}
			attributes.Add ("Wallpaper",
			                new Dictionary <string, string> {{"align", theme.WallpaperAlign.ToString ()}, {"opacity", theme.WallpaperOpacity.ToString ()}});

			attributes.Add ("Sticker",
			                new Dictionary <string, string> {{"opacity", theme.StickerOpacity.ToString ()}});
		}

		public static void populateList ()
		{
			string json = loadThemeToPopulate (CLI.gpNew, CLI.getPathNew, true, CLI.isDefault ());

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			PopulateDictionaryFromTheme (theme, ref CLI.localAttributes, ref CLI.names);
			
			CLI.align = (Alignment) theme.WallpaperAlign;
			CLI.opacity = theme.WallpaperOpacity;
			CLI.sopacity = theme.StickerOpacity;
		}
	
	}
}