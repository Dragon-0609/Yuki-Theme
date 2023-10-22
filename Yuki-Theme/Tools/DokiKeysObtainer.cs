using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace YukiTheme.Tools;

public static class DokiKeysObtainer
{
	public delegate void ParseKey(string key, string color);

	private static readonly Dictionary<string, string> GroupNames = new()
	{
		{ "Azur Lane", "AzurLane: " },
		{ "Kill la Kill", "KillLaKill: " },
		{ "Blend S", "BlendS: " },
		{ "Guilty Crown", "GuiltyCrown: " },
		{ "Code Geass", "CodeGeass: " },
		{ "Helpful Fox Senko-san", "Senko-san: " },
		{ "Charlotte", "Charlotte: " },
		{ "Toaru Majutsu no Index", "Railgun: " },
		{ "The Rising of Shield Hero", "ShieldHero: " },
		{ "Chuunibyou", "Chuunibyou: " },
		{ "Re Zero", "Re:Zero: " },
		{ "One Punch Man", "OPM: " },
		{ "Shokugeki no Soma", "Shokugeki: " },
		{ "Haikyu!!", "Haikyu: " },
		{ "That Time I Get ReIncarnated As A Slime", "Slime: " },
		{ "Love Live", "LoveLive: " },
		{ "Literature Club", "DDLC: " },
		{ "KonoSuba", "KonoSuba: " },
		{ "Darling in the Franxx", "Franxx: " },
		{ "Bunny Senpai", "BunnySenpai: " },
		{ "Steins Gate", "SG: " },
		{ "Gate", "Gate: " },
		{ "Quintessential Quintuplets", "QQ: " },
		{ "Fate", "TypeMoon: " },
		{ "Type-Moon", "TypeMoon: " },
		{ "Daily Life With A Monster Girl", "MonsterMusume: " },
		{ "Vocaloid", "Vocaloid: " },
		{ "DanganRonpa", "DR: " },
		{ "High School DxD", "DxD: " },
		{ "Sword Art Online", "SAO: " },
		{ "Lucky Star", "LS: " },
		{ "Evangelion", "EVA: " },
		{ "EroManga Sensei", "EroManga: " },
		{ "Miss Kobayashi's Dragon Maid", "DM: " },
		{ "OreGairu", "OreGairu: " },
		{ "OreImo", "OreImo: " },
		{ "The Great Jahy Will Not Be Defeated", "JahySama: " },
		{ "Future Diary", "FutureDiary: " },
		{ "Kakegurui", "Kakegurui: " },
		{ "Monogatari", "Monogatari: " },
		{ "Don't Toy with me Miss Nagatoro", "DTWMMN: " },
		{ "Miscellaneous", "Misc: " },
		{ "Yuru Camp", "YuruCamp: " },
		{ "NekoPara", "NekoPara: " },
	};

	private static string ConvertGroup(string st)
	{
		string res = st;
		if (GroupNames.ContainsKey(st))
			res = GroupNames[st];
		return res;
	}


	public static void Obtain(string themeName, ParseKey parse)
	{
		string content = ResourceHelper.LoadString($"{themeName}.json", "Themes.Doki.");

		JObject json = JObject.Parse(content);

		json.Validate("Json is null");

		string name = GetThemeName(json);
		parse("name", name);

		parse("align", json["stickers"]?["default"]?["anchor"]?.ToString());
		parse("opacity", json["stickers"]?["default"]?["opacity"]?.ToString());

		bool isDark = IsDark(json);

		AddDefaults(isDark, parse);

		JToken colorsToken = json["colors"];
		colorsToken.Validate("Colors not found");

		foreach (JToken colorToken in colorsToken!)
		{
			ParseColor(colorToken, parse);
		}
	}

	private static bool IsDark(JObject json)
	{
		return bool.Parse(json["dark"]?.ToString() ?? "false");
	}

	public static string GetThemeName(JObject json)
	{
		return ConvertGroup(json["group"]?.ToString()) + json["name"];
	}


	private static void AddDefaults(bool isDark, ParseKey parse)
	{
		parse("foregroundColor", isDark ? "#F8F8F2" : "#4D4D4A");
		parse("constantColor", isDark ? "#86dbfd" : "#4C94D6");
		parse("comments", isDark ? "#6272a4" : "#6a737d");
	}

	private static void ParseColor(JToken colorToken, ParseKey parse)
	{
		JProperty color = (JProperty)colorToken;
		parse(color.Name, color.Value.ToString());
	}

	private static void Validate(this JToken token, string message)
	{
		if (token == null) throw new NullReferenceException(message);
	}
}