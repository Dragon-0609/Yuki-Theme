using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using ExCSS;
using Newtonsoft.Json;
using YukiTheme.Tools.ThemeDefinitions;

namespace YukiTheme.Tools;

public static class DefaultThemeNames
{
	public static string[] Themes { get; } =
	{
		"Darcula",
		"Dracula",
		"Github Dark",
		"Github Light",
		"Monokai Dark",
		"Monokai Light",
		"Nightshade",
		"Oblivion",
		"Shades of Purple"
	};
}

public static class DokiThemeNames
{
	public static string[] Themes { get; } =
	{
		"AzurLane: Essex",
		"BlendS: Maika",
		"BunnySenpai: Mai Dark",
		"BunnySenpai: Mai Light",
		"Charlotte: Tomori Nao",
		"Chuunibyou: Takanashi Rikka",
		"CodeGeass: C.C.",
		"DDLC: Monika Dark",
		"DDLC: Monika Light",
		"DDLC: Natsuki Dark",
		"DDLC: Natsuki Light",
		"DDLC: Sayori Dark",
		"DDLC: Sayori Light",
		"DDLC: Yuri Dark",
		"DDLC: Yuri Light",
		"DM: Kanna",
		"DM: Tohru",
		"DR: Mioda Ibuki Dark",
		"DR: Mioda Ibuki Light",
		"DTWMMN: Hayase Nagatoro",
		"DxD: Rias Crimson",
		"DxD: Rias Onyx",
		"EroManga: Sagiri",
		"EVA: Katsuragi Misato",
		"EVA: Rei",
		"Franxx: Hiro x Zero Two",
		"Franxx: Hiro",
		"Franxx: Zero Two Dark Obsidian",
		"Franxx: Zero Two Dark Rose",
		"Franxx: Zero Two Light Lily",
		"Franxx: Zero Two Light Sakura",
		"FutureDiary: Gasai Yuno",
		"Gate: Rory Mercury",
		"GuiltyCrown: Yuzuriha Inori",
		"Haikyu: Hinata Shoyo",
		"JahySama: Jahy",
		"Kakegurui: Jabami Yumeko",
		"KillLaKill: Ryuko Dark",
		"KillLaKill: Ryuko Light",
		"KillLaKill: Satsuki Dark",
		"KillLaKill: Satsuki Light",
		"KonoSuba: Aqua",
		"KonoSuba: Darkness Dark",
		"KonoSuba: Darkness Light",
		"KonoSuba: Megumin",
		"LoveLive: Sonoda Umi",
		"LS: Konata",
		"Monogatari: Hanekawa Tsubasa",
		"MonsterMusume: Miia",
		"NekoPara: Azuki",
		"NekoPara: Chocola",
		"NekoPara: Christmas Chocola",
		"NekoPara: Cinnamon",
		"NekoPara: Coconut",
		"NekoPara: Maple Dark",
		"NekoPara: Maple Light",
		"NekoPara: Shigure",
		"NekoPara: Vanilla",
		"OPM: Genos",
		"OreGairu: Yukinoshita Yukino",
		"OreImo: Kirino",
		"QQ: Nakano Ichika",
		"QQ: Nakano Itsuki",
		"QQ: Nakano Miku",
		"QQ: Nakano Nino",
		"QQ: Nakano Yotsuba",
		"Railgun: Misaka Mikoto",
		"Re:Zero: Beatrice",
		"Re:Zero: Echidna",
		"Re:Zero: Emilia Dark",
		"Re:Zero: Emilia Light",
		"Re:Zero: Ram",
		"Re:Zero: Rem",
		"SAO: Asuna Dark",
		"SAO: Asuna Light",
		"Senko-san: Senko",
		"SG: Makise Kurisu",
		"ShieldHero: Raphtalia",
		"Shokugeki: Yukihira Soma",
		"Slime: Rimuru Tempest",
		"TypeMoon: Astolfo",
		"TypeMoon: Gray",
		"TypeMoon: Ishtar Dark",
		"TypeMoon: Ishtar Light",
		"TypeMoon: Tohsaka Rin",
		"Vocaloid: Hatsune Miku",
		"YuruCamp: Nadeshiko",
		"YuruCamp: Shima Rin"
	};
}

public static class YukiThemeNames
{
	public static IReadOnlyList<string> Themes => _themes;
	private static List<string> _themes = new();

	private static Dictionary<string, string> _themeLocations = new();

	static YukiThemeNames()
	{
		Update();
	}

	public static void Update()
	{
		_themes.Clear();
		_themeLocations.Clear();
		string[] paths = ThemesLocation.Paths;
		foreach (string path in paths)
		{
			if (!Directory.Exists(path)) continue;
			string[] themes = Directory.GetFiles(path, "*.yuki", SearchOption.AllDirectories);
			if (themes.Length == 0) continue;

			foreach (string theme in themes)
			{
				string themeFile = Path.Combine(path, theme);
				if (!TryGetThemeName(themeFile, out string name)) continue;

				_themes.Add(name);
				_themeLocations.Add(name, themeFile);
			}
		}
	}

	public static bool TryGetThemeFileLocation(string name, out string path)
	{
		return _themeLocations.TryGetValue(name, out path);
	}

	private static bool TryGetThemeName(string path, out string name)
	{
		name = "";
		if (TryGetThemeContent(path, out var theme))
		{
			name = theme.Name;
			return true;
		}

		return false;
	}

	public static bool TryGetThemeContent(string path, out Theme theme)
	{
		theme = null;
		if (!File.Exists(path)) return false;

		if (ZipHelper.TryGetContent(path, "theme.json", out string content))
		{
			if (string.IsNullOrEmpty(content)) return false;

			try
			{
				theme = JsonConvert.DeserializeObject<Theme>(content);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		return false;
	}
}