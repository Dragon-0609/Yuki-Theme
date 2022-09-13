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
		protected API_Base api;
		
		public virtual void LoadThemeToCLI ()
		{
			Theme theme = PopulateList (api.nameToLoad, true);
			api.currentTheme = theme;
			if (theme == null)
			{
				api.ShowError (api.Translate ("messages.theme.invalid.full"), api.Translate ("messages.theme.invalid.short"));
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

		protected void LoadImage (bool exist, bool needToDoActions, string pathForMemory, Action<Image> hasImage, Action doesntHaveImage)
		{
			if (needToDoActions)
			{
				if (exist)
				{
					if (hasImage != null)
					{
						hasImage (Helper.GetImage (pathForMemory));
					}
				} else
				{
					if (doesntHaveImage != null)
						doesntHaveImage ();
				}
			}
		}
		protected void LoadSticker (bool exist, bool needToDoActions, string pathForMemory, Action<Image> hasImage, Action doesntHaveImage)
		{
			if (needToDoActions)
			{
				if (exist)
				{
					if (hasImage != null)
					{
						hasImage (Helper.GetSticker (pathForMemory));
					}
				} else
				{
					if (doesntHaveImage != null)
						doesntHaveImage ();
				}
			}
		}

		protected void LoadImage (bool exist, bool needToDoActions, string pathForMemory, Assembly a, Action<Image> hasImage, Action doesntHaveImage)
		{
			if (needToDoActions)
			{
				if (exist)
				{
					if (hasImage != null)
					{
						hasImage (Helper.GetImageFromMemory (pathForMemory, a));
					}
				} else
				{
					if (doesntHaveImage != null)
						doesntHaveImage ();
				}
			}
		}
		protected void LoadSticker (bool exist, bool needToDoActions, string pathForMemory, Assembly a, Action<Image> hasImage, Action doesntHaveImage)
		{
			if (needToDoActions)
			{
				if (exist)
				{
					if (hasImage != null)
					{
						hasImage (Helper.GetStickerFromMemory (pathForMemory, a));
					}
				} else
				{
					if (doesntHaveImage != null)
						doesntHaveImage ();
				}
			}
		}
	}
}