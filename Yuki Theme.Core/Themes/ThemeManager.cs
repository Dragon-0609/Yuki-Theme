using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Themes
{
	internal class ThemeManager
	{
		private API_Actions     _actions;
		private ThemeFormatBase _newThemeFormat;
		private ThemeFormatBase _oldThemeFormat;
	

		internal void SetActionsAPI (API_Actions value) => _actions = value;
	
		internal void SetNewThemeFormat (ThemeFormatBase value) => _newThemeFormat = value;
	
		internal void SetOldThemeFormat (ThemeFormatBase value) => _oldThemeFormat = value;
	
	
		public Theme GetTheme (string name)
		{
			if (CentralAPI.Current.ThemeInfos.ContainsKey (name))
			{
				Theme theme = CentralAPI.Current.ThemeInfos [name].isOld
					? _oldThemeFormat.PopulateList (name, false)
					: _newThemeFormat.PopulateList (name, false);

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

			string ext = Helper.GetExtension (CentralAPI.Current.ThemeInfos [name].isOld);
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
				if (ZipManager.IsZip (stream))
				{
					_actions.CleanDestination ();
					Tuple<bool, Image> img = Helper.GetImage (nxp);
					Tuple<bool, Image> sticker = Helper.GetSticker (nxp);

					ZipManager.ExtractZip (nxp, path, img.Item1, sticker.Item1, false);
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
			string source = CentralAPI.Current.currentTheme.fullPath;
			bool iszip = ZipManager.IsZip (source);
			if (!iszip)
			{
				// File.Copy (source, path, true);
				return;
			}

			_actions.CleanDestination ();

			Tuple<bool, Image> img = Helper.GetImage (source);
			Tuple<bool, Image> sticker = Helper.GetSticker (source);

			ZipManager.ExtractZip (source, path, img.Item1, sticker.Item1, false);
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
				_oldThemeFormat.WriteName (path, name);
			} else
			{
				_newThemeFormat.WriteName (path, name);
			}
		}

		internal int AddTheme (string copyFrom, string name, string destination, bool exist)
		{
			CopyTheme (copyFrom, name, destination, true);
			if (!exist)
			{
				CentralAPI.Current.Schemes.Add (name);
				_actions.AddThemeInfo ( name,
					new ThemeInfo (false, CentralAPI.Current.ThemeInfos [copyFrom].isOld, ThemeLocation.File, CentralAPI.Current.Translate ("messages.theme.group.custom")));
			}

			if (Helper.mode == ProductMode.CLI)
				if (API_Events.showSuccess != null)
					API_Events.showSuccess (CentralAPI.Current.Translate ("messages.theme.duplicate"), CentralAPI.Current.Translate ("messages.buttons.done"));

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
			if (CentralAPI.Current.currentTheme.isDefault && CentralAPI.Current.ThemeInfos [CentralAPI.Current.currentTheme.Name].location == ThemeLocation.Memory)
			{
				CopyFromMemory (CentralAPI.Current.currentTheme.path, CentralAPI.Current.currentTheme.Name, path, true);
			} else
			{
				ExportTheme (path);
			}
		}	
	
		private string [] IdentifySyntaxHighlightings (string [] files)
		{
			List<string> unknownThemes = new List<string> ();
			foreach (string file in files)
			{
				string name = _oldThemeFormat.GetNameOfTheme (file);
				if (name.Length > 0)
				{
					if (!CentralAPI.Current.Schemes.Contains (name))
					{
						unknownThemes.Add (file);
					}
				}
			}

			return unknownThemes.ToArray ();
		}

		private static void CopyTheme (string copyFrom, string themeName, string destination, bool check)
		{
			if (check && CentralAPI.Current.ThemeInfos [copyFrom].location == ThemeLocation.Memory)
			{
				PathGenerator.PathToMemory (copyFrom);
				CentralAPI.Current._themeManager.CopyFromMemory (copyFrom, copyFrom, destination);
			} else
			{
				string path = PathGenerator.PathToFile (Helper.ConvertNameToPath (copyFrom), CentralAPI.Current.ThemeInfos [copyFrom].isOld);
				File.Copy (path, destination);
			}

			if (destination.EndsWith (Helper.FILE_EXTENSTION_OLD))
			{
				CentralAPI.Current._oldThemeFormat.WriteNameAndResetToken (destination, themeName);
			} else
			{
				CentralAPI.Current._newThemeFormat.WriteNameAndResetToken (destination, themeName);
			}
		}
	}
}