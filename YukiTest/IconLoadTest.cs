using System.IO;
using NUnit.Framework;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTest;

[TestFixture]
public class IconLoadTest
{
	[Test]
	public void TestIcons()
	{
		string[] iconNames = IconAlterer.IconNames;
		foreach (string name in iconNames)
		{
			Stream stream = ResourceHelper.LoadStream(name + ".svg", "Icons.svg.");
			Assert.NotNull(stream);
			stream.Dispose();
		}
	}
}