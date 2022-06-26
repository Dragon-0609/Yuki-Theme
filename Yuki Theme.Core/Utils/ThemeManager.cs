using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Yuki_Theme.Core.Formats;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Utils;

internal class ThemeManager
{
	private API_Actions _actions;

	internal API_Actions SetActionsAPI
	{
		set => _actions = value;
	}
	
	public Theme GetTheme (string name)
	{
		if (API.ThemeInfos.ContainsKey (name))
		{
			Theme theme = API.ThemeInfos [name].isOld
				? OldThemeFormat.populateList (name, false)
				: NewThemeFormat.populateList (name, false);

			return theme;
		} else
		{
			return null;
		}
	}

	internal Stream GetStreamFromMemory (string file, string name)
	{
		IThemeHeader header = DefaultThemes.headers [name];
		Assembly a = header.Location;
		if (file.Contains (":"))
		{
			file = Helper.ConvertNameToPath (file);
		}

		string ext = Helper.GetExtension (API.ThemeInfos [name].isOld);
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
	internal void CopyFromMemory (string file, string name, string path, bool extract = false)
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
				_actions.CleanDestination ();
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
	private void ExportTheme (string path)
	{
		string source = API.currentTheme.fullPath;
		bool iszip = Helper.IsZip (source);
		if (!iszip)
		{
			// File.Copy (source, path, true);
			return;
		}

		_actions.CleanDestination ();

		Tuple <bool, Image> img = Helper.GetImage (source);
		Tuple <bool, Image> sticker = Helper.GetSticker (source);

		Helper.ExtractZip (source, path, img.Item1, sticker.Item1, false);
	}
	
	/// <summary>
	/// Write name of the theme to the theme file (.xshd), so Yuki Theme can show it properly (symbols like ':')
	/// </summary>
	/// <param name="path">Full path to theme</param>
	/// <param name="name">New name of the theme</param>
	internal void WriteName (string path, string name)
	{
		if (_actions.IsOldTheme (path))
		{
			OldThemeFormat.WriteName (path, name);
		} else
		{
			NewThemeFormat.WriteName (path, name);
		}
	}

	internal int AddTheme (string copyFrom, string name, string destination, bool exist)
	{
		if (API.CopyTheme (copyFrom, name, destination, out _, true)) return 0;
		if (!exist)
		{
			API.Schemes.Add (name);
			_actions.AddThemeInfo (
				name,
				new ThemeInfo (false, API.ThemeInfos [copyFrom].isOld, ThemeLocation.File, API.Translate ("messages.theme.group.custom")));
		}

		if (Helper.mode == ProductMode.CLI)
			if (API_Events.showSuccess != null)
				API_Events.showSuccess (API.Translate ("messages.theme.duplicate"), API.Translate ("messages.buttons.done"));

		return exist ? 2 : 1;
	}

	internal void DeleteOldThemeIfNeed ()
	{
		string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
		if (files is { Length: > 0 })
		{
			string [] unknownThemes = IdentifySyntaxHighlightings (files);
			// Console.WriteLine ("UNKNOWN: " + unknownThemes.Length);
			if (unknownThemes.Length == 0)
			{
				_actions.DeleteFiles (files);
			} else
			{
				int result = 2;
				result = _actions.GetActionChoice (result);

				if (result != 2)
				{
					if (result == 1) _actions.CopyFiles (unknownThemes);
					_actions.DeleteFiles (unknownThemes);
				}
			}

			files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.png");
			_actions.DeleteFiles (files);
		}
	}

	internal void DeleteOldTheme ()
	{
		string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
		if (files is { Length: > 0 })
		{
			_actions.DeleteFiles (files);
		}
	}


	internal void CopyThemeToDirectory (string path)
	{
		if (API.currentTheme.isDefault && API.ThemeInfos [API.currentTheme.Name].location == ThemeLocation.Memory)
		{
			CopyFromMemory (API.currentTheme.path, API.currentTheme.Name, path, true);
		} else
		{
			ExportTheme (path);
		}
	}	
	
	private string [] IdentifySyntaxHighlightings (string [] files)
	{
		List <string> unknownThemes = new List <string> ();
		foreach (string file in files)
		{
			string name = OldThemeFormat.GetNameOfTheme (file);
			if (name.Length > 0)
			{
				if (!API.Schemes.Contains (name))
				{
					unknownThemes.Add (file);
				}
			}
		}

		return unknownThemes.ToArray ();
	}
	
}