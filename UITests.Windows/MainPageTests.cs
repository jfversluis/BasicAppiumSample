using NUnit.Framework;
using OpenQA.Selenium.Appium;
using UITests.Shared;

namespace UITests.Windows;

[TestFixture(TargetPlatform.Windows)]
public class MainPageTests : Shared.MainPageTests
{
    public MainPageTests(TargetPlatform platform)
        : base(platform)
    {
    }

    // For Windows apps we need to find UI elements differently
    protected override AppiumElement FindUIElement(string automationId)
    {
        return App.FindElement(MobileBy.AccessibilityId(automationId));
    }
}