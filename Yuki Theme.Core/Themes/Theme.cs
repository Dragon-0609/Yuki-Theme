// #define CONSOLE_LOGS_ACTIVE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Themes
{
	public class Theme
	{
		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("group")]
		public string Group { get; set; }

		[JsonProperty ("version")]
		public int Version { get; set; }

		[JsonProperty ("hasWallpaper")]
		public bool HasWallpaper { get; set; }

		[JsonProperty ("hasSticker")]
		public bool HasSticker { get; set; }

		[JsonProperty ("wallpaperOpacity")]
		public int WallpaperOpacity { get; set; }

		[JsonProperty ("wallpaperAlign")]
		public int WallpaperAlign { get; set; }

		[JsonProperty ("stickerOpacity")]
		public int StickerOpacity { get; set; }

		[DefaultValue ("null")]
		[JsonProperty ("token", DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Token { get; set; }

		[JsonProperty ("fields")]
		public Dictionary <string, ThemeField> Fields { get; set; }

		[JsonIgnore]
		public bool isDefault;

		[JsonIgnore]
		public string path;

		[JsonIgnore]
		public string fullPath;

		/// <summary>
		/// Used for Downloading
		/// </summary>
		[JsonIgnore]
		public string imagePath;

		/// <summary>
		/// Used for Downloading
		/// </summary>
		[JsonIgnore]
		public string link;

		[JsonIgnore]
		public Alignment align => (Alignment)WallpaperAlign;


		[JsonIgnore]
		public bool IsOld => API.ThemeInfos [Name].isOld;

		public static bool operator == (Theme t1, Theme t2)
		{
			if (t1 is null)
				return t2 is null;
			else if (t2 is null)
				return false;

			bool isEqual = true;
			isEqual = isEqual && t1.Name == t2.Name;
			if (isEqual)
			{
#if CONSOLE_LOGS_ACTIVE
				Console.WriteLine (t1.Name + " A:- " + t1.align + " | " + t2.align);
#endif
				isEqual = t1.WallpaperAlign == t2.WallpaperAlign;
			}

			if (isEqual)
			{
#if CONSOLE_LOGS_ACTIVE
				Console.WriteLine (t1.Name + " Op:- " + t1.WallpaperOpacity + " | " + t2.WallpaperOpacity);
#endif
				isEqual = t1.WallpaperOpacity == t2.WallpaperOpacity;
			}

			if (isEqual)
			{
#if CONSOLE_LOGS_ACTIVE
				Console.WriteLine (t1.Name + " Sop:- " + t1.StickerOpacity + " | " + t2.StickerOpacity);
#endif
				isEqual = t1.StickerOpacity == t2.StickerOpacity;
			}

			if (isEqual)
			{
				isEqual = t1.Fields.ContentEquals (t2.Fields);
			}

			return isEqual;
		}

		public static bool operator != (Theme t1, Theme t2)
		{
			return !(t1 == t2);
		}

		public void ParseWallpaperAlign (string target)
		{
			target = target.ToLower ();
			if (target == "left") WallpaperAlign = (int)Alignment.Left;
			else if (target == "center") WallpaperAlign = (int)Alignment.Center;
			else WallpaperAlign = (int)Alignment.Right;
		}
	}

	public static class ThemeFunctions
	{
		public static bool IsZip (this Theme theme)
		{
			return theme.HasWallpaper || theme.HasWallpaper;
		}

		public static Theme LoadDefault ()
		{
			Theme theme = new Theme ();
			theme.WallpaperAlign = 1;
			theme.StickerOpacity = 100;
			theme.WallpaperOpacity = 15;
			return theme;
		}

		public static void SetAdditionalInfo (this Theme theme, Dictionary <string, string> additionalInfo)
		{
			theme.WallpaperAlign = int.Parse (additionalInfo ["align"]);
			theme.WallpaperOpacity = int.Parse (additionalInfo ["opacity"]);
			theme.StickerOpacity = int.Parse (additionalInfo ["stickerOpacity"]);
			theme.Token = additionalInfo ["token"];
			if (additionalInfo.ContainsKey ("group"))
				theme.Group = additionalInfo ["group"];
		}

		public static bool ContentEquals (this Dictionary <string, ThemeField> dictionary,
		                                  Dictionary <string, ThemeField>      otherDictionary)
		{
			bool equality = true;

			/*
			foreach (KeyValuePair <string, ThemeField> pair in dictionary)
			{
				Console.WriteLine ("FIRST: {0} => {1}", pair.Key, pair.Value);
			}*/
			Dictionary <string, ThemeField> localDictionary;
			if (Settings.settingMode == SettingMode.Light)
			{
				localDictionary = new Dictionary <string, ThemeField> ();
				foreach (KeyValuePair <string, ThemeField> pair in dictionary)
				{
					string shadowName = ShadowNames.GetShadowName (pair.Key, SyntaxType.Pascal, true);
					if (!localDictionary.ContainsKey (shadowName))
					{
						localDictionary.Add (shadowName, pair.Value);
					}
				}
			} else
			{
				localDictionary = dictionary;
			}

			foreach (KeyValuePair <string, ThemeField> pair in localDictionary)
			{
				if (otherDictionary.ContainsKey (pair.Key))
				{
					equality = pair.Value.IsEqual (otherDictionary [pair.Key]);
				} else
				{
					equality = false;
				}
#if CONSOLE_LOGS_ACTIVE
				Console.WriteLine ("{0} -> {1} == {2} => {3}", pair.Key, pair.Value,
				                   otherDictionary.ContainsKey (pair.Key) ? otherDictionary [pair.Key] : "null", equality);
#endif
				if (!equality)
					break;
			}

			return equality;
		}
	}
}