using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Yuki_Theme.Forms;
using Yuki_Theme.Themes;

namespace Yuki_Theme
{
	public class JetBrainsParser
	{
		private Dictionary <string, Dictionary <string, string>> attributes;

		public void Parse (string path, MForm form)
		{
			attributes = new Dictionary <string, Dictionary <string, string>> ();
			populateList (path);
			if (!Directory.Exists ("Themes"))
				Directory.CreateDirectory ("Themes");
			string syt = form.schemes.Items [1].ToString ();
			string st = Path.GetFileNameWithoutExtension (path);
			string patsh = $"Themes/{st}.yukitheme";
			if (DefaultThemes.isDefault (syt))
				form.CopyFromMemory (syt, patsh);
			else
				File.Copy ($"Themes/{st}.yukitheme", patsh,true );
			
			MergeFiles (patsh);
			
			form.schemes.Items.Add (st);
			form.schemes.SelectedItem = st;
		}

		private void MergeFiles (string path)
		{
			
				var doc = new XmlDocument ();
				doc.Load (path);

				#region Environment

				var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");

				foreach (XmlNode childNode in node.ChildNodes)
					if (childNode.Attributes != null &&
					    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
					{
						var nms = childNode.Name;
						if (childNode.Name == "Span" || childNode.Name == "KeyWords")
							nms = childNode.Attributes ["name"].Value;
						if (!attributes.ContainsKey (nms)) continue;

						var attrs = attributes [nms];

						foreach (var att in attrs)
							// Console.WriteLine($"N: {childNode.Name}, ATT: {att.Key},");
							childNode.Attributes [att.Key].Value = att.Value;
					}

				#endregion

				#region Digits

				node = doc.SelectSingleNode ("/SyntaxDefinition/Digits");
				if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = node.Name;
					if (node.Name == "Span" || node.Name == "KeyWords") nms = node.Attributes ["name"].Value;
					if (attributes.ContainsKey (nms))
					{
						var attrs = attributes [nms];

						foreach (var att in attrs) node.Attributes [att.Key].Value = att.Value;
					}
				}

				#endregion

				#region Syntax

				node = doc.SelectSingleNode ("/SyntaxDefinition/RuleSets");
				foreach (XmlNode xne in node.ChildNodes)
				{
					foreach (XmlNode xn in xne.ChildNodes)
						if (xn.Attributes != null &&
						    !string.Equals (xn.Name, "Delimiters", StringComparison.Ordinal))
						{
							var nms = xn.Name;
							if (xn.Name == "Span" || xn.Name == "KeyWords")
								nms = xn.Attributes ["name"].Value;
							if (!attributes.ContainsKey (nms)) continue;

							var attrs = attributes [nms];

							foreach (var att in attrs)
								// Console.WriteLine($"2N: {xn.Attributes["name"].Value}, ATT: {att.Key},");
								xn.Attributes [att.Key].Value = att.Value;
						}
				}

				;

				#endregion

				doc.Save (path);
		}

		private void populateList (string path)
		{
			var doc = new XmlDocument ();

			doc.Load (path);

			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='TEXT']"));
			PopulateByXMLNodeSingleType ("Selection", null,
			                             doc.SelectSingleNode ("/scheme/colors/option[@name='SELECTION_BACKGROUND']"));
			PopulateByXMLNodeSingleType ("VRuler", doc.SelectSingleNode ("/scheme/colors/option[@name='VISUAL_INDENT_GUIDE']"),
			                             null);
			PopulateByXMLNodeSingleType ("CaretMarker", doc.SelectSingleNode ("/scheme/colors/option[@name='CARET_COLOR']"), null);

			Dictionary <string, string> df = attributes ["Default"];
			/*foreach (KeyValuePair <string, string> rf in df)
			{
				Console.WriteLine($"{rf.Key}: {rf.Value}");
			}
			*/

			// Console.WriteLine (attributes.Count);
			PopulateByXMLNodeSingleType ("LineNumbers", doc.SelectSingleNode ("/scheme/colors/option[@name='LINE_NUMBERS_COLOR']"),
			                             null, new Dictionary <string, string> (){{"bgcolor",df["bgcolor"]}});
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_NUMBER']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_KEYWORD']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_FUNCTION_DECLARATION']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_LINE_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_BLOCK_COMMENT']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_STRING']"));
			PopulateByXMLNodeTreeType (doc.SelectSingleNode ("/scheme/attributes/option[@name='DEFAULT_COMMA']"));
			Dictionary <string, string> dic = new Dictionary <string, string> ();
			dic.Add ("color", df["color"]);
			dic.Add ("bgcolor", df["bgcolor"]);
			attributes.Add ("FoldMarker", dic);
			attributes.Add ("SelectedFoldLine", dic);
			dic.Remove ("bgcolor");
			attributes.Add ("FoldLine", dic);

			// To Add: _LineNumbers->bg from default->bg, FoldMarker from default,_ SelectedFoldLine from default
		}

		private void PopulateByXMLNodeTreeType (XmlNode node)
		{
			// Console.WriteLine (node.Name);
			/*foreach (XmlAttribute att in node.Attributes)
			{
				Console.WriteLine ($"{node.Name}, {att.Name}: {att.Value}");				
			}*/
			var name = getName (node.Attributes ["name"].Value);

			var attrs = new Dictionary <string, string> ();

			var child = node.SelectSingleNode ("value/option[@name='FOREGROUND']");

			if (child != null) attrs.Add ("color", "#" + GetValue (child));

			child = node.SelectSingleNode ("value/option[@name='BACKGROUND']");

			if (child != null) attrs.Add ("bgcolor", "#" + GetValue (child));

			if (attrs.Count == 0) attrs = populateDefaultAttributes (name [0]);

			foreach (var nm in name)
			{
				attributes.Add (nm, attrs);
			}


			// Console.WriteLine("TEST");
		}

		private void PopulateByXMLNodeSingleType (string                      name, XmlNode node, XmlNode node2,
		                                          Dictionary <string, string> defaultValues = null)
		{
			var attrs = new Dictionary <string, string> ();

			if (node != null)
				attrs.Add ("color", "#" + GetValue (node));

			if (node2 != null)
				attrs.Add ("bgcolor", "#" + GetValue (node2));
			if (defaultValues != null)
			{
				foreach (var value in defaultValues)
				{
					attrs.Add (value.Key, value.Value);
				}
			}

			attributes.Add (name, attrs);
		}

		private string GetValue (XmlNode child)
		{
			var attr_value = child.Attributes ["value"].Value;

			return attr_value;
		}

		private Dictionary <string, string> populateDefaultAttributes (string name)
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

		private bool isNecessaryAttribute (string name)
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

		private string [] getName (string st)
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
	}
}