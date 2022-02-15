using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core
{
	public static class OldThemeFormat
	{
		
		#region XML

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeParent (XmlNode           node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                            ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNode (XmlNode           node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                      ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeSingular (XmlNode           node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                              ref List <string> namesExtra)
		{
			var attrs = new Dictionary <string, string> ();
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
								attrs.Add (att.Name, att.Value);
						} else
						{
							attrs.Add (att.Name, att.Value);
						}
					}

					if (Highlighter.isInNames (nm, true))
					{
						var dsfbold = "null";
						var dsfitalic = "null";
						if (!attrs.ContainsKey ("bold")) attrs.Add ("bold", "false");

						if (att.Name == "bold")
						{
							attrs ["bold"] = att.Value;
							dsfbold = att.Value;
						}

						if (!attrs.ContainsKey ("italic")) attrs.Add ("italic", "false");

						if (att.Name == "italic")
						{
							attrs ["italic"] = att.Value;
							dsfitalic = att.Value;
						}

					}
				}
				if (!namesExtra.Contains (nm))
				{
					namesExtra.Add (nm);
					attributes.Add (nm, attrs);

					PasteWallpaperAndSticker (ref namesExtra, nm);
				}
			}
		}


		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeParentForLight (XmlNode   node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                            ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNodeForLight (xne, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeForLight (XmlNode   node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                      ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingularForLight (xn, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		public static void PopulateByXMLNodeSingularForLight (XmlNode   node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                              ref List <string> namesExtra)
		{
			// Console.WriteLine("TEST");
			var attrs = new Dictionary <string, string> ();
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nm = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nm = node.Attributes ["name"].Value;

				foreach (XmlAttribute att in node.Attributes)
				{
					if (att.Name == "color" || att.Name == "bgcolor")
					{
						// Console.WriteLine($"{nm}: {att.Name}");
						if (node.Name == "SelectedFoldLine")
						{
							if (att.Name == "color")
								attrs.Add (att.Name, att.Value);
						} else
						{
							attrs.Add (att.Name, att.Value);
						}
					}

					if (Highlighter.isInNames (nm, true))
					{
						if (!attrs.ContainsKey ("bold")) attrs.Add ("bold", "false");

						if (att.Name == "bold")
						{
							attrs ["bold"] = att.Value;
						}

						if (!attrs.ContainsKey ("italic")) attrs.Add ("italic", "false");

						if (att.Name == "italic")
						{
							attrs ["italic"] = att.Value;
						}
					}
				}
				string shadowName = ShadowNames.GetShadowName (nm, SyntaxType.Pascal);
				if (!namesExtra.Contains (shadowName))
				{
					if (!attributes.ContainsKey (shadowName))
					{
						// Console.WriteLine ( $"InList: {nm}|{attributes.ContainsKey (nm)}");		
						attributes.Add (shadowName, attrs);
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

		public static string GetNameOfThemeOld (string path)
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

			XmlNode nod = docu.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			string nm = "";

			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("name"))
				{
					nm = comm.Value.Substring (5);
					break;
				}
			}

			return nm;
		}

		/// <summary>
		/// Save current theme in old format. It can be used to export to old version of Yuki Theme.
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		public static void saveList (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			var doc = new XmlDocument ();
			bool iszip = false;
			string themePath = File.Exists (CLI.getPath) ? CLI.getPath : File.Exists (CLI.getPathNew) ? CLI.getPathNew : null;
			Tuple <bool, string> content = Helper.GetTheme (themePath);
			if (content.Item1)
			{
				doc.LoadXml (content.Item2);
				iszip = true;
			} else
			{
				doc.Load (themePath);
			}

			#region Environment

			var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
			bool hadSavedImage = false; // This is check for alpha version of v2.0
			foreach (XmlNode childNode in node.ChildNodes)
				if (childNode.Attributes != null &&
				    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = childNode.Name;
					if (childNode.Name == "Span" || childNode.Name == "KeyWords")
						nms = childNode.Attributes ["name"].Value;
					if (!CLI.localAttributes.ContainsKey (nms)) continue;
					if (nms == "Wallpaper")
						hadSavedImage = true;
					var attrs = CLI.localAttributes [nms];

					foreach (var att in attrs)
						childNode.Attributes [att.Key].Value = att.Value;
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
				if (CLI.localAttributes.ContainsKey (nms))
				{
					var attrs = CLI.localAttributes [nms];

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
						if (!CLI.localAttributes.ContainsKey (nms)) continue;

						var attrs = CLI.localAttributes [nms];

						foreach (var att in attrs)
							// Console.WriteLine($"2N: {xn.Attributes["name"].Value}, ATT: {att.Key},");
							xn.Attributes [att.Key].Value = att.Value;
					}
			}

			#endregion

			node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				Dictionary <string, bool> comments = new Dictionary <string, bool>
				{
					{"name", false}, {"align", false}, {"opacity", false}, {"sopacity", false},
					{"hasImage", false}, {"hasSticker", false}
				};

				Dictionary <string, string> commentValues = new Dictionary <string, string>
				{
					{"name", "name:" + CLI.currentoFile}, {"align", "align:" + ((int) CLI.align)},
					{"opacity", "opacity:" + (CLI.opacity)},
					{"sopacity", "sopacity:" + (CLI.sopacity)},
					{"hasImage", "hasImage:" + (img2 != null)}, {"hasSticker", "hasSticker:" + (img3 != null)}
				};
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("align"))
					{
						comm.Value = commentValues ["align"];
						comments ["align"] = true;
					} else if (comm.Value.StartsWith ("opacity"))
					{
						comm.Value = commentValues ["opacity"];
						comments ["opacity"] = true;
					} else if (comm.Value.StartsWith ("sopacity"))
					{
						comm.Value = commentValues ["sopacity"];
						comments ["sopacity"] = true;
					} else if (comm.Value.StartsWith ("name"))
					{
						comm.Value = commentValues ["name"];
						comments ["name"] = true;
					} else if (comm.Value.StartsWith ("hasImage"))
					{
						comm.Value = commentValues ["hasImage"];
						comments ["hasImage"] = true;
					} else if (comm.Value.StartsWith ("hasSticker"))
					{
						comm.Value = commentValues ["hasSticker"];
						comments ["hasSticker"] = true;
					}
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
				node.AppendChild (doc.CreateComment ("name:" + CLI.currentoFile));
				node.AppendChild (doc.CreateComment ("align:" + ((int) CLI.align)));
				node.AppendChild (doc.CreateComment ("opacity:" + (CLI.opacity)));
				node.AppendChild (doc.CreateComment ("sopacity:" + (CLI.sopacity)));
				node.AppendChild (doc.CreateComment ("hasImage:" + (img2 != null)));
				node.AppendChild (doc.CreateComment ("hasSticker:" + (img3 != null)));
			}

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
					Helper.UpdateZip (themePath, txml, img2, wantToKeep, img3, wantToKeep);
				} else
				{
					Helper.Zip (themePath, txml, img2, img3);
				}
			}
		}

		public static void PopulateDictionaryFromDoc (XmlDocument          doc, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                              ref List <string> namesExtra)
		{
			if (CLI.settingMode == SettingMode.Light) // It's for better performance
			{
				if (doc.SelectNodes ("/SyntaxDefinition/Environment").Count == 1)
					PopulateByXMLNodeForLight (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref attributes, ref namesExtra);
				PopulateByXMLNodeSingularForLight (doc.SelectNodes ("/SyntaxDefinition/Digits") [0], ref attributes, ref namesExtra);
				PopulateByXMLNodeParentForLight (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0], ref attributes, ref namesExtra);
			} else
			{
				if (doc.SelectNodes ("/SyntaxDefinition/Environment").Count == 1) PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref attributes, ref namesExtra);
				PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref attributes, ref namesExtra);
				PopulateByXMLNodeSingular (doc.SelectNodes ("/SyntaxDefinition/Digits") [0], ref attributes, ref namesExtra);
				PopulateByXMLNodeParent (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0], ref attributes, ref namesExtra);
			}
		}

		public static Tuple <bool, bool> loadThemeToPopulate (ref XmlDocument doc, string pathForMemory, string pathForFile,
		                                                      bool            needToReturn, bool isDefault)
		{
			bool hasImage = false;
			bool hasSticker = false;
			if (isDefault)
			{
				var a = CLI.GetCore ();


				Tuple <bool, string> content = Helper.GetThemeFromMemory (pathForMemory, a);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathForMemory, a);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasImage != null)
							{
								CLI.ifHasImage (iag.Item2);
							}

							if (CLI.ifHasImage2 != null)
							{
								CLI.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHave != null)
								CLI.ifDoesntHave ();
							if (CLI.ifDoesntHave2 != null)
								CLI.ifDoesntHave2 ();
						}
					} else
					{
						hasImage = iag.Item1;
					}

					iag = null;
					iag = Helper.GetStickerFromMemory (pathForMemory, a);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasSticker != null)
							{
								CLI.ifHasSticker (iag.Item2);
							}

							if (CLI.ifHasSticker2 != null)
							{
								CLI.ifHasSticker2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHaveSticker != null)
								CLI.ifDoesntHaveSticker ();
							if (CLI.ifDoesntHaveSticker2 != null)
								CLI.ifDoesntHaveSticker2 ();
						}
					} else
					{
						hasSticker = iag.Item1;
					}
				} else
				{
					if (!needToReturn)
					{
						if (CLI.ifDoesntHave != null)
							CLI.ifDoesntHave ();

						if (CLI.ifDoesntHaveSticker != null)
							CLI.ifDoesntHaveSticker ();

						if (CLI.ifDoesntHave2 != null)
							CLI.ifDoesntHave2 ();

						if (CLI.ifDoesntHaveSticker2 != null)
							CLI.ifDoesntHaveSticker2 ();
					}

					doc.Load (a.GetManifestResourceStream (pathForMemory));
				}
			} else
			{
				CLI.imagePath = "";
				Tuple <bool, string> content = Helper.GetTheme (pathForFile);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.GetImage (pathForFile);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasImage != null)
							{
								CLI.ifHasImage (iag.Item2);
							}

							if (CLI.ifHasImage2 != null)
							{
								CLI.ifHasImage2 (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHave != null)
								CLI.ifDoesntHave ();

							if (CLI.ifDoesntHave2 != null)
								CLI.ifDoesntHave2 ();
						}
					} else
					{
						hasImage = iag.Item1;
					}

					iag = Helper.GetSticker (pathForFile);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (CLI.ifHasSticker != null)
							{
								CLI.ifHasSticker (iag.Item2);
							}
						} else
						{
							if (CLI.ifDoesntHaveSticker != null)
								CLI.ifDoesntHaveSticker ();

							if (CLI.ifDoesntHaveSticker2 != null)
								CLI.ifDoesntHaveSticker2 ();
						}
					} else
					{
						hasSticker = iag.Item1;
					}
				} else
				{
					if (!needToReturn)
					{
						if (CLI.ifDoesntHave != null)
							CLI.ifDoesntHave ();
						if (CLI.ifDoesntHaveSticker != null)
							CLI.ifDoesntHaveSticker ();

						if (CLI.ifDoesntHave2 != null)
							CLI.ifDoesntHave2 ();
						if (CLI.ifDoesntHaveSticker2 != null)
							CLI.ifDoesntHaveSticker2 ();
					}

					try
					{
						doc.Load (pathForFile);
					} catch (XmlException)
					{
						if (CLI.hasProblem != null)
							CLI.hasProblem (
								"There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected");

						return null;
					}
				}
			}

			return new Tuple <bool, bool> (hasImage, hasSticker);
		}

		public static Dictionary <string, string> GetAdditionalInfoFromDoc (XmlDocument doc)
		{
			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			Dictionary <string, string> dictionary = new Dictionary <string, string> ();
			dictionary.Add ("align", ((int) Alignment.Center).ToString ());
			dictionary.Add ("opacity", "15");
			dictionary.Add ("stickerOpacity", "100");
			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("align"))
				{
					dictionary ["align"] = comm.Value.Substring (6);
				} else if (comm.Value.StartsWith ("opacity"))
				{
					dictionary ["opacity"] = comm.Value.Substring (8);
				} else if (comm.Value.StartsWith ("sopacity"))
				{
					dictionary ["stickerOpacity"] = comm.Value.Substring (9);
				}
			}

			return dictionary;
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
				node.AppendChild (doc.CreateComment ("name:" + CLI.currentoFile));
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
		/// Populate list with values. For example Default Background color, Default Foreground color and etc. 
		/// </summary>
		public static void populateList ()
		{
			var doc = new XmlDocument ();
			loadThemeToPopulate (ref doc, CLI.gp, CLI.getPath, false, CLI.isDefault ());

			PopulateDictionaryFromDoc (doc, ref CLI.localAttributes, ref CLI.names);
			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			string al = additionalInfo ["align"];
			string op = additionalInfo ["opacity"];
			string sop = additionalInfo ["stickerOpacity"];

			CLI.localAttributes.Add ("Wallpaper",
			                         new Dictionary <string, string> {{"align", al}, {"opacity", op}});

			CLI.localAttributes.Add ("Sticker",
			                         new Dictionary <string, string> {{"opacity", sop}});
			/*string all = "";
			foreach (KeyValuePair <string, Dictionary <string, string>> pair in localAttributes)
			{
				all += pair.Key + "\n";
			}
			*/

			// System.Windows.Forms.Clipboard.SetText (all);

			CLI.align = (Alignment) (int.Parse (CLI.localAttributes ["Wallpaper"] ["align"]));
			CLI.opacity = int.Parse (CLI.localAttributes ["Wallpaper"] ["opacity"]);
			CLI.sopacity = int.Parse (CLI.localAttributes ["Sticker"] ["opacity"]);
		}

	}
}