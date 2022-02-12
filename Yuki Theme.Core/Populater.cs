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

		public static string getChangedName (string str)
		{
			string res = "";
			switch (str)
			{
				case "Default" :
				{
					res = "Default";
				}
					break;
				case "Selection" :
				{
					res = "Selection";
				}
					break;
				case "LineNumbers" :
				{
					res = "LineNumber";
				}
					break;
				case "CaretMarker" :
				{
					res = "Caret";
				}
					break;
				case "VRuler" :
				{
					res = "VRuler";
				}
					break;
				case "FoldLine" :
				{
					res = "Fold";
				}
					break;
				case "FoldMarker" :
				{
					res = "FoldMarker";
				}
					break;
				case "SelectedFoldLine" :
				{
					res = "SelectedFold";
				}
					break;
				case "Digits" :
				{
					res = "Digit";
				}
					break;
				case "LineBigComment" :
				{
					res = "Comment";
				}
					break;
				case "String" :
				{
					res = "String";
				}
					break;
				case "KeyWords" :
				{
					res = "Keyword";
				}
					break;
				case "BeginEnd" :
				{
					res = "BeginEnd";
				}
					break;
				case "Punctuation" :
				{
					res = "Punctuation";
				}
					break;
				case "OperatorKeywords" :
				{
					res = "Operator";
				}
					break;
				case "InternalConstant" :
				{
					res = "Constant";
				}
					break;
				case "Wallpaper" :
				{
					res = "Image";
				}
					break;
				case "Sticker" :
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
	}
}