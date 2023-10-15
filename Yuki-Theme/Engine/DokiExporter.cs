using System;
using System.IO;
using System.Linq;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class DokiExporter
{
    public static DokiExporter Instance;

    private string _themeToExport;
    private DokiThemeCollector _themeCollector;
    private string _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "YukiTheme");

    public DokiExporter()
    {
        Instance = this;
        _themeCollector = new DokiThemeCollector();
        PluginEvents.Instance.ThemeChanged += ThemeChanged;
    }

    private void ThemeChanged(string name)
    {
        Export(Normalize(name));
    }

    private string Normalize(string name)
    {
        string output = name;
        if (name.Contains(": "))
        {
            output = name.Split(new[] { ": " }, StringSplitOptions.None).Last();
        }

        return output;
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
        string content = _themeCollector.Get(_themeToExport);
        string theme = Path.Combine(_folder, "theme.xshd");
        if (File.Exists(theme))
        {
            File.Delete(theme);
        }

        File.WriteAllText(theme, content);
    }


    private void ExportWallpaper()
    {
    }

    private void ExportSticker()
    {
    }
}