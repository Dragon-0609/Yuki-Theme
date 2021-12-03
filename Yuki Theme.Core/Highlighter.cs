﻿using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.Forms;
using static System.Drawing.Brushes;
using TextStyle = FastColoredTextBoxNS.TextStyle;

namespace Yuki_Theme.Core
{
	public class Highlighter
	{
		#region FSTB Fields

		protected static readonly Platform platformType = PlatformType.GetOperationSystemPlatform ();

		//styles

		private MForm form;

		private Dictionary <string, Dictionary <string, string>> localAttributes => form.localAttributes;

		private Dictionary <string, Regex>     regexes;
		public static Dictionary <string, TextStyle> styles;

		private string [] names =
		{
			"string", "linebigcomment", "linecomment", "blockcomment", "blockcomment2", "digits", "beginend",
			"keywords", "programsections", "punctuation", "nonreserved1", "operatorkeywords", "selectionstatements",
			"iterationstatements", "exceptionhandlingstatements", "raisestatement", "jumpstatements", "jumpprocedures",
			"internalconstant", "internaltypes", "referencetypes", "modifiers", "accessmodifiers", "accesskeywords1",
			"errorwords", "warningwords", "direcivenames", "specialdirecivenames", "direcivevalues"
		};

		private Regex currentRegex;

		private EllipseStyle ellipseStyle = new EllipseStyle ();

		public static RegexOptions RegexCompiledOption
		{
			get
			{
				if (platformType == Platform.X86)
					return RegexOptions.Compiled;
				return RegexOptions.None;
			}
		}

		public FastColoredTextBox sBox;

		#endregion

		public Highlighter (FastColoredTextBox fs, MForm fm)
		{
			form = fm;
			sBox = fs;
			sBox.Clear ();
			sBox.TextChanged += PascalSyntaxHighlight;
		}

		public void updateColors ()
		{
			if (styles == null)
				InitStyles ();
			foreach (KeyValuePair <string, Dictionary <string, string>> style in localAttributes)
			{
				string key = style.Key.ToLower ();
				if (isInNames (key))
				{
					if (style.Value.ContainsKey ("color"))
						styles [key].ForeBrush = new SolidBrush (Parse (style.Value ["color"]));

					if (style.Value.ContainsKey ("bgcolor"))
						styles [key].BackgroundBrush = new SolidBrush (Parse (style.Value ["bgcolor"]));

					if (style.Value.ContainsKey ("bold"))
					{
						// Console.WriteLine($"YS {key}");
						styles [key].FontStyle = collectFontStyle (style.Value);
					}

					if (key == "keywords")
					{
						Helper.fgKeyword = Parse (style.Value ["color"]);
					}
					// else
					// Console.WriteLine($"BL {key}");
				} else
				{
					// Console.WriteLine (style.Key);
					switch (style.Key)
					{
						case "Default" :
						{
							sBox.BackColor = Parse (style.Value ["bgcolor"]);
							sBox.ForeColor = Parse (style.Value ["color"]);
							Helper.bgColor = Helper.DarkerOrLighter (sBox.BackColor, 0.05f);
							Helper.fgColor = Helper.DarkerOrLighter (sBox.ForeColor, 0.2f);
							Helper.bgClick = Helper.DarkerOrLighter (sBox.BackColor, 0.25f);
							Helper.fgHover = Helper.DarkerOrLighter (sBox.ForeColor, 0.4f);

							// sBox.WhitespaceColor = sBox.BackColor;
							form.updateBackgroundColors ();
						}
							break;
						case "Selection" :
						{
							sBox.SelectionColor = Parse (style.Value ["bgcolor"]);
						}
							break;
						case "VRuler" :
						{
							sBox.ServiceLinesColor = Parse (style.Value ["color"]);
						}
							break;
						case "CaretMarker" :
						{
							sBox.CaretColor = Parse (style.Value ["color"]);
							Helper.bgBorder = sBox.CaretColor;
						}
							break;
						case "LineNumbers" :
						{
							sBox.LineNumberColor = Parse (style.Value ["color"]);
							sBox.IndentBackColor = Parse (style.Value ["bgcolor"]);
							sBox.PaddingBackColor = Parse (style.Value ["bgcolor"]);
						}
							break;
						case "FoldMarker" :
						{
							sBox.ServiceColors.CollapseMarkerForeColor = Parse (style.Value ["color"]);
							sBox.ServiceColors.ExpandMarkerForeColor = Parse (style.Value ["color"]);
							sBox.ServiceColors.CollapseMarkerBackColor = Parse (style.Value ["bgcolor"]);
							sBox.ServiceColors.ExpandMarkerBackColor = Parse (style.Value ["bgcolor"]);
						}
							break;
						case "SelectedFoldLine" :
						{
							sBox.ServiceColors.SelectedMarkerBorderColor = Parse (style.Value ["color"]);
						}
							break;
					}
				}
			}

			sBox.Refresh ();
		}

