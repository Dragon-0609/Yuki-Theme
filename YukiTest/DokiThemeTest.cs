using System.Drawing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using YukiTheme.Export;
using YukiTheme.Tools;

namespace YukiTest
{
	[TestFixture]
	public class DokiTest
	{
		[Test]
		public void TestDokiThemeFiles()
		{
			foreach (string theme in DokiThemeNames.Themes)
			{
				LoadDokiTheme(DokiExporter.Normalize(theme));
			}

			Assert.True(true);
		}

		private void LoadDokiTheme(string themeName)
		{
			string content = ResourceHelper.LoadString($"{themeName}.json", "Themes.Doki.");

			Assert.True(!string.IsNullOrWhiteSpace(content));

			JObject json = JObject.Parse(content);
			json.Validate("Json is null");

			string name = DokiKeysObtainer.GetThemeName(json);

			Assert.True(!string.IsNullOrWhiteSpace(name));

			ValidateImage(themeName, "Wallpapers.");
			ValidateImage(themeName, "Stickers.");
		}

		private static void ValidateImage(string themeName, string Namespace)
		{
			Image image = ResourceHelper.LoadImage($"{themeName}.png", Namespace);
			Assert.True(image != null);

			image.Dispose();
		}
	}
}