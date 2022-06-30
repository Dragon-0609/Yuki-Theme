﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using Formatting = Newtonsoft.Json.Formatting;

namespace Yuki_Theme.Core;

internal sealed class API_Actions
{
	private readonly ThemeManager _themeManager; 
	private readonly ThemeFormatBase _newThemeFormat; 
	private readonly OldThemeFormat _oldThemeFormat; 

	internal API_Actions ()
	{
		_themeManager = API._themeManager;
		_newThemeFormat = API._newThemeFormat;
		_oldThemeFormat = (OldThemeFormat)API._oldThemeFormat;
		_themeManager.SetActionsAPI (this);
		_themeManager.SetNewThemeFormat (_newThemeFormat);
		_themeManager.SetOldThemeFormat (_oldThemeFormat);
	}

	#region Merge with Template

	internal void MergeSyntax (string dir, SyntaxType syntax)
	{
		string destination = Path.Combine (dir, $"{API.pathToLoad}_{syntax}.xshd");
		ExtractSyntaxTemplate (syntax, destination);

		Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, API.currentTheme);
		Console.WriteLine (syntax.ToString ());
		MergeFiles (destination, localDic);
	}

	internal void ExtractSyntaxTemplate (SyntaxType syntax, string destination)
	{
		Assembly a = API.GetCore ();
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

	private void MergeFiles (string path, Dictionary <string, ThemeField> local)
	{
		XmlDocument doc = new XmlDocument ();
		doc.Load (path);

		MergeFiles (local, API.currentTheme, ref doc);

		doc.Save (path);
	}

	private void MergeFiles (Dictionary <string, ThemeField> fields, Theme themeToMerge, ref XmlDocument doc)
	{
		_oldThemeFormat.MergeThemeFieldsWithFile (fields, doc);

		_oldThemeFormat.MergeCommentsWithFile (themeToMerge, doc);
	}

	public void LoadFieldsAndMergeFiles (string content, string path, Theme theme)
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (content);

		Dictionary <string, ThemeField> localFields = ThemeField.GetThemeFieldsWithRealNames (SyntaxType.Pascal, API.currentTheme);

		MergeFiles (localFields, theme, ref doc);

		_oldThemeFormat.SaveXML (null, null, true, theme.IsZip (), ref doc, path);
	}

	#endregion
	
	
	#region File Manager

	/// <summary>
	/// Clean destination before export. Delete background image and sticker 
	/// </summary>
	internal void CleanDestination ()
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
	internal void CopyFiles (string [] files)
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
	internal void DeleteFiles (string [] files)
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
	internal bool ReGenerateTheme (string path, string oldPath, string name, string oldName, bool forceRegenerate)
	{
		if ((IsOldTheme (path) && IsOldTheme (oldPath)) || (!IsOldTheme (path) && !IsOldTheme (oldPath)))
			return false;
		if (!IsOldTheme (oldPath) && (Settings.saveAsOld || forceRegenerate))
			_newThemeFormat.ReGenerate (path, oldPath, name, oldName, this);
		else
			_oldThemeFormat.ReGenerate (path, oldPath, name, oldName, this);
		return true;
	}

	#endregion

	
	#region Checkers

	internal bool IsOldTheme (string path)
	{
		return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD);
	}

	internal bool IsNewTheme (string path)
	{
		return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_NEW);
	}
	
	/// <summary>
	/// Is current theme in default themes
	/// </summary>
	/// <returns></returns>
	internal bool IsDefault ()
	{
		return DefaultThemes.isDefault (API.nameToLoad);
	}
	
	
	internal bool AskToOverrideFile (string destination, ref bool exist, out int add)
	{
		if (!API_Events.SaveInExport (API.Translate ("messages.file.exist.override.full"), API.Translate ("messages.file.exist.override.short")))
		{
			if (API_Events.showError != null) API_Events.showError (API.Translate ("messages.name.exist.full"), API.Translate ("messages.name.exist.short"));
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

	internal bool CheckNameToRenameTo (string to, bool canShowError, out int rename)
	{
		rename = 1;
		if (to.Length < 3)
		{
			ShowError (canShowError, "messages.name.short.full", "messages.name.short.short");
			rename = 0;
		}
			
		if (API.ThemeInfos.ContainsKey (to))
		{
			ShowError (canShowError, "messages.name.exist.full", "messages.name.exist.short");
			rename = 0;
		}

		return rename == 0;
	}

	internal void AskToSaveInExport (Image img2, Image img3, bool wantToKeep)
	{
		if (!IsDefault () && Helper.mode != ProductMode.Plugin && API.isEdited)
		{
			if (API_Events.SaveInExport ("messages.theme.save.full", "messages.theme.save.short"))
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
	internal void PopulateList (Action onSelect = null)
	{
		if (API.ThemeInfos [API.nameToLoad].isOld)
		{
			_oldThemeFormat.LoadThemeToCLI ();
		} else
		{
			_newThemeFormat.LoadThemeToCLI ();
		}

		if (onSelect != null)
			onSelect ();
	}
	
	internal void LoadSchemesByExtension (string extension)
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
						API.Schemes.Add (pts);
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

	private void ForceAddToDefaults (string name, string file, string group)
	{
		if (!API.Schemes.Contains (name))
		{
			int index = GetIndexForInsert (name, @group);
			if (index != -1)
				API.Schemes.Insert (index, name);
			else
				API.Schemes.Add (name);
			DefaultThemes.categories.Add (name, @group);
			if (!DefaultThemes.categoriesList.Contains (@group))
			{
				DefaultThemes.categoriesList.Add (@group);
			}
		}

		ThemeInfo info = new ThemeInfo (true, IsOldTheme (file), ThemeLocation.File, @group, true);
		if (API.ThemeInfos.ContainsKey (name))
			API.ThemeInfos [name] = info;
		else
			API.ThemeInfos.Add (name, info);
			
		if (!DefaultThemes.names.Contains (name))
			DefaultThemes.names.Add (name);

		// Console.WriteLine ("{0}\n\n", ThemeInfos [name]);

	}
	
	#endregion
	
	
	#region Index Responsible

	private int GetIndexForInsert (string forName, string group)
	{
		int result = -1;

		string target = DefaultThemes.categories.Where (item => item.Value == @group).Select (item => item.Key).FirstOrDefault();

		if (target is { Length: > 1 })
		{
			result = FindIndex (forName, target);
		}

		return result;
	}

	private int FindIndex (string forName, string target)
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
				res = API.Schemes.FindIndex (item => item == targ2);

				if (res + 1 < API.Schemes.Count)
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
	private Tuple <bool, string> VerifyToken (string path)
	{
		if (path == "" || path.Length < 5) return new Tuple <bool, string> (false, "");
		if (IsNewTheme (path))
			return _newThemeFormat.VerifyToken (path);
		return _oldThemeFormat.VerifyToken (path);
	}

	internal void AddThemeInfo (string name, ThemeInfo themeInfo)
	{
		API.ThemeInfos.Add (name, themeInfo);
	}

	internal void ShowEndMessage ()
	{
		if (Helper.mode != ProductMode.Plugin)
			if (API_Events.showSuccess != null)
				API_Events.showSuccess (API.Translate ("messages.export.success"), API.Translate ("messages.buttons.done"));
	}

	internal int GetActionChoice (int result)
	{
		if (Helper.mode != ProductMode.Plugin && Helper.mode != ProductMode.CLI)
		{
			if (Settings.askChoice)
			{
				result = API_Events.AskChoice ();
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

	internal void ShowError (bool canShowError, string content, string title)
	{
		if (canShowError) API_Events.showError (API.Translate (content), API.Translate (title));
	}

	internal Dictionary <string, ThemeField> ConvertToRealNames (SyntaxType syntax)
	{
		Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, API.currentTheme);
		return localDic;
	}
}