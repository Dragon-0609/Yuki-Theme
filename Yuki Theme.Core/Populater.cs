using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Yuki_Theme.Core
{
	public class Populater
	{
		public static bool isInList (string str, ListBox.ObjectCollection coll)
		{
			return isInList (str, coll.Cast <String> ().ToList ());
		}

		public static bool isInList (string str, List <string> coll)
		{
			bool res = false;
			switch (str)
			{
				case "SpaceMarkers" :
				case "TabMarkers" :
				{
					res = coll.Contains ("EOLMarkers");
				}
					break;
				case "LineComment" :
				case "BlockComment" :
				case "BlockComment2" :
				{
					res = coll.Contains ("LineBigComment");
				}
					break;

				case "ProgramSections" :
				case "Async" :
				case "RaiseStatement" :
				case "JumpStatements" :
				case "JumpProcedures" :
				case "Modifiers" :
				case "AccessModifiers" :
				{
					res = coll.Contains ("KeyWords");
				}
					break;

				case "SpecialDireciveNames" :
				case "DireciveValues" :
				{
					res = coll.Contains ("DireciveNames");
				}
					break;
			}

			return res;
		}

		public static string [] getDependencies (string str)
		{
			string [] res = new string [] { };
			switch (str)
			{
				case "EOLMarkers" :
				{
					res = new string [] {"SpaceMarkers", "TabMarkers"};
				}
					break;

				case "LineBigComment" :
				{
					res = new string [] {"LineComment", "BlockComment", "BlockComment2"};
				}
					break;

				case "KeyWords" :

				{
					res = new string []
					{
						"ProgramSections", "Async", "RaiseStatement", "JumpStatements", "JumpProcedures", "Modifiers",
						"AccessModifiers"
					};
				}
					break;

				case "DireciveNames" :

				{
					res = new string [] {"SpecialDireciveNames", "DireciveValues"};
				}
					break;

				case "Image" :

				{
					res = new string [] {"BackgroundImage"};
				}
					break;
			}

			return res;
		}

		public static string getNormalizedName (string str)
		{
			string res = "";
			switch (str)
			{
				case "default" :
				{
					res = "Default";
				}
					break;
				case "linenumber" :
				{
					res = "LineNumbers";
				}
					break;
				case "foldline" :
				{
					res = "FoldLine";
				}
					break;
				case "foldmarker" :
				{
					res = "FoldMarker";
				}
					break;
				case "selectedfoldline" :
				{
					res = "SelectedFoldLine";
				}
					break;
				case "digit" :
				{
					res = "Digits";
				}
					break;
				case "comment" :
				{
					res = "LineBigComment";
				}
					break;
				case "string" :
				{
					res = "String";
				}
					break;
				case "keyword" :
				{
					res = "KeyWords";
				}
					break;
				case "beginend" :
				{
					res = "BeginEnd";
				}
					break;
				case "punctuation" :
				{
					res = "Punctuation";
				}
					break;
				case "operator" :
				{
					res = "OperatorKeywords";
				}
					break;
				case "constant" :
				{
					res = "InternalConstant";
				}
					break;
				case "image" :
				{
					res = "BackgroundImage";
				}
					break;
				case "sticker" :
				{
					res = "Sticker";
				}
					break;
			}

			return res;
		}
	}
}