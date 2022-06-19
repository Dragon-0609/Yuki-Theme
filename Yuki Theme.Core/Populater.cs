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
					res = coll.Contains ("Punctuation");
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
				case "NonReserved1" :
				case "ExceptionHandlingStatements" :
				case "ReferenceTypes" :
				case "DireciveNames" :
				case "SpecialDireciveNames" :
				case "DireciveValues" :
				{
					res = coll.Contains ("KeyWords");
				}
					break;

				case "SelectionStatements" :
				case "IterationStatements" :
				{
					res = coll.Contains ("OperatorKeywords");
				}
					break;
			}

			return res;
		}

		public static bool isEnvironmentColor (string str)
		{
			bool res = false;
			switch (str)
			{
				case "SpaceMarkers" :
				case "TabMarkers" :
				case "EOLMarkers" :
				{
					res = true;
				}
					break;
			}

			return res;
		}

		public static string [] getDependencies (string str)
		{
			if (ShadowNames.PascalFields.ContainsKey (str))
				return ShadowNames.PascalFields [str];
			else
				return null;
		}

		public static string getNormalizedName (string str)
		{
			str = str.ToLower ();
			string res = "";
			switch (str)
			{
				case "default" :
				{
					res = "Default Text";
				}
					break;
				case "selection" :
				{
					res = "Selection";
				}
					break;
				case "linenumber" :
				{
					res = "Line Number";
				}
					break;
				case "caret" :
				{
					res = "Caret";
				}
					break;
				case "vruler" :
				{
					res = "Vertical Ruler";
				}
					break;
				case "fold" :
				{
					res = "Fold's Line";
				}
					break;
				case "foldmarker" :
				{
					res = "Fold's Rectangle";
				}
					break;
				case "selectedfold" :
				{
					res = "Selected Fold's Line";
				}
					break;
				case "digit" :
				{
					res = "Number";
				}
					break;
				case "comment" :
				{
					res = "Comment";
				}
					break;
				case "string" :
				{
					res = "String";
				}
					break;
				case "keyword" :
				{
					res = "Keyword";
				}
					break;
				case "beginend" :
				{
					res = "Begin, End";
				}
					break;
				case "punctuation" :
				{
					res = "Punctuation";
				}
					break;
				case "operator" :
				{
					res = "Keyword";
				}
					break;
				case "constant" :
				{
					res = "Constants";
				}
					break;
				case "image" :
				{
					res = "Wallpaper";
				}
					break;
				case "sticker" :
				{
					res = "Sticker";
				}
					break;
				default :
				{
					res = str;
				}
					break;
			}

			return res;
		}

		public static Dictionary <string, string> changedNames = new ()
		{
			{ "Default", "Default" },
			{ "Selection", "Selection" },
			{ "LineNumbers", "LineNumber" },
			{ "CaretMarker", "Caret" },
			{ "VRuler", "VRuler" },
			{ "FoldLine", "Fold" },
			{ "FoldMarker", "FoldMarker" },
			{ "SelectedFoldLine", "SelectedFold" },
			{ "Digits", "Digit" },
			{ "LineBigComment", "Comment" },
			{ "String", "String" },
			{ "KeyWords", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Punctuation", "Punctuation" },
			{ "OperatorKeywords", "Operator" },
			{ "InternalConstant", "Constant" },
			{ "Wallpaper", "Image" },
			{ "Sticker", "Sticker" }
		};

		public static string getChangedName (string str)
		{
			string res = "";
			if (changedNames.ContainsKey (str))
				res = changedNames [str];

			return res;
		}
	}
}