using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class DokiExporter
{
	public static DokiExporter Instance;

	private string _themeToExport;

	public DokiExporter()
	{
		Instance = this;
	}

	public void Export(string name)
	{
		_themeToExport = name;
		ExportTheme();
		ExportWallpaper();
		ExportSticker();
	}

	private void ExportTheme()
	{
	}

	private void ExportWallpaper()
	{
	}

	private void ExportSticker()
	{
	}
}