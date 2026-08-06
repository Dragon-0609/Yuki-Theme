using NUnit.Framework;
using YukiTheme.Engine;

namespace YukiTest;

[TestFixture]
public class UpdateTest
{
	// [Test]
	public async System.Threading.Tasks.Task TestUpdate()
	{
		UpdateChecker checker = new UpdateChecker();
		UpdateInfo info = await checker.CheckForUpdateAsync();
		Assert.True(info.IsAvailable);
	}
}