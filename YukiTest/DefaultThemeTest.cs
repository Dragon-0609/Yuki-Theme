using NUnit.Framework;
using YukiTheme.Tools;

namespace YukiTest
{
	[TestFixture]
	public class DefaultThemeTest
	{
		[Test]
		public void TestTheme()
		{
			foreach (string theme in DefaultThemeNames.Themes)
			{
				LoadTheme(theme);
			}
		}

		private void LoadTheme(string themeName)
		{
			string content = ResourceHelper.LoadString($"{themeName}.xshd", "Themes.Default.");
			Assert.True(!string.IsNullOrWhiteSpace(content));
		}
	}
}