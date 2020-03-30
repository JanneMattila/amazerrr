using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Amazerrr.ReleaseTests
{
    [TestClass]
    public class BrowserTests
    {
        private IWebDriver _driver;
        private string _baseUrl;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            var azureDevOpsBinaryLocation = Environment.GetEnvironmentVariable("ChromeWebDriver") ?? ".";
            _driver = new ChromeDriver(azureDevOpsBinaryLocation, options, TimeSpan.FromMinutes(3));
            _baseUrl = TestContext.Properties["baseUrl"].ToString();
        }

        [TestMethod]
        [TestCategory("Release")]
        public void Front_Page_Test()
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


        [TestMethod]
        [TestCategory("Release")]
        public void Solve_Test()
        {
            // Arrange
            var expected = "Solution in 3 moves:\r\nRight\r\nUp\r\nLeft";
            var image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Test1.png";
            _driver.Navigate().GoToUrl($"{_baseUrl}");
            _driver.FindElement(By.Id("puzzle")).SendKeys(image);
            _driver.FindElement(By.Id("solveButton")).Click();
            var solutionElement = _driver.FindElement(By.Id("solution"));

            // Wait solver to solve the puzzle
            for (int i = 0; i < 60; i++)
            {
                if (solutionElement.Text.Length == 0||
                    solutionElement.Text == "Uploading & Solving...")
                {
                    Thread.Sleep(1000);
                    continue;
                }
                break;
            }

            // Act
            var actual = solutionElement.Text;

            // Collect data
            TakeImage();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
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
