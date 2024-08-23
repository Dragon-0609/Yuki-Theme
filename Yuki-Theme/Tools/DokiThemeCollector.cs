using System.Collections.Generic;

namespace YukiTheme.Tools;

public class DokiThemeCollector
{
	public string Get(string name)
	{
		var keys = GetKeys(name);
		return ReplaceKeys(keys);
	}

	private Dictionary<string, string> GetKeys(string name)
	{
		var keys = new Dictionary<string, string>();
		var colors = new Dictionary<string, string>();

		DokiKeysObtainer.Obtain(name, (key, color) => keys[key] = color);

		foreach (var pair in keys) colors.Add(GetTranslation(pair.Key).ToUpper(), pair.Value);

		return colors;
	}


	private string GetTranslation(string key)
	{
		return DokiKeysTranslator.GetTranslation(key);
	}

	private string ReplaceKeys(Dictionary<string, string> keys)
	{
		var template = GetTemplate();

		template = template.Replace(keys);

		return template;
	}

	private string GetTemplate()
	{
		return ResourceHelper.LoadString("Template.xshd", "Templates.");
	}
}