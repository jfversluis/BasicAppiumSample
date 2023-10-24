using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace UITests.iOS;

[TestFixture]
public class MainPageTests : Shared.MainPageTests<IOSDriver<IOSElement>, IOSElement>
{
    protected override AppiumOptions GetAppiumOptions()
    {
        AppiumOptions optionsToReturn = new()
        {
            // Always iOS for iOS
            PlatformName = "iOS",
        };

        // Specify XCUITest as the driver, typically don't need to change this
        optionsToReturn.AddAdditionalCapability($"appium:{MobileCapabilityType.AutomationName}", "XCUITest");

        // Specific iOS device you want to use. Don't specify if you don't want a specific device
        optionsToReturn.AddAdditionalCapability($"appium:{MobileCapabilityType.DeviceName}", "iPhone 15 Pro");

        // iOS version to use
        optionsToReturn.AddAdditionalCapability($"appium:{MobileCapabilityType.PlatformVersion}", "17.0");

        // The bundle id of the app is already installed on the device
        optionsToReturn.AddAdditionalCapability($"appium:{IOSMobileCapabilityType.BundleId}", "com.companyname.basicappiumsample");

        // Note there are many more options that you can use to influence the app under test according to your needs

        return optionsToReturn;
    }

    protected override IOSDriver<IOSElement> GetDriver() 
        => new (AppiumLocalService, GetAppiumOptions(), TimeSpan.FromSeconds(180));
}