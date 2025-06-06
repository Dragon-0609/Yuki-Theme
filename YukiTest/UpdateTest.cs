using NUnit.Framework;
using YukiTheme.Engine;

namespace YukiTest;

[TestFixture]
public class UpdateTest
{
	// [Test]
	public void TestUpdate()
	{
		UpdateChecker checker = new UpdateChecker();
		Assert.True(checker.IsUpdateAvailable(testing: true));
	}
}