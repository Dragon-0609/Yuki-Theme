using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml;
using Svg;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core
{
	public static class Helper
	{
		public static Color bgColor, bgClick, bgBorder, fgColor, fgHover, fgKeyword;

		public static ProductMode  mode;
		public static RelativeUnit unit;

		private static Size Standart32 = new Size (32, 32);
		public static string CustomThemesBegin => Path.Combine (CLI.currentPath, "Themes");

		public static Rectangle GetSizes (Size ima, int mWidth, int mHeight, Alignment align)
		{
			Rectangle res = new Rectangle ();
			double rY = (double) mHeight / ima.Height;
			res.Width = (int) (ima.Width * rY);
			res.Height = (int) (ima.Height * rY);
			res.X = (mWidth - res.Width) / (int) align;

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
			double rY = (double) mWidth / ima.Width;
			res.Width = (int) (ima.Width * rY);
			res.Height = (int) (ima.Height * rY);
			res.Y = (mHeight - res.Height) / 2;

			return res;
		}

		public static string currentTheme;

		#region CONST

		private const string THEME_NAME_OLD      = "theme.xshd";
		private const string THEME_NAME_NEW      = "theme.json";
		private const string WALLPAPER_NAME      = "background.png";
		private const string STICKER_NAME        = "sticker.png";
		public const  string FILE_EXTENSTION_OLD = ".yukitheme";
		public const  string FILE_EXTENSTION_NEW = ".yuki";
		public const  string PASCALTEMPLATE      = "Yuki_Theme.Core.Resources.Syntax_Templates.Pascal.xshd";
		
		#endregion
		
		
		#region Get Image

		public static Tuple <bool, Image> GetImage (string path)
		{
			return GetImage (path, WALLPAPER_NAME);
		}

		public static Tuple <bool, Image> GetSticker (string path)
		{
			return GetImage (path, STICKER_NAME);
		}

		private static Tuple <bool, Image> GetImage (string path, string filename)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					ZipArchiveEntry imag = zipFile.GetEntry (filename);
					if (imag != null)
					{
						Image img = null;
						bool b = false;
						using (Stream stream = imag.Open ())
						{
							img = Image.FromStream (stream);
							b = true;
						}

						return new Tuple <bool, Image> (b, img);
					} else
					{
						return new Tuple <bool, Image> (false, null);
					}
				}
			} catch (InvalidDataException)
			{
				return new Tuple <bool, Image> (false, null);
			}
		}

		public static Tuple <bool, Image> GetStickerFromMemory (string path, Assembly a)
		{
			return GetImageFromMemory (path, STICKER_NAME, a);
		}

		public static Tuple <bool, Image> GetImageFromMemory (string path, Assembly a)
		{
			return GetImageFromMemory (path, WALLPAPER_NAME, a);
		}

		public static Tuple <bool, Image> GetImageFromMemory (string path, string filename, Assembly a)
		{
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					ZipArchiveEntry imag = zipFile.GetEntry (filename);
					if (imag != null)
					{
						Image img = null;
						bool b = false;
						using (Stream stream = imag.Open ())
						{
							img = Image.FromStream (stream);
							b = true;
						}

						return new Tuple <bool, Image> (b, img);
					} else
					{
						return new Tuple <bool, Image> (false, null);
					}
				}
			} catch (InvalidDataException)
			{
				return new Tuple <bool, Image> (false, null);
			}
		}
		
		#endregion

		
		#region Get Theme

		public static Tuple <bool, string> GetTheme (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					if (path.ToLower ().EndsWith (FILE_EXTENSTION_OLD))
						return ReadThemeFromZip (zipFile, THEME_NAME_OLD);
					else
						return ReadThemeFromZip (zipFile, THEME_NAME_NEW);
				}
			} catch (InvalidDataException)
			{
				return new Tuple <bool, string> (false, "");
			}
		}

		public static Tuple <bool, string> GetThemeFromMemory (string path, Assembly a)
		{
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					if (path.ToLower().EndsWith (FILE_EXTENSTION_OLD))
						return ReadThemeFromZip (zipFile, THEME_NAME_OLD);
					else
						return ReadThemeFromZip (zipFile, THEME_NAME_NEW);
				}
			} catch (InvalidDataException)
			{
				return new Tuple <bool, string> (false, "");
			}
		}

		private static Tuple <bool, string> ReadThemeFromZip (ZipArchive zipFile, string themefile)
		{
			var theme = zipFile.GetEntry (themefile);
			if (theme != null)
			{
				string content = "";
				using (StreamReader reader = new StreamReader (theme.Open ()))
				{
					content = reader.ReadToEnd ();
				}

				return new Tuple <bool, string> (true, content);
			} else
			{
				return new Tuple <bool, string> (false, "");
			}
		}

		public static ThemeFormat GetThemeFormat (bool isDefault, string path, string name)
		{
			if (isDefault)
			{
				Assembly assembly;
				string pathHeader;
				IThemeHeader header = DefaultThemes.headers [name];
				assembly = header.Location;
				pathHeader = header.ResourceHeader;
				// Console.WriteLine(pathHeader);
				// Console.WriteLine (path);
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
		
		
		#region Zip

		public static bool IsZip (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					if (path.ToLower().EndsWith (FILE_EXTENSTION_OLD))
						return IsZip (zipFile, THEME_NAME_OLD);
					else
						return IsZip (zipFile, THEME_NAME_NEW);
				}
			} catch (InvalidDataException)
			{
				return false;
			}
		}

		public static bool IsZip (Stream stream)
		{
			try
			{
				using (var zipFile = new ZipArchive (stream, ZipArchiveMode.Read))
				{
					bool res = IsZip (zipFile, THEME_NAME_OLD);
					if (!res)
						res = IsZip (zipFile, THEME_NAME_NEW);
					return res;
				}
			} catch (InvalidDataException)
			{
				return false;
			}
		}

		private static bool IsZip (ZipArchive zipFile, string themefile)
		{
			var theme = zipFile.GetEntry (themefile);
			return true;
			
		}

		public static void UpdateZip (string path, string content, Image img, bool wantToKeepImage = false, Image sticker = null,
		                              bool   wantToKeepSticker = false, string themeName = "", bool asOld = true)
		{
			if (!wantToKeepImage && !wantToKeepSticker && img == null && sticker == null)
			{
				SaveToFile (path, content, asOld);
			} else
			{
				using (var archive = ZipFile.Open (path, ZipArchiveMode.Update))
				{
					if (themeName.Length == 0) themeName = GetThemeSaveName (asOld);
					ZipArchiveEntry entry = archive.GetEntry (themeName);

					entry?.Delete ();
					// To be sure that there's no old theme file
					entry = archive.GetEntry (THEME_NAME_OLD);
					entry?.Delete ();
					
					entry = archive.CreateEntry (themeName, CompressionLevel.Optimal);


					using (StreamWriter writer = new StreamWriter (entry.Open ()))
					{
						writer.Write (content);
					}

					if (!wantToKeepImage)
					{
						entry = archive.GetEntry (WALLPAPER_NAME);

						entry?.Delete ();
						AddImageToZip (archive, img, WALLPAPER_NAME);
					}

					if (!wantToKeepSticker)
					{
						entry = archive.GetEntry (STICKER_NAME);

						entry?.Delete ();
						AddImageToZip (archive, sticker, STICKER_NAME);
					}
				}
			}
		}

		private static void SaveToFile (string path, string content, bool asOld)
		{
			if (File.Exists (path)) File.Delete (path);
			if (asOld)
				SaveToFileOld (path, content);
			else
				SaveToFileNew (path, content);
		}

		private static void SaveToFileOld (string path, string content)
		{
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (content);
			doc.Save (path);
		}

		private static void SaveToFileNew (string path, string content)
		{
			File.WriteAllText (path, content);
		}

		public static void ExtractZip (string source, string destination, bool needImage = false, bool needSticker = false,
		                               bool   needTheme = true)
		{
			using (ZipArchive archive = ZipFile.OpenRead (source))
			{
				if (needTheme)
				{
					ZipArchiveEntry entry = archive.GetEntry (THEME_NAME_OLD);
					if (entry == null)
						entry = archive.GetEntry (THEME_NAME_NEW);
					entry.ExtractToFile (destination, true);
				}

				if (needImage)
					ExtractFile (archive, WALLPAPER_NAME, destination);
				if (needSticker)
					ExtractFile (archive, STICKER_NAME, destination);
			}
		}

		private static void ExtractFile (ZipArchive archive, string filename, string destination)
		{
			try
			{
				ZipArchiveEntry entry = archive.GetEntry (filename);
				entry.ExtractToFile (Path.Combine (Path.GetDirectoryName (destination), filename), true);
			} catch (Exception ex)
			{
				if (ex is ArgumentException || ex is ArgumentNullException || ex is NullReferenceException)
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (CLI.Translate ("messages.file.notexist.withname.param", filename),
						                       CLI.Translate ("messages.file.notexist.withname", filename));
				} else
				{
					throw;
				}
			}
		}

		public static void Zip (string path, string content, Image img, Image sticker = null, string themeName = "", bool asOld = true)
		{
			if (img == null && sticker == null)
			{
				SaveToFile (path, content, asOld);
			} else
			{
				using (var fileStream = new FileStream (path, FileMode.Create))
				{
					using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Create))
					{
						if (themeName.Length == 0) themeName = GetThemeSaveName (asOld);
						ZipArchiveEntry entry = archive.CreateEntry (themeName, CompressionLevel.Optimal);


						using (StreamWriter writer = new StreamWriter (entry.Open ()))
						{
							writer.Write (content);
						}

						AddImageToZip (archive, img, WALLPAPER_NAME);
						AddImageToZip (archive, sticker, STICKER_NAME);
					}
				}
			}
		}

		private static void AddImageToZip (ZipArchive archive, Image img, string filename)
		{
			if (img != null)
			{
				var file = archive.CreateEntry (filename, CompressionLevel.Optimal);
				using (var stream = new MemoryStream ())
				{
					img.Save (stream, ImageFormat.Png);
					using (var entryStream = file.Open ())
					{
						// to keep it as image better to have it as bytes
						var bytes = stream.ToArray ();
						entryStream.Write (bytes, 0, bytes.Length);
					}
				}
			}
		}

		#endregion

		
		#region Color Management

		public static Color ChangeColorBrightness (Color color, float correctionFactor)
		{
			float red = (float) color.R;
			float green = (float) color.G;
			float blue = (float) color.B;

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

			return Color.FromArgb (color.A, (int) red, (int) green, (int) blue);
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
			IntPtr ptr = ((Bitmap) RenderSvg (Standart32, svg, custom, cSize, customColor, clr)).GetHicon ();

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
				svg.Color = new SvgColourServer (fgColor);

			if (!custom)
				return svg.Draw (im.Width, im.Height);
			else
				return svg.Draw (cSize.Width, cSize.Height);
		}

		#endregion
		
		
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
			CLI.load_schemes ();
			string [] files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
			bool sett = false;
			if (files.Length > 0)
			{
				foreach (string s in files)
				{
					string sp = CLI.GetNameOfTheme (s);
					if (CLI.schemes.Contains (sp))
					{
						// Console.WriteLine(nod.Attributes ["name"].Value);
						currentTheme = sp;
						CLI.selectedItem = currentTheme;
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
			if (!Directory.Exists (Path.Combine (CLI.currentPath, "Themes")))
				Directory.CreateDirectory (Path.Combine (CLI.currentPath, "Themes"));
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
	}

	public static class GoogleAnalyticsHelper
	{
		private static readonly string endpoint         = "https://www.google-analytics.com/collect";
		private static readonly string googleVersion    = "1";
		private static readonly string googleTrackingId = "UA-213918512-2";
		private static readonly string googleClientId   = "555";

		/// <summary>
		/// By this method I'll track installs. I won't abuse. I'll track just installs and that's all. You may ask: why?
		/// Well, I'll switch to passive development on 1st April of 2022. By this I mean I'll work on the project rarely.
		/// After passing some months or years, I'll be back. In that moment I'll be glad to know that many people use the app. 
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
}