using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium;

namespace UITests;

[SetUpFixture]
public abstract class BaseTest<T, W>
    where T : AppiumDriver<W>
    where W : IWebElement
{
    // private const string androidApkPath = @"C:\Users\joverslu\source\repos\BasicAppiumSample\MauiApp\bin\Debug\net7.0-android\com.companyname.basicappiumsample.apk";
    // private const string macOSAppPath = "/Users/jfversluis/Documents/GitHub/BasicAppiumSample/MauiApp/bin/Debug/net7.0-maccatalyst/maccatalyst-x64/BasicAppiumSample.app";
    // private const string macOSBundleId = "com.companyname.basicappiumsample";
    // private const string windowsAppId = "com.companyname.basicappiumsample_9zz4h110yvjzm!App";

    protected readonly Uri AppiumServerUri = new("http://127.0.0.1:4723/");
    protected AppiumLocalService? AppiumLocalService;

    // It's a AppiumDriver, but app fits the mental model better
    private AppiumDriver<W>? driver;
    protected AppiumDriver<W> App
    {
        get
        {
            return driver ?? throw new NullReferenceException("AppiumDriver is null");
        }
    }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new AppiumServiceBuilder()
            .WithIPAddress(AppiumServerUri.Host)
            .UsingPort(AppiumServerUri.Port);

        builder.WithArguments(new OpenQA.Selenium.Appium.Service.Options.OptionCollector()
            .AddArguments(new KeyValuePair<string, string>("-–base-path", "/wd/hub")));

        // Start the server with the builder
        AppiumLocalService = builder.Build();
        AppiumLocalService.Start();

        driver = GetDriver();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
        AppiumLocalService?.Dispose();
    }

    protected abstract T GetDriver();

    protected abstract AppiumOptions GetAppiumOptions();

    protected virtual W FindUIElement(string automationId)
    {
        return App.FindElement(By.Id(automationId));
    }

    // static AppiumOptions GetMacOptions()
    // {
    //     var macOptions = new AppiumOptions
    //     {
    //         // Specify mac2 as the driver, typically don't need to change this
    //         AutomationName = "mac2",
    //         // Always Mac for Mac
    //         PlatformName = "Mac",
    //         // The full path to the .app file to test
    //         App = macOSAppPath,
    //     };

    //     // Setting the Bundle ID is required, else the automation will run on Finder
    //     macOptions.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, macOSBundleId);

    //     return macOptions;
    // }

    // static AppiumOptions GetWindowsOptions()
    // {
    //     var windowsOptions = new AppiumOptions
    //     {
    //         // Specify windows as the driver, typically don't need to change this
    //         AutomationName = "windows",
    //         // Always Windows for Windows
    //         PlatformName = "Windows",
    //         // The identifier of the deployed application to test
    //         App = windowsAppId,
    //     };

    //     return windowsOptions;
    // }
}
