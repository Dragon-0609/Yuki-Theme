using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace YukiUtility
{
    internal class Program
    {
        private static string GetCurrentFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Main()
        {
            string themeExtension = "theme.xshd";
            string stickerExtension = "sticker.png";
            string backgroundExtension = "background.png";

            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "YukiTheme");

            string theme = Path.Combine(folder, themeExtension);
            if (File.Exists(theme))
            {
                string[] strings = Directory.GetFiles(Path.Combine(GetCurrentFolder, "Highlighting"), "*.xshd");
                if (strings.Length > 0)
                {
                    foreach (string s in strings)
                    {
                        File.Delete(s);
                    }
                }

                DeleteFromHighlighting(themeExtension);
                CreateSymbolicLink(GetHighlighting(themeExtension), theme, SymbolicLink.File);
            }

            string background = Path.Combine(folder, backgroundExtension);
            if (File.Exists(background))
            {
                DeleteFromHighlighting(backgroundExtension);
                CreateSymbolicLink(GetHighlighting(backgroundExtension), background, SymbolicLink.File);
            }

            string sticker = Path.Combine(folder, stickerExtension);
            if (File.Exists(sticker))
            {
                DeleteFromHighlighting(stickerExtension);
                CreateSymbolicLink(GetHighlighting(stickerExtension), sticker, SymbolicLink.File);
            }
        }

        private static void DeleteFromHighlighting(string fileName)
        {
            if (File.Exists(GetHighlighting(fileName)))
            {
                File.Delete(GetHighlighting(fileName));
            }
        }

        private static string GetHighlighting(string file)
        {
            return Path.Combine(GetCurrentFolder, "Highlighting", file);
        }

        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(
            string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }
    }
}