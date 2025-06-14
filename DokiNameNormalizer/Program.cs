using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DokiNameNormalizer
{
	internal class Program
	{
		public static void Main()
		{
			Console.WriteLine(
				"Specify type: 1 - Normalize Themes Name, 2 - Normalize Wallpaper and Sticker path, 3 - Normalize Wallpaper and sticker name");
			string type = Console.ReadLine();
			if (type == "1")
				NormalizeThemeName();
			else if (type == "2")
				NormalizeWallpaperStickerPath();
			else if (type == "3")
				NormalizeWallpaperAndStickerName();
			else
				// ReSharper disable once NotResolvedInText
				throw new ArgumentOutOfRangeException("Type not specified");
		}

		private static void NormalizeThemeName()
		{
			Console.WriteLine("Write path to the directory with theme definitions:");
			string path = Console.ReadLine();

			if (File.Exists(path)) throw new InvalidDataException("You should set directory path, not file path");

			string[] files = Directory.GetFiles(path, "*.master.definition.json", SearchOption.TopDirectoryOnly);

			foreach (string file in files)
			{
				string name = Path.GetFileName(file);

				string newName = char.ToUpper(name[0]) + name.Substring(1).Replace(".master.definition.json", ".json");

				Regex regex = new Regex("(?!\\.\\w*$)\\.");

				MatchCollection matches = regex.Matches(newName);

				foreach (Match match in matches)
				{
					StringBuilder builder = new StringBuilder(newName);

					builder[match.Index] = ' ';
					builder[match.Index + 1] = char.ToUpper(newName[match.Index + 1]);
					newName = builder.ToString();
				}

				newName = Path.Combine(path, newName);

				Console.WriteLine(newName);
				File.Move(file, newName);
			}
		}

		private static void NormalizeWallpaperStickerPath()
		{
			Console.WriteLine("Write path to the directory with wallpapers and stickers:");
			string path = Console.ReadLine();

			string[] files = Directory.GetFiles(path, "background.png", SearchOption.AllDirectories);
			Move(files, "Wallpaper");
			files = Directory.GetFiles(path, "sticker.png", SearchOption.AllDirectories);
			Move(files, "Sticker");
		}

		private static void Move(string[] files, string to)
		{
			foreach (string file in files)
			{
				string toFolder = Path.Combine(Directory.GetParent(file).FullName, to,
					$"{Path.GetDirectoryName(file)} {Path.GetFileName(file)}");
				Console.WriteLine(toFolder);
				File.Move(file, toFolder);
			}
		}

		private static void NormalizeWallpaperAndStickerName()
		{
			Console.WriteLine("Write path to the directory with wallpapers and stickers:");
			string path = Console.ReadLine();

			string[] files = Directory.GetFiles(path, "background.png", SearchOption.AllDirectories);
			NormalizeName(files, " background");
			files = Directory.GetFiles(path, "sticker.png", SearchOption.AllDirectories);
			NormalizeName(files, " sticker");
		}

		private static void NormalizeName(string[] files, string toReplace)
		{
			foreach (string file in files)
			{
				string to = file.Replace(toReplace, "");

				File.Move(file, to);
			}
		}
	}
}