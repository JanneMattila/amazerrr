using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Amazerrr.ReleaseTests
{
    public class BrowserTests
    {
        private IWebDriver _driver;
        private string _baseUrl;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var chromeOptions = new ChromeOptions();
            var azureDevOpsBinaryLocation = Environment.GetEnvironmentVariable("ChromeWebDriver") ?? ".";
            _driver = new ChromeDriver(azureDevOpsBinaryLocation, chromeOptions, TimeSpan.FromMinutes(3));
            _baseUrl = TestContext.Properties["baseUrl"].ToString();
        }

        [TestMethod]
        [TestCategory("Release")]
        public void Home_Index_Route()
        {
            // Arrange
            var expectedTitleContains = "Amazerrr";
            _driver.Navigate().GoToUrl($"{_baseUrl}");

            // Act
            var actualTitle = _driver.Title;

            // Collect data
            TakeImage();

            // Assert
            Assert.IsNotNull(actualTitle);
            Assert.IsTrue(actualTitle.Contains(expectedTitleContains));
        }

        private void TakeImage()
        {
            var name = Guid.NewGuid().ToString("d");
            var fileName = $"{name}.png";
            ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
            TestContext.AddResultFile(fileName);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _driver.Quit();
        }
    }
}
