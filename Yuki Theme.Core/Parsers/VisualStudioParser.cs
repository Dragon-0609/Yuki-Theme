using System;
using System.Collections.Generic;
using System.Xml;

namespace Yuki_Theme.Core.Parsers
{
	public class VisualStudioParser : AbstractParser
	{
		public override void populateList (string path)
		{
			var doc = new XmlDocument ();

			doc.Load (path);
			XmlNode nodeparent = doc.SelectSingleNode ("/UserSettings/Category/Category/FontsAndColors/Categories/Category/Items");
			// PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='Plain Text']"));
			
			PopulateByXMLNodeSingleType ("Selection", nodeparent.SelectSingleNode ("Item[@Name='SELECTION_BACKGROUND']"));
			
			PopulateByXMLNodeSingleType ("VRuler", doc.SelectSingleNode ("/UserSettings/colors/Item[@Name='VISUAL_INDENT_GUIDE']"),
			                             null);
			PopulateByXMLNodeSingleType ("CaretMarker", doc.SelectSingleNode ("/UserSettings/colors/Item[@Name='CARET_COLOR']"), null);

			Dictionary <string, string> df = attributes ["Default"];
			/*foreach (KeyValuePair <string, string> rf in df)
			{
				Console.WriteLine($"{rf.Key}: {rf.Value}");
			}
			*/

			// Console.WriteLine (attributes.Count);
			PopulateByXMLNodeSingleType ("LineNumbers", doc.SelectSingleNode ("/UserSettings/colors/Item[@Name='LINE_NUMBERS_COLOR']"),
			                             null, new Dictionary <string, string> (){{"bgcolor",df["bgcolor"]}});
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_NUMBER']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_KEYWORD']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_FUNCTION_DECLARATION']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_LINE_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_BLOCK_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_STRING']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/UserSettings/attributes/Item[@Name='DEFAULT_COMMA']"));
			Dictionary <string, string> dic = new Dictionary <string, string> ();
			dic.Add ("color", df["color"]);
			dic.Add ("bgcolor", df["bgcolor"]);
			attributes.Add ("FoldMarker", dic);
			attributes.Add ("SelectedFoldLine", dic);
			dic.Remove ("bgcolor");
			attributes.Add ("FoldLine", dic);

			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		new public void PopulateByXMLNodeSingleType (string                      name, XmlNode node, XmlNode node2=null,
		                                          Dictionary <string, string> defaultValues = null)
		{
			
			var attrs = new Dictionary <string, string> ();
			
			attrs.Add ("color", "#" + GetValue (node));

			attrs.Add ("bgcolor", "#" + GetValue (node));
			if (defaultValues != null)
			{
				foreach (var value in defaultValues)
				{
					attrs.Add (value.Key, value.Value);
				}
			}

			attributes.Add (name, attrs);
		}

		public override string GetValue (XmlNode child)
		{
			return "";
		}

		public override void PopulateByXMLNodeTreeType (XmlNode node)
		{
			// Console.WriteLine (node.Name);
			/*foreach (XmlAttribute att in node.Attributes)
			{
				Console.WriteLine ($"{node.Name}, {att.Name}: {att.Value}");				
			}*/
			var name = getName (node.Attributes ["Name"].Value);

			var attrs = new Dictionary <string, string> ();

			var child = node.SelectSingleNode ("value/Item[@Name='FOREGROUND']");

			if (child != null) attrs.Add ("color", "#" + GetValue (child));

			child = node.SelectSingleNode ("value/Item[@Name='BACKGROUND']");

			if (child != null) attrs.Add ("bgcolor", "#" + GetValue (child));

			if (attrs.Count == 0) attrs = populateDefaultAttributes (name [0]);

			foreach (var nm in name)
			{
				attributes.Add (nm, attrs);
			}


			// Console.WriteLine("TEST");
		}

		public string GetValue (XmlNode child, int mode=0)
		{
			string attr_value = child.Attributes [mode == 0 ? "Foreground" : "Background"].Value;
			if (attr_value.StartsWith ("0x"))
			{
				attr_value = attr_value.Substring (4);
			}
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

		private bool isInNames (string nam)
		{
			return false;
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

				case "CARET_COLOR" :
				{
					res = new [] {"String"};
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