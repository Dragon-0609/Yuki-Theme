    using System;
using System.IO;
using System.Linq;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Export;

public class DokiExporter : Exporter
{
    private string _themeToExport;
    private DokiThemeCollector _themeCollector;


    public DokiExporter()
    {
        _themeCollector = new DokiThemeCollector();
        ExportListener.Instance.SetExporter(this);
    }

    protected override Exporter Next { get; } = new PlainExporter();

    protected override bool HasTheme(string name) => DokiThemeNames.Themes.Contains(name);

    protected override void ExportTheme(string name)
    {
        ExportFiles(Normalize(name));
    }

    public static string Normalize(string name)
    {
        string output = name;
        if (name.Contains(": "))
        {
            output = name.Split(new[] { ": " }, StringSplitOptions.None).Last();
        }

        return output;
    }

    private void ExportFiles(string name)
    {
        _themeToExport = name;
        ExportTheme();
        ExportWallpaper();
        ExportSticker();
    }

    private void ExportTheme()
    {
        string content = _themeCollector.Get(_themeToExport);
        ExportThemeFile(content);
    }


    private void ExportWallpaper()
    {
        DeleteIfExist(BackgroundName);
        ResourceHelper.Save(Path.Combine(_folder, BackgroundName), $"{_themeToExport}.png", "Wallpapers.");
    }

    private void ExportSticker()
    {
        DeleteIfExist(StickerName);
        ResourceHelper.Save(Path.Combine(_folder, StickerName), $"{_themeToExport}.png", "Stickers.");
    }
}