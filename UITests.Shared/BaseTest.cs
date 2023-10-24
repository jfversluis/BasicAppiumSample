using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium;
using UITests.Shared;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Mac;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;

[SetUpFixture]
public abstract class BaseTest
{
    protected readonly Uri AppiumServerUri = new("http://127.0.0.1:4723/");
    protected AppiumLocalService? AppiumLocalService;

    // It's a AppiumDriver, but app fits the mental model better
    private AppiumDriver? driver;
    protected AppiumDriver App
    {
        get
        {
            return driver ?? throw new NullReferenceException("AppiumDriver is null");
        }
    }

    protected readonly TargetPlatform platform;

    public BaseTest(TargetPlatform platform)
    {
        this.platform = platform;
    }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new AppiumServiceBuilder()
            .WithIPAddress(AppiumServerUri.Host)
            .UsingPort(AppiumServerUri.Port);

        //builder.WithArguments(new OpenQA.Selenium.Appium.Service.Options.OptionCollector()
            //.AddArguments(new KeyValuePair<string, string>("-–base-path", "/wd/hub")));

        // Start the server with the builder
        AppiumLocalService = builder.Build();
        AppiumLocalService.Start();

        driver = GetDriver(platform, AppiumLocalService);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumLocalService?.Dispose();
    }

    protected AppiumDriver GetDriver(TargetPlatform platform, AppiumLocalService appiumService)
    {
        return platform switch
        {
            TargetPlatform.iOS => new IOSDriver(appiumService, GetOptions(platform), TimeSpan.FromSeconds(180)),
            TargetPlatform.Android => new AndroidDriver(appiumService, GetOptions(platform), TimeSpan.FromSeconds(180)),
            TargetPlatform.macOS => new MacDriver(appiumService, GetOptions(platform), TimeSpan.FromSeconds(180)),
            TargetPlatform.Windows => new WindowsDriver(appiumService, GetOptions(platform), TimeSpan.FromSeconds(180)),
            _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
        };
    }

    protected AppiumOptions GetOptions(TargetPlatform platform)
    {
        return platform switch
        {
            TargetPlatform.iOS => GetiOSOptions(),
            TargetPlatform.Android => GetAndroidOptions(),
            TargetPlatform.macOS => GetMacOptions(),
            TargetPlatform.Windows => GetWindowsOptions(),
            _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
        };
    }

    protected virtual AppiumElement FindUIElement(string automationId)
    {
        return App.FindElement(MobileBy.Id(automationId));
    }

    protected virtual AppiumOptions GetAndroidOptions()
    {
        var androidOptions = new AppiumOptions
        {
            // Specify UIAutomator2 as the driver, typically don't need to change this
            AutomationName = "UIAutomator2",
            // Always Android for Android
            PlatformName = "Android",
            // This is the Android version, not API level
            // This is ignored if you use the avd option below
            PlatformVersion = "13",
            // The full path to the .apk file to test or the package name if the app is already installed on the device
            App = "com.companyname.basicappiumsample",
        };

        // Specifying the avd option will boot the emulator for you
        // make sure there is an emulator with the name below
        // If not specified, make sure you have an emulator booted
        //androidOptions.AddAdditionalAppiumOption("avd", "pixel_5_-_api_33");

        return androidOptions;
    }

    static AppiumOptions GetiOSOptions()
    {
        var iOSOptions = new AppiumOptions
        {
            // Specify XCUITest as the driver, typically don't need to change this
            AutomationName = "XCUITest",
            // Always iOS for iOS
            PlatformName = "iOS",
            // iOS Version
            PlatformVersion = "17.0",
            // Don't specify if you don't want a specific device
            DeviceName = "iPhone 15 Pro",
            // The full path to the .app file to test or the bundle id if the app is already installed on the device
            App = "com.companyname.basicappiumsample",
        };

        //optionsToReturn.AddAdditionalCapability($"appium:{IOSMobileCapabilityType.BundleId}", "com.companyname.basicappiumsample");

        // Note there are many more options that you can use to influence the app under test according to your needs

        return iOSOptions;
    }

    protected virtual AppiumOptions GetMacOptions()
    {
        var macOptions = new AppiumOptions
        {
            // Specify mac2 as the driver, typically don't need to change this
            AutomationName = "mac2",
            // Always Mac for Mac
            PlatformName = "Mac",
            // The full path to the .app file to test
            App = "/Users/jfversluis/Documents/GitHub/BasicAppiumSample/MauiApp/bin/Debug/net7.0-maccatalyst/maccatalyst-x64/BasicAppiumSample.app",
        };

        // Setting the Bundle ID is required, else the automation will run on Finder
        macOptions.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, "com.companyname.basicappiumsample");

        return macOptions;
    }

    protected virtual AppiumOptions GetWindowsOptions()
    {
        var windowsOptions = new AppiumOptions
        {
            // Specify windows as the driver, typically don't need to change this
            AutomationName = "windows",
            // Always Windows for Windows
            PlatformName = "Windows",
            // The identifier of the deployed application to test
            App = "com.companyname.basicappiumsample_9zz4h110yvjzm!App",
        };

        return windowsOptions;
    }
}
