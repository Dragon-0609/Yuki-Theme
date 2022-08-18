using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core;

public abstract class API_Base
{
	public static API_Base Current = new CommonAPI ();

	#region Public Fields

	public List<string> names => API.names;

	public Theme currentTheme
	{
		get => API.currentTheme;
		set => API.currentTheme = value;
	}

	public bool isEdited
	{
		get => API.isEdited;
		set => API.isEdited = value;
	}

	public List<string> Schemes => API.Schemes;

	public string nameToLoad => API.nameToLoad;
	public string pathToLoad => API.pathToLoad;
	
	public Dictionary<string, ThemeInfo> ThemeInfos => API.ThemeInfos;

	internal ThemeFormatBase _newThemeFormat => API._newThemeFormat;
	internal ThemeFormatBase _oldThemeFormat => API._oldThemeFormat;
	internal ThemeManager    _themeManager   => API._themeManager;

	public string selectedItem
	{
		get => API.selectedItem;
		set => API.selectedItem = value;
	}
	
	#endregion
	
	#region Main Commands

	public void LoadSchemes (Func<string> ifZero = null) => API.LoadSchemes (ifZero);

	public int AddTheme (string copyFrom, string name) => API.AddTheme (copyFrom, name);

	public void RemoveTheme (string name, Func<string, string, bool> askToDelete, Func<string, object> afterAsk = null,
							 Action<string, object> afterDelete = null) => API.RemoveTheme (name, askToDelete, afterAsk, afterDelete);

	public void Save (Image wallpaper = null, Image sticker = null, bool wantToKeep = false) => API.Save (wallpaper, sticker, wantToKeep);

	public void ExportTheme (Image wallpaper, Image sticker, Action setTheme = null, Action startSettingTheme = null,
							 bool wantToKeep = false) => API.ExportTheme (wallpaper, sticker, setTheme, startSettingTheme, wantToKeep);

	public void Preview (SyntaxType syntax, bool needToDelete, Action setTheme = null) => API.Preview (syntax, needToDelete, setTheme);

	public void ImportTheme (string path, Func<string, string, bool> exist) => API.ImportTheme (path, exist);

	public void ImportTheme (string path, bool ask = true, bool select = true, Action<string, string> defaultTheme = null,
							 Func<string, string, bool> exist = null, Action<string> addToUIList = null,
							 Action<string> selectAfterParse = null) => API.ImportTheme (path, ask, select, defaultTheme, exist,
		addToUIList, selectAfterParse);

	public int RenameTheme (string from, string to) => API.RenameTheme (from, to);

	public void Restore (bool wantClean = true, Action onSelect = null) => API.Restore (wantClean, onSelect);

	public bool SelectTheme (string name) => API.SelectTheme (name);

	#endregion

	#region Helper Methods

	public bool IsPascalDirectory (string path) => API.IsPascalDirectory (path);
	
	public string GetNameOfTheme (string path) => API.GetNameOfTheme (path);

	public void SaveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false) =>
		API.SaveTheme (themeToSave, img2, img3, wantToKeep);

	public string Translate (string key) => API.Translate (key);
	public string Translate (string key, string p1) => API.Translate (key, p1);
	public string Translate (string key, string p1, string p2) => API.Translate (key, p1, p2);
	
	public Assembly GetCore () => API.GetCore ();
	
	public Theme GetTheme (string name) => API.GetTheme (name);
	
	public bool IsDefault () => API.IsDefault ();

	public bool ReGenerateTheme (string path, string oldPath, string name, string oldName, bool forceRegenerate) =>
		API.ReGenerateTheme (path, oldPath, name, oldName, forceRegenerate);

	public bool IsOldTheme (string path) => API.IsOldTheme (path);
	
	public void ExtractSyntaxTemplate (SyntaxType syntax, string destination) => API.ExtractSyntaxTemplate (syntax, destination);

	public void CopyFromMemory (string file, string name, string path, bool extract = false) =>
		API.CopyFromMemory (file, name, path, extract);

	public void AddThemeInfo (string name, ThemeInfo themeInfo) => API.AddThemeInfo (name, themeInfo);
	
	public void ShowError (string message, string title) => API.ShowError (message, title);
	
	public void ShowError (bool canShowError, string message, string title) => API.ShowError (canShowError, message, title);

	#endregion
}