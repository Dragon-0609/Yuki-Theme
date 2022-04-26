using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Yuki_Theme.Core.Formats;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public abstract class AbstractParser
	{
		// public Dictionary <string, Dictionary <string, string>> attributes;

		public Theme theme;

		public string PathToSave   = "";
		public string flname    = "";
		public bool   ask       = false;
		public bool   overwrite = false;

		public Action <string, string> defaultTheme;

		public void Parse (string path, string st, string pathToSave, /*MForm form, */bool ak = false, bool rewrite =false, bool select = true)
		{
			theme = ThemeFunctions.LoadDefault ();
			theme.Fields = new Dictionary <string, ThemeField> ();
			PathToSave = pathToSave;
			flname = st;
			ask = ak;
			overwrite = rewrite;
			try
			{
				populateList (path);
			} catch (InvalidDataException e)
			{
				defaultTheme (e.Message, "Error");
				return;
			}
			
			if (!Directory.Exists (Path.Combine (CLI.currentPath, "Themes")))
				Directory.CreateDirectory (Path.Combine (CLI.currentPath, "Themes"));
			// Console.WriteLine (PathToSave);

			if (!overwrite)
			{
				string syt = CLI.schemes [1];
				if (DefaultThemes.isDefault (syt))
					CLI.CopyFromMemory (syt, PathToSave, PathToSave);
				else
				{
					// Here I check if the theme isn't exist. Else, just its colors will be replaced, not wallpaper or sticker. 
					if (!CLI.schemes.Contains (flname))
						File.Copy (Path.Combine (CLI.currentPath, "Themes", $"{syt}.yukitheme"), PathToSave, true);
				}
			} else
			{
				if (PathToSave.EndsWith (Helper.FILE_EXTENSTION_OLD)) // Get old opacity from theme file
				{
					XmlDocument document = new XmlDocument ();
					OldThemeFormat.loadThemeToPopulate (ref document, PathToSave, false, false, ref theme, flname, Helper.FILE_EXTENSTION_OLD, false);
					Dictionary <string, string> additionalInfo = OldThemeFormat.GetAdditionalInfoFromDoc (document);
					theme.Name = OldThemeFormat.GetNameOfTheme (PathToSave);
					theme.SetAdditionalInfo (additionalInfo);
				}
			}

			MergeFiles (PathToSave);
			finishParsing (path);
			if (!overwrite)
			{
				CLI.AddThemeToLists (flname, false, true);
				CLI.names.Add (flname);
				/*if (form != null)
					form.schemes.Items.Add (flname);*/
			}
			/*if(select && form != null)
			{
				// If selected theme is not theme that we just parsed
				if ((string)form.schemes.SelectedItem != flname)
					form.schemes.SelectedItem = flname;
				else
					form.restore_Click (form, EventArgs.Empty); // Reset
			}*/
		}

		public void MergeFiles (string path)
		{
			XmlDocument doc = new XmlDocument ();

			OldThemeFormat.loadThemeToPopulate (ref doc, Helper.PASCALTEMPLATE, false, true,
			                                    ref theme, PathToSave, Helper.FILE_EXTENSTION_OLD, true);
			
			OldThemeFormat.MergeThemeFieldsWithFile (theme.Fields, doc);
			OldThemeFormat.MergeCommentsWithFile (theme, doc);

			OldThemeFormat.SaveXML (null, null, true, Helper.IsZip (PathToSave), ref doc, PathToSave);
		}

		public abstract void populateList (string path);

		public abstract void PopulateByXMLNodeTreeType (XmlNode node);

		public void PopulateByXMLNodeSingleType (string                      name, XmlNode node, XmlNode node2,
		                                         Dictionary <string, string> defaultValues = null)
		{
			var attrs = new ThemeField ();

			if (node != null)
				attrs.Foreground  = "#" + GetValue (node);

			if (node2 != null)
				attrs.Background = "#" + GetValue (node2);
			if (defaultValues != null)
			{
				foreach (var value in defaultValues)
				{
					attrs.SetAttributeByName (value.Key, value.Value);
				}
			}

			theme.Fields.Add (name, attrs);
		}

		public abstract string GetValue (XmlNode child);

		public abstract ThemeField populateDefaultAttributes (string name);

		public abstract bool isNecessaryAttribute (string name);

		public abstract string [] getName (string st);

		public abstract void finishParsing (string path);

	}
}