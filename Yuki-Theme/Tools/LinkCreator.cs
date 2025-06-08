using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public class LinkCreator
{
	private string _folder;

	public bool CheckLinks()
	{
		_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "YukiTheme");

		if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);

		var themeExtension = "theme.xshd";
		var stickerExtension = "sticker.png";
		var backgroundExtension = "background.png";
		CheckFiles();
		if (IsValid(themeExtension, false) && IsValid(backgroundExtension, true) &&
		    IsValid(stickerExtension, true)) return false;

		IDEConsole.Log($"Running utility");
		IDEAlterer.ReleaseImages();
		RunUtility();

		return true;
	}

	private bool IsValid(string nameAndExtension, bool isImage)
	{
		var path = GetHighlighting(nameAndExtension);
		var valid = true;

		if (!IsSymbolic(path)) return false;

		if (isImage)
			try
			{
				Image.FromFile(path).Dispose();
			}
			catch
			{
				valid = false;
			}
		else
			try
			{
				File.ReadAllText(path);
			}
			catch
			{
				valid = false;
			}

		return valid;
	}

	private bool IsSymbolic(string path)
	{
		if (!File.Exists(path)) return false;
		var pathInfo = new FileInfo(path);
		return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
	}

	private void CheckFiles()
	{
		var themeExtension = "theme.xshd";
		var stickerExtension = "sticker.png";
		var backgroundExtension = "background.png";

		var theme = Path.Combine(_folder, themeExtension);
		if (!File.Exists(theme)) CopyDefault(themeExtension, _folder);

		var background = Path.Combine(_folder, backgroundExtension);
		if (!File.Exists(background)) CopyDefault(backgroundExtension, _folder);

		var sticker = Path.Combine(_folder, stickerExtension);
		if (!File.Exists(sticker)) CopyDefault(stickerExtension, _folder);
	}

	internal static void CopyDefault(string file, string folder)
	{
		var resource = $"YukiTheme.Resources.Default.{file}";

		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

		using (var fileStream = File.Create(Path.Combine(folder, file)))
		{
			stream.Seek(0, SeekOrigin.Begin);
			stream.CopyTo(fileStream);
			stream.Close();
		}
	}

	private void RunUtility()
	{
		var psi = new ProcessStartInfo(
			Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "YukiUtility.exe"));
		// psi.CreateNoWindow = true;
		// psi.UseShellExecute = false;
		bool exited = Process.Start(psi).WaitForExit(5000);
		if (!exited)
		{
		}
	}

	private static string GetHighlighting(string file)
	{
		return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "Highlighting", file);
	}
}