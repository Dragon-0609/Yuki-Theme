using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components.Windows;

public partial class DownloadWindow : SnapWindow
{

	public DownloadWindow(UpdateInfo updateInfo, bool debug = false)
	{
		InitializeComponent();
		AlignX = AlignmentX.Right;
		AlignY = AlignmentY.Bottom;
		GetColors();
		ProgressText.Text = $"0.0 MB / {updateInfo.FormattedSize}  (0%)";

		UpdateDownloader.Instance.SetDownloadInfo(updateInfo);
		UpdateDownloader.Instance.SetDebugMode(debug);
		
		Loaded += OnLoaded;
		Closing += OnClosing;
	}

	public void GetColors()
	{
		if (WpfColorContainer.Instance == null) return;
		Background = WpfColorContainer.Instance.BackgroundBrush;
		Foreground = WpfColorContainer.Instance.ForegroundBrush;
		Tag = WpfColorContainer.Instance;

		Progress.Background = WpfColorContainer.Instance.BackgroundDefaultBrush;
		Progress.Foreground = WpfColorContainer.Instance.KeywordBrush;

		TitleText.Foreground = WpfColorContainer.Instance.BeginEndBrush;
		Border.BorderBrush = WpfColorContainer.Instance.BorderBrush;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		UpdateDownloader.Instance.OnProgressUpdated += UpdateProgress;
		UpdateDownloader.Instance.OnNetworkError += OnNetworkError;
		UpdateDownloader.Instance.OnCompleted += Close;
		UpdateDownloader.Instance.OnCanceled += Close;
		
		UpdateDownloader.Instance.StartDownload();
	}

	private void OnClosing(object sender, CancelEventArgs e)
	{
		UpdateDownloader.Instance.OnProgressUpdated -= UpdateProgress;
		UpdateDownloader.Instance.OnNetworkError -= OnNetworkError;
		UpdateDownloader.Instance.OnCompleted -= Close;
		UpdateDownloader.Instance.OnCanceled -= Close;
	}

	private void UpdateProgress(DownloadInfo obj)
	{
		Dispatcher.Invoke(() =>
		{
			Progress.Value = obj.Percent;
			ProgressText.Text = $"{obj.Downloaded:0.0} MB / {obj.Total}  ({obj.Percent}%)";
		});
	}

	private void CancelButton_OnClick(object sender, RoutedEventArgs e)
	{
		UpdateDownloader.Instance.Cancel();
	}

	private void CloseButton_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
	
	private void OnNetworkError(string error)
	{
		Dispatcher.Invoke(() =>
		{
			TitleText.Text = "Download failed";
			ProgressText.Text = error;
		});
	}
}
