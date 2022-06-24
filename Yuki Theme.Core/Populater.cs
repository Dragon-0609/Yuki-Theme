using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Yuki_Theme.Core
{
	public static class Populater
	{
		private static readonly Dictionary <string, string> _shadowNames = new ()
		{
			{ "SpaceMarkers", "Punctuation" },
			{ "TabMarkers", "Punctuation" },
			{ "LineComment", "LineBigComment" },
			{ "BlockComment", "LineBigComment" },
			{ "BlockComment2", "LineBigComment" },
			{ "ProgramSections", "KeyWords" },
			{ "Async", "KeyWords" },
			{ "RaiseStatement", "KeyWords" },
			{ "JumpStatements", "KeyWords" },
			{ "JumpProcedures", "KeyWords" },
			{ "Modifiers", "KeyWords" },
			{ "AccessModifiers", "KeyWords" },
			{ "NonReserved1", "KeyWords" },
			{ "ExceptionHandlingStatements", "KeyWords" },
			{ "ReferenceTypes", "KeyWords" },
			{ "DireciveNames", "KeyWords" },
			{ "SpecialDireciveNames", "KeyWords" },
			{ "DireciveValues", "KeyWords" },
			{ "SelectionStatements", "OperatorKeywords" },
			{ "IterationStatements", "OperatorKeywords" },
		};

		private static readonly string [] EnvironmentColors = { "SpaceMarkers", "TabMarkers", "EOLMarkers", };

		private static readonly Dictionary <string, string> NormalizedNames = new ()
		{
			{ "default", "Default Text" },
			{ "selection", "Selection" },
			{ "linenumber", "Line Number" },
			{ "caret", "Caret" },
			{ "vruler", "Vertical Ruler" },
			{ "fold", "Fold's Line" },
			{ "foldmarker", "Fold's Rectangle" },
			{ "selectedfold", "Selected Fold's Line" },
			{ "digit", "Number" },
			{ "comment", "Comment" },
			{ "string", "String" },
			{ "keyword", "Keyword" },
			{ "beginend", "Begin, End" },
			{ "punctuation", "Punctuation" },
			{ "operator", "Keyword" },
			{ "constant", "Constants" },
			{ "image", "Wallpaper" },
			{ "sticker", "Sticker" },
		};

		public static bool IsInList (string str, ListBox.ObjectCollection coll)
		{
			return IsInList (str, coll.Cast <String> ().ToList ());
		}

		public static bool IsInList (string str, List <string> coll)
		{
			bool res = false;
			if (_shadowNames.ContainsKey (str))
				res = coll.Contains (_shadowNames [str]);
			return res;
		}

		public static bool IsEnvironmentColor (string str)
		{
			return EnvironmentColors.Contains (str);
		}

		public static string [] GetDependencies (string str)
		{
			if (ShadowNames.PascalFields.ContainsKey (str))
				return ShadowNames.PascalFields [str];
			return null;
		}

		public static string GetNormalizedName (string str)
		{
			str = str.ToLower ();
			string res;
			res = NormalizedNames.ContainsKey (str) ? NormalizedNames [str] : str;
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

		public static string GetChangedName (string str)
		{
			string res = "";
			if (changedNames.ContainsKey (str))
				res = changedNames [str];

			return res;
		}
	}
}