using System.Collections.Generic;
using YukiTheme.Tools.ThemeDefinitions;

namespace YukiTheme.Tools
{
	public class YukiThemeCollector
	{
		private Dictionary<string, string> _keys = new();

		public string Get(Theme theme)
		{
			Dictionary<string, string> keys = GetKeys(theme);

			return ReplaceKeys(_keys);
		}

		private Dictionary<string, string> GetKeys(Theme theme)
		{
			_keys.Clear();

			_keys.Add("NAME", theme.Name);
			_keys.Add("ALIGN", theme.WallpaperAlign.ToString());
			_keys.Add("OPACITY", theme.WallpaperOpacity.ToString());
			_keys.Add("SOPACITY", theme.StickerOpacity.ToString());
			_keys.Add("WALLPAPER", theme.HasWallpaper.ToString());
			_keys.Add("STICKER", theme.HasSticker.ToString());

			foreach (KeyValuePair<string, ThemeField> pair in theme.Fields)
			{
				string key = pair.Key;
				ThemeField field = pair.Value;
				if (key == "Default Text")
				{
					_keys.Add("FOREGROUND", field.Foreground);
					_keys.Add("BACKGROUND", field.Background);
				}
				else if (key == "Selection")
				{
					_keys.Add("SELECTION", field.Background);
				}
				else if (key == "Vertical Ruler")
				{
					_keys.Add("VERTICAL_RULER", field.Foreground);
				}
				else if (key == "Caret")
				{
					_keys.Add("ACCENT", field.Foreground);
				}
				else if (key == "Line Number")
				{
					_keys.Add("LINE_COLOR", field.Foreground);
					_keys.Add("LINE_BG", field.Background);
				}
				else if (key == "Fold's Line")
				{
					_keys.Add("FOLD_LINE", field.Foreground);
				}
				else if (key == "Fold's Rectangle")
				{
					_keys.Add("FOLD_COLOR", field.Foreground);
					_keys.Add("FOLD_BG", field.Background);
				}
				else if (key == "Selected Fold's Line")
				{
					_keys.Add("SELECTED_FOLDS_COLOR", field.Foreground);
				}
				else if (key == "Other Marker")
				{
					_keys.Add("MARKER", field.Foreground);
				}
				else if (key == "Number")
				{
					_keys.Add("DIGITS", field.Foreground);
				}
				else if (key == "Comment")
				{
					_keys.Add("COMMENT", field.Foreground);
				}
				else if (key == "String")
				{
					_keys.Add("STRING", field.Foreground);
				}
				else if (key == "Method")
				{
					_keys.Add("METHOD", field.Foreground);
				}
				else if (key == "Keyword")
				{
					_keys.Add("KEYWORD", field.Foreground);
				}
				else if (key == "If, else Statements")
				{
					_keys.Add("IFELSE", field.Foreground);
				}
				else if (key == "Iteration Statements")
				{
					_keys.Add("ITERATIONS", field.Foreground);
				}
				else if (key == "Begin, End")
				{
					_keys.Add("KEYS", field.Foreground);
				}
			}

			return _keys;
		}

		private string ReplaceKeys(Dictionary<string, string> keys)
		{
			var template = GetTemplate();

			template = template.Replace(keys);

			return template;
		}

		private string GetTemplate()
		{
			return ResourceHelper.LoadString("YukiTemplate.xshd", "Templates.");
		}
	}
}