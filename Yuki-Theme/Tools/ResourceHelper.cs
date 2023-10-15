using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace YukiTheme.Tools;

public static class ResourceHelper
{
    public static Image LoadImage(string name, string nameSpace = "Icons.")
    {
        Stream stream = GetResource(name, nameSpace);
        stream.Validate(GetResourcePath(name, nameSpace));

        return Image.FromStream(stream);
    }
    public static string LoadString(string name, string nameSpace = "Icons.")
    {
        Stream stream = GetResource(name, nameSpace);
        stream.Validate($"YukiTheme.Resources.{nameSpace}{name}");

        string output = "";
        using (StreamReader reader = new StreamReader(stream))
        {
            output = reader.ReadToEnd();
        }

        return output;
    }

    private static void Validate(this Stream stream, string name)
    {
        if (stream == null) throw new NullReferenceException($"File not found: {name}");
    }


    public static void Save(string path, string name, string nameSpace)
    {
        using (Stream stream = GetResource(name, nameSpace))
        {
            stream.Validate(GetResourcePath(name, nameSpace));
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream file = File.Create(path))
            {
                file.Position = 0;
                stream.CopyTo(file);
            }
        }
    }

    private static Stream GetResource(string name, string nameSpace)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetResourcePath(name, nameSpace));
    }

    private static string GetResourcePath(string name, string nameSpace)
    {
        return $"YukiTheme.Resources.{nameSpace}{name}";
    }

}