using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Xml;
using CommonMark;
using Svg;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using Size = System.Drawing.Size;

namespace Yuki_Theme.Core
{
	public static class Helper
	{
		public static ProductMode?  mode;
		public static RelativeUnit unit;

		private static Size   Standart32 = new Size (32, 32);
		public static  string CustomThemesBegin => Path.Combine (SettingsConst.CurrentPath, "Themes");

		public static Rectangle GetSizes (Size ima, int mWidth, int mHeight, Alignment align)
		{
			Rectangle res = new Rectangle ();
			double rY = (double)mHeight / ima.Height;
			res.Width = (int)(ima.Width * rY);
			res.Height = (int)(ima.Height * rY);
			res.X = (mWidth - res.Width) / (int)align;

			// If image's drawing rectangle's width is smaller than mWidth.

			if (res.Width < mWidth && Settings.autoFitByWidth)
				res = GetSizesHorizontal (ima, mWidth, mHeight);

			return res;
		}

		/// <summary>
		/// It's used to fill free space of width. 
		/// For example, image rectangle is:   1066x600, Area rectangle is: 1400x600. In this case, size will be calculated by width.
		/// </summary>
		/// <param name="ima">Image</param>
		/// <param name="mWidth">Max Width</param>
		/// <param name="mHeight">Max Height</param>
		/// <returns>Calculated size according width</returns>
		private static Rectangle GetSizesHorizontal (Size ima, int mWidth, int mHeight)
		{
			Rectangle res = new Rectangle ();
			double rY = (double)mWidth / ima.Width;
			res.Width = (int)(ima.Width * rY);
			res.Height = (int)(ima.Height * rY);
			res.Y = (mHeight - res.Height) / 2;

			return res;
		}

		public static string currentTheme;

		#region CONST

		internal const string THEME_NAME_OLD      = "theme.xshd";
		internal const string THEME_NAME_NEW      = "theme.json";
		internal const string WALLPAPER_NAME      = "background.png";
		internal const string STICKER_NAME        = "sticker.png";
		public const  string FILE_EXTENSTION_OLD = ".yukitheme";
		public const  string FILE_EXTENSTION_NEW = ".yuki";
		public const  string PASCALTEMPLATE      = "Yuki_Theme.Core.Resources.Syntax_Templates.Pascal.xshd";

		#endregion


		#region Get Image


		public static bool DoesImageExist (string path)
		{
			return ZipManager.DoesImageExist (path, WALLPAPER_NAME);
		}

		public static bool DoesStickerExist (string path)
		{
			return ZipManager.DoesImageExist (path, STICKER_NAME);
		}
		
		public static Image GetImage (string path)
		{
			return ZipManager.GetImage (path, WALLPAPER_NAME);
		}

		public static Image GetSticker (string path)
		{
			return ZipManager.GetImage (path, STICKER_NAME);
		}

		public static Image GetStickerFromMemory (string path, Assembly a)
		{
			return ZipManager.GetImageFromMemory (path, STICKER_NAME, a);
		}

		public static Image GetImageFromMemory (string path, Assembly a)
		{
			return ZipManager.GetImageFromMemory (path, WALLPAPER_NAME, a);
		}

		public static bool DoesImageExistInMemory (string path, Assembly a)
		{
			return ZipManager.DoesImageExistInMemory (path, WALLPAPER_NAME, a);
		}

		public static bool DoesStickerExistInMemory (string path, Assembly a)
		{
			return ZipManager.DoesImageExistInMemory (path, STICKER_NAME, a);
		}
		
		#endregion


		#region Get Theme

