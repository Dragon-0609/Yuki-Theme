using System.Collections.Generic;
using System.Linq;

namespace Yuki_Theme.Core
{
	public enum SyntaxType
	{
		Pascal, CSharp, Haskell, KuMir, KuMir00, Python, VB
	}

	public static class ShadowNames
	{
		public static string [] names => new []
		{
			"Default Text",
			"Wallpaper",
			"Sticker",
			"Line Number",
			"Selection",
			"Vertical Ruler",
			"Invalid Line",
			"Caret",
			"Fold's Line",
			"Selected Fold's Line",
			"Fold's Rectangle",
			"Other Marker",
			"Number",
			"Comment",
			"String",
			"Keyword",
			"Begin, End",
			"Special Character",
			"Punctuation",
			"If, else Statements",
			"Iteration Statements",
			"Exception Handlers",
			"Types",
			"Modifiers",
			"Constants",
			"DireciveNames"
		};

		public static string [] GetFields (SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.Pascal :
					return GetPascalFields ();
					break;
				case SyntaxType.CSharp :
					return GetCSharpFields ();
					break;
				case SyntaxType.Haskell :
					return GetHaskellFields ();
					break;
				case SyntaxType.KuMir :
					return GetKuMirFields ();
					break;
				case SyntaxType.KuMir00 :
					return GetKuMir00Fields ();
					break;
				case SyntaxType.Python :
					return GetPythonFields ();
					break;
				case SyntaxType.VB :
					return GetVBFields ();
					break;

				default :
					return null;
			}
		}

		public static string [] GetEnvironmentFields ()
		{
			string [] res =
			{
				"Default",
				"Selection",
				"VRuler",
				"InvalidLines",
				"CaretMarker",
				"LineNumbers",
				"FoldLine",
				"FoldMarker",
				"SelectedFoldLine",
				"EOLMarkers",
				"SpaceMarkers",
				"TabMarkers",
			};
			return res;
		}

