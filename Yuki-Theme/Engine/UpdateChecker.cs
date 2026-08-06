using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class UpdateChecker
{
	private const string USER_AGENT =
		"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36";

	private const string RELEASES_URL = "https://api.github.com/repos/dragon-0609/yuki-theme/releases";
	private const string LATEST_URL = "https://api.github.com/repos/Dragon-0609/Yuki-Theme/releases/latest";
	private const string GITHUB_TAG_URL = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/";
	private const string GITHUB_LATEST_URL = "https://github.com/Dragon-0609/Yuki-Theme/releases/latest";

	public async Task<UpdateInfo> CheckForUpdateAsync()
	{
		try
		{
			var info = new UpdateInfo { IsAvailable = false };

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);

				bool checkBeta = DataSaver.Load(SettingsConst.BETA);
				string url = checkBeta ? RELEASES_URL : LATEST_URL;

				HttpResponseMessage response;
				try
				{
					response = await client.GetAsync(url);
				}
				catch (HttpRequestException e)
				{
					info.GotError = true;
					info.ErrorMessage = e.Message;
					return info;
				}

				if (response == null || !response.IsSuccessStatusCode)
					return info;

				var json = await response.Content.ReadAsStringAsync();

				JObject release;
				if (checkBeta)
					release = (JObject)JArray.Parse(json).First;
				else
					release = JObject.Parse(json);

				var tagName = release["tag_name"]?.ToString();
				if (string.IsNullOrEmpty(tagName))
					return info;

				bool hasBeta = tagName.Contains("-");
				string versionNumber = ParseVersionNumber(tagName, hasBeta);
				double nextVer = double.Parse(versionNumber, CultureInfo.InvariantCulture);

				bool isNewer = SettingsConst.CURRENT_VERSION < nextVer
				               || (Math.Abs(SettingsConst.CURRENT_VERSION - nextVer) < 0.001
				                   && SettingsConst.CURRENT_VERSION_ADD.Length != 0
				                   && !hasBeta);

				if (!isNewer)
					return info;

				var versionName = release["name"]?.ToString() ?? tagName;
				if (versionName.StartsWith("v"))
					versionName = versionName.Substring(1);

				var sizeStr = release["assets"]?[0]?["size"]?.ToString();
				long sizeBytes = 0;
				if (!string.IsNullOrEmpty(sizeStr))
					long.TryParse(sizeStr, out sizeBytes);

				info.IsAvailable = true;
				info.VersionName = versionName;
				info.VersionNumber = nextVer;
				info.IsBeta = hasBeta;
				info.SizeBytes = sizeBytes;
				info.DownloadUrl = release["assets"]?[0]?["browser_download_url"]?.ToString();
				info.GitHubUrl = checkBeta ? GITHUB_TAG_URL + tagName : GITHUB_LATEST_URL;
			}

			return info;
		}
		catch (Exception e)
		{
			string error = e.Message;
			return new UpdateInfo()
			{
				IsAvailable = false,
				GotError = true,
				ErrorMessage = error
			};
		}
	}

	private static string ParseVersionNumber(string tag, bool hasBeta)
	{
		if (hasBeta)
		{
			var prefix = tag.Split('-')[0];
			return prefix.Substring(1, prefix.Length - 3);
		}

		return tag.Substring(1, tag.Length - 3);
	}
}
