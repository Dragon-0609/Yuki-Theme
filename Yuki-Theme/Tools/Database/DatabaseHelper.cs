using System;
using System.Collections.Generic;
using System.Linq;

namespace YukiTheme.Tools.Database;

public static class DatabaseHelper
{
	public static void LoadData(string data, Dictionary<string, string> dictionary)
	{
		var lines = data.Split('\n');
		foreach (var line in lines)
		{
			var param = line.Split(new[] { " => " }, StringSplitOptions.None);
			if (param.Length == 2) dictionary.Add(param[0], param[1]);
		}
	}

	public static string SaveData(Dictionary<string, string> dictionary)
	{
		var output = "";
		var o = dictionary.Count - 1;
		for (var i = 0; i < dictionary.Count; i++)
		{
			output += $"{dictionary.Keys.ElementAt(i)} => {dictionary.Values.ElementAt(i)}";
			if (i != o) output += "\n";
		}

		return output;
	}
}