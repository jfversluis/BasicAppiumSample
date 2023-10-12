using NUnit.Framework;
using OpenQA.Selenium.Appium.Service;

namespace UITests;

[SetUpFixture]
public class SetUpClass
{
    AppiumLocalService? appiumLocalService;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        AppiumServiceBuilder builder = new();
        builder.WithIPAddress("127.0.0.1");
        builder.UsingPort(4723);

        //Start the server with the builder
        appiumLocalService = builder.Build();
        appiumLocalService.Start();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        appiumLocalService?.Dispose();
    }
}

