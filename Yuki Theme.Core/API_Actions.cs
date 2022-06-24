using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Yuki_Theme.Core.Formats;
using Yuki_Theme.Core.Themes;
using Formatting = Newtonsoft.Json.Formatting;

namespace Yuki_Theme.Core;

public static class API_Actions
{

	#region Public Actions Fields

	public static Func <string, string, bool> SaveInExport;
	public static Action <string, string>     setPath;
	public static Action <string, string>     showSuccess;
	public static Action <string, string>     showError;
	public static Func <int>                  AskChoice;
	public static Action <string>             hasProblem           = null;
	public static Action                      onBGIMAGEChange      = null;
	public static Action                      onSTICKERChange      = null;
	public static Action                      onSTATUSChange       = null;
	public static Action <Image>              ifHasImage           = null;
	public static Action                      ifDoesntHave         = null;
	public static Action <Image>              ifHasSticker         = null;
	public static Action                      ifDoesntHaveSticker  = null;
	public static Action <Image>              ifHasImage2          = null;
	public static Action                      ifDoesntHave2        = null;
	public static Action <Image>              ifHasSticker2        = null;
	public static Action                      ifDoesntHaveSticker2 = null;
	public static Action <string, string>     onRename;

	#endregion

	
	#region Theme Manager

	public static Theme GetTheme (string name)
	{
		if (API.themeInfos.ContainsKey (name))
		{
			Theme theme;
			if (API.themeInfos [name].isOld)
			{
				theme = OldThemeFormat.populateList (name, false);
			} else
			{
				theme = NewThemeFormat.populateList (name, false);
			}

			return theme;
		} else
		{
			return null;
		}
	}

	private static Stream GetStreamFromMemory (string file, string name)
	{
		IThemeHeader header = DefaultThemes.headers [name];
		Assembly a = header.Location;
		if (file.Contains (":"))
		{
			file = Helper.ConvertNameToPath (file);
		}

		string ext = Helper.GetExtension (API.themeInfos [name].isOld);
		Stream stream = a.GetManifestResourceStream ($"{header.ResourceHeader}.{file}" + ext);
		return stream;
	}

	/// <summary>
	/// Copy theme from memory. It's used to copy default themes.
	/// </summary>
	/// <param name="file">Copy from (theme name as path)</param>
	/// <param name="name">Copy from (theme name)</param>
	/// <param name="path">Copy to path</param>
	/// <param name="extract">Do you want to extract background image and sticker?</param>
	public static void CopyFromMemory (string file, string name, string path, bool extract = false)
	{
		Stream stream = GetStreamFromMemory (file, name);
		string nxp = extract ? path + "A" : path;
		using (FileStream fs = new FileStream (nxp, FileMode.Create))
		{
			stream.Seek (0, SeekOrigin.Begin);
			stream.CopyTo (fs);
		}

		if (extract)
		{
			if (Helper.IsZip (stream))
			{
				CleanDestination ();
				Tuple <bool, Image> img = Helper.GetImage (nxp);
				Tuple <bool, Image> sticker = Helper.GetSticker (nxp);

				Helper.ExtractZip (nxp, path, img.Item1, sticker.Item1, false);
				File.Delete (nxp);
				return;
			}

			File.Move (nxp, path);
		}

		stream.Dispose ();
	}
	
	/// <summary>
	/// Export theme to the path (pascal directory)
	/// </summary>
	/// <param name="path">Path</param>
	public static void ExportTheme (string path)
	{
		string source = API.currentTheme.fullPath;
		bool iszip = Helper.IsZip (source);
		if (!iszip)
		{
			// File.Copy (source, path, true);
			return;
		}

		CleanDestination ();

		Tuple <bool, Image> img = Helper.GetImage (source);
		Tuple <bool, Image> sticker = Helper.GetSticker (source);

		Helper.ExtractZip (source, path, img.Item1, sticker.Item1, false);
	}
	
	/// <summary>
	/// Write name of the theme to the theme file (.xshd), so Yuki Theme can show it properly (symbols like ':')
	/// </summary>
	/// <param name="path">Full path to theme</param>
	/// <param name="name">New name of the theme</param>
	public static void WriteName (string path, string name)
	{
		if (IsOldTheme (path))
		{
			OldThemeFormat.WriteName (path, name);
		} else
		{
			NewThemeFormat.WriteName (path, name);
		}
	}

