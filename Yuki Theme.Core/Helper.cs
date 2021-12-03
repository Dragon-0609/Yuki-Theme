using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
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
		Plugin  = 1
	}

	public static class Helper
	{
		public static Color bgColor, bgClick, bgBorder, fgColor, fgHover, fgKeyword;

		public static ProductMode mode;
		
		public static Rectangle getSizes (Size ima, int mWidth, int mHeight, Alignment align)
		{
			Rectangle res = new Rectangle ();
			double rY = (double) mHeight / ima.Height;
			res.Width = (int) (ima.Width * rY);
			res.Height = (int) (ima.Height * rY);
			res.X = (mWidth - res.Width) / (int) align;

			return res;
		}
		

		public static Tuple <bool, Image> getImage (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					ZipArchiveEntry imag = zipFile.GetEntry ("background.png");
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


		public static Tuple <bool, Image> getImageFromMemory (string path, Assembly a)
		{
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					ZipArchiveEntry imag = zipFile.GetEntry ("background.png");
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

		public static void updateZip (string path, string content, Image img, bool wantToKeep = false)
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

					if(!wantToKeep)
					{
						entry = archive.GetEntry ("background.png");

						entry?.Delete ();
						if (img != null)
						{
							var file = archive.CreateEntry ("background.png", CompressionLevel.Optimal);
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
				}
			
		}

		public static void extractZip (string source, string destination)
		{
			using (ZipArchive archive = ZipFile.OpenRead(source))
			{
				ZipArchiveEntry entry = archive.GetEntry ("theme.xshd");
				entry.ExtractToFile (destination, true);
				entry = archive.GetEntry ("background.png");
				entry.ExtractToFile (Path.Combine (Path.GetDirectoryName (destination), "background.png"), true);
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

		public static void zip (string path, string content, Image img)
		{
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
				{

					ZipArchiveEntry entry = archive.CreateEntry ("theme.xshd", CompressionLevel.Optimal);


					using (StreamWriter writer = new StreamWriter (entry.Open ()))
					{
						writer.Write (content);
					}
					
					if(img != null)
					{
						var file = archive.CreateEntry ("background.png", CompressionLevel.Optimal);
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
		
		public static SvgDocument loadsvg (string name, Assembly a)
		{
			var doc = new XmlDocument ();
			doc.Load (a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.SVG.{name}.svg"));
			SvgDocument svg = SvgDocument.Open (doc);
			return svg;
		}

		public static void renderSVG (Control im, SvgDocument svg, bool custom = false, Size cSize = default)
		{
			im.BackgroundImage?.Dispose ();
			
			svg.Color = new SvgColourServer (fgColor);
			if(!custom)
				im.BackgroundImage = svg.Draw (im.Width, im.Height);
			else
				im.BackgroundImage = svg.Draw (cSize.Width, cSize.Height);
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


	}

}