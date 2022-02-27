using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yuki_Theme.Core
{
	public class Theme
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		
		[JsonProperty("group")]
		public string Group { get; set; }
		
		[JsonProperty("version")]
		public int Version { get; set; }
		
		[JsonProperty("hasWallpaper")]
		public bool HasWallpaper { get; set; }
		
		[JsonProperty("hasSticker")]
		public bool HasSticker { get; set; }
		
		[JsonProperty("wallpaperOpacity")]
		public int WallpaperOpacity { get; set; }
		
		[JsonProperty("wallpaperAlign")]
		public int WallpaperAlign { get; set; }
		
		[JsonProperty("stickerOpacity")]
		public int StickerOpacity   { get; set; }

		[JsonProperty("fields")]
		public Dictionary <string, ThemeField> Fields { get; set; }

		[JsonIgnore]
		public bool isDefault;
		
		[JsonIgnore]
		public string path;
		
		[JsonIgnore]
		public string fullPath;

		[JsonIgnore]
		public Alignment align => (Alignment) WallpaperAlign;


		[JsonIgnore]
		public bool IsOld => CLI.oldThemeList [Name];

	}
	
	public static class ThemeFunctions {
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
		}

	}
}