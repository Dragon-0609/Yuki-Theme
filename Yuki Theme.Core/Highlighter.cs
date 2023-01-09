using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using static System.Drawing.Brushes;
using TextStyle = FastColoredTextBoxNS.TextStyle;

namespace Yuki_Theme.Core
{

	public class Highlighter : HighlighterBase
	{
		private Dictionary<string, ThemeField> localAttributes => API.CentralAPI.Current.currentTheme.Fields;

		private FastColoredTextBox sBox;
		
		public Highlighter()
		{
			
		}
		
		public Highlighter (FastColoredTextBox fs)
		{
			sBox = fs;
			Init();
		}

		private void Init()
		{
			sBox.Clear();
			sBox.TextChanged += PascalSyntaxHighlight;
		}

		public void UpdateColors ()
		{
			UpdateColors(sBox, localAttributes, ref _styles);
		}

		private void LaunchMarker ()
		{
			sBox.Range.ClearStyle (ellipseStyle);

			if (_currentRegex != null)
			{
				sBox.Range.SetStyle (ellipseStyle, _currentRegex);
			}
		}

		public void ActivateColors (string str)
		{
			_currentRegex = null;
			if (_regexes == null)
				InitPascalRegex ();
			if (_regexes.ContainsKey (str.ToLower ()) || Settings.settingMode == SettingMode.Light)
			{
				if (Settings.settingMode == SettingMode.Light)
				{
					string [] srt = Populater.GetDependencies (str);
					if (srt != null)
					{
						string rgx = "";
						int ik = 0;

						foreach (string dependency in srt)
						{
							if (_regexes.ContainsKey (dependency.ToLower ()))
							{
								if (ik == 0)
								{
									rgx = $"{_regexes [dependency.ToLower ()]}";
									ik++;
								} else if (!Populater.IsEnvironmentColor (dependency))
								{
									rgx += $"|{_regexes [dependency.ToLower ()]}";
								}
							}
						}

						_currentRegex = new Regex (rgx, RegexOptions.Multiline | RegexCompiledOption);
					} else
						_currentRegex = _regexes [str.ToLower ()];
				} else
					_currentRegex = _regexes [str.ToLower ()];
			}

			if (_currentRegex != null)
			{
				List <int> sl = sBox.FindLines (_currentRegex.ToString (), RegexOptions.Compiled);
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
			sBox.CommentPrefix = "//";
			sBox.LeftBracket = '(';
			sBox.RightBracket = ')';
			//clear style of changed range

			if (_styles == null)
				InitStyles (ref _styles);
			TextStyle [] tstyles = new TextStyle [_styles.Count];
			_styles.Values.CopyTo (tstyles, 0);
			e.ChangedRange.ClearStyle (tstyles);

			if (_regexes == null)
				InitPascalRegex ();

			foreach (string name in HighlitherUtil.names)
			{
				e.ChangedRange.SetStyle (_styles [name], _regexes [name]);
			}

			//clear folding markers
			e.ChangedRange.ClearFoldingMarkers ();
			e.ChangedRange.SetFoldingMarkers (@"begin\b", @"end\b");
			e.ChangedRange.SetFoldingMarkers (@"uses\b", @"end.\b");
		}

		public void InitializeSyntax ()
		{
			InitializeSyntax(sBox, localAttributes);
		}
	}
}