		private void InitPascalRegex ()
		{
			regexes = new Dictionary <string, Regex> ();
			regexes.Add ("string", new Regex (@"''|'.*?[^\\]'", RegexCompiledOption));
			regexes.Add ("linebigcomment", new Regex (@"////.*$", RegexOptions.Multiline | RegexCompiledOption));
			regexes.Add ("linecomment", new Regex (@"//.*$", RegexOptions.Multiline | RegexCompiledOption));
			regexes.Add ("blockcomment", new Regex (@"({.*?\})|({.*)", RegexOptions.Singleline | RegexCompiledOption));
			regexes.Add ("blockcomment2", new Regex (@"(\(\*.*?\*\))|(.*\*\))",
			                                         RegexOptions.Singleline | RegexOptions.RightToLeft |
			                                         RegexCompiledOption));
			regexes.Add ("digits", new Regex (@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b",
			                                  RegexCompiledOption));
			regexes.Add ("beginend", new Regex (@"\b(?i)(begin|end)\b", RegexOptions.Singleline | RegexCompiledOption));
			regexes.Add ("keywords", new Regex (
				             @"\b(?i)(external|in|array|sequence|yield|auto|static|template|sealed|partial|const|lock|constructor|destructor|downto|file|loop|function|inherited|procedure|operator|property|record|repeat|set|type|then|to|until|uses|var|event|while|params|with|of|label|implicit|explicit|initialization|finalization|where|match|when)\b",
				             RegexCompiledOption));
			regexes.Add ("programsections", new Regex (
				             @"\b(?i)(unit|library|namespace|program|interface|implementation)\b",
				             RegexCompiledOption));
			regexes.Add ("punctuation", new Regex (@"[*,.;+-/()<>^&]|(<=)|(>=)|(\[)|(\])", RegexCompiledOption));
			regexes.Add ("nonreserved1", new Regex (@"\b(?i)(self|result|value)\b", RegexCompiledOption));
			regexes.Add ("async", new Regex (@"\b(?i)(async|asyncparam)\b", RegexCompiledOption));
			regexes.Add ("operatorkeywords",
			             new Regex (@"\b(?i)(or|xor|and|div|mod|shl|shr|not|as|is|new|sizeof|typeof)\b",
			                        RegexCompiledOption));
			regexes.Add ("selectionstatements", new Regex (@"\b(?i)(else|if|case)\b", RegexCompiledOption));
			regexes.Add ("iterationstatements", new Regex (@"\b(?i)(do|for|foreach)\b", RegexCompiledOption));
			regexes.Add ("exceptionhandlingstatements",
			             new Regex (@"\b(?i)(except|on|try|finally)\b", RegexCompiledOption));
			regexes.Add ("raisestatement", new Regex (@"\b(?i)(raise)\b", RegexCompiledOption));
			regexes.Add ("jumpstatements", new Regex (@"\b(?i)(goto)\b", RegexCompiledOption));
			regexes.Add ("jumpprocedures", new Regex (@"\b(?i)(break|exit|continue)\b", RegexCompiledOption));
			regexes.Add ("internalconstant", new Regex (@"\b(?i)(true|false|nill)\b", RegexCompiledOption));
			regexes.Add ("internaltypes",
			             new Regex (
				             @"\b(?i)(boolean|byte|shortint|smallint|word|integer|BigInteger|longword|uint64|cardinal|int64|single|longint|string|char|real|double|pointer|object|decimal)\b",
				             RegexCompiledOption));
			regexes.Add ("referencetypes", new Regex (@"\b(?i)(class|interface)\b", RegexCompiledOption));
			regexes.Add ("modifiers",
			             new Regex (
				             @"\b(?i)(abstract|overload|reintroduce|override|extensionmethod|virtual|default|forward)\b",
				             RegexCompiledOption));
			regexes.Add ("accessmodifiers",
			             new Regex (@"\b(?i)(internal|public|protected|private)\b", RegexCompiledOption));
			regexes.Add ("accesskeywords1",
			             new Regex (@"\b(?i)(inherited)\b", RegexCompiledOption));
			regexes.Add ("errorwords", new Regex (@"\b(?i)(TODO|FIXME)\b", RegexCompiledOption));
			regexes.Add ("warningwords", new Regex (@"\b(?i)(HACK|UNDONE)\b", RegexCompiledOption));
			regexes.Add ("direcivenames",
			             new Regex (
				             @"\b(?i)(apptype|resource|reference|version|product|company|copyright|trademark|mainresource|NullBasedStrings|gendoc)\b",
				             RegexCompiledOption));
			regexes.Add ("specialdirecivenames", new Regex (@"\b(?i)(savepcu)\b", RegexCompiledOption));
			regexes.Add ("direcivevalues", new Regex (@"\b(?i)(console|windows|dll|pcu)\b", RegexCompiledOption));
		}

