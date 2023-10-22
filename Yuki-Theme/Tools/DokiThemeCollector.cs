using System.Collections.Generic;

namespace YukiTheme.Tools;

public class DokiThemeCollector
{
    public string Get(string name)
    {
        Dictionary<string, string> keys = GetKeys(name);
        return ReplaceKeys(keys);
    }

    private Dictionary<string, string> GetKeys(string name)
    {
        Dictionary<string, string> keys = new Dictionary<string, string>();
        Dictionary<string, string> colors = new Dictionary<string, string>();

        DokiKeysObtainer.Obtain(name, (key, color) => keys[key] = color);

        foreach (KeyValuePair<string, string> pair in keys)
        {
            colors.Add(GetTranslation(pair.Key).ToUpper(), pair.Value);
        }

        return colors;
    }


    private string GetTranslation(string key) => DokiKeysTranslator.GetTranslation(key);

    private string ReplaceKeys(Dictionary<string, string> keys)
    {
        string template = GetTemplate();

        template = template.Replace(keys);

        return template;
    }

    private string GetTemplate()
    {
        return ResourceHelper.LoadString("Template.xshd", "Templates.");
    }
}