	public static int AddTheme (string copyFrom, string name, string destination, bool exist)
	{
		if (API.CopyTheme (copyFrom, name, destination, out _, true)) return 0;
		if (!exist)
		{
			API.schemes.Add (name);
			AddThemeInfo (
				name,
				new ThemeInfo (false, API.themeInfos [copyFrom].isOld, ThemeLocation.File, API.Translate ("messages.theme.group.custom")));
		}

		if (Helper.mode == ProductMode.CLI)
			if (showSuccess != null)
				showSuccess (API.Translate ("messages.theme.duplicate"), API.Translate ("messages.buttons.done"));

		return exist ? 2 : 1;
	}
	
	public static void DeleteOldThemeIfNeed ()
	{
		string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
		if (files is { Length: > 0 })
		{
			string [] unknownThemes = IdentifySyntaxHighlightings (files);
			// Console.WriteLine ("UNKNOWN: " + unknownThemes.Length);
			if (unknownThemes.Length == 0)
			{
				DeleteFiles (files);
			} else
			{
				int result = 2;
				result = GetActionChoice (result);

				if (result != 2)
				{
					if (result == 1) CopyFiles (unknownThemes);
					DeleteFiles (unknownThemes);
				}
			}

			files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.png");
			DeleteFiles (files);
		}
	}
	
	public static void DeleteOldTheme ()
	{
		string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
		if (files is { Length: > 0 })
		{
			DeleteFiles (files);
		}
	}	
	
	public static void AddThemeInfo (string name, ThemeInfo themeInfo)
	{
		API.themeInfos.Add (name, themeInfo);
	}

	public static void CopyThemeToDirectory (string path)
	{
		if (API.currentTheme.isDefault && API.themeInfos [API.currentTheme.Name].location == ThemeLocation.Memory)
		{
			CopyFromMemory (API.currentTheme.path, API.currentTheme.Name, path, true);
		} else
		{
			ExportTheme (path);
		}
	}	
	
	private static string [] IdentifySyntaxHighlightings (string [] files)
	{
		List <string> unknowThemes = new List <string> ();
		foreach (string file in files)
		{
			string name = OldThemeFormat.GetNameOfTheme (file);
			if (name.Length > 0)
			{
				if (!API.schemes.Contains (name))
				{
					unknowThemes.Add (file);
				}
			}
		}

		return unknowThemes.ToArray ();
	}
	
	#endregion

	
	#region Merge with Template