		public static void InitStyles ()
		{
			styles = new Dictionary <string, TextStyle> ();
			styles.Add ("string", new TextStyle (DarkRed, null, FontStyle.Regular));
			styles.Add ("digits", new TextStyle (Blue, null, FontStyle.Regular));
			styles.Add ("linebigcomment", new TextStyle (Green, null, FontStyle.Regular));
			styles.Add ("linecomment", new TextStyle (Green, null, FontStyle.Regular));
			styles.Add ("blockcomment", new TextStyle (Green, null, FontStyle.Regular));
			styles.Add ("blockcomment2", new TextStyle (Green, null, FontStyle.Regular));
			styles.Add ("beginend", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("keywords", new TextStyle (PowderBlue, null, FontStyle.Bold));
			styles.Add ("programsections", new TextStyle (PowderBlue, null, FontStyle.Bold));
			styles.Add ("punctuation", new TextStyle (Red, null, FontStyle.Regular));
			styles.Add ("nonreserved1", new TextStyle (PowderBlue, null, FontStyle.Regular));
			styles.Add ("async", new TextStyle (PowderBlue, null, FontStyle.Regular));
			styles.Add ("operatorkeywords", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("selectionstatements", new TextStyle (Gray, null, FontStyle.Bold));
			styles.Add ("iterationstatements", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("exceptionhandlingstatements", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("raisestatement", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("jumpstatements", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("jumpprocedures", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("internalconstant", new TextStyle (Red, null, FontStyle.Regular));
			styles.Add ("internaltypes", new TextStyle (Red, null, FontStyle.Regular));
			styles.Add ("referencetypes", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("modifiers", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("accessmodifiers", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("accesskeywords1", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("errorwords", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("warningwords", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("direcivenames", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("specialdirecivenames", new TextStyle (Red, null, FontStyle.Bold));
			styles.Add ("direcivevalues", new TextStyle (Red, null, FontStyle.Regular));
		}

		private void LaunchMarker ()
		{
			sBox.Range.ClearStyle (ellipseStyle);

			if (currentRegex != null)
			{
				// Console.WriteLine("TEST");
				sBox.Range.SetStyle (ellipseStyle, currentRegex);
			}
		}

		public void activateColors (string str)
		{
			currentRegex = null;
			if (regexes == null)
				InitPascalRegex ();
			if (regexes.ContainsKey (str.ToLower ()))
			{
				if (form.settingMode == 0)
				{
					string [] srt = Populater.getDependencies (str);
					if (srt != null)
					{
						string rgx = $"{regexes [str.ToLower ()]}";

						foreach (string dependency in srt)
						{
							// Console.WriteLine(dependency);
							rgx += $"|{regexes [dependency.ToLower ()]}";
						}

						// Console.WriteLine(rgx.ToString());
						currentRegex = new Regex (rgx, RegexOptions.Multiline | RegexCompiledOption);
					} else
						currentRegex = regexes [str.ToLower ()];
				} else
					currentRegex = regexes [str.ToLower ()];
			}

			if (currentRegex != null)
			{
				List <int> sl = sBox.FindLines (currentRegex.ToString (), RegexOptions.Compiled);
				if (sl != null && sl.Count > 0)
				{
					int ms = (sBox.VisibleRange.ToLine - sBox.VisibleRange.FromLine) / 2;
					Range rns = sBox.GetRange (new Place (173, sl [0]), new Place (173, sl [0] + ms));
					sBox.DoRangeVisible (rns);
				}
			}

			LaunchMarker ();
		}

		private void PascalSyntaxHighlight (object sender, TextChangedEventArgs e)
		{
			sBox.LeftBracket = '(';
			sBox.RightBracket = ')';
			sBox.CommentPrefix = "//";
			//clear style of changed range

			if (styles == null)
				InitStyles ();
			TextStyle [] tstyles = new TextStyle [styles.Count];
			styles.Values.CopyTo (tstyles, 0);
			e.ChangedRange.ClearStyle (tstyles);

			if (regexes == null)
				InitPascalRegex ();

			foreach (string name in names)
			{
				e.ChangedRange.SetStyle (styles [name], regexes [name]);
			}

			//clear folding markers
			e.ChangedRange.ClearFoldingMarkers ();
			e.ChangedRange.SetFoldingMarkers (@"begin\b", @"end\b");
			e.ChangedRange.SetFoldingMarkers (@"uses\b", @"end.\b");
		}

		public void InitializeSyntax ()
		{
			sBox.Text = Placeholder.place;

			updateColors ();
		}

		private Color Parse (string str)
		{
			return ColorTranslator.FromHtml (str);
		}

		private string Translate (Color clr)
		{
			return ColorTranslator.ToHtml (clr);
		}

		public static bool isInNames (string str)
		{
			if (styles == null)
				InitStyles ();
			return styles.ContainsKey (str.ToLower ());
		}


		private FontStyle addFontStyle (FontStyle font, FontStyle f2, bool ts)
		{
			if (ts)
			{
				if (font != FontStyle.Regular)
					return font | f2;
				else
					return f2;
			} else
				return font;
		}

		private FontStyle collectFontStyle (Dictionary <string, string> val)
		{
			FontStyle font = FontStyle.Regular;
			font = addFontStyle (font, FontStyle.Bold, bool.Parse (val ["bold"]));
			font = addFontStyle (font, FontStyle.Italic, bool.Parse (val ["italic"]));
			return font;
		}
	}
}