		public static ThemeFormat GetThemeFormat (bool isDefault, string path, string name)
		{
			if (isDefault)
			{
				Assembly assembly;
				string pathHeader;
				IThemeHeader header = DefaultThemes.headers [name];
				assembly = header.Location;
				pathHeader = header.ResourceHeader;
				if (assembly.GetManifestResourceStream ($"{pathHeader}.{path}{FILE_EXTENSTION_OLD}") != null)
					return ThemeFormat.Old;
				else if (assembly.GetManifestResourceStream ($"{pathHeader}.{path}{FILE_EXTENSTION_NEW}") != null)
					return ThemeFormat.New;
				else
				{
					return ThemeFormat.Null;
				}
			} else
			{
				if (File.Exists (Path.Combine (CustomThemesBegin, path + FILE_EXTENSTION_OLD)))
					return ThemeFormat.Old;
				else if (File.Exists (Path.Combine (CustomThemesBegin, path + FILE_EXTENSTION_NEW)))
					return ThemeFormat.New;
				else
				{
					return ThemeFormat.Null;
				}
			}
		}

		#endregion
		

		#region Color Management

		public static Color ChangeColorBrightness (Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			} else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb (color.A, (int)red, (int)green, (int)blue);
		}

		public static bool IsDark (Color clr)
		{
			bool dark = ((clr.R + clr.G + clr.B) / 3 < 127);
			return dark;
		}

		public static Color DarkerOrLighter (Color clr, float percent = 0)
		{
			if (IsDark (clr))
				return ChangeColorBrightness (clr, percent);
			else
				return ChangeColorBrightness (clr, -percent);
		}

		#endregion


		#region SVG Manager

		public static SvgDocument LoadSvg (string name, Assembly a, string customName = "Yuki_Theme.Core.Resources.SVG")
		{
			var doc = new XmlDocument ();
			doc.Load (a.GetManifestResourceStream ($"{customName}.{name}.svg"));
			SvgDocument svg = SvgDocument.Open (doc);
			return svg;
		}

		public static void RenderSvg (Control im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false,
		                              Color   clr = default)
		{
			im.BackgroundImage?.Dispose ();

			im.BackgroundImage = RenderSvg (im.Size, svg, custom, cSize, customColor, clr);
		}

		public static void RenderSvg (ToolStripButton im,                  SvgDocument svg, bool custom = false, Size cSize = default,
		                              bool            customColor = false, Color       clr = default)
		{
			im.Image?.Dispose ();

			im.Image = RenderSvg (im.Size, svg, custom, cSize, customColor, clr);
		}

		public static void RenderSvg (Form im,                  SvgDocument svg, bool custom = false, Size cSize = default,
		                              bool customColor = false, Color       clr = default)
		{
			// im.Icon?.Dispose ();
			IntPtr ptr = ((Bitmap)RenderSvg (Standart32, svg, custom, cSize, customColor, clr)).GetHicon ();

			im.Icon = Icon.FromHandle (ptr);
			// DestroyIcon (ptr);
		}

		public static void RenderSvg (ToolStripMenuItem im,                  SvgDocument svg, bool custom = false, Size cSize = default,
		                              bool              customColor = false, Color       clr = default)
		{
			im.Image?.Dispose ();

			im.Image = RenderSvg (im.Size, svg, custom, cSize, customColor, clr);
		}

		public static Image RenderSvg (Size  im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false,
		                               Color clr = default)
		{
			if (customColor)
				svg.Color = new SvgColourServer (clr);
			else
				svg.Color = new SvgColourServer (ColorKeeper.fgColor);

			if (!custom)
				return svg.Draw (im.Width, im.Height);
			else
				return svg.Draw (cSize.Width, cSize.Height);
		}

		public static Image RenderSvg (Size im, SvgDocument svg, Dictionary <string, Color> idColors, bool customColor = false, Color clr = default)
		{
			if (customColor)
				svg.Color = new SvgColourServer (clr);
			else
				svg.Color = new SvgColourServer (ColorKeeper.fgColor);

			if (idColors != null)
			{
				foreach (KeyValuePair <string, Color> idColor in idColors)
				{
					SvgElement element = svg.GetElementById (idColor.Key);
					element.Fill = new SvgColourServer (idColor.Value);
				}
			}
			
			return svg.Draw (im.Width, im.Height);
		}

