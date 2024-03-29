﻿using System;
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

		public bool   needToWrite = false;
		public string groupName   = "";

		public Action <string, string> defaultTheme;

		public void Parse (string path,          string fileName, string pathToSave, bool askToOverwrite = false, bool rewrite = false,
		                   bool   select = true, Action <string> addToUIList = null, Action <string> selectAfterParse = null)
		{
			theme = ThemeFunctions.LoadDefault ();
			theme.Fields = new Dictionary <string, ThemeField> ();
			PathToSave = pathToSave;
			flname = fileName;
			ask = askToOverwrite;
			overwrite = rewrite;
			try
			{
				populateList (path);
			} catch (InvalidDataException e)
			{
				defaultTheme (e.Message, "Error");
				return;
			}

			if (needToWrite)
			{
				if (!Directory.Exists (Path.Combine (CLI.currentPath, "Themes")))
					Directory.CreateDirectory (Path.Combine (CLI.currentPath, "Themes"));
				Console.WriteLine (PathToSave);

				if (!overwrite)
				{
					string syt = CLI.schemes [1];
					if (CLI.ThemeInfos[syt].location == ThemeLocation.Memory && DefaultThemes.headers.ContainsKey (syt))
						CLI.CopyFromMemory (syt, syt, PathToSave);
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
						OldThemeFormat.loadThemeToPopulate (ref document, PathToSave, false, false, ref theme, Helper.FILE_EXTENSTION_OLD,
						                                    false, true);
						Dictionary <string, string> additionalInfo = OldThemeFormat.GetAdditionalInfoFromDoc (document);
						theme.Name = OldThemeFormat.GetNameOfTheme (PathToSave);
						theme.SetAdditionalInfo (additionalInfo);
					}
				}

				MergeFiles (PathToSave);
				finishParsing (path);
				if (!overwrite)
				{
					CLI.AddThemeInfo (
						flname, new ThemeInfo (false, true, ThemeLocation.File, CLI.Translate ("messages.theme.group.custom")));
					CLI.names.Add (flname);
				if (addToUIList != null)
					addToUIList (flname);
				/*if (form != null)
					form.schemes.Items.Add (flname);*/
				}

				if (select && selectAfterParse != null)
				{
					selectAfterParse (flname);
				}
			}
		}

		public void MergeFiles (string path)
		{
			XmlDocument doc = new XmlDocument ();

			OldThemeFormat.loadThemeToPopulate (ref doc, Helper.PASCALTEMPLATE, false, true,
			                                    ref theme,Helper.FILE_EXTENSTION_OLD, true, true);
			
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