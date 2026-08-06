using YukiTheme.Components;
using YukiTheme.Components.Windows;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public class DebugTools : MonoBehaviour
{
	private void Start()
	{
		IDEConsole.Log("DebugTools keys — popups: U available, N no-update, E error. " +
		               "Download: O open, S step +10%, C complete, R error, X close.");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			ShowAvailablePopup();
		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			ShowNoUpdatePopup();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			ShowErrorPopup();
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			OpenDownload();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			UpdateDownloader.Instance.DebugStep();
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			UpdateDownloader.Instance.DebugComplete();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			UpdateDownloader.Instance.DebugError();
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			Download?.Close();
		}
	}

	private static DownloadWindow Download => IDEAlterer.UpdateManager.CurrentDownloadWindow;

	private static void ShowAvailablePopup() =>
		IDEAlterer.UpdateManager.HandleUpdate(DebugInfo(), true, true);

	private static void ShowNoUpdatePopup() =>
		IDEAlterer.UpdateManager.HandleUpdate(new UpdateInfo { IsAvailable = false }, true);

	private static void ShowErrorPopup() =>
		IDEAlterer.UpdateManager.HandleUpdate(
			new UpdateInfo { IsAvailable = false, GotError = true, ErrorMessage = "debug network error" }, true);

	private static void OpenDownload() =>
		IDEAlterer.UpdateManager.ShowDownloadWindow(DebugInfo(), IDEAlterer.Instance.Form1, true);

	private static UpdateInfo DebugInfo() => new UpdateInfo
	{
		IsAvailable = true,
		VersionName = "debug",
		SizeBytes = 50L * 1024 * 1024,
		DownloadUrl = "",
		GitHubUrl = ""
	};
}