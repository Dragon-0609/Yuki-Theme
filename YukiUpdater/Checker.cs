using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class UpdateChecker
{
	private const string USER_AGENT = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36";
	private const string STABLE_URL = "https://api.github.com/repos/dragon-0609/yuki-theme/releases";
	private readonly string _githubUrl = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/";
	private bool _isAvailable = false;

	private static void Main()
	{
	}

	public bool IsUpdateAvailable()
	{
		_isAvailable = false;

		CheckVersion().Wait();
		return _isAvailable;
	}

	public async Task CheckVersion()
	{
		// try
		// {
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
			HttpResponseMessage response;
			try
			{
				response = await client.GetAsync(STABLE_URL);
			}
			catch (HttpRequestException)
			{
				response = null;
			}

			if (response != null)
			{
				var json = await response.Content.ReadAsStringAsync();
				JObject jresponse;

				jresponse = (JObject)JArray.Parse(json).First;

				var tg = jresponse["tag_name"].ToString();

				var nextVersion = "";
				var hasBeta = tg.Contains("-");
				nextVersion = ParseVersion(hasBeta, tg);

				var nextVer = double.Parse(nextVersion, CultureInfo.InvariantCulture);
				// _isAvailable = IsNewVersion(nextVer) || (Math.Abs(SettingsConst.CURRENT_VERSION - nextVer) < 1 &&
				// SettingsConst.CURRENT_VERSION_ADD.Length != 0 && !hasBeta);
				if (_isAvailable) PrepareForDownloading(jresponse);
				// afterParsing(available);
			}
		}
	}

	private bool IsNewVersion(double nextVer)
	{
		return false;
		// return SettingsConst.CURRENT_VERSION < nextVer;
	}

	private void PrepareForDownloading(JObject jresponse)
	{
		var _formattedSize = jresponse["assets"]?[0]?["size"]?.ToString();
		_formattedSize = $"{double.Parse(_formattedSize ?? string.Empty) / 1024 / 1024:0.0} MB";
		var _downloadUrl = jresponse["assets"][0]?["browser_download_url"]?.ToString();

		var version = jresponse["name"].ToString();
		if (version.StartsWith("v")) version = version.Substring(1);

		string _approximateSize = jresponse["assets"][0]?["size"]?.ToString();
		_approximateSize = $"{double.Parse(_approximateSize ?? string.Empty) / 1024 / 1024:0.0}";
	}

	private static string ParseVersion(bool hasBeta, string tg)
	{
		string nv;
		if (hasBeta)
		{
			nv = tg.Split('-')[0];
			nv = nv.Substring(1, nv.Length - 3);
		}
		else
		{
			nv = tg.Substring(1, tg.Length - 3);
		}

		return nv;
	}
}