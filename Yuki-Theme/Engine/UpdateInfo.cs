namespace YukiTheme.Engine;

public struct UpdateInfo
{
	public bool IsAvailable;
	public string DownloadUrl;
	public string VersionName;
	public long SizeBytes;
	public double VersionNumber;
	public string FormattedSize => $"{SizeBytes / 1024.0 / 1024.0:0.0} MB";
	public bool IsBeta;

	public string GitHubUrl;
	public bool GotError;
	public string ErrorMessage;

	public override string ToString()
	{
		return
			$"Is update available: {IsAvailable}, Version: {VersionName}, size: {FormattedSize}, has error: {GotError}, ErrorMessage: {ErrorMessage}";
	}
}
