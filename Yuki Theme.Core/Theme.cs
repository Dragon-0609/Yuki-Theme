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
	}
}