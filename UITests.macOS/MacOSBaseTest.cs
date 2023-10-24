using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Mac;

namespace UITests.macOS;

public abstract class MacOSBaseTest : BaseTest<MacDriver<MacElement>, MacElement>
{
    protected override AppiumOptions GetAppiumOptions()
    {
        throw new NotImplementedException();
    }

    protected override MacDriver<MacElement> GetDriver() 
        => new (AppiumLocalService, GetAppiumOptions(), TimeSpan.FromSeconds(180));
}