		#endregion

		
		#region HTML Zone

		/// <summary>
		/// Load HTML from memory and return as string
		/// </summary>
		/// <param name="name">Name of file with extension (.html)</param>
		/// <param name="nameSpace">Namespace of resource. Default: Yuki_Theme.Core.Resources. (dot must be included)</param>
		/// <returns></returns>
		public static string ReadHTML (string name, string nameSpace = "Yuki_Theme.Core.Resources.")
		{
			Assembly a = Assembly.GetExecutingAssembly ();

			Stream stm = a.GetManifestResourceStream (nameSpace + name);
			string md = "";
			using (StreamReader reader = new StreamReader (stm))
			{
				md = reader.ReadToEnd ();
			}

			stm.Dispose ();

			return md;
		}
		
		public static string ReplaceHTMLColors (string html)
		{
			html = html.Replace ("__bg__", ColorTranslator.ToHtml (ColorKeeper.bgColor));
			html = html.Replace ("__clr__", ColorTranslator.ToHtml (ColorKeeper.fgColor));
			html = html.Replace ("__clr_click__", ColorTranslator.ToHtml (ColorKeeper.fgHover));
			if (html.Contains ("__border__"))
			{
				html = html.Replace ("__border__", ColorTranslator.ToHtml (ColorKeeper.bgBorder));
			}
			return html;
		}

		private static string ReplaceMDCheckbox (string md)
		{
			md = md.Replace ("- [x]", "<br><input disabled type='checkbox' checked='checked'>");
			md = md.Replace ("- [ ]", "<br><input disabled type='checkbox'>");
			return md;
		}
		
		
		public static string ReadNConvertMDToHTML (string target, string nameSpace = "Yuki_Theme.Core.Resources.")
		{
			string md = ReadResource (target, nameSpace);
			md = md.Split (new [] { "###" }, StringSplitOptions.None) [1];
			md = ReplaceMDCheckbox (md);
			md = CommonMarkConverter.Convert (md);

			string html = ReadHTML ("CHANGELOG.html", nameSpace);
			html = ReplaceHTMLColors (html);
			html = html.Replace ("__content__", md);

			return html;
		}
		
		#endregion

		
		public static string ToStringLower (this bool bol)
		{
			return bol.ToString ().ToLower ();
		}

		public static string GetThemeSaveName (bool asOld)
		{
			return "theme." + GetSaveFormat (asOld);
		}

		public static string GetSaveFormat (bool asOld)
		{
			return asOld ? "xshd" : "json";
		}


