using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public class JetBrainsParser : AbstractParser
	{
		
		public override void populateList (string path)
		{
			var doc = new XmlDocument ();

			if (needToWrite)
				doc.Load (path);
			else
				doc.LoadXml (path);

			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='TEXT']"));
			PopulateByXMLNodeSingleType ("Selection", null,
			                             doc.SelectSingleNode ("/scheme/colors/option[@name='SELECTION_BACKGROUND']"));
			PopulateByXMLNodeSingleType ("VRuler", doc.SelectSingleNode ("/scheme/colors/option[@name='VISUAL_INDENT_GUIDE']"),
			                             null);
			PopulateByXMLNodeSingleType ("CaretMarker", doc.SelectSingleNode ("/scheme/colors/option[@name='CARET_COLOR']"), null);

			ThemeField df = theme.Fields ["Default"];
			/*foreach (KeyValuePair <string, string> rf in df)
			{
				Console.WriteLine($"{rf.Key}: {rf.Value}");
			}
			*/

			// Console.WriteLine (attributes.Count);
			PopulateByXMLNodeSingleType ("LineNumbers", doc.SelectSingleNode ("/scheme/colors/option[@name='LINE_NUMBERS_COLOR']"),
			                             null, new Dictionary <string, string> (){{"bgcolor",df.Background}});
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_NUMBER']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_KEYWORD']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_FUNCTION_DECLARATION']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_LINE_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_BLOCK_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_STRING']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_COMMA']"));
			ThemeField dic = new ThemeField();
			dic.Foreground = df.Foreground;
			dic.Background = df.Background;
			theme.Fields.Add ("FoldMarker", dic);
			theme.Fields.Add ("SelectedFoldLine", dic);
			dic.Background = null;
			theme.Fields.Add ("FoldLine", dic);

			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		public override void PopulateByXMLNodeTreeType (XmlNode node)
		{
			
			var name = getName (node.Attributes ["name"].Value);

			var attrs = new ThemeField ();

			var child = node.SelectSingleNode ("value/option[@name='FOREGROUND']");

			if (child != null) attrs.Foreground = "#" + GetValue (child);

			child = node.SelectSingleNode ("value/option[@name='BACKGROUND']");

			if (child != null) attrs.Background = "#" + GetValue (child);

			child = node.SelectSingleNode ("value/option[@name='FONT_TYPE']");
			
			if (child != null)
			{
				string ds = GetValue (child);
				attrs.Bold = ds == "1" || ds == "3";
				attrs.Italic = ds == "2" || ds == "3";
			}

			if (attrs.isNull ()) attrs = populateDefaultAttributes (name [0]);

			foreach (var nm in name)
			{
				theme.Fields.Add (nm, attrs);
			}


			// Console.WriteLine("TEST");
		}

		public override string GetValue (XmlNode child)
		{
			var attr_value = child.Attributes ["value"].Value;

			return attr_value;
		}

		public override ThemeField populateDefaultAttributes (string name)
		{
			var att = new ThemeField ();
			switch (name)
			{
				case "DEFAULT_COMMA" :
				{
					att.Foreground = "#FFFFFF";
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

		public override string [] getName (string st)
		{
			string [] res = new string[] { };
			switch (st)
			{
				case "TEXT" :
				{
					res = new [] {"Default"};
				}
					break;

				case "DEFAULT_NUMBER" :
				{
					res = new [] {"Digits"};
				}
					break;

				case "DEFAULT_LINE_COMMENT" :
				{
					res = new [] {"LineBigComment", "LineComment"};
				}
					break;

				case "DEFAULT_BLOCK_COMMENT" :
				{
					res = new [] {"BlockComment", "BlockComment2"};
				}
					break;
				
				case "DEFAULT_STRING" :
				{
					res = new [] {"String"};
				}
					break;

				case "DEFAULT_KEYWORD" :
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

				case "DEFAULT_FUNCTION_DECLARATION" :
				{
					res = new []
					{
						"BeginEnd"
					};
				}
					break;

				case "DEFAULT_COMMA" :
				{
					res = new [] {"Punctuation"};
				}
					break;
			}

			return res;
		}

		public override void finishParsing (string path)
		{
			
		}
	}
}