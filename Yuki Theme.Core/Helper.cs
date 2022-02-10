using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Svg;

namespace Yuki_Theme.Core
{
	public enum Alignment:int
	{
		Left   = 1000,
		Center = 2,
		Right  = 1
	}

	public enum ProductMode:int
	{
		Program = 0,
		Plugin  = 1,
		CLI  = 2
	}

	public enum RelativeUnit : int
	{
		Pixel = 0,
		Percent = 1
	}
	
	public static class Helper
	{
		public static Color bgColor, bgClick, bgBorder, fgColor, fgHover, fgKeyword;

		public static ProductMode  mode;
		public static RelativeUnit unit;

		private static Size Standart32 = new Size (32, 32);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);
		
		public static Rectangle getSizes (Size ima, int mWidth, int mHeight, Alignment align)
		{
			Rectangle res = new Rectangle ();
			double rY = (double) mHeight / ima.Height;
			res.Width = (int) (ima.Width * rY);
			res.Height = (int) (ima.Height * rY);
			res.X = (mWidth - res.Width) / (int) align;
			
			// If image's drawing rectangle's width is smaller than mWidth.
			
			if (res.Width < mWidth && CLI.autoFitByWidth) 
				res = getSizesHorizontal (ima, mWidth, mHeight);
			
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
		public static Rectangle getSizesHorizontal (Size ima, int mWidth, int mHeight)
		{
			Rectangle res = new Rectangle ();
			double rY = (double) mWidth / ima.Width;
			res.Width = (int) (ima.Width * rY);
			res.Height = (int) (ima.Height * rY);
			res.Y = (mHeight - res.Height) / 2;
			
			return res;
		}

		public static string CurrentTheme;

		public static Action <string> GiveMessage;

		public static Tuple <bool, Image> getImage (string path)
		{
			return getImage (path, "background.png");
		}
		
		public static Tuple <bool, Image> getSticker (string path)
		{
			return getImage (path, "sticker.png");
		}

		private static Tuple <bool, Image> getImage (string path, string filename)
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


		public static Tuple <bool, Image> getStickerFromMemory (string path, Assembly a)
		{
			return getImageFromMemory (path, "sticker.png", a);
		}

		public static Tuple <bool, Image> getImageFromMemory (string path, Assembly a)
		{
			return getImageFromMemory (path, "background.png", a);
		}
		
		public static Tuple <bool, Image> getImageFromMemory (string path, string filename, Assembly a)
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


		public static Tuple <bool, string> getTheme (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					var theme = zipFile.GetEntry ("theme.xshd");
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
			} catch (InvalidDataException)
			{
				return new Tuple <bool, string> (false, "");
			}
		}

