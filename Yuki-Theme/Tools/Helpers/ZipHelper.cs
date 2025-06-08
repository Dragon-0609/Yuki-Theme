using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace YukiTheme.Tools
{
	public static class ZipHelper
	{
		public static bool TryGetContent(string path, string fileName, out string content)
		{
			content = "";
			try
			{
				using ZipArchive zipFile = ZipFile.OpenRead(path);

				ZipArchiveEntry theme = zipFile.GetEntry(fileName);

				if (theme != null)
				{
					using StreamReader reader = new StreamReader(theme.Open());
					content = reader.ReadToEnd();
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(
					$"Something went wrong on reading {path} theme file. Make sure it is a valid zip file. \nException: {e.Message}");
			}

			return false;
		}

		public static bool TryGetImage(string path, string fileName, out Stream image, out ZipArchive zip)
		{
			image = null;
			try
			{
				ZipArchive zipFile = ZipFile.OpenRead(path);

				ZipArchiveEntry imageFile = zipFile.GetEntry(fileName);
				zip = zipFile;
				if (imageFile != null)
				{
					image = imageFile.Open();
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(
					$"Something went wrong on reading {path} {fileName} file. Make sure it is a valid zip file. \nException: {e.Message}");
			}

			zip = null;
			return false;
		}
	}
}