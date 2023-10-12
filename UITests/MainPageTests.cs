using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Android;

namespace UITests;

public class MainPageTests
{
    AppiumDriver? driver;

    AppiumOptions? options;

    const TargetPlatform Platform = TargetPlatform.Android;

    [SetUp]
    public void Setup()
    {
        switch (Platform)
        {
            case TargetPlatform.Apple:
                options = GetAppleOptions();

                driver = new IOSDriver(new Uri("http://localhost:4723/"), options,
                    TimeSpan.FromSeconds(180));
                break;
            case TargetPlatform.Android:
                options = GetAndroidOptions();
                driver = new AndroidDriver(new Uri("http://localhost:4723/"), options,
                    TimeSpan.FromSeconds(10));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [Test]
    public void ClickCounterTest()
    {
        var element = driver.FindElement(MobileBy.Id("CounterBtn"));
        element.Click();
        driver.GetScreenshot().SaveAsFile(nameof(ClickCounterTest), ScreenshotImageFormat.Png);
        Assert.That(element.Text, Is.EqualTo("Clicked 1 time"));
    }

    static AppiumOptions GetAppleOptions()
    {
        var appleOptions = new AppiumOptions
        {
            PlatformName = "iOS",
            PlatformVersion = "16.4", // iOS Version
            AutomationName = "XCUITest",
            DeviceName = "iPhone 14 Pro Max", // Don't specify if you don't want a specific device
            App = "/Users/jfversluis/Documents/GitHub/BasicAppiumSample/MauiApp/bin/Debug/net7.0-ios/iossimulator-x64/BasicAppiumSample.app"
        };

        return appleOptions;
    }

    static AppiumOptions GetAndroidOptions()
    {
        var androidOptions = new AppiumOptions
        {
            PlatformName = "Android",
            PlatformVersion = "13", // This is the Android version, not API level
            AutomationName = "uiautomator2",
            App = "/Users/jfversluis/Documents/GitHub/BasicAppiumSample/MauiApp/bin/Debug/net7.0-android/com.companyname.basicappiumsample.apk"
        };

        // Specifying the avd option will boot the emulator for you
        androidOptions.AddAdditionalAppiumOption("avd","pixel_5_-_api_33");

        return androidOptions;
    }

    enum TargetPlatform
    {
        Android,
        Apple
    }
}
