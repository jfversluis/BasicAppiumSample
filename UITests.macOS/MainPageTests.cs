using NUnit.Framework;
using UITests.Shared;

namespace UITests.macOS;

[TestFixture(TargetPlatform.macOS)]
public class MainPageTests : Shared.MainPageTests
{
    public MainPageTests(TargetPlatform platform)
        : base(platform)
    {
    }
}