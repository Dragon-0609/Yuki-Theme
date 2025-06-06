using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YukiTheme.Engine;
using YukiTheme.Tools;
using YukiTheme.Tools.ThemeDefinitions;

namespace YukiTheme.Export
{
	public class YukiExporter : Exporter
	{
		private string _themeToExport;
		private YukiThemeCollector _themeCollector;

		protected override Exporter Next { get; } = new PlainExporter();

		public YukiExporter()
		{
			_themeCollector = new();
		}


		protected override bool HasTheme(string name)
		{
			YukiThemeNames.Update();
			return YukiThemeNames.Themes.Contains(name);
		}

		protected override void ExportTheme(string name)
		{
			ExportFiles(name);
		}

		private void ExportFiles(string name)
		{
			if (!YukiThemeNames.TryGetThemeFileLocation(name, out _themeToExport))
			{
				MessageBox.Show($"Couldn't find theme file: {name}", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			ExportTheme();
		}

		private void ExportTheme()
		{
			if (!YukiThemeNames.TryGetThemeContent(_themeToExport, out Theme theme)) return;

			var content = _themeCollector.Get(theme);
			ExportThemeFile(content);
			ExportWallpaper();
			ExportSticker();
		}

		private void ExportWallpaper()
		{
			DeleteIfExist(BackgroundName);
			SaveImageToFolder("background.png", Path.Combine(_folder, BackgroundName));
		}

		private void ExportSticker()
		{
			DeleteIfExist(StickerName);
			SaveImageToFolder("sticker.png", Path.Combine(_folder, StickerName));
		}

		private void SaveImageToFolder(string fileName, string savePath)
		{
			if (ZipHelper.TryGetImage(_themeToExport, fileName, out Stream stream))
			{
				using (var file = File.Create(savePath))
				{
					file.Position = 0;
					stream.CopyTo(file);
				}

				stream.Dispose();
			}
		}
	}
}