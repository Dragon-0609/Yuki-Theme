using System;
using System.IO;
using NUnit.Framework;
using YukiTheme.Tools;

namespace YukiTest
{
	[TestFixture]
	public class DefaultResourcesTest
	{
		private Tuple<string, string>[] _resources =
		{
			new("background.png", "Default."),
			new("sticker.png", "Default."),
			new("theme.xshd", "Default."),
			new("Template.xshd", "Templates."),
		};

		[Test]
		public void TestResources()
		{
			foreach (var (key, value) in _resources)
			{
				LoadResource(key, value);
			}
		}

		private void LoadResource(string key, string value)
		{
			Stream stream = ResourceHelper.LoadStream(key, value);
			Assert.True(stream != null);
			stream.Dispose();
		}
	}
}