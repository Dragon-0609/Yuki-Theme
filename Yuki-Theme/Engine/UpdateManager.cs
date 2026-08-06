using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Yuki_Theme.Core.Utils;
using YukiTheme.Components.Windows;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class UpdateManager
{
	public static UpdateManager Instance;
	
	
	private readonly UpdateChecker _checker = new UpdateChecker();

	public UpdateManager()
	{
		Instance = this;
		new UpdateDownloader();
	}
	
	public async Task<UpdateInfo> CheckAsync()
	{
		return await _checker.CheckForUpdateAsync();
	}

	public bool Check(Action<UpdateInfo> onComplete)
	{
		if (UpdateDownloader.Instance.IsDownloading) return false;

		if (IsValidUpdate())
		{
			ShowInstallationPopup();
			
			return false;
		}
		
		Task<UpdateInfo> task = CheckAsync();

		var complete = onComplete;
		task.ContinueWith(result =>
		{
			complete?.Invoke(result.Result);
		}, TaskScheduler.FromCurrentSynchronizationContext());
		
		return true;
	}

	public DownloadWindow CurrentDownloadWindow { get; private set; }

	public DownloadWindow ShowDownloadWindow(UpdateInfo info, System.Windows.Forms.Form owner = null, bool debug = false)
	{
		WpfConverter.ConvertGuiColorsNBrushes();

		if (CurrentDownloadWindow != null)
		{
			CurrentDownloadWindow.Focus();
			return CurrentDownloadWindow;
		}

		if (UpdateDownloader.Instance.IsDownloading)
		{
			info = UpdateDownloader.Instance.GetUpdateInfo();
		}
		
		var window = new DownloadWindow(info, debug);

		if (owner != null)
			window.SetOwner(owner);

		CurrentDownloadWindow = window;
		window.Closed += (s, e) =>
		{
			if (CurrentDownloadWindow == window)
				CurrentDownloadWindow = null;
		};

		window.ResetPosition();
		window.Show();
		DelayedCall.Run(0.02f, window.ResetPosition);
		DelayedCall.Run(0.1f, window.ResetPosition);

		return window;
	}

	public static string GetUpdateDirectory()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Yuki Theme");
	}

	public static string GetUpdatePath()
	{
		return Path.Combine(GetUpdateDirectory(), "yuki_theme.zip");
	}

	public static string GetUpdateInfoPath()
	{
		return Path.Combine(GetUpdateDirectory(), "yuki_theme_info.txt");
	}

	public static bool IsUpdateDownloaded()
	{
		string path = GetUpdatePath();
		if (!File.Exists(path))
			return false;

		long sizeKb = new FileInfo(path).Length / 1024;
		return sizeKb > 1024;
	}

	public static bool IsValidUpdate()
	{
		string path = GetUpdatePath();
		if (!IsUpdateDownloaded())
			return false;

		return ZipHasFile("YukiTheme.dll", path)
		       && ZipHasFile("Newtonsoft.Json.dll", path);
	}

	private static bool ZipHasFile(string fileName, string zipPath)
	{
		try
		{
			using (var archive = ZipFile.OpenRead(zipPath))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(fileName, StringComparison.Ordinal))
						return true;
				}
			}
		}
		catch
		{
			return false;
		}

		return false;
	}

	public void HandleUpdate(UpdateInfo info, bool manual = false, bool debug = false)
	{
		if (info.GotError)
		{
			IDEConsole.Log($"Error on checking update: {info.ErrorMessage}");
			if (manual)
				PopupWindow.ShowWindow("Check failed",
					"Something went wrong while checking update. Make sure internet and github are available", "Ok",
					null, true);
			return;
		}

		if (!info.IsAvailable)
		{
			IDEConsole.Log("No update available");
			if (manual)
				PopupWindow.ShowWindow("Update", "No update available", "Ok", null);
			return;
		}

		PopupWindow.ShowWindow("Update", $"{info.VersionName} is available. Update?", "Yes", () =>
		{
			ShowDownloadWindow(info, IDEAlterer.Instance.Form1, debug);
		}, "No", null);
	}

	public void InstallUpdate()
	{
		if (IsValidUpdate())
		{
			InstallationPreparer preparer = new InstallationPreparer();
			preparer.Prepare(true);
		}
	}
	
	public void OpenDownloadWindow()
	{
		ShowDownloadWindow(UpdateDownloader.Instance.GetUpdateInfo(), IDEAlterer.Instance.Form1, UpdateDownloader.Instance.IsDebug);
	}

	public void ShowInstallationPopup()
	{
		PopupWindow.ShowWindow("Update Downloaded", "Update Downloaded. IDE needs to be closed. Install now? (you can install it later by clicking Check button in Yuki Theme's settings)",
			"Yes", Instance.InstallUpdate, "Not yet", () =>
			{
			}, true);
	}
}