		public static Tuple <bool, string> getThemeFromMemory (string path, Assembly a)
		{
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					var theme = zipFile.GetEntry ("theme.xshd");
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
			} catch (InvalidDataException)
			{
				return new Tuple <bool, string> (false, "");
			}
		}
		
		public static bool isZip (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					var theme = zipFile.GetEntry ("theme.xshd");
					return true;
				}
			} catch (InvalidDataException)
			{
				return false;
			}
		}
		
		public static bool isZip (Stream stream)
		{
			try
			{
				using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read))
				{
					var theme = zipFile.GetEntry ("theme.xshd");
					return true;
				}
			} catch (InvalidDataException)
			{
				return false;
			}
		}

		public static void updateZip (string path, string content, Image img, bool wantToKeep = false, Image sticker = null, bool wantToKeepSticker = false)
		{
			if (!wantToKeep && !wantToKeepSticker && img == null && sticker == null)
			{
				saveToFile (path, content);
			} else
			{
				using (var archive = ZipFile.Open (path, ZipArchiveMode.Update))
				{
					ZipArchiveEntry entry = archive.GetEntry ("theme.xshd");

					entry?.Delete ();
					entry = archive.CreateEntry ("theme.xshd", CompressionLevel.Optimal);


					using (StreamWriter writer = new StreamWriter (entry.Open ()))
					{
						writer.Write (content);
					}

					if (!wantToKeep)
					{
						entry = archive.GetEntry ("background.png");

						entry?.Delete ();
						AddImageToZip (archive, img, "background.png");
					}

					if (!wantToKeepSticker)
					{
						entry = archive.GetEntry ("sticker.png");

						entry?.Delete ();
						AddImageToZip (archive, sticker, "sticker.png");
					}
				}
			}
		}

		private static void saveToFile (string path, string content)
		{
			if (File.Exists (path)) File.Delete (path);
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (content);
			doc.Save (path);
		}

		public static void extractZip (string source, string destination, bool needImage = false, bool needSticker = false)
		{
			using (ZipArchive archive = ZipFile.OpenRead(source))
			{
				ZipArchiveEntry entry = archive.GetEntry ("theme.xshd");
				entry.ExtractToFile (destination, true);
				if(needImage)
					extractFile (archive, entry, "background.png", destination);
				if(needSticker)
					extractFile (archive, entry, "sticker.png", destination);
			} 
		}

		private static void extractFile (ZipArchive archive, ZipArchiveEntry entry, string filename, string destination)
		{
			try
			{
				entry = archive.GetEntry (filename);
				entry.ExtractToFile (Path.Combine (Path.GetDirectoryName (destination), filename), true);
			}
			catch (Exception ex)            
			{                
				if (ex is ArgumentException || ex is ArgumentNullException || ex is NullReferenceException)
				{
					if (GiveMessage != null)
						GiveMessage ($"There's no { filename }");
				} else
				{
					throw;
				}
			}
		}
		
		public static void extractZip (Stream stream, string destination)
		{
			using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
			{
				ZipArchiveEntry entry = archive.GetEntry ("theme.xshd");
				entry.ExtractToFile (destination, true);
				entry = archive.GetEntry ("background.png");
				entry.ExtractToFile (Path.Combine (Path.GetDirectoryName (destination), "background.png"), true);
			} 
		}

		public static void zip (string path, string content, Image img, Image sticker = null)
		{
			if (img == null && sticker == null)
			{
				saveToFile (path, content);
			} else
			{
				using (var fileStream = new FileStream (path, FileMode.Create))
				{
					using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Create))
					{

						ZipArchiveEntry entry = archive.CreateEntry ("theme.xshd", CompressionLevel.Optimal);


						using (StreamWriter writer = new StreamWriter (entry.Open ()))
						{
							writer.Write (content);
						}

						AddImageToZip (archive, img, "background.png");
						AddImageToZip (archive, sticker, "sticker.png");
					}
				}
			}
		}

		private static void AddImageToZip (ZipArchive archive, Image img, string filename)
		{
			if(img != null)
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
		
		public static Color ChangeColorBrightness(Color color, float correctionFactor)
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
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}


		public static bool isDark (Color clr)
		{
			bool dark = ((clr.R + clr.G + clr.B) / 3 < 127);
			return dark;
		}

		public static Color DarkerOrLighter (Color clr, float percent = 0)
		{
			if (isDark (clr))
				return Helper.ChangeColorBrightness (clr, percent);
			else
				return Helper.ChangeColorBrightness (clr, -percent);
		}
		
		public static SvgDocument loadsvg (string name, Assembly a, string customName = "Yuki_Theme.Core.Resources.SVG")
		{
			var doc = new XmlDocument ();
			doc.Load (a.GetManifestResourceStream ($"{customName}.{name}.svg"));
			SvgDocument svg = SvgDocument.Open (doc);
			return svg;
		}

		public static void renderSVG (Control im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false, Color clr = default)
		{
			im.BackgroundImage?.Dispose ();

			im.BackgroundImage = renderSVG (im.Size, svg, custom, cSize, customColor, clr);
		}

		public static void renderSVG (ToolStripButton im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false, Color clr = default)
		{
			im.Image?.Dispose ();

			im.Image = renderSVG (im.Size, svg, custom, cSize, customColor, clr);
		}

		public static void renderSVG (Form im, SvgDocument svg, bool custom = false, Size cSize = default,
		                              bool customColor = false, Color clr = default)
		{
			// im.Icon?.Dispose ();
			IntPtr ptr = ((Bitmap) renderSVG (Standart32, svg, custom, cSize, customColor, clr)).GetHicon ();

			im.Icon = Icon.FromHandle (ptr);
			// DestroyIcon (ptr);
		}

		public static void renderSVG (ToolStripMenuItem im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false, Color clr = default)
		{
			im.Image?.Dispose ();

			im.Image = renderSVG (im.Size, svg, custom, cSize, customColor, clr);
		}
		
		public static Image renderSVG (Size im, SvgDocument svg, bool custom = false, Size cSize = default, bool customColor = false, Color clr = default)
		{
			if (customColor)
				svg.Color = new SvgColourServer (clr);
			else
				svg.Color = new SvgColourServer (fgColor);
			
			if(!custom)
				return svg.Draw (im.Width, im.Height);
			else
				return svg.Draw (cSize.Width, cSize.Height);
		}

		public static Image setOpacity (Image image, float opacity)
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
			string [] files = Directory.GetFiles (Path.Combine (CLI.pascalPath, "Highlighting"), "*.xshd");
			bool sett = false;
			if (files.Length > 0)
			{
				foreach (string s in files)
				{
					string sp = CLI.GetNameOfTheme (s);
					if (CLI.schemes.Contains (sp))
					{
						// Console.WriteLine(nod.Attributes ["name"].Value);
						CurrentTheme = sp;
						CLI.selectedItem = CurrentTheme;
						sett = true;
						break;
					}
				}
			}

			if (!sett)
				CurrentTheme = "unknown";
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
	}

	public static class GoogleAnalyticsHelper {
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
		public static async Task<HttpResponseMessage> TrackEvent()
		{
			using (var httpClient = new HttpClient())
			{
				var postData = new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("v", googleVersion),
					new KeyValuePair<string, string>("tid", googleTrackingId),
					new KeyValuePair<string, string>("cid", googleClientId),
					new KeyValuePair<string, string>("t", "pageview"),
					new KeyValuePair<string, string>("dp", "%2Fusages.html"),
				};
				
				return await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(postData)).ConfigureAwait(false);
			}
		}
	
	}
	
}