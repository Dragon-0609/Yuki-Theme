using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Formats
{
	public static class OldThemeFormat
	{
		#region XML

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeParent (XmlNode           node, ref Theme theme,
		                                            ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNode (XmlNode           node, ref Theme theme,
		                                      ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeSingular (XmlNode           node, ref Theme theme,
		                                              ref List <string> namesExtra)
		{
			var attrs = new ThemeField ();
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nm = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nm = node.Attributes ["name"].Value;

				foreach (XmlAttribute att in node.Attributes)
				{
					if (att.Name == "color" || att.Name == "bgcolor")
					{
						if (node.Name == "SelectedFoldLine")
						{
							if (att.Name == "color")
								attrs.Foreground = att.Value;
						} else
						{
							attrs.SetAttributeByName (att.Name, att.Value);
						}
					}

					if (Highlighter.isInNames (nm, true))
					{
						if (!attrs.Bold == null) attrs.Bold = false;

						if (att.Name == "bold")
						{
							attrs.Bold = bool.Parse (att.Value);
						}

						if (!attrs.Italic == null) attrs.Italic = false;

						if (att.Name == "italic")
						{
							attrs.Italic = bool.Parse (att.Value);
						}
					}
				}

				if (!namesExtra.Contains (nm))
				{
					namesExtra.Add (nm);
					theme.Fields.Add (nm, attrs);

					PasteWallpaperAndSticker (ref namesExtra, nm);
				}
			}
		}


		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeParentForLight (XmlNode           node, ref Theme theme,
		                                                    ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNodeForLight (xne, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeForLight (XmlNode           node, ref Theme theme,
		                                              ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingularForLight (xn, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeSingularForLight (XmlNode           node, ref Theme theme,
		                                                      ref List <string> namesExtra)
		{
			// Console.WriteLine("TEST");
			var attrs = new ThemeField ();
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nm = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nm = node.Attributes ["name"].Value;

				foreach (XmlAttribute att in node.Attributes)
				{
					if (att.Name == "color" || att.Name == "bgcolor")
					{
						if (node.Name == "SelectedFoldLine")
						{
							if (att.Name == "color")
								attrs.Foreground = att.Value;
						} else
						{
							attrs.SetAttributeByName (att.Name, att.Value);
						}
					}

					if (Highlighter.isInNames (nm, true))
					{
						if (!attrs.Bold == null) attrs.Bold = false;

						if (att.Name == "bold")
						{
							attrs.Bold = bool.Parse (att.Value);
						}

						if (!attrs.Italic == null) attrs.Italic = false;

						if (att.Name == "italic")
						{
							attrs.Italic = bool.Parse (att.Value);
						}
					}
				}

				string shadowName = ShadowNames.GetShadowName (nm, SyntaxType.Pascal, true);
				if (!namesExtra.Contains (shadowName))
				{
					if (!theme.Fields.ContainsKey (shadowName))
					{
						// Console.WriteLine ( $"InList: {nm}|{attributes.ContainsKey (nm)}");		
						theme.Fields.Add (shadowName, attrs);
						if (!Populater.isInList (shadowName, namesExtra)) namesExtra.Add (shadowName);
					}

					PasteWallpaperAndSticker (ref namesExtra, shadowName);
				}
			}
		}

		private static void PasteWallpaperAndSticker (ref List <string> namesExtra, string shadowName)
		{
			if (shadowName.Equals ("selection", StringComparison.OrdinalIgnoreCase))
			{
				if (!namesExtra.Contains ("Wallpaper"))
				{
					namesExtra.Remove ("Selection");
					namesExtra.Add ("Wallpaper");
					namesExtra.Add ("Selection");
				}

				if (!namesExtra.Contains ("Sticker"))
				{
					namesExtra.Remove ("Selection");
					namesExtra.Add ("Sticker");
					namesExtra.Add ("Selection");
				}
			}
		}

		#endregion

		public static string GetNameOfTheme (string path)
		{
			XmlDocument docu = new XmlDocument ();

			Tuple <bool, string> content = Helper.GetTheme (path);
			if (content.Item1)
			{
				docu.LoadXml (content.Item2);
			} else
			{
				docu.Load (path);
			}

			string nm = GetThemeName (docu);

			return nm;
		}

		private static string GetThemeName (XmlDocument docu)
		{
			XmlNode nod = docu.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			string nm = "";

			if (comms != null)
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("name"))
					{
						nm = comm.Value.Substring (5);
						break;
					}
				}

			if (nm == "")
			{
				if (nod.Attributes != null) nm = nod.Attributes ["name"].Value;
			}

			return nm;
		}

		/// <summary>
		/// Save current theme in old format. It can be used to export to old version of Yuki Theme.
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		public static void saveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			var doc = new XmlDocument ();
			string themePath = themeToSave.fullPath;
			Tuple <bool, string> content = Helper.GetTheme (themePath);
			bool iszip = content.Item1;

			doc.LoadXml (ReadThemeTemplate ());

			Dictionary <string, ThemeField> localDic;

			if (Settings.settingMode == SettingMode.Light)
				localDic = ThemeField.GetThemeFieldsWithRealNames (SyntaxType.Pascal, themeToSave);
			else
				localDic = themeToSave.Fields;
			MergeThemeFieldsWithFile (localDic, doc);

			MergeCommentsWithFile (themeToSave, doc);

			SaveXML (img2, img3, wantToKeep, iszip, ref doc, themePath);
		}

		public static void SaveXML (Image img2, Image img3, bool wantToKeep, bool iszip, ref XmlDocument doc, string themePath)
		{
			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				doc.Save (themePath);
			else
			{
				string txml = doc.OuterXml;
				if (iszip)
				{
					Helper.UpdateZip (themePath, txml, img2, wantToKeep, img3, wantToKeep, "theme.xshd", true);
				} else
				{
					Helper.Zip (themePath, txml, img2, img3, "theme.xshd", true);
				}
			}
		}

		public static void PopulateDictionaryFromDoc (XmlDocument       doc, ref Theme theme,
		                                              ref List <string> namesExtra)
		{
			if (Settings.settingMode == SettingMode.Light) // It's for better performance
			{
				if (doc.SelectNodes ("/SyntaxDefinition/Environment").Count == 1)
					PopulateByXMLNodeForLight (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref theme, ref namesExtra);
				PopulateByXMLNodeSingularForLight (doc.SelectNodes ("/SyntaxDefinition/Digits") [0], ref theme, ref namesExtra);
				PopulateByXMLNodeParentForLight (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0], ref theme, ref namesExtra);
			} else
			{
				if (doc.SelectNodes ("/SyntaxDefinition/Environment").Count == 1)
					PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref theme, ref namesExtra);
				PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref theme, ref namesExtra);
				PopulateByXMLNodeSingular (doc.SelectNodes ("/SyntaxDefinition/Digits") [0], ref theme, ref namesExtra);
				PopulateByXMLNodeParent (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0], ref theme, ref namesExtra);
			}
		}

		public static void loadThemeToPopulate (ref XmlDocument doc, string pathToTheme, bool needToDoActions, bool isDefault,
		                                        ref Theme       themeToSet, string extension,
		                                        bool            customNameForMemory, bool needToSetDefaultField)
		{
			if (isDefault)
			{
				Assembly a;
				string pathForMemory = "";
				string pathToLoad = Helper.ConvertNameToPath (pathToTheme);
				if (customNameForMemory)
				{
					a = CLI.GetCore ();
					pathForMemory = pathToTheme;
				} else
				{
					if (DefaultThemes.names.Contains (pathToTheme))
					{
						IThemeHeader header = DefaultThemes.headers [pathToTheme];
						a = header.Location;
						pathForMemory = $"{header.ResourceHeader}.{pathToLoad}{extension}";
					} else
					{
						a = CLI.GetCore ();
						pathForMemory = $"{DefaultThemesHeader.CoreThemeHeader}.{pathToLoad}{extension}";
					}
				}

				Tuple <bool, string> content = Helper.GetThemeFromMemory (pathForMemory, a);
				themeToSet.fullPath = pathForMemory;
				if (needToSetDefaultField)
					themeToSet.isDefault = true;
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathForMemory, a);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasImage != null)
							{
								CLI_Actions.ifHasImage (iag.Item2);
							}

							if (CLI_Actions.ifHasImage2 != null)
							{
								CLI_Actions.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHave != null)
								CLI_Actions.ifDoesntHave ();
							if (CLI_Actions.ifDoesntHave2 != null)
								CLI_Actions.ifDoesntHave2 ();
						}
					} else
					{
						// Release resource
						iag.Item2.Dispose ();
					}

					themeToSet.HasWallpaper = iag.Item1;

					iag = null;
					iag = Helper.GetStickerFromMemory (pathForMemory, a);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasSticker != null)
							{
								CLI_Actions.ifHasSticker (iag.Item2);
							}

							if (CLI_Actions.ifHasSticker2 != null)
							{
								CLI_Actions.ifHasSticker2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHaveSticker != null)
								CLI_Actions.ifDoesntHaveSticker ();
							if (CLI_Actions.ifDoesntHaveSticker2 != null)
								CLI_Actions.ifDoesntHaveSticker2 ();
						}
					} else
					{
						// Release resource
						iag.Item2.Dispose ();
					}

					themeToSet.HasSticker = iag.Item1;
				} else
				{
					if (needToDoActions)
					{
						if (CLI_Actions.ifDoesntHave != null)
							CLI_Actions.ifDoesntHave ();

						if (CLI_Actions.ifDoesntHaveSticker != null)
							CLI_Actions.ifDoesntHaveSticker ();

						if (CLI_Actions.ifDoesntHave2 != null)
							CLI_Actions.ifDoesntHave2 ();

						if (CLI_Actions.ifDoesntHaveSticker2 != null)
							CLI_Actions.ifDoesntHaveSticker2 ();
					}

					doc.Load (a.GetManifestResourceStream (pathForMemory));
				}
			} else
			{
				Tuple <bool, string> content = Helper.GetTheme (pathToTheme);
				if (needToSetDefaultField)
					themeToSet.isDefault = false;
				themeToSet.fullPath = pathToTheme;
				if (content.Item1)
				{
					try
					{
						doc.LoadXml (content.Item2);
					} catch (XmlException)
					{
						if (CLI_Actions.hasProblem != null)
							CLI_Actions.hasProblem (
								CLI.Translate ("messages.theme.invalid.full"));
						throw;
					}

					Tuple <bool, Image> iag = Helper.GetImage (pathToTheme);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasImage != null)
							{
								CLI_Actions.ifHasImage (iag.Item2);
							}

							if (CLI_Actions.ifHasImage2 != null)
							{
								CLI_Actions.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHave != null)
								CLI_Actions.ifDoesntHave ();

							if (CLI_Actions.ifDoesntHave2 != null)
								CLI_Actions.ifDoesntHave2 ();
						}
					} else
					{
						// Release resource
						iag.Item2.Dispose ();
					}

					themeToSet.HasWallpaper = iag.Item1;

					iag = Helper.GetSticker (pathToTheme);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI_Actions.ifHasSticker != null)
							{
								CLI_Actions.ifHasSticker (iag.Item2);
							}
						} else
						{
							if (CLI_Actions.ifDoesntHaveSticker != null)
								CLI_Actions.ifDoesntHaveSticker ();

							if (CLI_Actions.ifDoesntHaveSticker2 != null)
								CLI_Actions.ifDoesntHaveSticker2 ();
						}
					} else
					{
						// Release resource
						iag.Item2.Dispose ();
					}

					themeToSet.HasSticker = iag.Item1;
				} else
				{
					if (needToDoActions)
					{
						if (CLI_Actions.ifDoesntHave != null)
							CLI_Actions.ifDoesntHave ();
						if (CLI_Actions.ifDoesntHaveSticker != null)
							CLI_Actions.ifDoesntHaveSticker ();

						if (CLI_Actions.ifDoesntHave2 != null)
							CLI_Actions.ifDoesntHave2 ();
						if (CLI_Actions.ifDoesntHaveSticker2 != null)
							CLI_Actions.ifDoesntHaveSticker2 ();
					}

					themeToSet.HasWallpaper = false;
					themeToSet.HasSticker = false;

					try
					{
						doc.Load (pathToTheme);
					} catch (XmlException)
					{
						if (CLI_Actions.hasProblem != null)
							CLI_Actions.hasProblem (
								CLI.Translate ("messages.theme.invalid.full"));
						throw;
					}
				}
			}
		}

		public static Dictionary <string, string> GetAdditionalInfoFromDoc (XmlDocument doc)
		{
			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			Dictionary <string, string> dictionary = new Dictionary <string, string> ();
			dictionary.Add ("align", ((int)Alignment.Center).ToString ());
			dictionary.Add ("opacity", "15");
			dictionary.Add ("stickerOpacity", "100");
			dictionary.Add ("token", "null");
			dictionary.Add ("group", "");
			foreach (XmlComment comm in comms)
			{
				GetValueIfStarts (comm, dictionary, "align");
				GetValueIfStarts (comm, dictionary, "opacity");
				GetValueIfStarts (comm, dictionary, "stickerOpacity");
				GetValueIfStarts (comm, dictionary, "token");
				GetValueIfStarts (comm, dictionary, "group");
			}

			return dictionary;
		}

		private static void GetValueIfStarts (XmlComment comm, Dictionary <string, string> dictionary, string key)
		{
			if (comm.Value.StartsWith (key))
			{
				dictionary [key] = comm.Value.Substring (key.Length + 1);
			}
		}

		public static void WriteName (string path, string name)
		{
			var doc = new XmlDocument ();
			bool iszip = false;
			Tuple <bool, string> content = Helper.GetTheme (path);
			if (content.Item1)
			{
				doc.LoadXml (content.Item2);
				iszip = true;
			} else
			{
				doc.Load (path);
			}

			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNodeList comms = node.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				bool hasName = false;

				string nl = "name:" + name;
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("name"))
					{
						comm.Value = nl;
						hasName = true;
					}
				}

				if (!hasName)
				{
					node.AppendChild (doc.CreateComment (nl));
				}
			} else
			{
				node.AppendChild (doc.CreateComment ("name:" + CLI.nameToLoad));
			}

			if (!iszip)
				doc.Save (path);
			else
			{
				string txml = doc.OuterXml;

				Helper.UpdateZip (path, txml, null, true, null, true);
			}
		}

		/// <summary>
		/// Load Theme by name.
		/// </summary>
		/// <param name="name">Theme's name. It's mandatory for loading theme properly</param>
		/// <param name="ToCLI">Need to load to CLI? It'll affect "CLI.names".</param>
		/// <returns>Parsed theme</returns>
		public static Theme populateList (string name, bool ToCLI)
		{
			bool isDef = CLI.ThemeInfos [name].isDefault;
			bool fromAssembly = CLI.ThemeInfos [name].location == ThemeLocation.Memory && isDef;

			string path = Helper.ConvertNameToPath (name);
			Theme theme = new Theme
			{
				isDefault = isDef,
				Name = name
			};
			var doc = new XmlDocument ();
			try
			{
				loadThemeToPopulate (ref doc, fromAssembly ? name : CLI.pathToFile (path, true), ToCLI, fromAssembly, ref theme,
				                     Helper.FILE_EXTENSTION_OLD, false, false);
			} catch (Exception e)
			{
				Console.WriteLine ("OldTheme.Loading Theme failed");
				Console.WriteLine ("{0}\n{1}", e.Message, e.StackTrace);
				return null;
			}

			theme.Fields = new Dictionary <string, ThemeField> ();
			if (ToCLI)
				PopulateDictionaryFromDoc (doc, ref theme, ref CLI.names);
			else
			{
				List <string> localNames = new List <string> ();
				PopulateDictionaryFromDoc (doc, ref theme, ref localNames);
			}

			string methdoName = Settings.settingMode == SettingMode.Light ? "Method" : "MarkPrevious";
			if (!theme.Fields.ContainsKey (methdoName))
			{
				string keywordName = Settings.settingMode == SettingMode.Light ? "Keyword" : "KeyWords";
				theme.Fields.Add (methdoName, new ThemeField () { Foreground = theme.Fields [keywordName].Foreground });
				if (ToCLI)
					CLI.names.Add (methdoName);
			}

			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			theme.SetAdditionalInfo (additionalInfo);
			theme.path = path;

			return theme;
		}

		/// <summary>
		/// Load Theme directly to the CLI
		/// </summary>
		public static void LoadThemeToCLI ()
		{
			Theme theme = populateList (CLI.nameToLoad, true);
			CLI.currentTheme = theme;
			if (theme == null)
			{
				Console.WriteLine ("Theme is null");
				/*MessageBox.Show (CLI.Translate ("messages.theme.invalid.full"), CLI.Translate ("messages.theme.invalid.short"),
				                 MessageBoxButtons.OK, MessageBoxIcon.Error);*/
			}
		}

		public static void MergeThemeFieldsWithFile (Dictionary <string, ThemeField> local, XmlDocument doc)
		{
			#region Environment

			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
			bool hadSavedImage = false; // This is check for alpha version of v2.0
			foreach (XmlNode childNode in node.ChildNodes)
				if (childNode.Attributes != null &&
				    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = childNode.Name;
					if (childNode.Name == "Span" || childNode.Name == "KeyWords")
						nms = childNode.Attributes ["name"].Value;
					if (!local.ContainsKey (nms)) continue;
					if (nms == "Wallpaper")
						hadSavedImage = true;
					var attrs = local [nms].GetAttributes ();

					foreach (var att in attrs)
					{
						childNode.Attributes [att.Key].Value = att.Value;
					}
				}

			if (hadSavedImage)
			{
				node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
				node.RemoveChild (node.SelectSingleNode ("Wallpaper"));
			}

			#endregion

			#region Digits

			node = doc.SelectSingleNode ("/SyntaxDefinition/Digits");
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nms = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nms = node.Attributes ["name"].Value;
				if (local.ContainsKey (nms))
				{
					var attrs = local [nms].GetAttributes ();

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
						if (!local.ContainsKey (nms)) continue;

						var attrs = local [nms].GetAttributes ();

						foreach (var att in attrs)
							xn.Attributes [att.Key].Value = att.Value;
					}
			}

			#endregion
		}

		public static void MergeCommentsWithFile (Theme themeToMerge, XmlDocument doc)
		{
			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				Dictionary <string, bool> comments = new Dictionary <string, bool>
				{
					{ "name", false }, { "align", false }, { "opacity", false }, { "sopacity", false },
					{ "hasImage", false }, { "hasSticker", false }, { "token", false }, { "group", false }
				};

				Dictionary <string, string> commentValues = new Dictionary <string, string>
				{
					{ "name", "name:" + themeToMerge.Name }, { "align", "align:" + ((int)themeToMerge.WallpaperAlign) },
					{ "opacity", "opacity:" + (themeToMerge.WallpaperOpacity) },
					{ "sopacity", "sopacity:" + (themeToMerge.StickerOpacity) },
					{ "hasImage", "hasImage:" + themeToMerge.HasWallpaper }, { "hasSticker", "hasSticker:" + themeToMerge.HasSticker },
					{ "token", "token:" + themeToMerge.Token },
					{ "group", "group:" + themeToMerge.Group }
				};
				foreach (XmlComment comm in comms)
				{
					PasteIfStarts (comm, comments, "align", commentValues ["align"]);
					PasteIfStarts (comm, comments, "opacity", commentValues ["opacity"]);
					PasteIfStarts (comm, comments, "sopacity", commentValues ["sopacity"]);
					PasteIfStarts (comm, comments, "name", commentValues ["name"]);
					PasteIfStarts (comm, comments, "hasImage", commentValues ["hasImage"]);
					PasteIfStarts (comm, comments, "hasSticker", commentValues ["hasSticker"]);
					PasteIfStarts (comm, comments, "token", commentValues ["token"]);
					PasteIfStarts (comm, comments, "group", commentValues ["group"]);
				}

				foreach (KeyValuePair <string, bool> comment in comments)
				{
					if (!comment.Value)
					{
						node.AppendChild (doc.CreateComment (commentValues [comment.Key]));
					}
				}
			} else
			{
				AddComment (doc, node, "name:" + themeToMerge.Name);
				AddComment (doc, node, "align:" + ((int)themeToMerge.WallpaperAlign));
				AddComment (doc, node, "opacity:" + (themeToMerge.WallpaperOpacity));
				AddComment (doc, node, "sopacity:" + (themeToMerge.StickerOpacity));
				AddComment (doc, node, "hasImage:" + themeToMerge.HasWallpaper);
				AddComment (doc, node, "hasSticker:" + themeToMerge.HasSticker);
				AddComment (doc, node, "token:" + themeToMerge.Token);
				AddComment (doc, node, "group:" + themeToMerge.Group);
			}
		}

		private static void AddComment (XmlDocument doc, XmlNode node, string comment)
		{
			node.AppendChild (doc.CreateComment (comment));
		}

		private static void PasteIfStarts (XmlComment comm, Dictionary <string, bool> comments, string key, string value)
		{
			if (comm.Value.StartsWith (key))
			{
				comm.Value = value;
				comments [key] = true;
			}
		}

		private static string ReadThemeTemplate ()
		{
			string res = "";
			var a = CLI.GetCore ();
			var stream = a.GetManifestResourceStream (Helper.PASCALTEMPLATE);
			using (StreamReader reader = new StreamReader (stream))
			{
				res = reader.ReadToEnd ();
			}

			return res;
		}
		
		public static Tuple<bool, string> VerifyToken (string path)
		{
			bool valid = false;
			string group = "";
			Theme theme = new Theme ();

			var doc = new XmlDocument ();
			try
			{
				loadThemeToPopulate (ref doc, path, false, false, ref theme, Helper.FILE_EXTENSTION_OLD, false, false);
				Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
				theme.SetAdditionalInfo (additionalInfo);
				theme.Name = GetThemeName (doc);
				valid = Helper.VerifyToken (theme);
				group = theme.Group;
			} catch
			{
				// ignored
			}

			return new Tuple <bool, string> (valid, group);
		}
	}
}