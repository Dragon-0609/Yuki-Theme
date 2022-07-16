using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Yuki_Theme.Core.Utils;
using Formatting = System.Xml.Formatting;

namespace Yuki_Theme.Core.Themes
{
	internal class OldThemeFormat : ThemeFormatBase
	{
		#region XML

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNodeParent (XmlNode           node, ref Theme theme,
		                                            ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNode (XmlNode           node, ref Theme theme,
		                                      ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNodeSingular (XmlNode           node, ref Theme theme,
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

					if (HighlitherUtil.IsInColors (nm, true))
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
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNodeParentForLight (XmlNode           node, ref Theme theme,
		                                                    ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNodeForLight (xne, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNodeForLight (XmlNode           node, ref Theme theme,
		                                              ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingularForLight (xn, ref theme, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>PopulateList</code>
		/// </summary>
		/// <param name="node"></param>
		public void PopulateByXMLNodeSingularForLight (XmlNode           node, ref Theme theme,
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

					if (HighlitherUtil.IsInColors (nm, true))
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
						theme.Fields.Add (shadowName, attrs);
						if (!Populater.IsInList (shadowName, namesExtra)) namesExtra.Add (shadowName);
					}

					PasteWallpaperAndSticker (ref namesExtra, shadowName);
				}
			}
		}

		private void PasteWallpaperAndSticker (ref List <string> namesExtra, string shadowName)
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

		public override string GetNameOfTheme (string path)
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

		private string GetThemeName (XmlDocument docu)
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
		public override void SaveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
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

		public void SaveXML (Image img2, Image img3, bool wantToKeep, bool iszip, ref XmlDocument doc, string themePath)
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

		public void PopulateDictionaryFromDoc (XmlDocument       doc, ref Theme theme,
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

		public void LoadThemeToPopulate (ref XmlDocument doc, string pathToTheme, bool needToDoActions, bool isDefault,
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
					a = API.GetCore ();
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
						a = API.GetCore ();
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
							if (API_Events.ifHasImage != null)
							{
								API_Events.ifHasImage (iag.Item2);
							}

							if (API_Events.ifHasImage2 != null)
							{
								API_Events.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (API_Events.ifDoesntHave != null)
								API_Events.ifDoesntHave ();
							if (API_Events.ifDoesntHave2 != null)
								API_Events.ifDoesntHave2 ();
						}
					} else
					{
						// Release resource
						if (iag.Item2 != null)
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
							if (API_Events.ifHasSticker != null)
							{
								API_Events.ifHasSticker (iag.Item2);
							}

							if (API_Events.ifHasSticker2 != null)
							{
								API_Events.ifHasSticker2 (iag.Item2);
							}
						} else
						{
							if (API_Events.ifDoesntHaveSticker != null)
								API_Events.ifDoesntHaveSticker ();
							if (API_Events.ifDoesntHaveSticker2 != null)
								API_Events.ifDoesntHaveSticker2 ();
						}
					} else
					{
						// Release resource
						if (iag.Item2 != null)
							iag.Item2.Dispose ();
					}

					themeToSet.HasSticker = iag.Item1;
				} else
				{
					if (needToDoActions)
					{
						if (API_Events.ifDoesntHave != null)
							API_Events.ifDoesntHave ();

						if (API_Events.ifDoesntHaveSticker != null)
							API_Events.ifDoesntHaveSticker ();

						if (API_Events.ifDoesntHave2 != null)
							API_Events.ifDoesntHave2 ();

						if (API_Events.ifDoesntHaveSticker2 != null)
							API_Events.ifDoesntHaveSticker2 ();
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
						if (API_Events.hasProblem != null)
							API_Events.hasProblem (
								API.Translate ("messages.theme.invalid.full"));
						throw;
					}

					Tuple <bool, Image> iag = Helper.GetImage (pathToTheme);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (API_Events.ifHasImage != null)
							{
								API_Events.ifHasImage (iag.Item2);
							}

							if (API_Events.ifHasImage2 != null)
							{
								API_Events.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (API_Events.ifDoesntHave != null)
								API_Events.ifDoesntHave ();

							if (API_Events.ifDoesntHave2 != null)
								API_Events.ifDoesntHave2 ();
						}
					} else
					{
						// Release resource
						if (iag.Item2 != null)
							iag.Item2.Dispose ();
					}

					themeToSet.HasWallpaper = iag.Item1;

					iag = Helper.GetSticker (pathToTheme);
					if (needToDoActions)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (API_Events.ifHasSticker != null)
							{
								API_Events.ifHasSticker (iag.Item2);
							}
						} else
						{
							if (API_Events.ifDoesntHaveSticker != null)
								API_Events.ifDoesntHaveSticker ();

							if (API_Events.ifDoesntHaveSticker2 != null)
								API_Events.ifDoesntHaveSticker2 ();
						}
					} else
					{
						// Release resource
						if (iag.Item2 != null)
							iag.Item2.Dispose ();
					}

					themeToSet.HasSticker = iag.Item1;
				} else
				{
					if (needToDoActions)
					{
						if (API_Events.ifDoesntHave != null)
							API_Events.ifDoesntHave ();
						if (API_Events.ifDoesntHaveSticker != null)
							API_Events.ifDoesntHaveSticker ();

						if (API_Events.ifDoesntHave2 != null)
							API_Events.ifDoesntHave2 ();
						if (API_Events.ifDoesntHaveSticker2 != null)
							API_Events.ifDoesntHaveSticker2 ();
					}

					themeToSet.HasWallpaper = false;
					themeToSet.HasSticker = false;

					try
					{
						doc.Load (pathToTheme);
					} catch (XmlException)
					{
						if (API_Events.hasProblem != null)
							API_Events.hasProblem (
								API.Translate ("messages.theme.invalid.full"));
						throw;
					}
				}
			}
		}

		public Dictionary <string, string> GetAdditionalInfoFromDoc (XmlDocument doc)
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

		private void GetValueIfStarts (XmlComment comm, Dictionary <string, string> dictionary, string key)
		{
			if (comm.Value.StartsWith (key))
			{
				dictionary [key] = comm.Value.Substring (key.Length + 1);
			}
		}

		public override void WriteName (string path, string name)
		{
			XmlDocument doc = LoadTheme (path, out bool iszip);

			WriteName (name, doc);

			SaveModifiedTheme (path, iszip, doc);
		}

		private static void WriteName (string name, XmlDocument doc)
		{
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
				node.AppendChild (doc.CreateComment ("name:" + name));
			}
		}

		private static void ResetToken (XmlDocument doc)
		{
			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNodeList comms = node.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				bool hasToken = false;

				string nl = "token:null";
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("token"))
					{
						comm.Value = nl;
						hasToken = true;
					}
				}

				if (!hasToken)
				{
					node.AppendChild (doc.CreateComment (nl));
				}
			} else
			{
				node.AppendChild (doc.CreateComment ("token:null"));
			}
		}

		private static XmlDocument LoadTheme (string path, out bool iszip)
		{
			XmlDocument doc = new XmlDocument ();
			iszip = false;
			Tuple <bool, string> content = Helper.GetTheme (path);
			if (content.Item1)
			{
				doc.LoadXml (content.Item2);
				iszip = true;
			} else
			{
				doc.Load (path);
			}

			return doc;
		}

		public override void WriteNameAndResetToken (string path, string name)
		{
			XmlDocument doc = LoadTheme (path, out bool iszip);

			WriteName (name, doc);
			ResetToken (doc);
			
			SaveModifiedTheme (path, iszip, doc);
		}

		private static void SaveModifiedTheme (string path, bool iszip, XmlDocument doc)
		{
			if (!iszip)
				doc.Save (path);
			else
			{
				string txml = doc.OuterXml;

				Helper.UpdateZip (path, txml, null, true, null, true);
			}
		}

		public override void ReGenerate (string path, string oldPath, string name, string oldName, API_Actions apiActions)
		{
			Theme theme = new Theme
			{
				Fields = new Dictionary <string, ThemeField> ()
			};
			XmlDocument doc = new XmlDocument ();
			try
			{
				bool isDef = DefaultThemes.isDefault (oldName);
				LoadThemeToPopulate (ref doc, isDef ? oldName : oldPath, false, isDef, ref theme, Helper.FILE_EXTENSTION_OLD,
				                                     false, true);
			} catch
			{
				return;
			}

			List <string> namesList = new List <string> ();

			PopulateDictionaryFromDoc (doc, ref theme, ref namesList);

			string methodName = Settings.settingMode == SettingMode.Light ? "Method" : "MarkPrevious";
			if (!theme.Fields.ContainsKey (methodName))
			{
				string keywordName = Settings.settingMode == SettingMode.Light ? "Keyword" : "Keywords";
				theme.Fields.Add (methodName, new ThemeField () { Foreground = theme.Fields [keywordName].Foreground });
			}

			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			string al = additionalInfo ["align"];
			string op = additionalInfo ["opacity"];
			string sop = additionalInfo ["stickerOpacity"];
			theme.Name = name;
			theme.Group = "";
			theme.Version = Convert.ToInt32 (SettingsConst.CURRENT_VERSION);
			theme.WallpaperOpacity = int.Parse (op);
			theme.StickerOpacity = int.Parse (sop);
			theme.WallpaperAlign = int.Parse (al);
			string json = JsonConvert.SerializeObject (theme, Newtonsoft.Json.Formatting.Indented);
			bool isZip;

			if (DefaultThemes.isDefault (oldName))
			{
				Stream stream = API._themeManager.GetStreamFromMemory (oldName, oldName);
				isZip = Helper.IsZip (stream);
				stream.Dispose ();
			} else
			{
				isZip = Helper.IsZip (oldPath);
			}

			if (!isZip)
				File.WriteAllText (path, json);
			else
			{
				Helper.UpdateZip (path, json, null, true, null, true, "", false);
			}
		}

		/// <summary>
		/// Load Theme by name.
		/// </summary>
		/// <param name="name">Theme's name. It's mandatory for loading theme properly</param>
		/// <param name="ToCLI">Need to load to API? It'll affect "API.names".</param>
		/// <returns>Parsed theme</returns>
		public override Theme PopulateList (string name, bool ToCLI)
		{
			bool isDef = API.ThemeInfos [name].isDefault;
			bool fromAssembly = API.ThemeInfos [name].location == ThemeLocation.Memory && isDef;

			string path = Helper.ConvertNameToPath (name);
			Theme theme = new Theme
			{
				isDefault = isDef,
				Name = name
			};
			var doc = new XmlDocument ();
			try
			{
				LoadThemeToPopulate (ref doc, fromAssembly ? name : PathGenerator.PathToFile (path, true), ToCLI, fromAssembly, ref theme,
				                     Helper.FILE_EXTENSTION_OLD, false, false);
			} catch (Exception e)
			{
				Console.WriteLine ("OldTheme.Loading Theme failed");
				Console.WriteLine ("{0}\n{1}", e.Message, e.StackTrace);
				return null;
			}

			theme.Fields = new Dictionary <string, ThemeField> ();
			if (ToCLI)
				PopulateDictionaryFromDoc (doc, ref theme, ref API.names);
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
					API.names.Add (methdoName);
			}

			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			theme.SetAdditionalInfo (additionalInfo);
			theme.path = path;

			return theme;
		}

		public override void ProcessAfterParsing (Theme theme) {  }


		public void MergeThemeFieldsWithFile (Dictionary <string, ThemeField> local, XmlDocument doc)
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

		public void MergeCommentsWithFile (Theme themeToMerge, XmlDocument doc)
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

		private void AddComment (XmlDocument doc, XmlNode node, string comment)
		{
			node.AppendChild (doc.CreateComment (comment));
		}

		private void PasteIfStarts (XmlComment comm, Dictionary <string, bool> comments, string key, string value)
		{
			if (comm.Value.StartsWith (key))
			{
				comm.Value = value;
				comments [key] = true;
			}
		}

		private string ReadThemeTemplate ()
		{
			string res = "";
			var a = API.GetCore ();
			var stream = a.GetManifestResourceStream (Helper.PASCALTEMPLATE);
			using (StreamReader reader = new StreamReader (stream))
			{
				res = reader.ReadToEnd ();
			}

			return res;
		}
		
		public override Tuple<bool, string> VerifyToken (string path)
		{
			bool valid = false;
			string group = "";
			Theme theme = new Theme ();

			var doc = new XmlDocument ();
			try
			{
				LoadThemeToPopulate (ref doc, path, false, false, ref theme, Helper.FILE_EXTENSTION_OLD, false, false);
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