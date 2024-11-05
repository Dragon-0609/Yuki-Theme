using System.Linq;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Export;

public class PlainExporter : Exporter
{
	protected override Exporter Next { get; } = new ExternalExporter();

	protected override bool HasTheme(string name)
	{
		return DefaultThemeNames.Themes.Contains(name);
	}

	protected override void ExportTheme(string name)
	{
		ExportDefinition(name);
		ClearWallpaperAndSticker();
	}

	private void ExportDefinition(string name)
	{
		var content = ResourceHelper.LoadString($"{name}.xshd", "Themes.Default.");
		ExportThemeFile(content);
	}

	private void ClearWallpaperAndSticker()
	{
		DeleteIfExist(BackgroundName);
		DeleteIfExist(StickerName);

		LinkCreator.CopyDefault(BackgroundName, _folder);
		LinkCreator.CopyDefault(StickerName, _folder);
	}
}