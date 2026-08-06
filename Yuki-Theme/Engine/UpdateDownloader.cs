using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YukiTheme.Components.Windows;

namespace YukiTheme.Engine;

public class UpdateDownloader
{
	public static UpdateDownloader Instance;
	
	private CancellationTokenSource _cts;
	private UpdateInfo _updateInfo;
	private bool _downloading;
	private bool _completed;
	private Task _prevDownload;

	private bool _debug;
	private int _debugPercent;

	public event Action OnStarted;
	public event Action<DownloadInfo> OnProgressUpdated;
	public event Action OnCompleted;
	public event Action OnCanceled;
	public event Action<string> OnNetworkError;

	public bool IsCompleted => _completed;

	public bool IsDownloading => _downloading;
	public bool IsDebug => _debug;

	public UpdateDownloader()
	{
		Instance = this;
	}

	public void SetDownloadInfo(UpdateInfo updateInfo)
	{
		_updateInfo = updateInfo;
	}

	public void StartDownload()
	{
		// IDEConsole.Log($"Starting download, was downloading: {_downloading}, isDebug: {_debug}");
		
		if (_downloading)
		{
			if (_debug)
			{
				// IDEConsole.Log($"Update progress bar to {_debugPercent}");
				double mbTotal = _updateInfo.SizeBytes / 1024.0 / 1024.0;
				double mbRead = mbTotal * _debugPercent / 100.0;
				OnProgressUpdated?.Invoke(new DownloadInfo()
				{
					Percent = _debugPercent,
					Downloaded = mbRead,
					Total = _updateInfo.FormattedSize,
				});
			}
			
			return;
		}
		if (_debug)
		{
			_downloading = true;
			OnStarted?.Invoke();
			return;
		}

		_completed = false;
		_downloading = true;
		
		Task download = StartDownloadAsync();
		_prevDownload = download;

		_prevDownload.ContinueWith(_ =>
		{
			_downloading = false;
			if (_prevDownload != null && _prevDownload == download)
				_prevDownload = null;
		});
	}
	
	private async Task StartDownloadAsync()
	{
		_cts = new CancellationTokenSource();
		string dir = UpdateManager.GetUpdateDirectory();
		if (!Directory.Exists(dir))
			Directory.CreateDirectory(dir);

		string dest = UpdateManager.GetUpdatePath();
		string infoDest = UpdateManager.GetUpdateInfoPath();
		CleanupPartialDownload(infoDest);

		// IDEConsole.Log($"Download started");
		OnStarted?.Invoke();


		try
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("User-Agent",
					"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36");

				using (var response = await client.GetAsync(_updateInfo.DownloadUrl, HttpCompletionOption.ResponseHeadersRead, _cts.Token))
				{
					response.EnsureSuccessStatusCode();
					long totalBytes = response.Content.Headers.ContentLength ?? _updateInfo.SizeBytes;

					using (var contentStream = await response.Content.ReadAsStreamAsync())
					using (var fileStream = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
					{
						var buffer = new byte[8192];
						long bytesRead = 0;
						int read;

						while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length, _cts.Token)) > 0)
						{
							await fileStream.WriteAsync(buffer, 0, read, _cts.Token);
							bytesRead += read;

							if (totalBytes > 0)
							{
								int percent = (int)(bytesRead * 100 / totalBytes);
								double mbRead = bytesRead / 1024.0 / 1024.0;
								string totalMb = _updateInfo.FormattedSize;

								OnProgressUpdated?.Invoke(new DownloadInfo()
								{
									Percent = percent,
									Downloaded = mbRead,
									Total = totalMb,
								});
							}
						}
					}
				}
			}

			File.Delete(infoDest);
			File.WriteAllText(infoDest, $"{_updateInfo.VersionNumber};{_updateInfo.IsBeta}");

			_completed = true;
			UpdateManager.Instance.ShowInstallationPopup();

			OnCompleted?.Invoke();
			// DialogResult = true;
		}
		catch (OperationCanceledException)
		{
			CleanupPartialDownload(dest);
			// DialogResult = false;
			// IDEConsole.Log($"Download Canceled");
		}
		catch (HttpRequestException requestException)
		{
			CleanupPartialDownload(dest);
			OnNetworkError?.Invoke($"Network error: {requestException.Message}");
			IDEConsole.Log($"Network error: {requestException.Message}");
			// DialogResult = false;
		}
		catch (Exception exception)
		{
			CleanupPartialDownload(dest);
			OnNetworkError?.Invoke($"Error: {exception.Message}");
			IDEConsole.Log($"Error: {exception.Message}");
			// DialogResult = false;
		}

		_downloading = false;
		// IDEConsole.Log($"Downloading finished");
	}

	private static void CleanupPartialDownload(string path)
	{
		try
		{
			if (File.Exists(path))
				File.Delete(path);
		}
		catch
		{
			// ignored
		}
	}

	public void Cancel()
	{
		_cts?.Cancel();
		_prevDownload = null;
		OnCanceled?.Invoke();

		_downloading = false;
		_completed = false;
	}
	
	public void SetDebugMode(bool debug)
	{
		_debug = debug;
	}
	
	public void DebugStep()
	{
		if (!_debug) return;
		_debugPercent = Math.Min(100, _debugPercent + 10);
		
		double mbTotal = _updateInfo.SizeBytes / 1024.0 / 1024.0;
		double mbRead = mbTotal * _debugPercent / 100.0;
		OnProgressUpdated?.Invoke(new DownloadInfo()
		{
			Percent = _debugPercent,
			Downloaded = mbRead,
			Total = _updateInfo.FormattedSize,
		});
		
		if (_debugPercent >= 100)
		{
			_completed = true;
			_downloading = false;
			// IDEConsole.Log($"Download ended");
		}
	}

	public void DebugComplete()
	{
		if (!_debug) return;
		_debugPercent = 100;
		
		double mbTotal = _updateInfo.SizeBytes / 1024.0 / 1024.0;
		double mbRead = mbTotal * _debugPercent / 100.0;
		OnProgressUpdated?.Invoke(new DownloadInfo()
		{
			Percent = _debugPercent,
			Downloaded = mbRead,
			Total = _updateInfo.FormattedSize,
		});
		
		_completed = true;
		_downloading = false;
		// IDEConsole.Log($"Download completed");
	}

	public void DebugError()
	{
		if (!_debug) return;

		_downloading = false;
		_completed = false;
		OnNetworkError?.Invoke("Network error (debug)");
	}

	public UpdateInfo GetUpdateInfo()
	{
		return _updateInfo;
	}
}

public struct DownloadInfo
{
	public int Percent;
	public double Downloaded;
	public string Total;
}