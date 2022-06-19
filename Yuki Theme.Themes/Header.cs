using System.Collections.Generic;
using System.Reflection;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Themes
{
	public class Header : IThemeHeader
	{
		public string GroupName => "Doki Theme";

		public Dictionary <string, bool> ThemeNames => new ()
		{
			{ "AzurLane: Essex", true },
			{ "BlendS: Maika", true },
			{ "BunnySenpai: Mai Dark", true },
			{ "BunnySenpai: Mai Light", true },
			{ "Charlotte: Tomori Nao", true },
			{ "Chuunibyou: Takanashi Rikka", true },
			{ "CodeGeass: C.C.", true },
			{ "DDLC: Monika Dark", true },
			{ "DDLC: Monika Light", true },
			{ "DDLC: Natsuki Dark", true },
			{ "DDLC: Natsuki Light", true },
			{ "DDLC: Sayori Dark", true },
			{ "DDLC: Sayori Light", true },
			{ "DDLC: Yuri Dark", true },
			{ "DDLC: Yuri Light", true },
			{ "DM: Kanna", true },
			{ "DM: Tohru", true },
			{ "DR: Mioda Ibuki Dark", true },
			{ "DR: Mioda Ibuki Light", true },
			{ "DTWMMN: Hayase Nagatoro", true },
			{ "DxD: Rias: Crimson", true },
			{ "DxD: Rias: Onyx", true },
			{ "EroManga: Sagiri", true },
			{ "EVA: Katsuragi Misato", true },
			{ "EVA: Rei", true },
			{ "Franxx: Zero Two Dark", true },
			{ "Franxx: Zero Two Light", true },
			{ "FutureDiary: Gasai Yuno", true },
			{ "Gate: Rory Mercury", true },
			{ "GuiltyCrown: Yuzuriha Inori", true },
			{ "Haikyu: Hinata Shoyo", true },
			{ "JahySama: Jahy", true },
			{ "Kakegurui: Jabami Yumeko", true },
			{ "KillLaKill: Ryuko Dark", true },
			{ "KillLaKill: Ryuko Light", true },
			{ "KillLaKill: Satsuki Dark", true },
			{ "KillLaKill: Satsuki Light", true },
			{ "KonoSuba: Aqua", true },
			{ "KonoSuba: Darkness Dark", true },
			{ "KonoSuba: Darkness Light", true },
			{ "KonoSuba: Megumin", true },
			{ "LoveLive: Sonoda Umi", true },
			{ "LS: Konata", true },
			{ "Monogatari: Hanekawa Tsubasa", true },
			{ "MonsterMusume: Miia", true },
			{ "NekoPara: Azuki", true },
			{ "NekoPara: Chocola", true },
			{ "NekoPara: Christmas Chocola", true },
			{ "NekoPara: Cinnamon", true },
			{ "NekoPara: Coconut", true },
			{ "NekoPara: Maple Dark", true },
			{ "NekoPara: Maple Light", true },
			{ "NekoPara: Shigure", true },
			{ "NekoPara: Vanilla", true },
			{ "OPM: Genos", true },
			{ "OreGairu: Yukinoshita Yukino", true },
			{ "OreImo: Kirino", true },
			{ "QQ: Nakano Ichika", true },
			{ "QQ: Nakano Itsuki", true },
			{ "QQ: Nakano Miku", true },
			{ "QQ: Nakano Nino", true },
			{ "QQ: Nakano Yotsuba", true },
			{ "Railgun: Misaka Mikoto", true },
			{ "Re:Zero: Beatrice", true },
			{ "Re:Zero: Echidna", true },
			{ "Re:Zero: Emilia Dark", true },
			{ "Re:Zero: Emilia Light", true },
			{ "Re:Zero: Ram", true },
			{ "Re:Zero: Rem", true },
			{ "SAO: Asuna Dark", true },
			{ "SAO: Asuna Light", true },
			{ "Senko-san: Senko", true },
			{ "SG: Makise Kurisu", true },
			{ "ShieldHero: Raphtalia", true },
			{ "Shokugeki: Yukihira Soma", true },
			{ "Slime: Rimiru Tempest", true },
			{ "TypeMoon: Astolfo", true },
			{ "TypeMoon: Gray", true },
			{ "TypeMoon: Ishtar Dark", true },
			{ "TypeMoon: Ishtar Light", true },
			{ "TypeMoon: Tohsaka Rin", true },
			{ "Vocaloid: Hatsune Miku", true },
			{ "YuruCamp: Nadeshiko", true },
			{ "YuruCamp: Shima Rin", true }
		};

		public string ResourceHeader => "Yuki_Theme.Themes";

		public Assembly Location => Assembly.GetExecutingAssembly ();

		public Header ()
		{
		}
	}
}