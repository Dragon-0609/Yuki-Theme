using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Xml;
using Yuki_Theme.Core.API;

namespace Yuki_Theme.Core.Utils
{
	public static class ZipManager
	{
		public static bool IsZip (string path)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					if (path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD))
						return IsZip (zipFile, Helper.THEME_NAME_OLD);
					else
						return IsZip (zipFile, Helper.THEME_NAME_NEW);
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
					bool res = IsZip (zipFile, Helper.THEME_NAME_OLD);
					if (!res)
						res = IsZip (zipFile, Helper.THEME_NAME_NEW);
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
				using (var zipFile = ZipFile.Open (path, ZipArchiveMode.Update))
				{
					if (themeName.Length == 0) themeName = Helper.GetThemeSaveName (asOld);
					ZipArchiveEntry entry = zipFile.GetEntry (themeName);

					entry?.Delete ();
					// To be sure that there's no old theme file
					entry = zipFile.GetEntry (Helper.THEME_NAME_OLD);
					entry?.Delete ();

					entry = zipFile.CreateEntry (themeName, CompressionLevel.Optimal);


					using (StreamWriter writer = new StreamWriter (entry.Open ()))
					{
						writer.Write (content);
					}

					if (!wantToKeepImage)
					{
						entry = zipFile.GetEntry (Helper.WALLPAPER_NAME);

						entry?.Delete ();
						AddImageToZip (zipFile, img, Helper.WALLPAPER_NAME);
					}

					if (!wantToKeepSticker)
					{
						entry = zipFile.GetEntry (Helper.STICKER_NAME);

						entry?.Delete ();
						AddImageToZip (zipFile, sticker, Helper.STICKER_NAME);
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
			using (ZipArchive zipFile = ZipFile.OpenRead (source))
			{
				if (needTheme)
				{
					ZipArchiveEntry entry = zipFile.GetEntry (Helper.THEME_NAME_OLD);
					if (entry == null)
						entry = zipFile.GetEntry (Helper.THEME_NAME_NEW);
					entry.ExtractToFile (destination, true);
				}

				if (needImage) ExtractFile (zipFile, Helper.WALLPAPER_NAME, destination);
				if (needSticker) ExtractFile (zipFile, Helper.STICKER_NAME, destination);
			}
		}

		private static void ExtractFile (ZipArchive zipFile, string filename, string destination)
		{
			try
			{
				ZipArchiveEntry entry = zipFile.GetEntry (filename);
				entry.ExtractToFile (Path.Combine (Path.GetDirectoryName (destination), filename), true);
			} catch (Exception ex)
			{
				if (ex is ArgumentException || ex is ArgumentNullException || ex is NullReferenceException)
				{
					if (API_Events.showError != null)
						API_Events.showError (CentralAPI.Current.Translate ("messages.file.notexist.withname.param", filename),
						                      CentralAPI.Current.Translate ("messages.file.notexist.withname", filename));
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
					using (var zipFile = new ZipArchive (fileStream, ZipArchiveMode.Create))
					{
						if (themeName.Length == 0) themeName = Helper.GetThemeSaveName (asOld);
						ZipArchiveEntry entry = zipFile.CreateEntry (themeName, CompressionLevel.Optimal);


						using (StreamWriter writer = new StreamWriter (entry.Open ()))
						{
							writer.Write (content);
						}

						AddImageToZip (zipFile, img, Helper.WALLPAPER_NAME);
						AddImageToZip (zipFile, sticker, Helper.STICKER_NAME);
					}
				}
			}
		}

		private static void AddImageToZip (ZipArchive zipFile, Image img, string filename)
		{
			if (img != null)
			{
				var file = zipFile.CreateEntry (filename, CompressionLevel.Optimal);
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

		public static bool DoesImageExist (string path, string filename)
		{
			using (ZipArchive zipFile = ZipFile.OpenRead (path))
			{
				foreach (ZipArchiveEntry entry in zipFile.Entries)
				{
					if (entry.FullName.EndsWith(filename, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		public static bool DoesImageExistInMemory (string path, string filename, Assembly a)
		{
			using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
			{
				foreach (ZipArchiveEntry entry in zipFile.Entries)
				{
					if (entry.FullName.EndsWith(filename, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static Image GetImage (string path, string filename)
		{
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					ZipArchiveEntry imag = zipFile.GetEntry (filename);
					if (imag != null)
					{
						Image img = null;
						using (Stream stream = imag.Open ())
						{
							img = Image.FromStream (stream);
						}

						return img;
					} else
					{
						return null;
					}
				}
			} catch (InvalidDataException)
			{
				return null;
			}
		}
		
		public static Image GetImageFromMemory (string path, string filename, Assembly a)
		{
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					ZipArchiveEntry imag = zipFile.GetEntry (filename);
					if (imag != null)
					{
						Image img = null;
						using (Stream stream = imag.Open ())
						{
							img = Image.FromStream (stream);
						}

						return img;
					} else
					{
						return null;
					}
				}
			} catch (InvalidDataException)
			{
				return null;
			}
		}


		#region Get Theme
		
		public static Tuple <bool, string> GetTheme (string path)
		{
			Tuple <bool, string> result;
			try
			{
				using (ZipArchive zipFile = ZipFile.OpenRead (path))
				{
					if (path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD))
					{
						result = ReadThemeFromZip (zipFile, Helper.THEME_NAME_OLD);
					} else
						result = ReadThemeFromZip (zipFile, Helper.THEME_NAME_NEW);
				}
			} catch
			{
				result = new Tuple <bool, string> (false, "");
			}
			return result;
		}
		public static Tuple <bool, string> GetThemeFromMemory (string path, Assembly a)
		{
			Tuple <bool, string> result;
			try
			{
				using (ZipArchive zipFile = new ZipArchive (a.GetManifestResourceStream (path)))
				{
					if (path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD))
					{
						result = ReadThemeFromZip (zipFile, Helper.THEME_NAME_OLD);
					} else
						result = ReadThemeFromZip (zipFile, Helper.THEME_NAME_NEW);
				}
			} catch (InvalidDataException)
			{
				result = new Tuple <bool, string> (false, "");
			}
			return result;
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
		
		#endregion
	}
}