	public static void MergeSyntax (string dir, SyntaxType syntax)
	{
		string destination = Path.Combine (dir, $"{API.pathToLoad}_{syntax}.xshd");
		ExtractSyntaxTemplate (syntax, destination);

		Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, API.currentTheme);
		Console.WriteLine (syntax.ToString ());
		MergeFiles (destination, localDic);
	}

	public static void ExtractSyntaxTemplate (SyntaxType syntax, string destination)
	{
		Assembly a = GetCore ();
		Stream stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.Syntax_Templates.{syntax.ToString ()}.xshd");
		if (stream != null)
		{
			using (FileStream fs = new FileStream (destination, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}
		}
	}

	private static void MergeFiles (string path, Dictionary <string, ThemeField> local)
	{
		XmlDocument doc = new XmlDocument ();
		doc.Load (path);

		MergeFiles (local, API.currentTheme, ref doc);

		doc.Save (path);
	}

	public static void MergeFiles (Dictionary <string, ThemeField> fields, Theme themeToMerge, ref XmlDocument doc)
	{
		OldThemeFormat.MergeThemeFieldsWithFile (fields, doc);

		OldThemeFormat.MergeCommentsWithFile (themeToMerge, doc);
	}
	
	private static void LoadFieldsAndMergeFiles (string content, string path, Theme theme)
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (content);

		Dictionary <string, ThemeField> localFields = ThemeField.GetThemeFieldsWithRealNames (SyntaxType.Pascal, API.currentTheme);

		MergeFiles (localFields, theme, ref doc);

		OldThemeFormat.SaveXML (null, null, true, theme.IsZip (), ref doc, path);
	}

	#endregion
	
	
	#region File Manager

	/// <summary>
	/// Clean destination before export. Delete background image and sticker 
	/// </summary>
	private static void CleanDestination ()
	{
		string [] fil = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.png");
		foreach (string s in fil)
		{
			File.Delete (s);
		}
	}

	/// <summary>
	/// Copy files to <code>Themes</code> directory
	/// </summary>
	/// <param name="files">Files to be copied</param>
	public static void CopyFiles (string [] files)
	{
		foreach (string file in files)
		{
			string fs = Path.Combine (API.currentPath, "Themes", Path.GetFileNameWithoutExtension (file) + Helper.FILE_EXTENSTION_OLD);
			if (!File.Exists (fs))
				File.Copy (file, fs);
		}
	}


	/// <summary>
	/// Delete files if exist
	/// </summary>
	/// <param name="files"></param>
	public static void DeleteFiles (string [] files)
	{
		foreach (string file in files)
		{
			if (File.Exists (file))
				File.Delete (file);
		}
	}

	#endregion


	#region Regeneration

	/// <summary>
	/// Re Generate Theme to convert from old theme to new, or vice versa.
	/// </summary>
	/// <param name="path">Destination path</param>
	/// <param name="oldPath">Old path, that was copied from. It also can be path to the memory</param>
	/// <param name="name">New Name</param>
	/// <param name="oldName">Old Name</param>
	/// <param name="forceRegenerate">Force to Regenerate</param>
	/// <returns>If it's done, it will return true</returns>
	public static bool ReGenerateTheme (string path, string oldPath, string name, string oldName, bool forceRegenerate)
	{
		if ((IsOldTheme (path) && IsOldTheme (oldPath)) || (!IsOldTheme (path) && !IsOldTheme (oldPath)))
			return false;
		if (!IsOldTheme (oldPath) && (Settings.saveAsOld || forceRegenerate))
			ReGenerateFromNew (path, oldPath, name, oldName);
		else
			ReGenerateFromOld (path, oldPath, name, oldName);
		return true;
	}

	private static void ReGenerateFromOld (string path, string oldPath, string name, string oldName)
	{
		Theme theme = new Theme
		{
			Fields = new Dictionary <string, ThemeField> ()
		};
		XmlDocument doc = new XmlDocument ();
		try
		{
			bool isDef = DefaultThemes.isDefault (oldName);
			OldThemeFormat.loadThemeToPopulate (ref doc, isDef ? oldName : oldPath, false, isDef, ref theme, Helper.FILE_EXTENSTION_OLD,
			                                    false, true);
		} catch
		{
			return;
		}

		List <string> namesList = new List <string> ();

		OldThemeFormat.PopulateDictionaryFromDoc (doc, ref theme, ref namesList);

		string methdoName = Settings.settingMode == SettingMode.Light ? "Method" : "MarkPrevious";
		if (!theme.Fields.ContainsKey (methdoName))
		{
			string keywordName = Settings.settingMode == SettingMode.Light ? "Keyword" : "Keywords";
			theme.Fields.Add (methdoName, new ThemeField () { Foreground = theme.Fields [keywordName].Foreground });
		}

		Dictionary <string, string> additionalInfo = OldThemeFormat.GetAdditionalInfoFromDoc (doc);
		string al = additionalInfo ["align"];
		string op = additionalInfo ["opacity"];
		string sop = additionalInfo ["stickerOpacity"];
		theme.Name = name;
		theme.Group = "";
		theme.Version = Convert.ToInt32 (Settings.current_version);
		theme.WallpaperOpacity = int.Parse (op);
		theme.StickerOpacity = int.Parse (sop);
		theme.WallpaperAlign = int.Parse (al);
		string json = JsonConvert.SerializeObject (theme, Formatting.Indented);
		bool isZip;

		if (DefaultThemes.isDefault (oldName))
		{
			Stream stream = GetStreamFromMemory (oldName, oldName);
			isZip = Helper.IsZip (stream);
			stream.Dispose ();
		} else
		{
			isZip = Helper.IsZip (oldPath);
		}

		if (!isZip)
			File.WriteAllText (path, json);
		else
		{
			Helper.UpdateZip (path, json, null, true, null, true, "", false);
		}
	}

	private static void ReGenerateFromNew (string path, string oldPath, string name, string oldName)
	{
		Assembly a = GetCore ();
		string str;

		Stream resourceStream = a.GetManifestResourceStream (Helper.PASCALTEMPLATE);
		if (resourceStream == null) return;
		using (StreamReader reader = new StreamReader (resourceStream))
		{
			str = reader.ReadToEnd ();
		}
		bool isDefaultTheme = DefaultThemes.isDefault (oldName);
		string json = NewThemeFormat.loadThemeToPopulate (isDefaultTheme ? oldName : oldPath, false, isDefaultTheme, Helper.FILE_EXTENSTION_NEW);
		Theme theme = JsonConvert.DeserializeObject <Theme> (json);
		if (theme != null)
		{
			theme.Name = name;
			theme.Group = "";
			// Console.WriteLine ("REGENERATION...");
			LoadFieldsAndMergeFiles (str, path, theme);
		}
	}

	#endregion

	
	#region Checkers

	public static bool IsOldTheme (string path)
	{
		return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD);
	}

	public static bool IsNewTheme (string path)
	{
		return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_NEW);
	}
	
	/// <summary>
	/// Is current theme in default themes
	/// </summary>
	/// <returns></returns>
	public static bool IsDefault ()
	{
		return DefaultThemes.isDefault (API.nameToLoad);
	}
	
	
	public static bool AskToOverrideFile (string destination, ref bool exist, out int add)
	{
		if (!SaveInExport (API.Translate ("messages.file.exist.override.full"), API.Translate ("messages.file.exist.override.short")))
		{
			if (showError != null)
				showError (API.Translate ("messages.name.exist.full"), API.Translate ("messages.name.exist.short"));
			{
				add = 0;
				return true;
			}
		}

		add = 1;
		exist = true;
		File.Delete (destination);
		return false;
	}

	public static bool CheckNameToRenameTo (string to, bool canShowError, out int rename)
	{
		rename = 1;
		if (to.Length < 3)
		{
			ShowError (canShowError, "messages.name.short.full", "messages.name.short.short");
			rename = 0;
		}
			
		if (API.themeInfos.ContainsKey (to))
		{
			ShowError (canShowError, "messages.name.exist.full", "messages.name.exist.short");
			rename = 0;
		}

		return rename == 0;
	}

	public static void AskToSaveInExport (Image img2, Image img3, bool wantToKeep)
	{
		if (!IsDefault () && Helper.mode != ProductMode.Plugin && API.isEdited)
		{
			if (SaveInExport ("messages.theme.save.full", "messages.theme.save.short"))
				API.Save (img2, img3, wantToKeep);
		} else if (!IsDefault ())
		{
			API.Save (img2, img3, wantToKeep);
		}
	}
	
	#endregion

	
	#region Methods reference to API

	/// <summary>
	/// Populate list with values. For example Default Background color, Default Foreground color and etc. 
	/// </summary>
	/// <param name="onSelect">Action, after populating list</param>
	public static void PopulateList (Action onSelect = null)
	{
		if (API.themeInfos [API.nameToLoad].isOld)
		{
			OldThemeFormat.LoadThemeToCLI ();
		} else
		{
			NewThemeFormat.LoadThemeToCLI ();
		}

		if (onSelect != null)
			onSelect ();
	}
	
	public static void LoadSchemesByExtension (string extension)
	{
		foreach (string file in Directory.GetFiles (Path.Combine (API.currentPath, "Themes"), "*" + extension, SearchOption.TopDirectoryOnly))
		{
			string pts = Path.GetFileNameWithoutExtension (file);

			Tuple <bool, string> tokenVerification = VerifyToken (file);
			bool isValidToken = tokenVerification.Item1;
			string tokenGroupName = tokenVerification.Item2;
				
			if (!DefaultThemes.isDefault (pts) || isValidToken)
			{
				bool has = false;
				if (pts.Contains ("__"))
				{
					string stee = Path.Combine (API.currentPath, "Themes", $"{pts}{extension}");
					pts = API.GetNameOfTheme (stee);
						
					if (DefaultThemes.isDefault (pts))
					{
						has = true;
					}
				}
				if (!isValidToken)
				{
					if (!has && pts.Length > 0)
					{
						API.schemes.Add (pts);
						AddThemeInfo (
							pts,
							new ThemeInfo (false, IsOldTheme (file), ThemeLocation.File,
							               tokenGroupName == "" ? API.Translate ("messages.theme.group.custom") : tokenGroupName));
					}
				} else
				{
					// Console.WriteLine ("Token valid");
					ForceAddToDefaults (pts, file, tokenGroupName);
				}
			}
		}
	}

	private static void ForceAddToDefaults (string name, string file, string group)
	{
		if (!API.schemes.Contains (name))
		{
			int index = GetIndexForInsert (name, @group);
			if (index != -1)
				API.schemes.Insert (index, name);
			else
				API.schemes.Add (name);
			DefaultThemes.categories.Add (name, @group);
			if (!DefaultThemes.categoriesList.Contains (@group))
			{
				DefaultThemes.categoriesList.Add (@group);
			}
		}

		ThemeInfo info = new ThemeInfo (true, IsOldTheme (file), ThemeLocation.File, @group, true);
		if (API.themeInfos.ContainsKey (name))
			API.themeInfos [name] = info;
		else
			API.themeInfos.Add (name, info);
			
		if (!DefaultThemes.names.Contains (name))
			DefaultThemes.names.Add (name);

		// Console.WriteLine ("{0}\n\n", ThemeInfos [name]);

	}
	
	#endregion
	
	
	#region Index Responsible

	private static int GetIndexForInsert (string forName, string group)
	{
		int result = -1;

		string target = DefaultThemes.categories.Where (item => item.Value == @group).Select (item => item.Key).FirstOrDefault();

		if (target is { Length: > 1 })
		{
			result = FindIndex (forName, target);
		}

		return result;
	}

	private static int FindIndex (string forName, string target)
	{
		int res = -1;

		if (DefaultThemes.headers.ContainsKey (target))
		{
			IThemeHeader header = DefaultThemes.headers [target];
			List <string> themes = header.ThemeNames.Keys.ToList ();
			themes.Add (forName);
			themes.Sort ();
			res = themes.FindIndex (tg => tg == forName);
			if (res > 1)
				res = res - 1;
			else
			{
				if (themes.Count >= 2)
					res = 1;
			}

			if (res != -1)
			{
				string targ2 = themes [res];
				res = API.schemes.FindIndex (item => item == targ2);

				if (res + 1 < API.schemes.Count)
				{
					res += 1;
				}
			}
		}

		return res;
	}

	#endregion
	
	
	/// <summary>
	/// Verify theme token and get group name.
	/// </summary>
	/// <param name="path">Path to the theme</param>
	/// <returns>Is Token valid and group name</returns>
	private static Tuple <bool, string> VerifyToken (string path)
	{
		if (path == "" || path.Length < 5) return new Tuple <bool, string> (false, "");
		if (IsNewTheme (path))
			return NewThemeFormat.VerifyToken (path);
		return OldThemeFormat.VerifyToken (path);
	}

	public static void ShowEndMessage ()
	{
		if (Helper.mode != ProductMode.Plugin)
			if (showSuccess != null)
				showSuccess (API.Translate ("messages.export.success"), API.Translate ("messages.buttons.done"));
	}
	
	private static int GetActionChoice (int result)
	{
		if (Helper.mode != ProductMode.Plugin && Helper.mode != ProductMode.CLI)
		{
			if (Settings.askChoice)
			{
				result = AskChoice ();
			} else
			{
				if (Settings.actionChoice is 0 or 1)
					result = Settings.actionChoice;
			}
		} else
		{
			result = 0;
		}

		return result;
	}
	
	public static void ShowError (bool canShowError, string content, string title)
	{
		if (canShowError) showError (API.Translate (content), API.Translate (title));
	}

	public static Dictionary <string, ThemeField> ConvertToRealNames (SyntaxType syntax)
	{
		Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, API.currentTheme);
		return localDic;
	}

	/// <summary>
	/// Get this assembly
	/// </summary>
	/// <returns></returns>
	public static Assembly GetCore ()
	{
		return Assembly.GetExecutingAssembly ();
	}
}