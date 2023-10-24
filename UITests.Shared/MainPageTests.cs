using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;

namespace UITests.Shared;

public class MainPageTests<T, W> : BaseTest<T, W>
    where T : AppiumDriver<W>
    where W : IWebElement
{
    // Throw exception here, this is implemented by the platform specific class
    protected override AppiumOptions GetAppiumOptions()
    {
        throw new NotImplementedException();
    }

    // Throw exception here, this is implemented by the platform specific class
    protected override T GetDriver()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void AppLaunches()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(AppLaunches)}.png");
    }

    [Test]
    public void ClickCounterTest()
    {
        // Arrange
        // Find elements with the value of the AutomationId property
        var element = FindUIElement("CounterBtn");

        // Act
        element.Click();
        Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

        // Assert
        App.GetScreenshot().SaveAsFile($"{nameof(ClickCounterTest)}.png");
        Assert.That(element.Text, Is.EqualTo("Clicked 1 time"));
    }
}
