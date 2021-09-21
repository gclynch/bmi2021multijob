using Microsoft.VisualStudio.TestTools.UnitTesting;

// NuGet install Selenium WebDriver package and Support Classes
 
using OpenQA.Selenium;

// NuGet install Chrome Driver
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;


// run 2 instances of VS to do run Selenium tests against localhost
// instance 1 : run web app e.g. on IIS Express
// instance 2 : from Test Explorer run Selenium test
// or use the dotnet vstest task
// e.g. dotnet vstest seleniumtest\bin\debug\netcoreapp2.1\seleniumtest.dll /Settings:seleniumtest.runsettings

namespace SeleniumTest
{
    [TestClass]
    public class UnitTest1
    {
        // .runsettings file contains test run parameters
        // e.g. URI for app
        // test context for this run

        private TestContext testContextInstance;

        // test harness uses this property to initliase test context
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        // URI for web app being tested
        private String webAppUri;

        // .runsettings property overriden in vsts test runner 
        // release task to point to run settings file
        // also webAppUri overriden to use pipeline variable

        [TestInitialize]                // run before each unit test
        public void Setup()
        {
            // read URL from SeleniumTest.runsettings
            this.webAppUri = testContextInstance.Properties["webAppUri"].ToString();
            // e.g. "https://gc-bmicalculator-ga.azurewebsites.net";
        }

        [TestMethod]
        public void TestBMIUI()
        {

            String chromeDriverPath = Environment.GetEnvironmentVariable("ChromeWebDriver");
            if (chromeDriverPath is null)
            {
                chromeDriverPath = ".";                 // for IDE
            }
          
            using (IWebDriver driver = new ChromeDriver(chromeDriverPath))
            {
                // any exception below results in a test fail

                // navigate to URI for temperature converter
                // web app running on IIS express
                driver.Navigate().GoToUrl(webAppUri);

                // get weight in stone element
                IWebElement weightInStoneElement = driver.FindElement(By.Id("Bmi_WeightStones"));
                // enter 10 in element
                weightInStoneElement.SendKeys("10");

                // get weight in stone element
                IWebElement weightInPoundsElement = driver.FindElement(By.Id("Bmi_WeightPounds"));
                // enter 10 in element
                weightInPoundsElement.SendKeys("10");

                // get weight in stone element
                IWebElement heightFeetElement = driver.FindElement(By.Id("Bmi_HeightFeet"));
                // enter 10 in element
                heightFeetElement.SendKeys("5");

                // get weight in stone element
                IWebElement heightInchesElement = driver.FindElement(By.Id("Bmi_HeightInches"));
                // enter 10 in element
                heightInchesElement.SendKeys("5");

                // submit the form
                driver.FindElement(By.Id("convertForm")).Submit();

                // explictly wait for result with "BMIValue" item
                IWebElement BMIValueElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(c => c.FindElement(By.Id("bmiVal")));

                // item comes back like "BMIValue: 24.96"
                String bmi = BMIValueElement.Text.ToString();

                StringAssert.EndsWith(bmi, "24.96");

                driver.Quit();
            }
        }
    }
}
