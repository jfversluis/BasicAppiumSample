using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace UITests.Android;

[TestFixture]
public class MainPageTests : Shared.MainPageTests<AndroidDriver<AndroidElement>, AndroidElement>
{
    protected override AppiumOptions GetAppiumOptions()
    {
        AppiumOptions optionsToReturn = new()
        {
            // Always Android for Android
            PlatformName = "Android",
        };

        // Specify UIAutomator2 as the driver, typically don't need to change this
        optionsToReturn.AddAdditionalCapability(MobileCapabilityType.AutomationName, "UIAutomator2");

        // This is the Android version, not API level
        // This is ignored if you use the avd option below
        optionsToReturn.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13");

        // The full path to the .apk file to test or the package name if the app is already installed on the device
        optionsToReturn.AddAdditionalCapability(MobileCapabilityType.App, "com.companyname.basicappiumsample.apk");

        // Specifying the avd option will boot the emulator for you
        // make sure there is an emulator with the name below
        // If not specified, make sure you have an emulator booted
        optionsToReturn.AddAdditionalCapability(AndroidMobileCapabilityType.Avd, "pixel_5_-_api_33");

        // Note there are many more options that you can use to influence the app under test according to your needs

        return optionsToReturn;
    }

    protected override AndroidDriver<AndroidElement> GetDriver() 
        => new (AppiumLocalService, GetAppiumOptions(), TimeSpan.FromSeconds(180));
}