using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Mac;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;

[SetUpFixture]
[TestFixture(TargetPlatform.iOS)]
[TestFixture(TargetPlatform.Android)]
[TestFixture(TargetPlatform.Windows)]
[TestFixture(TargetPlatform.macOS)]
public abstract class BaseTest
{
    private const string androidApkPath = @"C:\Users\joverslu\source\repos\BasicAppiumSample\MauiApp\bin\Debug\net7.0-android\com.companyname.basicappiumsample.apk";
    private const string iOSIpaPath = "";
    private const string macOSAppPath = "/Users/jfversluis/Documents/GitHub/BasicAppiumSample/MauiApp/bin/Debug/net7.0-maccatalyst/maccatalyst-x64/BasicAppiumSample.app";
    private const string macOSBundleId = "com.companyname.basicappiumsample";
    private const string windowsAppId = "com.companyname.basicappiumsample_9zz4h110yvjzm!App";

    private readonly Uri appiumServerUri = new("http://127.0.0.1:4723");
    private AppiumLocalService? appiumLocalService;

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
            .WithIPAddress(appiumServerUri.Host)
            .UsingPort(appiumServerUri.Port);

        //Start the server with the builder
        appiumLocalService = builder.Build();
        appiumLocalService.Start();

        driver = GetDriver(platform, appiumLocalService);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        appiumLocalService?.Dispose();
    }

    protected AppiumElement FindUIElement(string id)
    {
        if (OperatingSystem.IsWindows())
        {
            return App.FindElement(MobileBy.AccessibilityId(id));
        }

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            return App.FindElement(MobileBy.Id(id));
        }

        throw new PlatformNotSupportedException();
    }

    static AppiumDriver GetDriver(TargetPlatform platform, AppiumLocalService appiumService)
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

    static AppiumOptions GetOptions(TargetPlatform platform)
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

    static AppiumOptions GetiOSOptions()
    {
        var iOSOptions = new AppiumOptions
        {
            // Specify XCUITest as the driver, typically don't need to change this
            AutomationName = "XCUITest",
            // Always iOS for iOS
            PlatformName = "iOS",
            // iOS Version
            PlatformVersion = "16.4",
            // Don't specify if you don't want a specific device
            DeviceName = "iPhone 14 Pro Max",
            // The full path to the .app file to test or the bundle id if the app is already installed on the device
            App = iOSIpaPath,
        };

        return iOSOptions;
    }

    static AppiumOptions GetAndroidOptions()
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
            App = androidApkPath,
        };

        // Specifying the avd option will boot the emulator for you
        // make sure there is an emulator with the name below
        // If not specified, make sure you have an emulator booted
        //androidOptions.AddAdditionalAppiumOption("avd", "pixel_5_-_api_33");

        return androidOptions;
    }

    static AppiumOptions GetMacOptions()
    {
        var macOptions = new AppiumOptions
        {
            // Specify mac2 as the driver, typically don't need to change this
            AutomationName = "mac2",
            // Always Mac for Mac
            PlatformName = "Mac",
            // The full path to the .app file to test
            App = macOSAppPath,
        };

        // Setting the Bundle ID is required, else the automation will run on Finder
        macOptions.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, macOSBundleId);

        return macOptions;
    }

    static AppiumOptions GetWindowsOptions()
    {
        var windowsOptions = new AppiumOptions
        {
            // Specify windows as the driver, typically don't need to change this
            AutomationName = "windows",
            // Always Windows for Windows
            PlatformName = "Windows",
            // The identifier of the deployed application to test
            App = windowsAppId,
        };

        return windowsOptions;
    }
}
