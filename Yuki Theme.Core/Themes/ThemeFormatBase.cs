using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Yuki_Theme.Core.API;

namespace Yuki_Theme.Core.Themes
{
	internal abstract class ThemeFormatBase
	{
		public virtual void LoadThemeToCLI ()
		{
			Theme theme = PopulateList (API.CentralAPI.Current.nameToLoad, true);
			API.CentralAPI.Current.currentTheme = theme;
			if (theme == null)
			{
				API.CentralAPI.Current.ShowError (API.CentralAPI.Current.Translate ("messages.theme.invalid.full"), API.CentralAPI.Current.Translate ("messages.theme.invalid.short"));
			} else
			{
				ProcessAfterParsing (theme);
			}
		}

		public abstract Theme PopulateList (string name, bool loadImages);

		public abstract void ProcessAfterParsing (Theme theme);

		public abstract Tuple <bool, string> VerifyToken (string path);

		public abstract string GetNameOfTheme (string path);

		public abstract void SaveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false);

		public abstract void WriteName (string path, string name);

		public abstract void WriteNameAndResetToken (string path, string name);

		public abstract void ReGenerate (string path, string oldPath, string name, string oldName, API_Actions apiActions);
	}
}