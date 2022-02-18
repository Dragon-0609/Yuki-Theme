using System.Collections.Generic;
using System.Linq;

namespace Yuki_Theme.Core.Themes
{
	public class DefaultThemes
	{
		public static string [] def
		{
			get
			{
				return new string []
				{
					"Darcula",
					"Dracula",
					"Github Dark",
					"Github Light",
					"Monokai Dark",
					"Monokai Light",
					"Nightshade",
					"Oblivion",
					"Shades of Purple",
					"AzurLane: Essex",
					"BlendS: Maika",
					"BunnySenpai: Mai Dark",
					"BunnySenpai: Mai Light",
					"Chuunibyou: Takanashi Rikka",
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
					"Franxx: Zero Two Dark",
					"Franxx: Zero Two Light",
					"FutureDiary: Gasai Yuno",
					"Gate: Rory Mercury",
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
					"OreGairu: Yukinoshita Yukino",
					"OreImo: Kirino",
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
					"SG: Makise Kurisu",
					"ShieldHero: Raphtalia",
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
		}

		public static bool isDefault (string str)
		{
			return def.Contains (str);
		}

		public static void addDefaultThemes (ref Dictionary <string, bool> themes)
		{
			foreach (string themeName in def)
			{
				themes.Add (themeName, true);
			}
		}

		public static string getCategory (string st)
		{
			string res = "Custom";
			switch (st)
			{
				case "Darcula" :
				case "Dracula" :
				case "Github Dark" :
				case "Github Light" :
				case "Monokai Dark" :
				case "Monokai Light" :
				case "Nightshade" :
				case "Oblivion" :
				case "Shades of Purple" :
				{
					res = "Default";
				}
					break;

				case "AzurLane: Essex" :
				case "BlendS: Maika" :
				case "BunnySenpai: Mai Dark" :
				case "BunnySenpai: Mai Light" :
				case "Chuunibyou: Takanashi Rikka" :
				case "DDLC: Monika Dark" :
				case "DDLC: Monika Light" :
				case "DDLC: Natsuki Dark" :
				case "DDLC: Natsuki Light" :
				case "DDLC: Sayori Dark" :
				case "DDLC: Sayori Light" :
				case "DDLC: Yuri Dark" :
				case "DDLC: Yuri Light" :
				case "DM: Kanna" :
				case "DM: Tohru" :
				case "DR: Mioda Ibuki Dark" :
				case "DR: Mioda Ibuki Light" :
				case "DTWMMN: Hayase Nagatoro" :
				case "DxD: Rias Crimson" :
				case "DxD: Rias Onyx" :
				case "EroManga: Sagiri" :
				case "EVA: Katsuragi Misato" :
				case "EVA: Rei" :
				case "Franxx: Zero Two Dark" :
				case "Franxx: Zero Two Light" :
				case "FutureDiary: Gasai Yuno" :
				case "Gate: Rory Mercury" :
				case "JahySama: Jahy" :
				case "Kakegurui: Jabami Yumeko" :
				case "KillLaKill: Ryuko Dark" :
				case "KillLaKill: Ryuko Light" :
				case "KillLaKill: Satsuki Dark" :
				case "KillLaKill: Satsuki Light" :
				case "KonoSuba: Aqua" :
				case "KonoSuba: Darkness Dark" :
				case "KonoSuba: Darkness Light" :
				case "KonoSuba: Megumin" :
				case "LoveLive: Sonoda Umi" :
				case "LS: Konata" :
				case "Monogatari: Hanekawa Tsubasa" :
				case "MonsterMusume: Miia" :
				case "NekoPara: Azuki" :
				case "NekoPara: Chocola" :
				case "NekoPara: Christmas Chocola" :
				case "NekoPara: Cinnamon" :
				case "NekoPara: Coconut" :
				case "NekoPara: Maple Dark" :
				case "NekoPara: Maple Light" :
				case "NekoPara: Shigure" :
				case "NekoPara: Vanilla" :
				case "OreGairu: Yukinoshita Yukino" :
				case "OreImo: Kirino" :
				case "QQ: Nakano Miku" :
				case "QQ: Nakano Nino" :
				case "QQ: Nakano Yotsuba" :
				case "Railgun: Misaka Mikoto" :
				case "Re:Zero: Beatrice" :
				case "Re:Zero: Echidna" :
				case "Re:Zero: Emilia Dark" :
				case "Re:Zero: Emilia Light" :
				case "Re:Zero: Ram" :
				case "Re:Zero: Rem" :
				case "SAO: Asuna Dark" :
				case "SAO: Asuna Light" :
				case "SG: Makise Kurisu" :
				case "ShieldHero: Raphtalia" :
				case "TypeMoon: Astolfo" :
				case "TypeMoon: Gray" :
				case "TypeMoon: Ishtar Dark" :
				case "TypeMoon: Ishtar Light" :
				case "TypeMoon: Tohsaka Rin" :
				case "Vocaloid: Hatsune Miku" :
				case "YuruCamp: Nadeshiko" :
				case "YuruCamp: Shima Rin" :
				{
					res = "Doki Theme";
				}
					break;
			}

			return res;
		}
	}
}