		public static Image SetOpacity (Image image, float opacity)
		{
			Bitmap bmp = new Bitmap (image.Width, image.Height);
			opacity = opacity == 0 ? opacity : opacity / 100f;
			using (Graphics g = Graphics.FromImage (bmp))
			{
				ColorMatrix matrix = new ColorMatrix ();
				matrix.Matrix33 = opacity;
				ImageAttributes attributes = new ImageAttributes ();
				attributes.SetColorMatrix (matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				g.DrawImage (image, new Rectangle (0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height,
				             GraphicsUnit.Pixel, attributes);
			}

			return bmp;
		}

		public static void LoadCurrent ()
		{
			CentralAPI.Current.LoadSchemes ();
			string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
			bool sett = false;
			if (files.Length > 0)
			{
				foreach (string s in files)
				{
					string sp = CentralAPI.Current.GetNameOfTheme (s);
					if (CentralAPI.Current.Schemes.Contains (sp))
					{
						currentTheme = sp;
						CentralAPI.Current.selectedItem = currentTheme;
						sett = true;
						break;
					}
				}
			}

			if (!sett)
				currentTheme = "unknown";
		}

		public static string ConvertNameToPath (string name)
		{
			return name.Replace (": ", "__").Replace (":", "");
		}

		public static void CreateThemeDirectory ()
		{
			if (CentralAPI.Current.CreateThemesDirectory && !Directory.Exists (Path.Combine (SettingsConst.CurrentPath, "Themes")))
				Directory.CreateDirectory (Path.Combine (SettingsConst.CurrentPath, "Themes"));
		}

		public static T GetRandomElement <T> (List <T> list)
		{
			Random random = new Random ();
			int start2 = random.Next (0, list.Count);
			return list [start2];
		}

		public static Image GetYukiThemeIconImage (Size size)
		{
			return RenderSvg (size, LoadSvg ("yuki_theme", Assembly.GetExecutingAssembly ()));
		}

		public static Icon GetYukiThemeIcon (Size size)
		{
			return Icon.FromHandle (((Bitmap)RenderSvg (size, LoadSvg ("yuki_theme", Assembly.GetExecutingAssembly ()))).GetHicon ());
		}

		#region Encryptor

		public static string EncryptString (string key, string plainText)
		{
			byte [] iv = new byte[16];
			byte [] array;
			key = KeyFillerNCutter (key);
			using (Aes aes = Aes.Create ())
			{
				KeySizes [] ks = aes.LegalKeySizes;
				
				aes.Key = Encoding.UTF8.GetBytes (key);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor (aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream ())
				{
					using (CryptoStream cryptoStream = new CryptoStream ((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter ((Stream)cryptoStream))
						{
							streamWriter.Write (plainText);
						}

						array = memoryStream.ToArray ();
					}
				}
			}

			return Convert.ToBase64String (array);
		}

		public static string DecryptString (string key, string cipherText)
		{
			byte [] iv = new byte[16];
			byte [] buffer = Convert.FromBase64String (cipherText);
			key = KeyFillerNCutter (key);
			using (Aes aes = Aes.Create ())
			{
				aes.Key = Encoding.UTF8.GetBytes (key);
				aes.IV = iv;
				ICryptoTransform decryptor = aes.CreateDecryptor (aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream (buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream ((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader ((Stream)cryptoStream))
						{
							return streamReader.ReadToEnd ();
						}
					}
				}
			}
		}

		public static string KeyFillerNCutter (string key)
		{
			string res = key;
			if (res.Length < 16)
			{
				res += new string ('.', 16 - res.Length);
			}else if (res.Length > 16)
			{
				res = res.Substring (0, 16);
			}
			return res;
		}
		

		#endregion

        public static string GetExtension (bool isOld)
        {
	        return isOld ? FILE_EXTENSTION_OLD : FILE_EXTENSTION_NEW;
        }
        
        public static void RenameKey<TKey, TValue>(this IDictionary <TKey, TValue> dic,
                                                   TKey                            fromKey, TKey toKey)
        {
	        TValue value = dic[fromKey];
	        dic.Remove(fromKey);
	        dic[toKey] = value;
        }

        public static bool VerifyToken (Theme theme)
        {
	        if (theme != null && theme.Token != null && theme.Token != "null")
	        {
		        string token = theme.Token;
		        if (token == "" || token.Length < 6) return false;
		        try
		        {
			        string decryption = DecryptString (theme.Name, token);
			        DateTime date = decryption.ToDateTime ("ddMMyyyy");
			        return true;
		        } catch (Exception e)
		        {
			        Console.WriteLine ("{0} -> {1}", e.Message, e.StackTrace);
			        // ignored
		        }
	        }

	        return false;
        }
        
        
        public static bool Exist (this string path)
        {
	        return File.Exists (path);
        }

        /// <summary>
        /// Load string from resources
        /// </summary>
        /// <param name="target">Target file</param>
        /// <param name="nameSpace">Namespace to be loaded. '.' (dot) must be included.
        /// Default: Yuki_Theme.Core.Resources. </param>
        /// <returns></returns>
        public static string ReadResource (string target, string nameSpace = "Yuki_Theme.Core.Resources.")
        {
	        string result = "";
	        Assembly a = CentralAPI.Current.GetCore ();
	        Stream stm = a.GetManifestResourceStream (nameSpace + target);
	        if (stm != null)
		        using (StreamReader reader = new StreamReader (stm))
		        {
			        result = reader.ReadToEnd ();
		        }

	        return result;
        }

        public static Size CalculateDimension (Size value)
        {
	        Size size = value;
	        int capMax = Settings.dimensionCapMax;
	        double aspect = 0;
	        if (IsDimensionAvailable ())
	        {
		        if ((DimensionCapUnit)Settings.dimensionCapUnit == DimensionCapUnit.Height)
		        {
			        if (size.Height > capMax)
			        {
				        aspect= (size.Width + 0.0) / size.Height;
				        size.Height = capMax;
				        size.Width = Convert.ToInt32 (Math.Round (aspect * capMax));
			        }
		        } else
		        {
			        if (size.Width > capMax)
			        {
				        aspect = (size.Height + 0.0) / size.Width;
				        size.Width = capMax;
				        size.Height = Convert.ToInt32 (Math.Round (aspect * capMax));
			        }
		        }
	        }

	        // MessageBox.Show ( $"Dimension: {Settings.dimensionCapUnit}, max: {capMax}, aspect: {aspect}, Size: {size}, Original: {value}");
	        return size;
        }

        internal static bool IsDimensionAvailable ()
        {
	        return Settings.useDimensionCap && Settings.dimensionCapMax > 20;
        }

        public static int RecognizeInstallationStatus ()
        {
			if (Settings.database.GetValue ("cli_update", "null") != "null")
				Settings.database.DeleteValue ("cli_update");

	        return Settings.database.GetValue ("install").Length != 0 ? 1 : 0;
        }

        public static void DeleteInstallationStatus ()
        {
			Settings.database.DeleteValue ("install");
        }

        public static NativeWindow ToNativeWindow (this Window window)
        {
	        NativeWindow win32Parent = new NativeWindow ();
	        win32Parent.AssignHandle (new WindowInteropHelper (window).Handle);
	        return win32Parent;
        }
	}

	public static class GoogleAnalyticsHelper
	{
		private static readonly string endpoint         = "https://www.google-analytics.com/collect";
		private static readonly string googleVersion    = "1";
		private static readonly string googleTrackingId = "UA-213918512-2";
		private static readonly string googleClientId   = "555";

		/// <summary>
		/// By this method I'll track installs. I won't abuse. I'll track just installs and that's all. 
		/// </summary>
		/// <returns></returns>
		public static async Task <HttpResponseMessage> TrackEvent ()
		{
			using (var httpClient = new HttpClient ())
			{
				var postData = new List <KeyValuePair <string, string>> ()
				{
					new KeyValuePair <string, string> ("v", googleVersion),
					new KeyValuePair <string, string> ("tid", googleTrackingId),
					new KeyValuePair <string, string> ("cid", googleClientId),
					new KeyValuePair <string, string> ("t", "pageview"),
					new KeyValuePair <string, string> ("dp", "%2Fusages.html"),
				};

				return await httpClient.PostAsync (endpoint, new FormUrlEncodedContent (postData)).ConfigureAwait (false);
			}
		}
	}
	
	public static class DateTimeExtensions {
		public static DateTime ToDateTime(this string s, 
		                                  string      format = "ddMMyyyy", string cultureString = "tr-TR") {
			try {
				var r = DateTime.ParseExact(
					s: s,
					format: format,
					provider: CultureInfo.GetCultureInfo(cultureString));
				return r;
			} catch (FormatException) {
				throw;
			} catch (CultureNotFoundException) {
				throw; // Given Culture is not supported culture
			}
		}

		public static DateTime ToDateTime(this string s, 
		                                  string      format, CultureInfo culture) {
			try {
				var r = DateTime.ParseExact(s: s, format: format, 
				                            provider: culture);
				return r;
			} catch (FormatException) {
				throw;
			} catch (CultureNotFoundException) {
				throw; // Given Culture is not supported culture
			}

		}

	}
	
}