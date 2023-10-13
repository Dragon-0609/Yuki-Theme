using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace YukiTheme.Tools;

public class LinkCreator
{
    private string _folder;

    public bool CheckLinks()
    {
        _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "YukiTheme");

        if (!Directory.Exists(_folder))
        {
            Directory.CreateDirectory(_folder);
        }

        string themeExtension = "theme.xshd";
        string stickerExtension = "sticker.png";
        string backgroundExtension = "background.png";
        CheckFiles();
        if (IsValid(themeExtension, false) && IsValid(backgroundExtension, true) && IsValid(stickerExtension, true))
        {
            return false;
        }

        RunUtility();

        return true;
    }

    private bool IsValid(string nameAndExtension, bool isImage)
    {
        string path = GetHighlighting(nameAndExtension);
        bool valid = true;

        if (isImage)
        {
            try
            {
                Image.FromFile(path).Dispose();
            }
            catch
            {
                valid = false;
            }
        }
        else
        {
            try
            {
                File.ReadAllText(path);
            }
            catch
            {
                valid = false;
            }
        }

        return valid;
    }

    private void CheckFiles()
    {
        string themeExtension = "theme.xshd";
        string stickerExtension = "sticker.png";
        string backgroundExtension = "background.png";

        string theme = Path.Combine(_folder, themeExtension);
        if (!File.Exists(theme))
        {
            CopyDefault(themeExtension);
        }

        string background = Path.Combine(_folder, backgroundExtension);
        if (!File.Exists(background))
        {
            CopyDefault(backgroundExtension);
        }

        string sticker = Path.Combine(_folder, stickerExtension);
        if (!File.Exists(sticker))
        {
            CopyDefault(stickerExtension);
        }
    }

    private void CopyDefault(string file)
    {
        string resource = $"YukiTheme.Resources.Default.{file}";

        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

        using (var fileStream = File.Create(Path.Combine(_folder, file)))
        {
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            stream.Close();
        }
    }

    private void RunUtility()
    {
        var psi = new ProcessStartInfo(Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "YukiUtility.exe"));
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        Process.Start(psi).WaitForExit();
    }

    private static string GetHighlighting(string file)
    {
        return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "Highlighting", file);
    }
}