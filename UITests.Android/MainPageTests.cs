using NUnit.Framework;
using UITests.Shared;

namespace UITests.Android;

[TestFixture(TargetPlatform.Android)]
public class MainPageTests : Shared.MainPageTests
{
    public MainPageTests(TargetPlatform platform)
        : base(platform)
    {
    }
}