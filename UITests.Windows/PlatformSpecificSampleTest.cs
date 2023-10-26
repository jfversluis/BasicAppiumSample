using NUnit.Framework;

namespace UITests.Shared;

public class PlatformSpecificSampleTest : BaseTest
{
    [Test]
    public void SampleTest()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(SampleTest)}.png");
    }
}