		public static string [] GetPascalFields ()
		{
			string [] res = 
			{
				"Digits",
				"LineBigComment",
				"LineComment",
				"BlockComment",
				"BlockComment2",
				"String",
				"KeyWords",
				"ProgramSections",
				"BeginEnd",
				"Special",
				"Async",
				"Punctuation",
				"AccessKeywords1",
				"NonReserved1",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"ExceptionHandlingStatements",
				"RaiseStatement",
				"JumpStatements",
				"JumpProcedures",
				"InternalConstant",
				"InternalTypes",
				"ReferenceTypes",
				"Modifiers",
				"AccessModifiers",
				"ErrorWords",
				"WarningWords",
				"DireciveNames",
				"SpecialDireciveNames",
				"DireciveValues"
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetCSharpFields ()
		{
			string [] res = 
			{
				"Digits",
				"PreprocessorDirectives",
				"DocLineComment",
				"LineComment",
				"LineComment2",
				"BlockComment",
				"String",
				"MultiLineString",
				"Char",
				"Punctuation",
				"AccessKeywords",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"JumpStatements",
				"ContextKeywords",
				"ExceptionHandlingStatements",
				"CheckedUncheckedStatements",
				"UnsafeFixedStatements",
				"ValueTypes",
				"ReferenceTypes",
				"Void",
				"ConversionKeyWords",
				"MethodParameters",
				"Modifiers",
				"AccessModifiers",
				"NameSpaces",
				"LockKeyWord",
				"GetSet",
				"Literals",
				"ErrorWords",
				"WarningWords",
				"XmlTag",
				"SpecialComment",
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetHaskellFields ()
		{
			string [] res = 
			{
				"Digits",
				"LineComment",
				"BlockComment",
				"CompilerDirectives",
				"Char",
				"String",
				"KeyWords",
				"ProgramSections",
				"BeginEnd",
				"Async",
				"Punctuation",
				"AccessKeywords1",
				"NonReserved1",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"ExceptionHandlingStatements",
				"RaiseStatement",
				"JumpStatements",
				"JumpProcedures",
				"InternalConstant",
				"InternalTypes",
				"ReferenceTypes",
				"Modifiers",
				"AccessModifiers",
				"ErrorWords",
				"WarningWords",
				"DireciveNames",
				"SpecialDireciveNames",
				"DireciveValues"
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetKuMirFields ()
		{
			string [] res = 
			{
				"Digits",
				"LineComment",
				"String",
				"Char",
				"KeyWords",
				"ProgramSections",
				"BeginEnd",
				"Punctuation",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"InternalConstant",
				"InternalTypes",
				"ErrorWords",
				"WarningWords"

			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetKuMir00Fields ()
		{
			string [] res = 
			{
				"Digits",
				"LineComment",
				"String",
				"Char",
				"KeyWords",
				"ProgramSections",
				"BeginEnd",
				"Punctuation",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"InternalConstant",
				"InternalTypes",
				"ErrorWords",
				"WarningWords"
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetPythonFields ()
		{
			string [] res = 
			{
				"Digits",
				"LineComment",
				"MultilineString1",
				"MultilineString2",
				"String1",
				"String2",
				"KeyWords",
				"BeginEnd",
				"Punctuation123",
				"Punctuation2",
				"OperatorKeywords",
				"SelectionStatements",
				"IterationStatements",
				"ExceptionHandlingStatements",
				"RaiseStatement",
				"JumpProcedures",
				"InternalConstant",
				"Builtins",
				"ReferenceTypes",
				"Modifiers",
				"ErrorWords",
				"WarningWords"
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetVBFields ()
		{
			string [] res = 
			{
				"Digits",
				"STRING",
				"PREPROCESSORDIRECTIVE",
				"DATELITERAL",
				"LINECOMMENT",
				"DataTypes",
				"Operators",
				"Constants",
				"CommonKeywords",
				"FunctionKeywords",
				"ParamModifiers",
				"AccessModifiers",
				"OtherModifiers",
				"Statements",
				"GlobalConstructs",
				"TypeLevelConstructs",
				"Constructs",
				"ContextKeywords",
				"PreProcessor"
			};
			res = (string []) GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string[] GetRealName (string shadow, SyntaxType type)
		{
			switch (type)
			{
				case SyntaxType.Pascal :
					return PascalFields [shadow];
					break;
				case SyntaxType.CSharp :
					return CSharpFields [shadow];
					break;
				case SyntaxType.Haskell :
					return HaskellFields [shadow];
					break;
				case SyntaxType.KuMir :
					return KuMirFields [shadow];
					break;
				case SyntaxType.KuMir00 :
					return KuMir00Fields [shadow];
					break;
				case SyntaxType.Python :
					return PythonFields [shadow];
					break;
				case SyntaxType.VB :
					return VBFields [shadow];
					break;

				default :
					return null;
			}
		}

		private static Dictionary <string, string []> EnvironmentFields = new Dictionary <string, string []>
		{
			{"Default Text" , new [] {"Default"} },
			{"Selection" , new [] {"Selection"} },
			{"Line Number" , new [] {"LineNumbers"} },
			{"Invalid Line" , new [] {"InvalidLines"} },
			{"Caret" , new [] {"CaretMarker"} },
			{"Fold's Line" , new [] {"FoldLine"} },
			{"Fold's Rectangle" , new [] {"FoldMarker"} },
			{"Other Marker" , new [] {"EOLMarkers", "SpaceMarkers", "TabMarkers"} },
			{"Selected Fold's Line" , new [] {"SelectedFoldLine"} }
		};

		private static Dictionary <string, string []> PascalFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] {"LineBigComment", "LineComment", "BlockComment", "BlockComment2" } },
			{"String" , new [] {"String"} },
			{"Keyword" , new [] {"KeyWords", "AccessKeywords1", "OperatorKeywords", "NonReserved1", "JumpStatements", "JumpProcedures", "Async"} },
			{"Begin, End" , new [] {"BeginEnd"} },
			{"Special Character" , new [] {"Special"} },
			{"Punctuation" , new [] {"Punctuation"} },
			{"If, else Statements" , new [] {"SelectionStatements"} },
			{"Iteration Statements" , new [] {"IterationStatements"} },
			{"Exception Handlers" , new [] {"ExceptionHandlingStatements", "RaiseStatement"} },
			{"Types" , new [] {"InternalTypes", "ReferenceTypes", "ProgramSections"} },
			{"Modifiers" , new [] {"Modifiers", "AccessModifiers"} },
			{"Constants" , new [] {"InternalConstant"} },
			{"DireciveNames" , new [] {"ErrorWords", "WarningWords", "DireciveNames", "SpecialDireciveNames", "DireciveValues"} },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> CSharpFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "DocLineComment", "LineComment", "LineComment2", "BlockComment" } },
			{"String" , new [] { "String", "MultiLineString", "Char" } },
			{"Keyword" , new [] { "AccessKeywords", "JumpStatements", "ContextKeywords", "CheckedUncheckedStatements", "UnsafeFixedStatements", "Void", "ConversionKeyWords", "MethodParameters", "NameSpaces", "LockKeyWord", "GetSet" } },
			{"Begin, End" , null },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation" } },
			{"If, else Statements" , new [] { "SelectionStatements" } },
			{"Iteration Statements" , new [] { "IterationStatements" } },
			{"Exception Handlers" , new [] { "ExceptionHandlingStatements" } },
			{"Types" , new [] { "ValueTypes", "ReferenceTypes", "XmlTag", "SpecialComment" } },
			{"Modifiers" , new [] { "Modifiers", "AccessModifiers" } },
			{"Constants" , new [] { "Literals", "OperatorKeywords" } },
			{"DireciveNames" , new [] { "PreprocessorDirectives", "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> HaskellFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "LineComment", "BlockComment" } },
			{"String" , new [] { "Char", "String" } },
			{"Keyword" , new [] { "KeyWords", "Async", "AccessKeywords1", "NonReserved1", "OperatorKeywords", "JumpStatements", "JumpProcedures", "", "", "", "" } },
			{"Begin, End" , new [] { "BeginEnd" } },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation" } },
			{"If, else Statements" , new [] { "SelectionStatements" } },
			{"Iteration Statements" , new [] { "IterationStatements" } },
			{"Exception Handlers" , new [] { "ExceptionHandlingStatements", "RaiseStatement" } },
			{"Types" , new [] { "ProgramSections", "InternalTypes", "ReferenceTypes" } },
			{"Modifiers" , new [] { "Modifiers", "AccessModifiers" } },
			{"Constants" , new [] { "InternalConstant" } },
			{"DireciveNames" , new [] { "CompilerDirectives", "ErrorWords", "WarningWords", "DireciveNames", "SpecialDireciveNames", "DireciveValues" } },	
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> KuMirFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "LineComment" } },
			{"String" , new [] { "String", "Char" } },
			{"Keyword" , new [] { "KeyWords", "OperatorKeywords" } },
			{"Begin, End" , new [] { "BeginEnd" } },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation" } },
			{"If, else Statements" , new [] { "SelectionStatements" } },
			{"Iteration Statements" , new [] { "IterationStatements" } },
			{"Exception Handlers" , null },
			{"Types" , new [] { "ProgramSections", "InternalTypes" } },
			{"Modifiers" , null },
			{"Constants" , new [] { "InternalConstant" } },
			{"DireciveNames" , new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> KuMir00Fields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "LineComment" } },
			{"String" , new [] { "String", "Char" } },
			{"Keyword" , new [] { "KeyWords", "OperatorKeywords" } },
			{"Begin, End" , new [] { "BeginEnd" } },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation" } },
			{"If, else Statements" , new [] { "SelectionStatements" } },
			{"Iteration Statements" , new [] { "IterationStatements" } },
			{"Exception Handlers" , null },
			{"Types" , new [] { "ProgramSections", "InternalTypes" } },
			{"Modifiers" , null },
			{"Constants" , new [] { "InternalConstant" } },
			{"DireciveNames" , new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> PythonFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "LineComment" } },
			{"String" , new [] { "MultilineString1", "MultilineString2", "String1", "String2" } },
			{"Keyword" , new [] { "KeyWords", "OperatorKeywords", "JumpProcedures", "Builtins" } },
			{"Begin, End" , new [] { "BeginEnd" } },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation2", "Punctuation123" } },
			{"If, else Statements" , new [] { "SelectionStatements" } },
			{"Iteration Statements" , new [] { "IterationStatements" } },
			{"Exception Handlers" , new [] { "ExceptionHandlingStatements", "RaiseStatement" } },
			{"Types" , new [] { "ReferenceTypes" } },
			{"Modifiers" , new [] { "Modifiers" } },
			{"Constants" , new [] { "InternalConstant" } },
			{"DireciveNames" , new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> VBFields = new Dictionary <string, string []>
		{
			{"Number" , new [] {"Digits"} },
			{"Comment" , new [] { "LINECOMMENT" } },
			{"String" , new [] { "STRING" } },
			{"Keyword" , new [] { "Operators", "CommonKeywords", "FunctionKeywords", "Statements", "TypeLevelConstructs", "ContextKeywords" } },
			{"Begin, End" , null },
			{"Special Character" , null },
			{"Punctuation" , new [] { "Punctuation" } },
			{"If, else Statements" , new [] { "Constructs" } },
			{"Iteration Statements" , null },
			{"Exception Handlers" , null },
			{"Types" , new [] { "DataTypes", "GlobalConstructs" } },
			{"Modifiers" , new [] { "ParamModifiers", "AccessModifiers", "OtherModifiers" } },
			{"Constants" , new [] { "Constants" } },
			{"DireciveNames" , new [] { "PREPROCESSORDIRECTIVE", "DATELITERAL", "PreProcessor" } },
		}.Merge (EnvironmentFields);

		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> merge)
		{
			foreach (var item in merge)
			{
				me[item.Key] = item.Value;
			}

			return me;
		}
		
	}
}