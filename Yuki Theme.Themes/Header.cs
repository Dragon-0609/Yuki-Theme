using System.Reflection;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Themes
{
	public class Header : IThemeHeader
	{
		public string GroupName => "Doki Theme";

		public string [] ThemeNames => new []
		{
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
		public string    ResourceHeader => "Yuki_Theme.Themes";

		public Assembly Location => Assembly.GetExecutingAssembly ();

		public Header ()
		{
		}
	}
}