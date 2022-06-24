using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public class JetBrainsParser : AbstractParser
	{
		private readonly Dictionary <string, ThemeField> _defaultAttributes = new ()
		{
			{ "DEFAULT_COMMA", new ThemeField { Foreground = "#FFFFFF" } }
		};

		private readonly string [] _necessaryAttributes = new [] { "FOREGROUND", "BACKGROUND" };

		private readonly Dictionary <string, string []> _convertedNames = new ()
		{
			{ "TEXT", new [] { "Default" } },
			{ "DEFAULT_NUMBER", new [] { "Digits" } },
			{ "DEFAULT_LINE_COMMENT", new [] { "LineBigComment", "LineComment" } },
			{ "DEFAULT_BLOCK_COMMENT", new [] { "BlockComment", "BlockComment2" } },
			{ "DEFAULT_STRING", new [] { "String" } },
			{
				"DEFAULT_KEYWORD", new []
				{
					"KeyWords", "ProgramSections", "Async", "AccessKeywords1", "NonReserved1", "OperatorKeywords",
					"SelectionStatements", "IterationStatements", "ExceptionHandlingStatements", "RaiseStatement",
					"JumpStatements", "JumpProcedures", "InternalConstant", "InternalTypes", "ReferenceTypes",
					"Modifiers", "AccessModifiers", "ErrorWords", "WarningWords", "DireciveNames",
					"SpecialDireciveNames", "DireciveValues"
				}
			},
			{ "DEFAULT_FUNCTION_DECLARATION", new [] { "BeginEnd" } },
			{ "DEFAULT_COMMA", new [] { "Punctuation" } },

		};

		public override void populateList (string path)
		{
			XmlDocument doc = new XmlDocument ();

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
			
			string[] names = getName (node.Attributes ["name"].Value);

			ThemeField attrs = new ThemeField ();

			XmlNode child = node.SelectSingleNode ("value/option[@name='FOREGROUND']");

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

			if (attrs.IsNull ()) attrs = populateDefaultAttributes (names [0]);

			foreach (string nm in names)
			{
				theme.Fields.Add (nm, attrs);
			}


			// Console.WriteLine("TEST");
		}

		public override string GetValue (XmlNode child)
		{
			string attrValue = "";
			if (child.Attributes != null)
			{
				attrValue = child.Attributes ["value"].Value;
			}
			return attrValue;
		}

		public override ThemeField populateDefaultAttributes (string name)
		{
			ThemeField att = new ThemeField ();
			if (_defaultAttributes.ContainsKey (name))
				att = _defaultAttributes [name];
			
			return att;
		}

		public override bool isNecessaryAttribute (string name)
		{
			return _necessaryAttributes.Contains (name);
		}

		public override string [] getName (string st)
		{
			string [] res;
			if (_convertedNames.ContainsKey (st))
				res = _convertedNames [st];
			else
				res = new string[0];
			return res;
		}

		public override void finishParsing (string path)
		{
			
		}
	}
}