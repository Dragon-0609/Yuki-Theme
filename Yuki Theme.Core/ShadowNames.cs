﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Yuki_Theme.Core
{
	public enum SyntaxType
	{
		Pascal, CSharp, Haskell, KuMir, KuMir00, Python, VBNET, NULL
	}

	public static class ShadowNames
	{
		private const string EXTENSION_CSHARP  = ".cs";
		private const string EXTENSION_HASKELL = ".hs";
		private const string EXTENSION_KUMIR   = ".alg";
		private const string EXTENSION_KUMIR00 = ".alg00";
		private const string EXTENSION_PASCAL  = ".pas";
		private const string EXTENSION_PASCAL2 = ".paspart_";
		private const string EXTENSION_PYTHON  = ".py";
		private const string EXTENSION_VBNET   = ".vb";

		public static string [] names = new []
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
			"Method",
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

		public static string [] imageNames = new []
		{
			"Wallpaper",
			"Sticker",
		};

		public static string [] GetFields (SyntaxType type)
		{
			string [] res = null;
			if (type == SyntaxType.Pascal)
				res = GetPascalFields ();
			else if (type == SyntaxType.CSharp)
				res = GetCSharpFields ();
			else if (type == SyntaxType.Haskell)
				res = GetHaskellFields ();
			else if (type == SyntaxType.KuMir)
				res = GetKuMirFields ();
			else if (type == SyntaxType.KuMir00)
				res = GetKuMir00Fields ();
			else if (type == SyntaxType.Python)
				res = GetPythonFields ();
			else if (type == SyntaxType.VBNET)
				res = GetVBNETFields ();
			return res;
		}

		public static Dictionary<string, string[]> GetDictionaryFields (SyntaxType type)
		{
			Dictionary<string, string[]> res = null;
			if (type == SyntaxType.Pascal)
				res = PascalFields;
			else if (type == SyntaxType.CSharp)
				res = CSharpFields;
			else if (type == SyntaxType.Haskell)
				res = HaskellFields;
			else if (type == SyntaxType.KuMir)
				res = KuMirFields;
			else if (type == SyntaxType.KuMir00)
				res = KuMir00Fields;
			else if (type == SyntaxType.Python)
				res = PythonFields;
			else if (type == SyntaxType.VBNET)
				res = VBNETFields;
			return res;
		}
		
		private static Dictionary <string, string> GetRevertedFields (SyntaxType type) 
		{
			Dictionary <string, string> fields = null;
			if (type == SyntaxType.Pascal)
				fields = PascalFields_Reverted;
			else if (type == SyntaxType.CSharp)
				fields = CSharpFields_Reverted;
			else if (type == SyntaxType.Haskell)
				fields = HaskellFields_Reverted;
			else if (type == SyntaxType.KuMir)
				fields = KuMirFields_Reverted;
			else if (type == SyntaxType.KuMir00)
				fields = KuMir00Fields_Reverted;
			else if (type == SyntaxType.Python)
				fields = PythonFields_Reverted;
			else if (type == SyntaxType.VBNET)
				fields = VBNETFields_Reverted;
			return fields;
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
				"MarkPrevious",
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
			res = (string [])GetEnvironmentFields ().Concat (res);
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
				"MarkPrevious",
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
			res = (string [])GetEnvironmentFields ().Concat (res);
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
			res = (string [])GetEnvironmentFields ().Concat (res);
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
			res = (string [])GetEnvironmentFields ().Concat (res);
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
			res = (string [])GetEnvironmentFields ().Concat (res);
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
			res = (string [])GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetVBNETFields ()
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
			res = (string [])GetEnvironmentFields ().Concat (res);
			return res;
		}

		public static string [] GetRealName (string shadow, SyntaxType type)
		{
			string [] res = null;
			Dictionary <string, string []> fields = GetDictionaryFields (type);
			if (fields != null)
				res = fields [shadow];
			return res;
		}

		public static bool HasRealName (string shadow, SyntaxType type)
		{
			bool res = false;
			Dictionary <string, string []> fields = GetDictionaryFields (type);
			if (fields != null)
				res = fields.ContainsKey (shadow);
			
			return res;
		}

		/// <summary>
		/// Get Shadow Name by the syntax file.
		/// </summary>
		/// <param name="real">Real name</param>
		/// <param name="type">Syntax</param>
		/// <param name="oldSaved">If it's old saved, then all fields will be got by Pascal syntax highlighting</param>
		/// <returns></returns>
		public static string GetShadowName (string real, SyntaxType type, bool oldSaved = false)
		{
			string res = null;
			if (oldSaved)
			{
				if (PascalFields_Reverted.ContainsKey (real))
					res = PascalFields_Reverted [real];
				else
					res = real;
			} else
			{
				Dictionary<string,string> fields = GetRevertedFields (type);

				if (fields != null && fields.ContainsKey (real))
					res = fields [real];
			}

			return res;
		}


		private static Dictionary <string, string []> EnvironmentFields = new Dictionary <string, string []>
		{
			{ "Default Text", new [] { "Default" } },
			{ "Selection", new [] { "Selection" } },
			{ "Line Number", new [] { "LineNumbers" } },
			{ "Invalid Line", new [] { "InvalidLines" } },
			{ "Vertical Ruler", new [] { "VRuler" } },
			{ "Caret", new [] { "CaretMarker" } },
			{ "Fold's Line", new [] { "FoldLine" } },
			{ "Fold's Rectangle", new [] { "FoldMarker" } },
			{ "Other Marker", new [] { "EOLMarkers", "SpaceMarkers", "TabMarkers" } },
			{ "Selected Fold's Line", new [] { "SelectedFoldLine" } }
		};

		public static Dictionary <string, string []> PascalFields_raw = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LineBigComment", "LineComment", "BlockComment", "BlockComment2" } },
			{ "String", new [] { "String" } },
			{ "Method", new [] { "MarkPrevious" } },
			{
				"Keyword",
				new [] { "KeyWords", "AccessKeywords1", "OperatorKeywords", "NonReserved1", "JumpStatements", "JumpProcedures", "Async" }
			},
			{ "Begin, End", new [] { "BeginEnd" } },
			{ "Special Character", new [] { "Special" } },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", new [] { "ExceptionHandlingStatements", "RaiseStatement" } },
			{ "Types", new [] { "InternalTypes", "ReferenceTypes", "ProgramSections" } },
			{ "Modifiers", new [] { "Modifiers", "AccessModifiers" } },
			{ "Constants", new [] { "InternalConstant" } },
			{ "DireciveNames", new [] { "ErrorWords", "WarningWords", "DireciveNames", "SpecialDireciveNames", "DireciveValues" } },
		};

		public static Dictionary <string, string []> PascalFields = MergeD (PascalFields_raw, EnvironmentFields);

		private static Dictionary <string, string []> CSharpFields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "DocLineComment", "LineComment", "LineComment2", "BlockComment" } },
			{ "String", new [] { "String", "MultiLineString", "Char" } },
			{ "Method", new [] { "MarkPrevious" } },
			{
				"Keyword",
				new []
				{
					"AccessKeywords", "JumpStatements", "ContextKeywords", "CheckedUncheckedStatements", "UnsafeFixedStatements", "Void",
					"ConversionKeyWords", "MethodParameters", "NameSpaces", "LockKeyWord", "GetSet"
				}
			},
			{ "Begin, End", null },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", new [] { "ExceptionHandlingStatements" } },
			{ "Types", new [] { "ValueTypes", "ReferenceTypes", "XmlTag", "SpecialComment" } },
			{ "Modifiers", new [] { "Modifiers", "AccessModifiers" } },
			{ "Constants", new [] { "Literals", "OperatorKeywords" } },
			{ "DireciveNames", new [] { "PreprocessorDirectives", "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> HaskellFields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LineComment", "BlockComment" } },
			{ "String", new [] { "Char", "String" } },
			{
				"Keyword",
				new [] { "KeyWords", "Async", "AccessKeywords1", "NonReserved1", "OperatorKeywords", "JumpStatements", "JumpProcedures" }
			},
			{ "Begin, End", new [] { "BeginEnd" } },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", new [] { "ExceptionHandlingStatements", "RaiseStatement" } },
			{ "Types", new [] { "ProgramSections", "InternalTypes", "ReferenceTypes" } },
			{ "Modifiers", new [] { "Modifiers", "AccessModifiers" } },
			{ "Constants", new [] { "InternalConstant" } },
			{
				"DireciveNames",
				new [] { "CompilerDirectives", "ErrorWords", "WarningWords", "DireciveNames", "SpecialDireciveNames", "DireciveValues" }
			},
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> KuMirFields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LineComment" } },
			{ "String", new [] { "String", "Char" } },
			{ "Keyword", new [] { "KeyWords", "OperatorKeywords" } },
			{ "Begin, End", new [] { "BeginEnd" } },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", null },
			{ "Types", new [] { "ProgramSections", "InternalTypes" } },
			{ "Modifiers", null },
			{ "Constants", new [] { "InternalConstant" } },
			{ "DireciveNames", new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> KuMir00Fields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LineComment" } },
			{ "String", new [] { "String", "Char" } },
			{ "Keyword", new [] { "KeyWords", "OperatorKeywords" } },
			{ "Begin, End", new [] { "BeginEnd" } },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", null },
			{ "Types", new [] { "ProgramSections", "InternalTypes" } },
			{ "Modifiers", null },
			{ "Constants", new [] { "InternalConstant" } },
			{ "DireciveNames", new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> PythonFields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LineComment" } },
			{ "String", new [] { "MultilineString1", "MultilineString2", "String1", "String2" } },
			{ "Keyword", new [] { "KeyWords", "OperatorKeywords", "JumpProcedures", "Builtins" } },
			{ "Begin, End", new [] { "BeginEnd" } },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation2", "Punctuation123" } },
			{ "If, else Statements", new [] { "SelectionStatements" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", new [] { "ExceptionHandlingStatements", "RaiseStatement" } },
			{ "Types", new [] { "ReferenceTypes" } },
			{ "Modifiers", new [] { "Modifiers" } },
			{ "Constants", new [] { "InternalConstant" } },
			{ "DireciveNames", new [] { "ErrorWords", "WarningWords" } },
		}.Merge (EnvironmentFields);

		private static Dictionary <string, string []> VBNETFields = new Dictionary <string, string []>
		{
			{ "Number", new [] { "Digits" } },
			{ "Comment", new [] { "LINECOMMENT" } },
			{ "String", new [] { "STRING" } },
			{
				"Keyword",
				new [] { "Operators", "CommonKeywords", "FunctionKeywords", "Statements", "TypeLevelConstructs", "ContextKeywords" }
			},
			{ "Begin, End", null },
			{ "Special Character", null },
			{ "Punctuation", new [] { "Punctuation" } },
			{ "If, else Statements", new [] { "Constructs" } },
			{ "Iteration Statements", new [] { "IterationStatements" } },
			{ "Exception Handlers", new [] { "Exception" } },
			{ "Types", new [] { "DataTypes", "GlobalConstructs" } },
			{ "Modifiers", new [] { "ParamModifiers", "AccessModifiers", "OtherModifiers" } },
			{ "Constants", new [] { "Constants" } },
			{ "DireciveNames", new [] { "PREPROCESSORDIRECTIVE", "DATELITERAL", "PreProcessor" } },
		}.Merge (EnvironmentFields);


		private static Dictionary <string, string> EnvironmentFields_Reverted = new Dictionary <string, string>
		{
			{ "Default", "Default Text" },
			{ "Selection", "Selection" },
			{ "LineNumbers", "Line Number" },
			{ "VRuler", "Vertical Ruler" },
			{ "InvalidLines", "Invalid Line" },
			{ "CaretMarker", "Caret" },
			{ "FoldLine", "Fold's Line" },
			{ "FoldMarker", "Fold's Rectangle" },
			{ "EOLMarkers", "Other Marker" },
			{ "SpaceMarkers", "Other Marker" },
			{ "TabMarkers", "Other Marker" },
			{ "SelectedFoldLine", "Selected Fold's Line" },
			{ "Wallpaper", null },
			{ "Sticker", null },
		};

		private static Dictionary <string, string> PascalFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LineBigComment", "Comment" },
			{ "LineComment", "Comment" },
			{ "BlockComment", "Comment" },
			{ "BlockComment2", "Comment" },
			{ "MarkPrevious", "Method" },
			{ "String", "String" },
			{ "KeyWords", "Keyword" },
			{ "AccessKeywords1", "Keyword" },
			{ "OperatorKeywords", "Keyword" },
			{ "NonReserved1", "Keyword" },
			{ "JumpStatements", "Keyword" },
			{ "JumpProcedures", "Keyword" },
			{ "Async", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Special", "Special Character" },
			{ "Punctuation", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ExceptionHandlingStatements", "Exception Handlers" },
			{ "RaiseStatement", "Exception Handlers" },
			{ "InternalTypes", "Types" },
			{ "ReferenceTypes", "Types" },
			{ "ProgramSections", "Types" },
			{ "Modifiers", "Modifiers" },
			{ "AccessModifiers", "Modifiers" },
			{ "InternalConstant", "Constants" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
			{ "DireciveNames", "DireciveNames" },
			{ "SpecialDireciveNames", "DireciveNames" },
			{ "DireciveValues", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> CSharpFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "DocLineComment", "Comment" },
			{ "LineComment", "Comment" },
			{ "LineComment2", "Comment" },
			{ "BlockComment", "Comment" },
			{ "String", "String" },
			{ "MarkPrevious", "Method" },
			{ "MultiLineString", "String" },
			{ "Char", "String" },
			{ "AccessKeywords", "Keyword" },
			{ "JumpStatements", "Keyword" },
			{ "ContextKeywords", "Keyword" },
			{ "CheckedUncheckedStatements", "Keyword" },
			{ "UnsafeFixedStatements", "Keyword" },
			{ "Void", "Keyword" },
			{ "ConversionKeyWords", "Keyword" },
			{ "MethodParameters", "Keyword" },
			{ "NameSpaces", "Keyword" },
			{ "LockKeyWord", "Keyword" },
			{ "GetSet", "Keyword" },
			{ "Punctuation", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ExceptionHandlingStatements", "Exception Handlers" },
			{ "ValueTypes", "Types" },
			{ "ReferenceTypes", "Types" },
			{ "XmlTag", "Types" },
			{ "SpecialComment", "Types" },
			{ "Modifiers", "Modifiers" },
			{ "AccessModifiers", "Modifiers" },
			{ "Literals", "Constants" },
			{ "OperatorKeywords", "Constants" },
			{ "PreprocessorDirectives", "DireciveNames" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> HaskellFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LineComment", "Comment" },
			{ "BlockComment", "Comment" },
			{ "Char", "String" },
			{ "String", "String" },
			{ "KeyWords", "Keyword" },
			{ "Async", "Keyword" },
			{ "AccessKeywords1", "Keyword" },
			{ "NonReserved1", "Keyword" },
			{ "OperatorKeywords", "Keyword" },
			{ "JumpStatements", "Keyword" },
			{ "JumpProcedures", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Punctuation", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ExceptionHandlingStatements", "Exception Handlers" },
			{ "RaiseStatement", "Exception Handlers" },
			{ "ProgramSections", "Types" },
			{ "InternalTypes", "Types" },
			{ "ReferenceTypes", "Types" },
			{ "Modifiers", "Modifiers" },
			{ "AccessModifiers", "Modifiers" },
			{ "Constants", "InternalConstant" },
			{ "CompilerDirectives", "DireciveNames" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
			{ "DireciveNames", "DireciveNames" },
			{ "SpecialDireciveNames", "DireciveNames" },
			{ "DireciveValues", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> KuMirFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LineComment", "Comment" },
			{ "String", "String" },
			{ "Char", "String" },
			{ "KeyWords", "Keyword" },
			{ "OperatorKeywords", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Punctuation", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ProgramSections", "Types" },
			{ "InternalTypes", "Types" },
			{ "InternalConstant", "Constants" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> KuMir00Fields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LineComment", "Comment" },
			{ "String", "String" },
			{ "Char", "String" },
			{ "KeyWords", "Keyword" },
			{ "OperatorKeywords", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Punctuation", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ProgramSections", "Types" },
			{ "InternalTypes", "Types" },
			{ "InternalConstant", "Constants" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> PythonFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LineComment", "Comment" },
			{ "MultilineString1", "String" },
			{ "MultilineString2", "String" },
			{ "String1", "String" },
			{ "String2", "String" },
			{ "KeyWords", "Keyword" },
			{ "OperatorKeywords", "Keyword" },
			{ "JumpProcedures", "Keyword" },
			{ "Builtins", "Keyword" },
			{ "BeginEnd", "Begin, End" },
			{ "Punctuation2", "Punctuation" },
			{ "Punctuation123", "Punctuation" },
			{ "SelectionStatements", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "ExceptionHandlingStatements", "Exception Handlers" },
			{ "RaiseStatement", "Exception Handlers" },
			{ "ReferenceTypes", "Types" },
			{ "Modifiers", "Modifiers" },
			{ "InternalConstant", "Constants" },
			{ "ErrorWords", "DireciveNames" },
			{ "WarningWords", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		private static Dictionary <string, string> VBNETFields_Reverted = new Dictionary <string, string>
		{
			{ "Digits", "Number" },
			{ "LINECOMMENT", "Comment" },
			{ "STRING", "String" },
			{ "Operators", "Keyword" },
			{ "CommonKeywords", "Keyword" },
			{ "FunctionKeywords", "Keyword" },
			{ "Statements", "Keyword" },
			{ "TypeLevelConstructs", "Keyword" },
			{ "ContextKeywords", "Keyword" },
			{ "Punctuation", "Punctuation" },
			{ "Constructs", "If, else Statements" },
			{ "IterationStatements", "Iteration Statements" },
			{ "Exception", "Exception Handlers" },
			{ "DataTypes", "Types" },
			{ "GlobalConstructs", "Types" },
			{ "ParamModifiers", "Modifiers" },
			{ "AccessModifiers", "Modifiers" },
			{ "OtherModifiers", "Modifiers" },
			{ "Constants", "Constants" },
			{ "PREPROCESSORDIRECTIVE", "DireciveNames" },
			{ "DATELITERAL", "DireciveNames" },
			{ "PreProcessor", "DireciveNames" },
		}.Merge (EnvironmentFields_Reverted);

		public static Dictionary <TKey, TValue> MergeD <TKey, TValue> (Dictionary <TKey, TValue> me, Dictionary <TKey, TValue> merge)
		{
			Dictionary <TKey, TValue> newDict = new Dictionary <TKey, TValue> ();
			foreach (var item in me)
			{
				newDict [item.Key] = item.Value;
			}

			foreach (var item in merge)
			{
				newDict [item.Key] = item.Value;
			}

			return newDict;
		}

		public static Dictionary <TKey, TValue> Merge <TKey, TValue> (this Dictionary <TKey, TValue> me, Dictionary <TKey, TValue> merge)
		{
			foreach (var item in merge)
			{
				me [item.Key] = item.Value;
			}

			return me;
		}

		private static readonly Dictionary <string, SyntaxType> SyntaxTypes = new ()
		{
			{"EXTENSION_CSHARP", SyntaxType.CSharp},
			{"EXTENSION_HASKELL", SyntaxType.Haskell},
			{"EXTENSION_KUMIR", SyntaxType.KuMir},
			{"EXTENSION_KUMIR00", SyntaxType.KuMir00},
			{"EXTENSION_PASCAL", SyntaxType.Pascal},
			{"EXTENSION_PASCAL2", SyntaxType.Pascal},
			{"EXTENSION_PYTHON", SyntaxType.Python},
			{"EXTENSION_VBNET", SyntaxType.VBNET},
		};

		public static SyntaxType GetSyntaxByExtension (string str)
		{
			SyntaxType type = SyntaxType.Pascal;
			if (str != null && SyntaxTypes.ContainsKey (str))
			{
				type = SyntaxTypes [str];
			}

			return type;
		}
	}
}