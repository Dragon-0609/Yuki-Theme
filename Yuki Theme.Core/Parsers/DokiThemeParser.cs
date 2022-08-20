using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public class DokiThemeParser : AbstractParser
	{
		private bool   _dark   = true;
		private string _currentDirectory   = "";
		private string _fileName  = "";
		private string _originalFileName = "";
		private string GetWallpaper => Path.Combine (_currentDirectory, _fileName);
		private string GetSticker   => Path.Combine (_currentDirectory, _fileName.Replace (".png", "_sticker.png"));
		
		public  Func <string, string, bool> exist;

		public static Dictionary <string, string> groups = new ()
		{
			
			{ "Azur Lane", "AzurLane: " },
			{ "Kill la Kill", "KillLaKill: " },
			{ "Blend S", "BlendS: " },
            {"Guilty Crown", "GuiltyCrown: "},
            { "Code Geass", "CodeGeass: "},
            { "Helpful Fox Senko-san", "Senko-san: "},
            { "Charlotte", "Charlotte: "},
			{ "Toaru Majutsu no Index", "Railgun: " },
			{ "The Rising of Shield Hero", "ShieldHero: " },
			{ "Chuunibyou", "Chuunibyou: " },
			{ "Re Zero", "Re:Zero: " },
            { "One Punch Man", "OPM: "},
            { "Shokugeki no Soma", "Shokugeki: "},
            { "Haikyu!!", "Haikyu: "},
            { "That Time I Get ReIncarnated As A Slime", "Slime: "},
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

		private string [] BoldStrings => new []
		{
			"foregroundColor",
			"constantColor",
			"comments",
			"stringColor",
			"keywordColor",
			"keyColor",
			"classNameColor"
		};

		private string [] ForegroundStrings => Concat (BoldStrings, new [] { "accentColor", "lineNumberColor" });

		public override void populateList (string path)
		{
			string text;

			if (needToWrite)
			{
				_currentDirectory = Path.GetDirectoryName (path);
				text = File.ReadAllText (path); // If it's file mode, then read from file, else read from input
			} else
				text = path;

			JObject json = JObject.Parse (text);
			JToken stickersToken = json ["stickers"];
			if (stickersToken != null)
			{
				SetStickerInfo (stickersToken, json);
			}

			theme.Group = groupName;
			
			theme.imagePath = _fileName;
			
			flname = theme.Name = _originalFileName;
			
			if (needToWrite)
			{
				PathToSave = Path.Combine (SettingsConst.CurrentPath, "Themes",
				                           $"{Helper.ConvertNameToPath (_originalFileName)}.yukitheme");
				if (!MainParser.CheckAvailableAndAsk (PathToSave, ask, exist))
					throw new InvalidDataException (API.CentralAPI.Current.Translate ("parser.theme.exist"));

				overwrite = File.Exists (PathToSave);
				Console.WriteLine ("{0} | Exist: {1}", PathToSave, overwrite);
			}

			if (json ["dark"] != null)
				_dark = bool.Parse (json ["dark"].ToString ());

			JToken colorsToken = json ["colors"];
			if (colorsToken == null) throw new InvalidDataException (API.CentralAPI.Current.Translate ("parser.theme.colors"));
			foreach (JToken colorToken in colorsToken)
			{
				ParseColor (colorToken);
			}


			theme.Fields ["LineNumbers"].Background = theme.Fields ["Default"].Background;

			if (!theme.Fields.ContainsKey ("Digits"))
				AddDefaults ("constantColor");
			AddDefaults ("foregroundColor");
			AddDefaults ("comments");

			ThemeField df = theme.Fields ["Default"];

			// throw new Exception ("ERROR");
			theme.Fields.Add ("FoldMarker", new ThemeField
			{
				Foreground = df.Background,
				Background = df.Background
			});
			theme.Fields.Add ("FoldLine", new ThemeField
			{
				Foreground = df.Background
			});
			df.Bold = null;
			df.Italic = null;
			
			theme.Token = Helper.EncryptString (theme.Name, DateTime.Now.ToString ("ddMMyyyy"));
			
			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		private void ParseColor (JToken colorToken)
		{
			JProperty color = (JProperty)colorToken;
			string [] name = getName (color.Name);

			ThemeField attrs = new ThemeField ();

			attrs = SetForegroundAndBackground (color, attrs);

			AddDefaults (color, attrs);

			SetFontStyle (color, attrs);


			AddToThemeFields (name, attrs);
		}

		private ThemeField SetForegroundAndBackground (JProperty color, ThemeField attrs)
		{
			bool fore = CanGetForeground (color.Name);
			if (fore)
				attrs.Foreground = color.Value.ToString ();
			bool back = canGetBackground (color.Name);
			if (back)
			{
				if (needToChange (color.Name))
				{
					ChangedWayOfSetting (color.Name, color.Value.ToString (), ref attrs);
				} else
				{
					attrs.Background = color.Value.ToString ();
				}
			}

			return attrs;
		}

		private void AddDefaults (JProperty color, ThemeField attrs)
		{
			Tuple <string, string> defaults = GetDefaultForeground (color.Name);

			if (defaults != null)
			{
				if (attrs.IsAttributeNull (defaults.Item1))
					attrs.SetAttributeByName (defaults.Item1, defaults.Item2);
			}
		}

		private void SetFontStyle (JProperty color, ThemeField attrs)
		{
			if (CanBold (color.Name))
			{
				attrs.Bold = false;
				attrs.Italic = false;
			}
		}

		private void AddToThemeFields (string [] name, ThemeField attrs)
		{
			foreach (string nm in name)
			{
				if (!theme.Fields.ContainsKey (nm))
				{
					theme.Fields.Add (nm, attrs);
				} else
				{
					theme.Fields [nm] = attrs.MergeWithAnother (theme.Fields [nm]);
				}
			}
		}

		private void SetStickerInfo (JToken stickersToken, JObject json)
		{
			JToken stickerTreeToken = stickersToken ["default"];
			if (stickerTreeToken != null)
			{
				_fileName = stickerTreeToken ["name"]?.ToString ();
				_originalFileName = ConvertGroup (json ["group"]?.ToString ()) + json ["name"];

				theme.ParseWallpaperAlign (stickerTreeToken ["anchor"]?.ToString ());

				if (stickerTreeToken ["opacity"] != null)
					theme.WallpaperOpacity = stickerTreeToken ["opacity"].ToObject <int> ();
			}
		}

		public override void PopulateByXMLNodeTreeType (XmlNode node)
		{
		}

		private string ConvertGroup (string st)
		{
			string res = st;
			if (groups.ContainsKey (st))
				res = groups [st];
			return res;
		}

		public override string GetValue (XmlNode child)
		{
			string attrValue = "";
			if (child.Attributes != null)
			{
				attrValue = child.Attributes ["value"].Value;
			}
			return attrValue;
		}

		public override ThemeField populateDefaultAttributes (string name)
		{
			return null;
		}

		public override bool isNecessaryAttribute (string name)
		{
			return name is "FOREGROUND" or "BACKGROUND";
		}

		private bool canGetBackground (string st)
		{
			return st is "textEditorBackground" or "selectionBackground" or "identifierHighlight";
		}

		private bool needToChange (string st)
		{
			return st == "identifierHighlight";
		}

		private void ChangedWayOfSetting (string nameOfField, string color, ref ThemeField field)
		{
			if (nameOfField == "identifierHighlight")
				field.Foreground = color;
		}

		private bool CanGetForeground (string st)
		{
			return ForegroundStrings.Contains (st);
		}

		private void AddDefaults (string st)
		{
			string[] dds = getName (st);
			Tuple <string, string> defs = GetDefaultForeground (st);
			Tuple <string, bool> defaultBold = GetDefaultBold (st);

			foreach (string nm in dds)
			{
				if (!theme.Fields.ContainsKey (nm))
				{
					ThemeField field = new ThemeField ();
					field.SetAttributeByName (defs.Item1, defs.Item2);
					if (defaultBold != null)
						field.SetAttributeByName (defaultBold.Item1, defaultBold.Item2.ToString());
					theme.Fields.Add (nm, field);
				} else if (theme.Fields [nm].IsAttributeNull (defs.Item1))
				{
					theme.Fields [nm].SetAttributeByName (defs.Item1, defs.Item2);
					
					if (defaultBold != null && theme.Fields [nm].IsAttributeNull (defaultBold.Item1))
						theme.Fields [nm].SetAttributeByName (defaultBold.Item1, defaultBold.Item2.ToString());
				}
			}
		}

		private readonly Dictionary <string, string> _defaultForegroundColors = new Dictionary <string, string> ()
			{ { "constantColor", "#4C94D6" }, { "foregroundColor", "#4D4D4A" }, { "comments", "#6a737d" } };

		private readonly Dictionary <string, string> _defaultDarkForegroundColors = new Dictionary <string, string> ()
			{ { "constantColor", "#86dbfd" }, { "foregroundColor", "#F8F8F2" }, { "comments", "#6272a4" } };

		private readonly Dictionary <string, bool> _defaultBold = new ()
			{ { "comments", false }, { "constantColor", false }, { "foregroundColor", false } };

		private Tuple <string, string> GetDefaultForeground (string st)
		{
			Tuple <string, string> res = null;
			if (_defaultForegroundColors.ContainsKey (st))
			{
				if (_dark)
				{
					res = new Tuple <string, string> ("color", _defaultDarkForegroundColors [st]);
				} else
				{
					res = new Tuple <string, string> ("color", _defaultForegroundColors [st]);
				}
			}

			return res;
		}

		private Tuple <string, bool> GetDefaultBold (string st)
		{
			Tuple <string, bool> res = null;
			if (_defaultBold.ContainsKey (st))
			{
				res = new Tuple <string, bool> ("bold", _defaultBold [st]);
			}

			return res;
		}

		private bool CanBold (string st)
		{
			return BoldStrings.Contains (st);
		}

		private Dictionary <string, string []> TranslatedNames = new ()
		{
			{ "textEditorBackground", new [] { "Default" } },
			{ "foregroundColor", new [] { "Default", "Punctuation" } },
			{ "selectionBackground", new [] { "Selection" } },
			{ "constantColor", new [] { "Digits" } },
			{ "comments", new [] { "LineBigComment", "LineComment", "BlockComment", "BlockComment2" } },
			{ "stringColor", new [] { "String" } },
			{
				"keywordColor", new []
				{
					"KeyWords", "ProgramSections", "Async", "AccessKeywords1", "NonReserved1", "OperatorKeywords",
					"SelectionStatements", "IterationStatements", "ExceptionHandlingStatements", "RaiseStatement",
					"JumpStatements", "JumpProcedures", "InternalConstant", "InternalTypes", "ReferenceTypes",
					"Modifiers", "AccessModifiers", "ErrorWords", "WarningWords", "DireciveNames",
					"SpecialDireciveNames", "DireciveValues"
				}
			},
			{ "classNameColor", new [] { "MarkPrevious" } },
			{ "keyColor", new [] { "BeginEnd" } },
			{ "accentColor", new [] { "CaretMarker", "SelectedFoldLine" } },
			{ "lineNumberColor", new [] { "LineNumbers" } },
			{ "identifierHighlight", new [] { "EOLMarkers", "SpaceMarkers", "TabMarkers" } },
		};

		public override string [] getName (string st)
		{
			string [] res = { };
			if (TranslatedNames.ContainsKey (st))
				res = TranslatedNames [st];
			return res;
		}

		public override void finishParsing (string path)
		{
			if (!overwrite)
			{
				XmlDocument doc = new XmlDocument ();
				doc.Load (PathToSave);

				Tuple <bool, Image> wallp = getImage (GetWallpaper);

				Tuple <bool, Image> stick = getImage (GetSticker);

				XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");

				XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
				XmlNodeList comms = nod.SelectNodes ("//comment()");
				if (comms.Count >= 3)
				{
					Dictionary <string, bool> comments = new Dictionary <string, bool> ()
					{
						{ "name", false }, { "align", false }, { "opacity", false }, { "sopacity", false },
						{ "hasImage", false }, { "hasSticker", false }
					};

					Dictionary <string, string> commentValues = new Dictionary <string, string> ()
					{
						{ "name", "name:" + _originalFileName }, { "align", "align:" + ((int)Alignment.Center).ToString () },
						{ "opacity", "opacity:" + (10).ToString () },
						{ "sopacity", "sopacity:" + (100).ToString () },
						{ "hasImage", "hasImage:" + wallp.Item1.ToString () },
						{ "hasSticker", "hasSticker:" + stick.Item1.ToString () }
					};
					foreach (XmlComment comm in comms)
					{
						if (comm.Value.StartsWith ("align"))
						{
							comm.Value = commentValues ["align"];
							comments ["align"] = true;
						} else if (comm.Value.StartsWith ("opacity"))
						{
							comm.Value = commentValues ["opacity"];
							comments ["opacity"] = true;
						} else if (comm.Value.StartsWith ("sopacity"))
						{
							comm.Value = commentValues ["sopacity"];
							comments ["sopacity"] = true;
						} else if (comm.Value.StartsWith ("name"))
						{
							comm.Value = commentValues ["name"];
							comments ["name"] = true;
						} else if (comm.Value.StartsWith ("hasImage"))
						{
							comm.Value = commentValues ["hasImage"];
							comments ["hasImage"] = true;
						} else if (comm.Value.StartsWith ("hasSticker"))
						{
							comm.Value = commentValues ["hasSticker"];
							comments ["hasSticker"] = true;
						}
					}

					foreach (KeyValuePair <string, bool> comment in comments)
					{
						if (!comment.Value)
						{
							node.AppendChild (doc.CreateComment (commentValues [comment.Key]));
						}
					}
				} else
				{
					node.AppendChild (doc.CreateComment ("name:" + _originalFileName));
					node.AppendChild (doc.CreateComment ("align:" + ((int)Alignment.Center).ToString ()));
					node.AppendChild (doc.CreateComment ("opacity:" + (10).ToString ()));
					node.AppendChild (doc.CreateComment ("sopacity:" + (100).ToString ()));
					node.AppendChild (doc.CreateComment ("hasImage:" + wallp.Item1.ToString ()));
					node.AppendChild (doc.CreateComment ("hasSticker:" + stick.Item1.ToString ()));
				}

				Helper.Zip (PathToSave, doc.OuterXml, wallp.Item2, stick.Item2, "", true);
			}
		}

		private Tuple <bool, Image> getImage (string path)
		{
			if (File.Exists (path))
				return new Tuple <bool, Image> (true, Image.FromFile (path));
			else
				return new Tuple <bool, Image> (false, null);
		}

		private static T[] Concat<T>(T[] x, T[] y)
		{
			if (x == null) throw new ArgumentNullException("x");
			if (y == null) throw new ArgumentNullException("y");
			T [] z = new T [x.Length + y.Length];
			x.CopyTo(z, 0);
			y.CopyTo(z, x.Length);
			return z;
		}
	}
}