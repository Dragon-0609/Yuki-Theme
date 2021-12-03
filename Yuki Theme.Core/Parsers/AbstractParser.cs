using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public abstract class AbstractParser
	{
		public Dictionary <string, Dictionary <string, string>> attributes;

		public string outname = "";
		public string flname = "";

		public void Parse (string path, string st, string patsh, MForm form, bool overwrite =false, bool select = true)
		{
			attributes = new Dictionary <string, Dictionary <string, string>> ();
			outname = patsh;
			flname = st;
			populateList (path);
			if (!Directory.Exists ("Themes"))
				Directory.CreateDirectory ("Themes");
			Console.WriteLine (outname);

			string syt = CLI.cli.schemes [1];
			if (DefaultThemes.isDefault (syt))
				CLI.cli.CopyFromMemory (syt, outname);
			else
				File.Copy ($"Themes/{syt}.yukitheme", outname, true);

			MergeFiles (outname);
			finishParsing (path);
			if (!overwrite)
			{
				CLI.cli.names.Add (flname);
				if (form != null)
					form.schemes.Items.Add (flname);
			}
			if(select && form != null)
				form.schemes.SelectedItem = flname;
		}

		public void MergeFiles (string path)
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
						{
							// Console.WriteLine ($"N: {childNode.Name}, ATT: {att.Key},");
							childNode.Attributes [att.Key].Value = att.Value;
						}
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

		public abstract void populateList (string path);

		public abstract void PopulateByXMLNodeTreeType (XmlNode node);

		public void PopulateByXMLNodeSingleType (string                      name, XmlNode node, XmlNode node2,
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

		public abstract string GetValue (XmlNode child);

		public abstract Dictionary <string, string> populateDefaultAttributes (string name);

		public abstract bool isNecessaryAttribute (string name);

		public abstract string [] getName (string st);

		public abstract void finishParsing (string path);

	}
}