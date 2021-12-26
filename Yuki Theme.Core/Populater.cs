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
				case "EOLMarkers" :
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
			string [] res = new string [] { };
			switch (str)
			{
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
						"AccessModifiers", "NonReserved1", "ExceptionHandlingStatements", "ReferenceTypes",
						"DireciveNames", "SpecialDireciveNames", "DireciveValues"
					};
				}
					break;

				case "OperatorKeywords" :
				{
					res = new string [] {"SelectionStatements", "IterationStatements"};
				}
					break;

				case "Punctuation" :
				{
					res = new string [] {"EOLMarkers", "SpaceMarkers", "TabMarkers"};
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
				case "selection" :
				{
					res = "Selection";
				}
					break;
				case "linenumber" :
				{
					res = "LineNumbers";
				}
					break;
				case "caret" :
				{
					res = "CaretMarker";
				}
					break;
				case "vruler" :
				{
					res = "VRuler";
				}
					break;
				case "fold" :
				{
					res = "FoldLine";
				}
					break;
				case "foldmarker" :
				{
					res = "FoldMarker";
				}
					break;
				case "selectedfold" :
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
					res = "Wallpaper";
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

		public static string getChangedName (string str)
		{
			string res = "";
			switch (str)
			{
				case "LineBigComment" :
				{
					res = "Comment";
				}
					break;
				case "InternalConstant" :
				{
					res = "Constant";
				}
					break; /*
				case "" :
				{
					res = "";
				}
					break;
				case "" :
				{
					res = "";
				}
					break;*/
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