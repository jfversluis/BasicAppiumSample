using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests.Windows;

public abstract class WindowsBaseTest : BaseTest<WindowsDriver<WindowsElement>, WindowsElement>
{
    // For Windows apps we need to find UI elements differently
    protected override WindowsElement FindUIElement(string automationId)
    {
        return App.FindElement(MobileBy.AccessibilityId(automationId));
    }

    protected override AppiumOptions GetAppiumOptions()
    {
        throw new NotImplementedException();
    }

    protected override WindowsDriver<WindowsElement> GetDriver() 
        => new (AppiumLocalService, GetAppiumOptions(), TimeSpan.FromSeconds(180));
}