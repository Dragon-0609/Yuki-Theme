using System;
using System.IO;

namespace YukiTheme.Engine;

public abstract class Exporter
{
    protected readonly string _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "YukiTheme");


    protected readonly string StickerName = "sticker.png";
    protected readonly string BackgroundName = "background.png";

    protected abstract Exporter Next { get; }

    public void Export(string name)
    {
        if (HasTheme(name)) ExportTheme(name);
        else
        {
            if (Next == null)
            {
                throw new NullReferenceException($"Exporter for {Next} not found");
            }

            Next.Export(name);
        }
    }

    protected abstract bool HasTheme(string name);

    protected abstract void ExportTheme(string name);

    protected void ExportThemeFile(string content)
    {
        string theme = Path.Combine(_folder, "theme.xshd");
        if (File.Exists(theme))
        {
            File.Delete(theme);
        }

        File.WriteAllText(theme, content);
    }

    protected void DeleteIfExist(string fileName)
    {
        IDEAlterer.ReleaseImages();
        string path = Path.Combine(_folder, fileName);
        if (File.Exists(path))
            File.Delete(path);
    }
}