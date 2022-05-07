using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public class DokiThemeParser : AbstractParser
	{
		private bool                        dark   = true;
		private string                      curd   = "";
		private string                      fname  = "";
		private string                      ofname = "";
		private string                      getWallpaper => Path.Combine (curd, fname);
		private string                      getSticker   => Path.Combine (curd, fname.Replace (".png", "_sticker.png"));
		public  Func <string, string, bool> exist;

		public override void populateList (string path)
		{
			string text = "";

			if (needToWrite)
			{
				curd = Path.GetDirectoryName (path);
				text = File.ReadAllText (path); // If it's file mode, then read from file, else read from input
			} else
				text = path;

			JObject json = JObject.Parse (text);
			fname = json ["stickers"] ["default"]["name"].ToString ();
			ofname = ConvertGroup (json ["group"].ToString ()) + json ["name"];

			theme.ParseWallpaperAlign (json ["stickers"] ["default"] ["anchor"].ToString ());
			theme.WallpaperOpacity = json ["stickers"] ["default"] ["opacity"].ToObject <int> ();
			
			flname = theme.Name = ofname;
			if (needToWrite)
			{
				PathToSave = Path.Combine (CLI.currentPath, "Themes",
				                           $"{Helper.ConvertNameToPath (ofname)}.yukitheme");
				if (!MainParser.checkAvailableAndAsk (PathToSave, ask, exist))
					throw new InvalidDataException ("The theme is exist...canceling...");

				overwrite = File.Exists (PathToSave);
				Console.WriteLine ("{0} | Exist: {1}", PathToSave, overwrite);
			}

			dark = bool.Parse (json ["dark"].ToString ());

			foreach (JProperty cl in json ["colors"])
			{
				var name = getName (cl.Name);

				var attrs = new ThemeField ();

				bool fore = canGetForeground (cl.Name);
				if (fore)
					attrs.Foreground = cl.Value.ToString ();
				bool back = canGetBackground (cl.Name);
				if (back)
				{
					if (needToChange (cl.Name))
					{
						ChangedWayOfSetting (cl.Name, cl.Value.ToString (), ref attrs);
					} else
					{
						attrs.Background = cl.Value.ToString ();
					}
					// Console.WriteLine ("{0} - {1}", cl.Name, attrs.Background);
				}

				Tuple <string, string> defaults = getDefault (cl.Name);

				if (defaults != null)
				{
					if (attrs.isAttributeNull (defaults.Item1))
						attrs.SetAttributeByName (defaults.Item1, defaults.Item2);
				}

				if (canBold (cl.Name))
				{
					attrs.Bold = false;
					attrs.Italic = false;
				}


				foreach (var nm in name)
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


			theme.Fields ["LineNumbers"].Background = theme.Fields ["Default"].Background;

			if (!theme.Fields.ContainsKey ("Digits"))
				addDefaults ("constantColor");
			addDefaults ("foregroundColor");
			addDefaults ("comments");

			ThemeField df = theme.Fields ["Default"];

			// throw new Exception ("ERROR");
			theme.Fields.Add ("FoldMarker", new ThemeField
			{
				Foreground = df.Background,
				Background = df.Background
			});
			ThemeField selectedF = theme.Fields ["SelectedFoldLine"].copyField ();
			selectedF.Background = df.Background;
			theme.Fields ["SelectedFoldLine"] = selectedF;
			theme.Fields.Add ("FoldLine", new ThemeField
			{
				Foreground = df.Background
			});
			
			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		public override void PopulateByXMLNodeTreeType (XmlNode node)
		{
		}

		private string ConvertGroup (string st)
		{
			string res = st;
			switch (st)
			{
				case "Kill la Kill" :
				{
					res = "KillLaKill: ";
				}
					break;
				case "Blend S" :
				{
					res = "BlendS: ";
				}
					break;
				case "Re Zero" :
				{
					res = "Re:Zero: ";
				}
					break;
				case "Love Live" :
				{
					res = "LoveLive: ";
				}
					break;
				case "Literature Club" :
				{
					res = "DDLC: ";
				}
					break;
				case "KonoSuba" :
				{
					res = "KonoSuba: ";
				}
					break;
				case "Darling in the Franxx" :
				{
					res = "Franxx: ";
				}
					break;
				case "Bunny Senpai" :
				{
					res = "BunnySenpai: ";
				}
					break;
				case "Steins Gate" :
				{
					res = "SG: ";
				}
					break;
				case "Gate" :
				{
					res = "Gate: ";
				}
					break;
				case "Quintessential Quintuplets" :
				{
					res = "QQ: ";
				}
					break;
				case "Fate" :
				{
					res = "TypeMoon: ";
				}
					break;
				case "Type-Moon" :
				{
					res = "TypeMoon: ";
				}
					break;
				case "Daily Life With A Monster Girl" :
				{
					res = "MonsterMusume: ";
				}
					break;
				case "Vocaloid" :
				{
					res = "Vocaloid: ";
				}
					break;
				case "DanganRonpa" :
				{
					res = "DR: ";
				}
					break;
				case "High School DxD" :
				{
					res = "DxD: ";
				}
					break;
				case "Sword Art Online" :
				{
					res = "SAO: ";
				}
					break;
				case "Lucky Star" :
				{
					res = "LS: ";
				}
					break;
				case "Evangelion" :
				{
					res = "EVA: ";
				}
					break;
				case "EroManga Sensei" :
				{
					res = "EroManga: ";
				}
					break;
				case "Miss Kobayashi's Dragon Maid" :
				{
					res = "DM: ";
				}
					break;
				case "OreGairu" :
				{
					res = "OreGairu: ";
				}
					break;
				case "OreImo" :
				{
					res = "OreImo: ";
				}
					break;
				case "The Great Jahy Will Not Be Defeated" :
				{
					res = "JahySama: ";
				}
					break;
				case "Future Diary" :
				{
					res = "FutureDiary: ";
				}
					break;
				case "Kakegurui" :
				{
					res = "Kakegurui: ";
				}
					break;
				case "Monogatari" :
				{
					res = "Monogatari: ";
				}
					break;
				case "Don't Toy with me Miss Nagatoro" :
				{
					res = "DTWMMN: ";
				}
					break;
				case "Miscellaneous" :
				{
					res = "Misc: ";
				}
					break;
				case "Yuru Camp" :
				{
					res = "YuruCamp: ";
				}
					break;
				case "NekoPara" :
				{
					res = "NekoPara: ";
				}
					break;
				case "Azur Lane" :
				{
					res = "AzurLane: ";
				}
					break;
				case "The Rising of Shield Hero" :
				{
					res = "ShieldHero: ";
				}
					break;
				case "Toaru Majutsu no Index" :
				{
					res = "Railgun: ";
				}
					break;
				case "Chuunibyou" :
				{
					res = "Chuunibyou: ";
				}
					break;
			}

			return res;
		}

		public override string GetValue (XmlNode child)
		{
			var attr_value = child.Attributes ["value"].Value;

			return attr_value;
		}

		public override ThemeField populateDefaultAttributes (string name)
		{
			return null;
		}

		public override bool isNecessaryAttribute (string name)
		{
			bool rs = false;
			switch (name)
			{
				case "FOREGROUND" :
				case "BACKGROUND" :
				{
					rs = true;
				}
					break;
			}

			return rs;
		}

		private bool canGetBackground (string st)
		{
			bool res = false;
			switch (st)
			{
				case "textEditorBackground" :
				case "selectionBackground" :
				case "identifierHighlight" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		private bool needToChange (string st)
		{
			bool res = false;
			switch (st)
			{
				case "identifierHighlight" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		private void ChangedWayOfSetting (string nameOfField, string color, ref ThemeField field)
		{
			switch (nameOfField)
			{
				case "identifierHighlight" :
				{
					field.Foreground = color;
				}
					break;
			}
		}

		private bool canGetForeground (string st)
		{
			bool res = false;
			switch (st)
			{
				case "foregroundColor" :
				case "constantColor" :
				case "comments" :
				case "stringColor" :
				case "keywordColor" :
				case "classNameColor" :
				case "accentColor" :
				case "lineNumberColor" :
				case "keyColor" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		private void addDefaults (string st)
		{
			var dds = getName (st);
			Tuple <string, string> defs = getDefault (st);

			foreach (var nm in dds)
			{
				if (!theme.Fields.ContainsKey (nm))
				{
					ThemeField field = new ThemeField ();
					field.SetAttributeByName (defs.Item1, defs.Item2);
					theme.Fields.Add (nm, field);
				} else if (theme.Fields [nm].isAttributeNull (defs.Item1))
					theme.Fields [nm].SetAttributeByName (defs.Item1, defs.Item2);
			}
		}

		private readonly Dictionary <string, string> _defaultForegroundColors = new Dictionary <string, string> ()
			{ { "constantColor", "#4C94D6" }, { "foregroundColor", "#4D4D4A" }, { "comments", "#6a737d" } };

		private readonly Dictionary <string, string> _defaultDarkForegroundColors = new Dictionary <string, string> ()
			{ { "constantColor", "#86dbfd" }, { "foregroundColor", "#F8F8F2" }, { "comments", "#6272a4" } };

		private Tuple <string, string> getDefault (string st)
		{
			Tuple <string, string> res = null;
			if (_defaultForegroundColors.ContainsKey (st))
			{
				if (dark)
				{
					res = new Tuple <string, string> ("color", _defaultDarkForegroundColors [st]);
				} else
				{
					res = new Tuple <string, string> ("color", _defaultForegroundColors [st]);
				}
			}

			return res;
		}

		private bool canBold (string st)
		{
			bool res = false;
			switch (st)
			{
				case "foregroundColor" :
				case "constantColor" :
				case "comments" :
				case "stringColor" :
				case "keywordColor" :
				case "keyColor" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		public override string [] getName (string st)
		{
			string [] res = new string [] { };
			switch (st)
			{
				case "textEditorBackground" :
				{
					res = new [] { "Default" };
				}
					break;

				case "foregroundColor" :
				{
					res = new [] { "Default", "Punctuation" };
				}
					break;

				case "selectionBackground" :
				{
					res = new [] { "Selection" };
				}
					break;

				case "constantColor" :
				{
					res = new [] { "Digits" };
				}
					break;

				case "comments" :
				{
					res = new [] { "LineBigComment", "LineComment", "BlockComment", "BlockComment2" };
				}
					break;

				case "stringColor" :
				{
					res = new [] { "String" };
				}
					break;

				case "keywordColor" :
				{
					res = new []
					{
						"KeyWords", "ProgramSections", "Async", "AccessKeywords1", "NonReserved1", "OperatorKeywords",
						"SelectionStatements", "IterationStatements", "ExceptionHandlingStatements", "RaiseStatement",
						"JumpStatements", "JumpProcedures", "InternalConstant", "InternalTypes", "ReferenceTypes",
						"Modifiers", "AccessModifiers", "ErrorWords", "WarningWords", "DireciveNames",
						"SpecialDireciveNames", "DireciveValues"
					};
				}
					break;

				case "classNameColor" :
				{
					res = new []
					{
						"MarkPrevious"
					};
				}
					break;

				case "keyColor" :
				{
					res = new []
					{
						"BeginEnd"
					};
				}
					break;

				case "accentColor" :
				{
					res = new [] { "CaretMarker", "SelectedFoldLine" };
				}
					break;

				case "lineNumberColor" :
				{
					res = new [] { "LineNumbers" };
				}
					break;

				case "identifierHighlight" :
				{
					res = new [] { "EOLMarkers", "SpaceMarkers", "TabMarkers" };
				}
					break;
			}

			return res;
		}

		public override void finishParsing (string path)
		{
			if (!overwrite)
			{
				var doc = new XmlDocument ();
				doc.Load (PathToSave);

				Tuple <bool, Image> wallp = getImage (getWallpaper);

				Tuple <bool, Image> stick = getImage (getSticker);

				var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");

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
						{ "name", "name:" + ofname }, { "align", "align:" + ((int)Alignment.Center).ToString () },
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
					node.AppendChild (doc.CreateComment ("name:" + ofname));
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
	}
}