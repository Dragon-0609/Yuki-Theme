using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Themes
{
	internal class NewThemeFormat : ThemeFormatBase
	{

		public NewThemeFormat (API_Base api)
		{
			this.api = api;
		}

		public Dictionary <string, ThemeField> GetFieldsFromDictionary (Dictionary <string, Dictionary <string, string>> attributes)
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

		public override void SaveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			string json = JsonConvert.SerializeObject (themeToSave, Formatting.Indented);
			bool iszip = ZipManager.IsZip (themeToSave.fullPath);


			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				File.WriteAllText (themeToSave.fullPath, json);
			else
			{
				if (iszip)
				{
					ZipManager.UpdateZip (themeToSave.fullPath, json, img2, wantToKeep, img3, wantToKeep, "", false);
				} else
				{
					ZipManager.Zip (themeToSave.fullPath, json, img2, img3, "", false);
				}
			}
		}

		public string LoadThemeToPopulate (string pathToTheme, bool needToGetImages, bool isDefault, string extension)
		{
			string json = "";
			if (isDefault)
			{
				string pathToLoad = Helper.ConvertNameToPath (pathToTheme);
				IThemeHeader header = DefaultThemes.headers [pathToTheme];
				Assembly a = header.Location;
				string pathToMemory = $"{header.ResourceHeader}.{pathToLoad}{extension}";
				// var a = API_Base.Current.GetCore ();

				Tuple <bool, string> content = ZipManager.GetThemeFromMemory (pathToMemory, a);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						bool exist = Helper.DoesImageExistInMemory (pathToMemory, a);
						LoadImage (exist, true, pathToMemory, a, API_Events.ifHasImage, API_Events.ifDoesntHave);

						exist = Helper.DoesStickerExistInMemory (pathToMemory, a);
						LoadSticker (exist, true, pathToMemory, a, API_Events.ifHasSticker, API_Events.ifDoesntHaveSticker);
					}
				} else
				{
					if (needToGetImages)
					{
						if (API_Events.ifDoesntHave != null)
							API_Events.ifDoesntHave ();

						if (API_Events.ifDoesntHaveSticker != null)
							API_Events.ifDoesntHaveSticker ();
					}

					StreamReader reader = new StreamReader (a.GetManifestResourceStream (pathToMemory));
					json = reader.ReadToEnd ();
				}
			} else
			{
				Tuple <bool, string> content = ZipManager.GetTheme (pathToTheme);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						bool exist = Helper.DoesImageExist (pathToTheme);
						LoadImage (exist, true, pathToTheme, API_Events.ifHasImage, API_Events.ifDoesntHave);

						exist = Helper.DoesStickerExist (pathToTheme);
						LoadSticker (exist, true, pathToTheme, API_Events.ifHasSticker, API_Events.ifDoesntHaveSticker);
					}
				} else
				{
					if (needToGetImages)
					{
						if (API_Events.ifDoesntHave != null)
							API_Events.ifDoesntHave ();
						if (API_Events.ifDoesntHaveSticker != null)
							API_Events.ifDoesntHaveSticker ();
					}

					json = File.ReadAllText (pathToTheme);
				}
			}

			return json;
		}

		public override string GetNameOfTheme (string path)
		{
			string nm = "";
			var theme = LoadTheme (path);
			nm = theme.Name;
			return nm;
		}

		private Theme LoadTheme (string path)
		{
			string json = LoadThemeToPopulate (path, false, false, "");
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			return theme;
		}


		private static Theme LoadTheme (string path, out bool iszip)
		{
			string json = "";

			Tuple <bool, string> content = ZipManager.GetTheme (path);
			iszip = content.Item1;

			if (content.Item1)
				json = content.Item2;
			else
				json = File.ReadAllText (path);

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			return theme;
		}

		public override void WriteName (string path, string name)
		{
			string json;
			Theme theme = LoadTheme (path, out bool iszip);

			theme.Name = name;
			json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			
			SaveModifiedTheme (path, iszip, json);
		}
		public override void WriteNameAndResetToken (string path, string name)
		{
			string json;
			Theme theme = LoadTheme (path, out bool iszip);

			theme.Name = name;
			theme.Token = "null";
			json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			
			SaveModifiedTheme (path, iszip, json);
		}

		private static void SaveModifiedTheme (string path, bool iszip, string json)
		{
			if (!iszip)
				File.WriteAllText (path, json);
			else
			{
				ZipManager.UpdateZip (path, json, null, true, null, true, "", false);
			}
		}
		
		public override void ReGenerate (string path, string oldPath, string name, string oldName, API_Actions apiActions)
		{
			Assembly a = api.GetCore ();
			string str;

			Stream resourceStream = a.GetManifestResourceStream (Helper.PASCALTEMPLATE);
			if (resourceStream == null) return;
			using (StreamReader reader = new StreamReader (resourceStream))
			{
				str = reader.ReadToEnd ();
			}
			bool isDefaultTheme = DefaultThemes.isDefault (oldName);
			string json = LoadThemeToPopulate (isDefaultTheme ? oldName : oldPath, false, isDefaultTheme, Helper.FILE_EXTENSTION_NEW);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			if (theme != null)
			{
				theme.Name = name;
				theme.Group = "";
				apiActions.LoadFieldsAndMergeFiles (str, path, theme);
			}
		}

		public void PopulateDictionaryFromTheme (Theme             theme, ref Dictionary <string, ThemeField> attributes,
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
		public override Theme PopulateList (string name, bool loadImages)
		{
			string path = Helper.ConvertNameToPath (name);
			bool isDef = api.ThemeInfos [name].isDefault;
			string json = LoadThemeToPopulate (isDef ? name : PathGenerator.PathToFile(path, false), loadImages, isDef, Helper.FILE_EXTENSTION_NEW);

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.isDefault = isDef;
			theme.fullPath = isDef ? PathGenerator.PathToMemory (path) : PathGenerator.PathToFile (path, false);
			theme.path = path;
			return theme;
		}

		public override void ProcessAfterParsing (Theme theme)
		{
			api.names.AddRange (theme.Fields.Keys);
			api.names.InsertRange (1, ShadowNames.imageNames);	
		}

		public override Tuple <bool, string> VerifyToken (string path)
		{
			bool valid = false;
			string json = LoadThemeToPopulate (path, false, false, Helper.FILE_EXTENSTION_NEW);
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