using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class ThemePreview : UserControl
	{
		private Dictionary <string, string []> localNames = new ()
		{
			{ "string", new [] { "str" } },
			{ "internaltypes", new [] { "type" } },
			{ "keywords", new [] { "keyw" } },
			{ "markprevious", new [] { "meth" } },
			{ "beginend", new [] { "bge", "bget" } },
		};

		public ThemePreview ()
		{
			InitializeComponent ();
		}

		public void SetTheme (Theme theme)
		{
			foreach (KeyValuePair <string, ThemeField> pair in theme.Fields)
			{
				if (localNames.ContainsKey (pair.Key.ToLower ()))
				{
					foreach (string target in localNames[pair.Key.ToLower()])
					{
						if (this.Resources [target] is Style style)
							style.Setters [0] = new Setter (ForegroundProperty, pair.Value);
					}

				}
			}
		}

	}
}