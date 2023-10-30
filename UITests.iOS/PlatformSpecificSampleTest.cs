using Xunit;

namespace UITests;

public class PlatformSpecificSampleTest : BaseTest
{
	[Fact]
	public void SampleTest()
	{
		App.GetScreenshot().SaveAsFile($"{nameof(SampleTest)}.png");
	}
}