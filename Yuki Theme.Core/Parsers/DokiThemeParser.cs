﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public class DokiThemeParser : AbstractParser
	{
		private bool   dark  = true;
		private string curd  = "";
		private string fname = "";
		private string ofname = "";
		private string getImage => Path.Combine (curd, fname);
		
		public override void populateList (string path)
		{
			curd = Path.GetDirectoryName (path);
			JObject json = JObject.Parse (File.ReadAllText (path));
			fname = json ["stickers"] ["default"].ToString();
			ofname = ConvertGroup (json ["group"].ToString ()) + json ["name"];
			flname = ofname;
			outname = $"Themes/{ofname.Replace (": ", "__").Replace (":", "")}.yukitheme";
			dark = bool.Parse (json ["dark"].ToString ());
			
			foreach (JProperty cl in json ["colors"])
			{
				var name = getName (cl.Name);
				
				var attrs = new Dictionary <string, string> ();

				bool fore = canGetForeground (cl.Name);
				if (fore)
					attrs.Add ("color", cl.Value.ToString());
				bool back = canGetBackground (cl.Name);
				if (back)
					attrs.Add ("bgcolor", cl.Value.ToString());

				Tuple <string, string> defaults = getDefault (cl.Name);

				if (defaults != null)
				{
					if(!attrs.ContainsKey (defaults.Item1))
						attrs.Add (defaults.Item1, defaults.Item2);
				}
				if(canBold(cl.Name))
				{
					attrs.Add ("bold", "false");
					attrs.Add ("italic", "false");
				}
				

				foreach (var nm in name)
				{
					attributes.Add (nm, attrs);
				}
				if (cl.Name.Equals ("selectionBackground", StringComparison.OrdinalIgnoreCase))
				{
					Dictionary <string, string> attrim = new Dictionary <string, string> () {{"align", "1000"}, {"opacity", "10"}};
					attributes.Remove ("Selection");
					attributes.Add ("BackgroundImage", attrim);
					attributes.Add ("Selection", attrs);
				}
				
			}
			
			
			Tuple <string, string> tp = new Tuple <string, string> ("bgcolor", attributes ["Default"] ["bgcolor"]);
			attributes ["LineNumbers"].Add (tp.Item1, tp.Item2);
			
			if(!attributes.ContainsKey ("Digits"))
				addDefaults ("constantColor");
			addDefaults ("foregroundColorEditor");
			
			Dictionary <string, string> df = attributes ["Default"];
			Dictionary <string, string> dic = new Dictionary <string, string> ();
			dic.Add ("color", df["color"]);
			dic.Add ("bgcolor", df["bgcolor"]);
			attributes.Add ("FoldMarker", dic);
			attributes.Add ("SelectedFoldLine", dic);
			dic.Remove ("bgcolor");
			attributes.Add ("FoldLine", dic);
			
			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		public override void PopulateByXMLNodeTreeType (XmlNode node)
		{
			
		}

		private string ConvertGroup (string st)
		{
			string res = "";
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
			}

			return res;
		}

		public override string GetValue (XmlNode child)
		{
			var attr_value = child.Attributes ["value"].Value;

			return attr_value;
		}

		public override Dictionary <string, string> populateDefaultAttributes (string name)
		{
			var att = new Dictionary <string, string> ();
			switch (name)
			{
				case "DEFAULT_COMMA" :
				{
					att.Add ("color", "#FFFFFF");
				}
					break;
			}

			return att;
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
				case "textEditorBackground" : case "selectionBackground" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		private bool canGetForeground (string st)
		{
			bool res = false;
			switch (st)
			{
				case "foregroundColorEditor" : case "constantColor" :
				case "comments" : case "stringColor" : case "keywordColor" :
				case "classNameColor" : case "accentColor" : 
				case "lineNumberColor" :
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
				if(!attributes.ContainsKey (nm))
					attributes.Add (nm, new Dictionary <string, string> () {{defs.Item1, defs.Item2}});
				else if (!attributes [nm].ContainsKey (defs.Item1))
					attributes [nm].Add (defs.Item1, defs.Item2);
			}

		}

		private Tuple<string,string> getDefault (string st)
		{
			Tuple <string, string> res = null;
			switch (st)
			{
				case "constantColor" :
				{
					if (dark)
						res = new Tuple <string, string> ("color", "#86dbfd");
					else
						res = new Tuple <string, string> ("color", "#4C94D6");
				}
					break;
				
				case "foregroundColorEditor" :
				{
					if (dark)
						res = new Tuple <string, string> ("color", "#F8F8F2");
					else
						res = new Tuple <string, string> ("color", "#4D4D4A");
				}
					break;
				
			}

			return res;
		}

		private bool canBold (string st)
		{
			bool res = false;
			switch (st)
			{
				case "foregroundColorEditor" : case "constantColor" :
				case "comments" : case "stringColor" : case "keywordColor" :
				case "classNameColor" : 
				{
					res = true;
				}
					break;
			}

			return res;
		}
		
		public override string [] getName (string st)
		{
			string [] res = new string[] { };
			switch (st)
			{
				case "textEditorBackground" :
				{
					res = new [] {"Default"};
				}
					break;
				
				case "foregroundColorEditor" :
				{
					res = new [] {"Default", "Punctuation"};
				}
					break;

				case "selectionBackground" :
				{
					res = new [] {"Selection"};
				}
					break;

				case "constantColor" :
				{
					res = new [] {"Digits"};
				}
					break;

				case "comments" :
				{
					res = new [] {"LineBigComment", "LineComment", "BlockComment", "BlockComment2"};
				}
					break;
				
				case "stringColor" :
				{
					res = new [] {"String"};
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
						"BeginEnd"
					};
				}
					break;

				case "accentColor" :
				{
					res = new [] {"CaretMarker"};
				}
					break;

				case "lineNumberColor" :
				{
					res = new [] {"LineNumbers"};
				}
					break;
			}

			return res;
		}

		public override void finishParsing (string path)
		{
			var doc = new XmlDocument ();
			doc.Load (outname);

			Image img = Image.FromFile (getImage);
			
			var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
			bool hadSavedImage = false;
			foreach (XmlNode childNode in node.ChildNodes)
				if (childNode.Attributes != null &&
				    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = childNode.Name;
					if (childNode.Name == "Span" || childNode.Name == "KeyWords")
						nms = childNode.Attributes ["name"].Value;
					if (nms == "BackgroundImage")
						hadSavedImage = true;
				}

			if (!hadSavedImage)
			{
				XmlNode image = doc.CreateNode (XmlNodeType.Element, "BackgroundImage", null);
				XmlAttribute attr = doc.CreateAttribute ("align");
				attr.Value = ((int) Alignment.Center).ToString ();
				image.Attributes.SetNamedItem (attr);
				XmlAttribute attr2 = doc.CreateAttribute ("opacity");
				attr2.Value = "15";
				image.Attributes.SetNamedItem (attr2);
				var def = node.SelectSingleNode ("Default");
				node = node.InsertAfter (image, def);
			}

			if(outname.Contains ("__"))
			{
				node = doc.SelectSingleNode ("/SyntaxDefinition");
				XmlAttribute attr = doc.CreateAttribute ("names");
				attr.Value = ofname;
				node.Attributes.SetNamedItem (attr);
			}
			
			Helper.zip (outname, doc.OuterXml, img);
		}
	}
}