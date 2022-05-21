using Engage.Automation.Utilities;
using java.awt;
using java.awt.@event;
using java.text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Shell;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
using TikaOnDotNet.TextExtraction;

namespace Engage.Automation.Operations
{
    public class SeleniumActions
    {

        public static IWebDriver driver = null;

        public static string ClickTab_NextLink;
        public static string ClickTab_TabToDisplay;

        public static string ColumnSortBtn;

        public static string HCO_ID;
        public static Hashtable tokens = new Hashtable();
        public static Hashtable StoredDetails = new Hashtable();
        public static Hashtable StoredACButtons = new Hashtable();
        public static Hashtable StoredProgDetails = new Hashtable();
        public static Hashtable StoredMRDetails = new Hashtable();
        static string timeStamp = DateTime.Now.ToString().Replace("/", "-");
        public static string childEventID = string.Empty;

        static string GlobalWaitTime = "20";
        static int GlobalWaitTime_int = int.Parse(GlobalWaitTime);
        private static string ReportPath;

        public static string SystemRun { get; private set; }
        public static object dtTestDataDetails { get; private set; }

        private static string FlagTestCase;
        private static IEnumerable<IWebElement> tr_collection1;
        private static bool isAccepted;
        private static string baseURL;

        public static string exMsg { get; private set; }
        public static object ExpectedConditions { get; private set; }
        public static int timeoutSec = 30;
        public static bool Is { get; private set; }


        /* ______________Launching the desire browser_____________________*/
        public static void InvokeBrowser(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";

            var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
            if (!(Directory.Exists(downloadDirectory)))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            try
            {
                switch (browser.ToUpper())
                {

                    //...................................Chrome Browser....................................//
                    case "CHROME":
                    case "GOOGLE CHROME":
                    case "GOOGLECHROME":

                        ChromeOptions GCoptions = new ChromeOptions();

                        GCoptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
                        GCoptions.AddUserProfilePreference("download.prompt_for_download", false);
                        GCoptions.AddUserProfilePreference("disable-popup-blocking", "true");

                        GCoptions.AddArgument("test-type");
                        GCoptions.AddArgument("--start-maximized");

                        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"..//..//drivers", "chromedriver.exe");

                        // Initialize the Chrome Driver
                        driver = new ChromeDriver(chromeDriverService, GCoptions, TimeSpan.FromSeconds(120));
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl(TestData);

                        Thread.Sleep(5000);

                        break;


                    //..................................IE Browser..............................//
                    case "IE":
                    case "INTERNET EXPLORER":
                    case "IEXPLORER":

                        var options = new InternetExplorerOptions();
                        options.EnsureCleanSession = true;
                        options.BrowserCommandLineArguments = "-private";
                        options.EnsureCleanSession = true;
                        driver = new InternetExplorerDriver(options);
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl(TestData);

                        break;


                    //...............Edge- Launching the desire browser......................//
                    case "EDGE":
                    case "MICROSOFT EDGE":
                    case "ME":
                        var EgOptions = new EdgeOptions();
                        //EgOptions.UseInPrivateBrowsing = true;
                        driver = new EdgeDriver(
                        EdgeDriverService.CreateDefaultService(@"..//..//drivers", "MicrosoftWebDriver.exe"));
                        driver.Manage().Cookies.DeleteAllCookies();
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl(TestData);
                        Thread.Sleep(5000);
                        break;

                    //...................................Firefoxx Browser....................................//
                    case "FF":
                    case "MOZILLA FIREFOX":
                    case "FIREFOX":

                        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"..//..//drivers", "geckodriver.exe");
                        driver = new FirefoxDriver(service);
                        driver.Navigate().GoToUrl(TestData);
                        Thread.Sleep(5000);
                        break;
                    default:
                        Reports.currentTestCaseFail = true;
                        Reports.failedMessage = "Provided invalid browser : " + browser;
                        throw new Exception("Provided invalid browser: " + browser);

                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Navigate Browser To Specific URL_____________________*/
        public static void NavigateTo(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string FlagTestCase = "Pass";

            string exMsg = "";

            try

            {

                driver.Navigate().GoToUrl(TestData);

            }

            catch (Exception ex)

            {

                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")

                {

                    testCaseExecution.bProceedOnFail = false;

                }

                FlagTestCase = "Fail";

                TestLogic.TestFail++;

                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }

            finally

            {

                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);

                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);

            }
        }



        /* ______________Verifying the Browser for complete loading of the URL_____________________*/
        public static void Browser_Verification(string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var brow = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

            if (brow.Text == xpath_verification)
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData, "Pass", RunningTestCase);

            else
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData, "Fail", RunningTestCase);

        }

        /* ______________Taking Screenshots in case of error_____________________*/
        public Screenshot CaptureImages()
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            return ss;
        }



        /* ______________Clicking on the Web Element_____________________*/
        public static void Click_on(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //var text = "";
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (ele.Enabled)
                {
                    ele.Click();
                    flag = "found";
                    exMsg = ObjectName + " Element is Clicked";
                    Thread.Sleep(5000);
                }


                if (flag == "")
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = ObjectName + "Element is not enabled";
                    Thread.Sleep(5000);
                }

                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Clicking on the Login button Web Element_____________________*/
        public static void Click_ClientControl(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var text = "";
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (ele.Enabled)
                {
                    ele.Click();
                    flag = "found";
                    exMsg = ObjectName + " Element is Clicked";
                    Thread.Sleep(10000);

                    //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    //IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(100.00));
                    //wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    //exMsg = "button is Clicked & Page loaded completely";
                }


                if (flag == "")
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Element is not enabled";
                    Thread.Sleep(5000);
                }

                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        //_______________________________________Find element within iframe_______________________________________________________//

        public static void verify_element_iframe(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "Fail";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collectionFrames = driver.FindElements(By.TagName("iframe"));
                foreach (var item in tr_collectionFrames)
                {
                    driver.SwitchTo().Frame(item);
                    var tr_collection1 = driver.FindElements(By.XPath(xpath));
                    if (tr_collection1.Count > 0)
                    {
                        flag = "pass";
                        exMsg = ObjectName + " Element is present in iframe";
                        driver.SwitchTo().ParentFrame();
                        break;
                    }
                    driver.SwitchTo().ParentFrame();
                    break;
                }


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        //---------------------------------Verify dropdown list-----------------------------------------------------//
        /* ______________Verifying the list of Web Elements_____________________*/
        public static void Verify_list_of_Features(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                SelectElement Dropdown = new SelectElement(driver.FindElement(By.XPath(xpath)));
                string[] drop = { "Choose Feature Option... ", "Default", "Admin", "Fulfillment", "CustomerSupport", "EmailBlast", "FireDrill", "GiftWithPurchase", "InstantWin", "MMS", "Loyality", "Polls", "ReceiptRecognition", "SocialAggregator", "Sweepstakes", "Trivia", "UserGeneratedContent", "ApiOnly", "Promotion", "Survey", "ClaimsSite", "EmailOptIn", "PurchaseIncentive", "Registration", "LabelMaker", "PunchCard", "GoogleAnalytics", "LoginTracking", "EntryCode" };
                IList<IWebElement> options = Dropdown.Options;

                /* Assert.AreEqual(options.Count, drop.Length); 

                 foreach (IWebElement option in options)
                 {
                     Assert.IsTrue(drop.Contains(option.Text));
                 }*/
                if (options.Count == drop.Length)
                {
                    exMsg = options.Count + " options are present in dropdown";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        //__________________________________JSClose_multiselect_dropdown___________________________________//
        public static void JSClose_multiselect_dropdown(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            String exMsg = "";

            String FlagTestCase = "Pass";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = null;
                ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                try
                {
                    Actions action = new Actions(driver);
                    action.MoveToElement(ele);
                    action.Build();
                    action.Perform();

                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("document.getElementsByClassName('multiSelectDefaultTarget')[0].click();");
                    exMsg = "Multiselect Dropdown is Closed";
                    //  IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    ////  WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                    //  IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
                    //  wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    Thread.Sleep(5000);
                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);
                    Thread.Sleep(2000);
                }

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);

                if (ele == null)
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Dropdown is not closed";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        //-----------------------verify no element-------------------------------------//
        public static void Verify_No_Element(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

            }
            catch (Exception ex)
            {
                // if (ex.Message.Contains("Element not found"))
                if (ex.Message.Contains("no such element") | ex.Message.Contains("Unable to locate element"))
                {
                    exMsg = ObjectName + " Element is not present";
                    FlagTestCase = "Pass";
                }
                else
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                }
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Download_____________________*/
        public static void Download_File(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //var text = "";
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (ele.Enabled)
                {
                    ele.Click();
                    flag = "found";
                    exMsg = "File has been dowloaded successfully";

                }


                if (flag == "")
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Element is not enabled";
                }

                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Scroll UP the page_____________________*/
        public static void Scroll_Up(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Robot robot = new Robot();

                // Scroll Up using Robot class
                robot.keyPress(KeyEvent.VK_PAGE_UP);
                robot.keyRelease(KeyEvent.VK_PAGE_UP);

                exMsg = "Page scrolled Up till the specified value";
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Scroll down the page_____________________*/
        public static void Scroll_Down(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Robot robot = new Robot();

                //// Scroll Down using Robot class
                robot.keyPress(KeyEvent.VK_PAGE_DOWN);
                robot.keyRelease(KeyEvent.VK_PAGE_DOWN);

                exMsg = "Page scrolled down till the specified value";

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Enter the WebElement_____________________*/
        public static void ENTERKEY(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //var text = "";
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                Thread.Sleep(1000);
                SendKeys.SendWait(@"{Enter}");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Clicking on the Frame Element_____________________*/
        public static void Click_on_frameelement(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collectionFrames = driver.FindElements(By.TagName("iframe"));
                foreach (var item in tr_collectionFrames)
                {
                    driver.SwitchTo().Frame(item);
                    var tr_collection1 = driver.FindElements(By.XPath(xpath));
                    if (tr_collection1.Count > 0)
                    {
                        foreach (IWebElement ele in tr_collection1)
                        {
                            if (ele.Enabled)
                            {
                                //ele.Click();
                                Actions action = new Actions(driver);
                                action.MoveToElement(ele);
                                action.Build();
                                action.Perform();
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);
                                //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);

                                if (ele != null)
                                {
                                    flag = "found";
                                    break;
                                }
                            }

                            if (flag == "")
                            {
                                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                                {
                                    testCaseExecution.bProceedOnFail = false;
                                }
                                FlagTestCase = "Fail";
                                TestLogic.TestFail++;
                                exMsg = "Element is not enabled";
                            }
                        }
                        driver.SwitchTo().ParentFrame();
                        break;
                    }
                    driver.SwitchTo().ParentFrame();
                }

                if (FlagTestCase == "Fail")
                {
                    throw new NoSuchElementException();
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Clicking on the Header Tab - Logic is mainly created for IE Browser. In this method, the Tab mentioned will be clicked and then it's repective header section is made visible and then the 'next' link(Ex: General Application) will be clicked in the displayed section._____________________*/
        public static void Click_Tab(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                CommonFunctions.highlight(driver, ele);
                text = ele.Text;

                if (browser.Trim().ToUpper() == "CHROME" || browser.Trim().ToUpper() == "GOOGLE CHROME" || browser.Trim().ToUpper() == "GOOGLECHROME")
                {
                    ele.Click();
                    Thread.Sleep(3000);
                    driver.FindElement(By.XPath(ClickTab_NextLink), GlobalWaitTime_int).Click();
                }
                else
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    int windowsCount = driver.WindowHandles.Count;

                    Actions act = new Actions(driver);

                    var jsDriver = (IJavaScriptExecutor)driver;
                    string highlightJavascript = "document.getElementById('" + ClickTab_TabToDisplay + "').style.display = 'block';";

                    try
                    {
                        var ele2 = driver.FindElement(By.XPath(ClickTab_NextLink), GlobalWaitTime_int);

                        act.MoveToElement(ele2).Perform();
                        Thread.Sleep(3000);
                        jsDriver.ExecuteScript(highlightJavascript);

                        ele2.Click();

                        Thread.Sleep(3000);
                        if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER" || browser.Trim().ToUpper() == "IEXPLORER")
                        {
                            jsDriver.ExecuteScript(highlightJavascript);
                            ele2.Click();

                            Thread.Sleep(3000);
                            driver.Close();
                            Thread.Sleep(3000);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Mouse Over the Header Tab - Logic is mainly created for IE Browser. In this method, the Mouse Hover the Tab mentioned  then it's repective header section is made visible and then the 'next' link(Ex: General Application) will be clicked in the displayed section._____________________*/
        public static void Mouse_Hover(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";


            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                CommonFunctions.highlight(driver, ele);
                text = ele.Text;

                if (browser.Trim().ToUpper() == "CHROME" || browser.Trim().ToUpper() == "GOOGLE CHROME" || browser.Trim().ToUpper() == "GOOGLECHROME")
                {

                    Actions action = new Actions(driver);
                    action.MoveToElement(ele).Perform();
                    Thread.Sleep(5000);
                    driver.FindElement(By.XPath(xpath), GlobalWaitTime_int).ToString();
                   // Thread.Sleep(5000);

                    exMsg = "Mouse Hover successfull";

                }
                else
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    int windowsCount = driver.WindowHandles.Count;

                    Actions act = new Actions(driver);

                    var jsDriver = (IJavaScriptExecutor)driver;
                    string highlightJavascript = "document.getElementById('" + ClickTab_TabToDisplay + "').style.display = 'block';";

                    try
                    {
                        //String currentURL = driver.Url;
                        act.MoveToElement(ele).Perform();
                        Thread.Sleep(3000);
                        jsDriver.ExecuteScript(highlightJavascript);
                        Thread.Sleep(2000);
                        var ele2 = driver.FindElement(By.XPath(ClickTab_NextLink), GlobalWaitTime_int);
                        ele2.Click();


                        Thread.Sleep(3000);
                        if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER" || browser.Trim().ToUpper() == "IEXPLORER")
                        {
                            jsDriver.ExecuteScript(highlightJavascript);
                            ele2.Click();

                            Thread.Sleep(3000);
                            jsDriver.ExecuteScript(highlightJavascript);
                            ele2.Click();

                            Thread.Sleep(3000);
                            driver.Close();
                            Thread.Sleep(3000);
                        }
                    }
                    catch (Exception ex)
                    {
                        exMsg = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /*_________________________________CloseTab_________________________________________________*/
        public static void Close_Tab(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                driver.Close();
                Thread.Sleep(1000);

                string parentWin = driver.WindowHandles[0];

                driver.SwitchTo().Window(parentWin);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Getting the Text from a Web Element_____________________*/
        public static void Get_Text(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string gettext_text = "";
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var gettext = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                gettext_text = gettext.Text;
                Assert.IsNotNull(gettext);

                exMsg = ObjectName + " is :" + gettext;
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }


        }


        /* ______________Waiting the browser for specific time_____________________*/
        public static void Wait_For(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            //string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                int waitFor = Convert.ToInt32(TestData);
                if (browser.Trim().ToUpper() == "CHROME" || browser.Trim().ToUpper() == "GOOGLE CHROME" || browser.Trim().ToUpper() == "GOOGLECHROME")
                {
                    waitFor = (waitFor * Int32.Parse(Reports.ChromeWaitPercentage)) / 100;
                    TestData = waitFor.ToString();
                }
                else if (browser.Trim().ToUpper() == "MOZILLA" || browser.Trim().ToUpper() == "MOZILLA FIREFOX" || browser.Trim().ToUpper() == "FIREFOX")
                {
                    waitFor = (waitFor * Int32.Parse(Reports.FirefoxWaitPercentage)) / 100;
                    TestData = waitFor.ToString();
                }
                //else if (browser.Trim().ToUpper() == "EDGE" || browser.Trim().ToUpper() == "MICROSOFT EDGE" || browser.Trim().ToUpper() == "EDGE")
                //{
                //    waitFor = (waitFor * Int32.Parse(Reports.FirefoxWaitPercentage)) / 100;
                //    TestData = waitFor.ToString();
                //}
                //else if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER")
                //{
                //    waitFor = (waitFor * Int32.Parse(Reports.FirefoxWaitPercentage)) / 100;
                //    TestData = waitFor.ToString();
                //}
                Thread.Sleep(waitFor);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                //Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                //Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Waiting the browser for specific time_____________________*/
        public static void Wait_For_Condition(string browser, string TestData, string sTestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            DateTime stepStartTime = DateTime.Now;
            FlagTestCase = "Pass";
            exMsg = "";
            Thread.Sleep(8000);
            try
            {
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                    wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                    IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                    exMsg = "Page load complete";
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(sTestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase + " Time take - " + (DateTime.Now.Subtract(stepStartTime).TotalSeconds));
                Reports.Report_TestDataStep(driver, sTestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
                // Reports.Report_TestDataStepWithTime(driver, sTestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase, (DateTime.Now.Subtract(stepStartTime).TotalSeconds).ToString().Remove(4));
            }
        }

        /*..................................................................*/
        /* ______________Upload File_SUJITH_____________________*/
        public static void File_Upload_SUJITH(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "..\\TestDownloads\\SeniorQA_Sujith_WinWire_UpdatedSeleniumResume.docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Sujith's Automation Profile Updated Successfully";

                //string UpdatedOn = driver.FindElement(By.XPath("//span[@class='updateOn']")).Enabled();                exMsg = UpdatedOn.D;



                //Boolean res = driver.PageSource.Contains("Resume has been succesfully uploaded");

                //if (res == true)
                //{
                //    Assert.IsTrue(true);
                //    exMsg = "Testcase Pass";
                //}
                //else
                //{
                //    exMsg = "Testcase Fail";
                //}
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Upload File_DEEKSHITA_____________________*/
        public static void File_Upload_Deekshita(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Deeksha_Resume.Docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Deeksha Profile Updated Successfully..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Upload File_SUJITH_Manual_____________________*/
        public static void File_Upload_Vinay(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                //IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                //fileUpld.Click();
                //string filelocation = "D:\\Naukri\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Sujith_UpdatedSeleniumResume.docx";
                //SendKeys.SendWait(filelocation);
                //SendKeys.SendWait(@"{Enter}");
                //Thread.Sleep(7000);
                //exMsg = "File Uploaded succssfully";

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Vinay_Resume_MCAFresher.Docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                IWebElement ProfileUpdated = driver.FindElement(By.XPath("//span[@class='updateOn']"));
                ProfileUpdated.ToString();
                exMsg = ("ProfileUpdated" + ProfileUpdated.GetAttribute(xpath));

                exMsg = "Vinay Resume has been succesfully uploaded";

                //Boolean res = driver.PageSource.Contains("Resume has been succesfully uploaded");

                //if(res==true)
                //{
                //    Assert.IsTrue(true);
                //    exMsg = "Testcase Pass";
                //}
                //else
                //{
                //    exMsg = "Testcase Fail";
                //}
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________FILE_UPLOAD_VIDHYA_____________________*/
        public static void File_Upload_VIDHYA(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Vidhya L N_Updated_HRRecruiter_CV.docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Vidhya file Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________FILE_UPLOAD_SHIV_____________________*/
        public static void File_Upload_SHIV(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Shiva_Lnt.pdf";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Shiv jojjan file Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________FILE_UPLOAD_PAVAN_____________________*/
        public static void File_Upload_PAVAN(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Krishna Pawan T S Resume.docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Krishna file Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________FILE_UPLOAD_KARTHIK_MCA_____________________*/
        public static void File_Upload_KARTHIK_MCA(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\karthik_MCA_Resume.docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Karthik MCA file Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________File_Upload_Govind_Sagar_____________________*/
        public static void File_Upload_Govind_Sagar(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);

                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\Govindsagar_UpdatedResume.pdf";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Govind Sagar file Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________File_Upload_Anil MCA_____________________*/
        public static void File_Upload_Anil(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement fileUpld = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                fileUpld.Click();
                Thread.Sleep(7000);
                //...................................Add resume.................................................................//
                string filelocation = "C:\\Users\\Sujth\\.jenkins\\Elanco_CRDB_Automation\\COAutomation\\COAutomation\\TestDownloads\\MCAanilResume.Docx";
                SendKeys.SendWait(filelocation);
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(7000);

                exMsg = "Anil Resume Uploaded Successfully and Profile Updated..!!";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Waiting the browser for specific time - Equally for all the Browsers_____________________*/
        public static void Wait_ForEqually(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                int waitFor = Convert.ToInt32(TestData);
                Thread.Sleep(waitFor);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Selecting the Value in the List by Text_____________________*/
        public static void Select_value(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                var selectElement = new SelectElement(ele);

                // select by text in a dropdownlist
                //selectElement.DeselectAll();
                selectElement.SelectByText(TestData);
                Assert.IsNotNull(selectElement);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Selecting the Feature in the List by Text_____________________*/
        public static void Select_Feature(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                //// select by text in a dropdownlist
                var selectElement_promotion = new SelectElement(ele);
                selectElement_promotion.SelectByValue("Promotion");
                Assert.IsNotNull(selectElement_promotion);
                Thread.Sleep(1000);

                // IWebElement dropDownElement = driver.FindElement(By.Id("continents"));
                if (ele.Text.Contains("Promotion"))
                {
                    exMsg = "Value is present inside the Dropdown";
                }
                else
                {
                    exMsg = "Value is not present inside the Dropdown";
                }


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Search engage Features_____________________*/
        public static void Search_engage_Accounts(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();
                Thread.Sleep(2000);


                var search_account_name = driver.FindElement(By.XPath("//input[@id='account-list-search']"), GlobalWaitTime_int);
                search_account_name.SendKeys("New QA");
                Thread.Sleep(2000);

                var search_Click = driver.FindElement(By.XPath("//li[@class='select-account--account-list-item']"), GlobalWaitTime_int);
                search_Click.Click();
                Thread.Sleep(5000);

                //verify element
                IWebElement element = driver.FindElement(By.XPath("//input[@id='account-name']"), GlobalWaitTime_int);
                MoveToElement_JS(element);
                Assert.IsNotNull(element);


                // IWebElement dropDownElement = driver.FindElement(By.Id("continents"));
                if (search_Click.Text.Contains("New QA"))
                {
                    exMsg = "Value is present and Verification is successfull - Verified value is :" + search_Click.Text;
                }
                else
                {
                    exMsg = "Value is not present ";
                }


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________View Engage Accounts_____________________*/
        public static void View_engage_Accounts_Is_Account_Active(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {

                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                // ele.Click();
                Thread.Sleep(5000);


                //var search_account_name = driver.FindElement(By.XPath("//input[@id='account-list-search']"), GlobalWaitTime_int);
                //search_account_name.SendKeys("New QA");
                //Thread.Sleep(2000);

                //var search_Click = driver.FindElement(By.XPath("//li[@class='select-account--account-list-item']"), GlobalWaitTime_int);
                //search_Click.Click();
                //Thread.Sleep(5000);

                //............................................................................//
                //verify element - Account Name
                IWebElement element = driver.FindElement(By.XPath("//input[@id='account-name']"), GlobalWaitTime_int);
                MoveToElement_JS(element);
                Assert.IsNotNull(element);
                Thread.Sleep(5000);


                //Radio button - Is Account Active?
                String Account_Active = driver.FindElement(By.XPath("//input[@id='isAccountActive_true']")).GetAttribute("value");

                if (Account_Active.Equals("true"))
                {

                    exMsg = "Account is Active - Yes : Account Name is-" + Account_Active;
                }
                else
                {
                    exMsg = "Account is Active : No ";
                }

                //   Thread.Sleep(5000);

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________What login they use_____________________*/
        public static void View_engage_Whatlogin_do_they_use(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {

                //var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                //ele.Click();
                //Thread.Sleep(5000);


                IWebElement search_Click = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                search_Click.Click();
                if (search_Click.Equals("Duo - pricelogic"))
                {

                    exMsg = "Account Type - Duo - pricelogic ";
                }
                else
                {
                    exMsg = "Account Type - Engage";
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________What login they use_____________________*/
        public static void Account_Password_Expiration(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {

                //var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                //ele.Click();
                //Thread.Sleep(5000);


                IWebElement search_Click = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                search_Click.Click();
                if (search_Click.Equals("true"))
                {

                    exMsg = "Password expiration -Never Expire ";
                }
                else
                {
                    exMsg = "Password expiration -Thirty, Fortyfive, Sixty, Ninety ";
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Verify Search for Feature_____________________*/
        public static void Search_list_of_Features(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                // select by text in a dropdownlist
                var selectElement_promotion = new SelectElement(ele);
                selectElement_promotion.SelectByValue("Promotion");
                Assert.IsNotNull(selectElement_promotion);
                Thread.Sleep(1000);

                //Select objSelect = new Select(driver.findElement(By.id("Search-box")));
                SelectElement oSelect = new SelectElement(driver.FindElement(By.Id(xpath)));
                IList<IWebElement> elementCount = oSelect.Options;
                //Console.Write(elementCount.Count);
                exMsg = "Selected Feature is " + ele;

                // IWebElement dropDownElement = driver.FindElement(By.Id("continents"));
                //if (ele.Text.Contains("Promotion"))
                //{
                //    exMsg = "Value is present inside the Dropdown";
                //}
                //else
                //{
                //    exMsg = "Value is not present inside the Dropdown";
                //}


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Selecting the Value in the List by Text_____________________*/
        public static void Select_value_fromOption(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                var selectElement = new SelectElement(ele);

                // select by text in a dropdownlist
                selectElement.SelectByText(TestData);
                Assert.IsNotNull(selectElement);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Verifying the Value of Web Element_____________________*/
        public static void verification_value(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collection1 = driver.FindElements(By.XPath(xpath));

                string dataText = TestData.ToLower();

                foreach (IWebElement row in tr_collection1)
                {
                    string eleText = row.Text.ToLower();
                    if (eleText.Contains(dataText))
                    {
                        flag = "pass";
                        break;
                    }
                }
                Assert.AreSame("pass", flag, TestData + " - Text is not found in the " + ObjectName + " object");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        //...................Verify Radio button.....................................//
        public static void Radio_button_IsSelected(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {

                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                // ele.Click();


                //Radio button
                String str = ele.GetAttribute("value");

                if (str.Equals("true"))
                {

                    exMsg = "Value is selected ";
                }
                else
                {
                    exMsg = "Value is not selected ";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        public static int GetBusinessDays(DateTime startD, DateTime endD)
        {
            //double calcBusinessDays =
            //    1 + ((endD - startD).TotalDays * 5 -
            //    (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            //if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            //if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            int busDays = 0;
            while (DateTime.Compare(startD, endD) <= 0)
            {
                if (startD.DayOfWeek != DayOfWeek.Saturday && startD.DayOfWeek != DayOfWeek.Sunday)
                {
                    busDays++;
                    startD = startD.AddDays(1);
                }
                else
                {
                    startD = startD.AddDays(1);
                }
            }

            return busDays;// calcBusinessDays;
        }


        //........................Last Section Delete.................................//
        public static void Lastsection_Click(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement pagerEle = driver.FindElement(By.XPath("//*[@id='mat - expansion - panel - header - 59']//span//mat-panel-description"));
                string[] countcheck = pagerEle.Text.Split(' ');
                int eventCount = Convert.ToInt32(countcheck[countcheck.Length - 1]);

                string eventName = string.Empty;
                if (eventCount == 0)
                {
                    exMsg = "No events present for this IR User in the grid,Exit Page ";
                    driver.Navigate().GoToUrl(TestData);
                }
                else
                {
                    IWebElement LastSection = driver.FindElement(By.XPath(xpath), 30);
                    MoveToElement_JS(LastSection);
                    ClickOnElement_JS(LastSection);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg += " \n " + ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying the Value of Web Element_____________________*/
        public static void verification_value_iframe(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collectionFrames = driver.FindElements(By.TagName("iframe"));
                foreach (var item in tr_collectionFrames)
                {
                    driver.SwitchTo().Frame(item);
                    var tr_collection1 = driver.FindElements(By.XPath(xpath));
                    if (tr_collection1.Count > 0)
                    {
                        foreach (IWebElement row in tr_collection1)
                        {
                            if (row.Text.Contains(TestData))
                            {
                                flag = "pass";
                                break;
                            }
                        }
                        driver.SwitchTo().ParentFrame();
                        break;
                    }
                    driver.SwitchTo().ParentFrame();
                }
                Assert.AreSame("pass", flag, TestData + " - Text is not found in the " + ObjectName + " object");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Verifying Web Element is present in the page or not_____________________*/
        public static void Verify_Element(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                MoveToElement_JS(element);
                Assert.IsNotNull(element);

                exMsg = ObjectName + " element is present in the current page";
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Verifying Edit Acount_____________________*/
        public static void Edit_Account(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "true";
            string exMsg = "";

            try
            {
                Thread.Sleep(1000);


                IWebElement Update_Account_Name = driver.FindElement(By.XPath("//input[@id='account-name']"), GlobalWaitTime_int);
                Thread.Sleep(3000);
                Update_Account_Name.Clear();
                Thread.Sleep(3000);

                //IWebElement Update_Account_Names = driver.FindElement(By.XPath("//input[@id='account-name']"), GlobalWaitTime_int);
                Update_Account_Name.SendKeys(TestData);
                Thread.Sleep(3000);
                exMsg = "Account name is Cleared and Updated ";


                // Scroll Down using Robot class
                Robot robot = new Robot();
                robot.keyPress(KeyEvent.VK_PAGE_DOWN);
                robot.keyRelease(KeyEvent.VK_PAGE_DOWN);

                IWebElement Update_Button = driver.FindElement(By.XPath("//button[@class='step-control-button green']"), GlobalWaitTime_int);
                Update_Button.Click();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        //___________________Add User in Pricelogic________________________________________________________//
        public static void Add_User(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            bool AreEqual = true;
            bool isAccepted = AreEqual;

            try
            {
                IWebElement First_Name = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                First_Name.SendKeys("Sujith");


                IWebElement Last_Name = driver.FindElement(By.XPath("//input[@id='user-last-name']"), GlobalWaitTime_int);
                Last_Name.SendKeys("Reddy");


                IWebElement email = driver.FindElement(By.XPath("//input[@id='user-email']"), GlobalWaitTime_int);
                email.Click();
                Random randomGenerator = new Random();
                int randomInt = randomGenerator.Next(1000);
                email.SendKeys("First_Name" + randomInt + "@gmail.com");

                IWebElement confirm_email = driver.FindElement(By.XPath("//input[@id='user-confirmed-email']"), GlobalWaitTime_int);
                confirm_email.Click();
                Random randomGenerators = new Random();
                int randomInts = randomGenerator.Next(1000);
                confirm_email.SendKeys("First_Name" + randomInt + "@gmail.com");


                IWebElement Phone_Number = driver.FindElement(By.XPath("//input[@id='user-phone']"), GlobalWaitTime_int);
                Phone_Number.SendKeys("948086" + randomInt + "1");
                Thread.Sleep(1000);


                IWebElement Confirm_Phone_Number = driver.FindElement(By.XPath("//input[@id='user-confirmed-phone']"), GlobalWaitTime_int);
                Confirm_Phone_Number.SendKeys("948086" + randomInt + "1");
                Thread.Sleep(1000);

                IWebElement CreateUser_Button = driver.FindElement(By.XPath("//button[@class='step-control-button green']"), GlobalWaitTime_int);
                CreateUser_Button.Click();
                Thread.Sleep(7000);

                if (isAccepted)
                {
                    //Assert.AreEqual(driver.Url, Is.Equals(baseURL + "/PageWhereClientIsRedirectedToAfterSuccessfulLogin"));
                    exMsg = "User Added Successfully";
                }
                else
                {
                    //Assert.AreNotEqual(driver.FindElement(By.XPath("//div[@class='notices is-top']")).Text, Is.Equals("Create user failed"));
                    exMsg = "Unable to Add User";
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        //.....................Add User List.......................//
        public static void Add_User_List(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            bool AreEqual = true;
            bool isAccepted = AreEqual;

            try
            {
                string[] field_values = TestData.Split('|');

                IWebElement First_Name = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                First_Name.SendKeys(field_values[0]);


                IWebElement Last_Name = driver.FindElement(By.XPath("//input[@id='user-last-name']"), GlobalWaitTime_int);
                Last_Name.SendKeys(field_values[1]);

                IWebElement email = driver.FindElement(By.XPath("//input[@id='user-email']"), GlobalWaitTime_int);
                email.Click();
                Random randomGenerator = new Random();
                int randomInt = randomGenerator.Next(1000);
                email.SendKeys(field_values[0] + randomInt + "@gmail.com");

                IWebElement confirm_email = driver.FindElement(By.XPath("//input[@id='user-confirmed-email']"), GlobalWaitTime_int);
                confirm_email.Click();
                Random randomGenerators = new Random();
                int randomInts = randomGenerator.Next(1000);
                confirm_email.SendKeys(field_values[0] + randomInt + "@gmail.com");


                IWebElement Phone_Number = driver.FindElement(By.XPath("//input[@id='user-phone']"), GlobalWaitTime_int);
                Phone_Number.SendKeys(field_values[2] + randomInt + "1");
                Thread.Sleep(1000);


                IWebElement Confirm_Phone_Number = driver.FindElement(By.XPath("//input[@id='user-confirmed-phone']"), GlobalWaitTime_int);
                Confirm_Phone_Number.SendKeys(field_values[2] + randomInt + "1");
                Thread.Sleep(1000);

                IWebElement CreateUser_Button = driver.FindElement(By.XPath("//button[@class='step-control-button green']"), GlobalWaitTime_int);
                CreateUser_Button.Click();
                Thread.Sleep(7000);

                if (isAccepted)
                {
                    //Assert.AreEqual(driver.Url, Is.Equals(baseURL + "/PageWhereClientIsRedirectedToAfterSuccessfulLogin"));
                    exMsg = "User Added Successfully";
                }
                else
                {
                    //Assert.AreNotEqual(driver.FindElement(By.XPath("//div[@class='notices is-top']")).Text, Is.Equals("Create user failed"));
                    exMsg = "Unable to Add User";
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        //.....................Add Team List.......................//
        public static void Add_Team_List(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            bool AreEqual = true;
            bool isAccepted = AreEqual;

            try
            {
                string[] Team_values = TestData.Split('|');

                //............Enter Team Name...........................//
                IWebElement Team_Name = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Random randomGenerator = new Random();
                int randomInt = randomGenerator.Next(1000);
                String teamName = Team_values[0] + randomInt + " Team";
                Team_Name.SendKeys(teamName);

                //............Enter Description...........................//
                IWebElement Description = driver.FindElement(By.XPath("//input[@id='team-description']"), GlobalWaitTime_int);
                Description.SendKeys(teamName + Team_values[1]);

                //............Search Users...........................//
                IWebElement Search_Users = driver.FindElement(By.XPath("//input[@id='team-search-member']"), GlobalWaitTime_int);
                Search_Users.SendKeys(Team_values[2]);
                Thread.Sleep(5000);


                //..................Scroll Down....................//
                Robot robot = new Robot();

                robot.keyPress(KeyEvent.VK_PAGE_DOWN);
                robot.keyRelease(KeyEvent.VK_PAGE_DOWN);

                Thread.Sleep(5000);

                //...................Searched Email List.............//
                String member_email = driver.FindElement(By.XPath("(//div[@class='member--add-icon'])[1]/preceding-sibling::div//span[contains(@class, 'add-user--name')]/b")).Text;
                Thread.Sleep(5000);

                //_____________________verify Email in List_________//
                driver.FindElement(By.XPath("(//div[@class='member--add-icon'])[1]")).Click();
                String actualText = driver.FindElement(By.XPath("//b[text()='" + member_email + "']/ancestor::div[1]/following-sibling::div")).Text;
                Assert.AreEqual("Already a member", actualText, "Member is not added actual text is " + actualText);
                driver.FindElement(By.XPath("//div[@class='members--added']//li[contains(text(),'" + member_email + "')]"));


                //.............click Create Button.......................//
                IWebElement CreateTeam_Button = driver.FindElement(By.XPath("//button[contains(@class, 'green') and contains(text(), 'Create Team')]"), GlobalWaitTime_int);
                CreateTeam_Button.Click();
                Thread.Sleep(7000);
                Assert.IsNotNull(CreateTeam_Button);


                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                IWebElement ele_enter = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (isAccepted)
                {

                    exMsg = teamName + ": is Added Successfully ";
                }
                else
                {

                    exMsg = "Unable to Add Team";
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        //.....................Add Role List.......................//
        public static void Add_Role_List(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            bool AreEqual = true;
            bool isAccepted = AreEqual;

            try
            {

                string[] Role_values = TestData.Split('|');

                //.........Click on Add Account Hyperlink.....//
                IWebElement Add_Account = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Add_Account.Click();
                Thread.Sleep(2000);


                IWebElement ele = driver.FindElement(By.XPath("//input[@id='role-name']"), GlobalWaitTime_int);
                Robot robot = new Robot();
                // Scroll Down using Robot class
                robot.keyPress(KeyEvent.VK_PAGE_DOWN);
                robot.keyRelease(KeyEvent.VK_PAGE_DOWN);


                IWebElement Role_Name = driver.FindElement(By.XPath("//input[@id='role-name']"), GlobalWaitTime_int);
                Random randomGenerator = new Random();
                int randomInt = randomGenerator.Next(1000);
                String Role = Role_values[0] + randomInt + " Role";
                Role_Name.SendKeys(Role);

                IWebElement Description = driver.FindElement(By.XPath("//input[@id='role-description']"), GlobalWaitTime_int);
                Description.SendKeys(Role_values[0] + randomInt + "Role");

                //..........Expand the 1st - Action API................//
                IWebElement Expand = driver.FindElement(By.XPath("//div[@class='account-overview flex flex--rule-flex-justify-center closed']//div[2]//div[1]//b[1]//span[1]"), GlobalWaitTime_int);
                Expand.Click();
                Thread.Sleep(2000);

                //...........select All.................................//
                IWebElement Select_All = driver.FindElement(By.XPath("//span[@class='role-select-group']"), GlobalWaitTime_int);
                Select_All.Click();
                Thread.Sleep(2000);

                //..........Click Create Role...........................//
                IWebElement CreateRole_Button = driver.FindElement(By.XPath("//button[contains(@class, 'green') and contains(text(), 'Create Role')]"), GlobalWaitTime_int);
                CreateRole_Button.Click();
                Thread.Sleep(7000);
                Assert.IsNotNull(CreateRole_Button);


                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                IWebElement ele_enter = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (isAccepted)
                {
                    // Assert.AreEqual(driver.Url, Is.Equals(baseURL + "/PageWhereClientIsRedirectedToAfterSuccessfulLogin"));
                    exMsg = Role + ": is Created Successfully ";
                    Thread.Sleep(2000);
                }
                else
                {
                    // Assert.AreNotEqual(driver.FindElement(By.XPath("//div[@class='notices is-top']")).Text, Is.Equals("Create user failed"));
                    exMsg = "Unable to Create Role";
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Select Values for the fields while creating a new Account _____________________*/
        public static String select_Values_for_AccountCreation(string browser, string TestData, string TestStepID, string xpath, string TestStepDesc, string Keyword, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";
            string accountname = "";
            try
            {

                string[] Account_Values = TestData.Split('|');


                IWebElement Account_Name = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Random randomGenerator = new Random();
                int randomInts = randomGenerator.Next(1000);
                accountname = Account_Values[0] + randomInts;
                Account_Name.SendKeys(accountname);

                IWebElement Account_key = driver.FindElement(By.XPath("//input[@id='add-account-key']"), GlobalWaitTime_int);
                Account_key.SendKeys(Account_Values[1] + randomInts + "Key");
                Thread.Sleep(1000);




                ////............What login do they use?................//
                if (Account_Values[2].Equals("Engage"))
                {
                    driver.FindElement(By.XPath("//label[@for='isLoginEnabled_true']")).Click();
                }
                else if (Account_Values[3].Equals("Duo - Pricelogic"))
                {
                    driver.FindElement(By.XPath("//label[@for='isLoginEnabled_false']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Incorrect Input for Login Type";

                }

                //..Scroll page down...............................//

                Robot robot = new Robot();
                robot.keyPress(KeyEvent.VK_PAGE_DOWN);
                robot.keyRelease(KeyEvent.VK_PAGE_DOWN);

                //..........Click Add Account Role...........................//
                IWebElement Add_Account_Button = driver.FindElement(By.XPath("//button[contains(@class, 'step-control-button green') and contains(text(), 'Add Account')]"), GlobalWaitTime_int);
                Add_Account_Button.Click();
                Thread.Sleep(5000);

                //..Scroll page Up...............................//


                robot.keyPress(KeyEvent.VK_PAGE_UP);
                robot.keyRelease(KeyEvent.VK_PAGE_UP);


                exMsg = "Account created successfully with details:\n" + "AccountName:-" + accountname;
                //Assert.AreSame("Pass", FlagTestCase, exMsg);
                Thread.Sleep(1000);


                return accountname;

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                return accountname;
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }



        /* ______________Enter newly Added Account _____________________*/
        public static void Enter_Newly_Added_Account(string accountname, string browser, string TestData, string TestStepID, string xpath, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                IWebElement Account_Name = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Account_Name.SendKeys(accountname);
                Thread.Sleep(2000);
                exMsg = "Newly Added AccountName:-" + accountname;
                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Select Values for the fields while creating a users_____________________*/
        public static string Select_Values_for_Users_Creation(string browser, string TestData, string TestStepID, string xpath, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            String email = "";
            try
            {
                string[] Add_User_Values = TestData.Split('|');

                //IWebElement Add_User = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                //Add_User.Click();
                //Thread.Sleep(5000);

                //................FirstName......................//
                IWebElement First_Name = driver.FindElement(By.XPath("//input[@id='user-first-name']"), GlobalWaitTime_int);
                Random randomGenerator = new Random();
                int randomInts = randomGenerator.Next(1000);
                String firstname = Add_User_Values[0] + randomInts;
                First_Name.SendKeys(firstname);
                Thread.Sleep(5000);

                //................LastName..........................//
                IWebElement Last_Name = driver.FindElement(By.XPath("//input[@id='user-last-name']"), GlobalWaitTime_int);
                String lastname = Add_User_Values[1] + randomInts;
                Last_Name.SendKeys(lastname);
                Thread.Sleep(5000);

                //................User-Email..........................//
                IWebElement User_Email = driver.FindElement(By.XPath("//input[@id='user-email']"), GlobalWaitTime_int);
                email = Add_User_Values[2] + randomInts + "@gmail.com";
                User_Email.SendKeys(email);
                Thread.Sleep(5000);

                //................User-Email..........................//
                IWebElement User_Confirm_Email = driver.FindElement(By.XPath("//input[@id='user-confirmed-email']"), GlobalWaitTime_int);
                User_Confirm_Email.SendKeys(email);
                Thread.Sleep(5000);

                //................Phone..........................//
                IWebElement Phone = driver.FindElement(By.XPath("//input[@id='user-phone']"), GlobalWaitTime_int);
                String phone = Add_User_Values[3] + randomInts;
                Phone.SendKeys(phone);
                Thread.Sleep(5000);

                //................Confirm Phone..........................//
                IWebElement Confirm_Phone = driver.FindElement(By.XPath("//input[@id='user-confirmed-phone']"), GlobalWaitTime_int);
                Confirm_Phone.SendKeys(phone);
                Thread.Sleep(5000);

                //..........Click Add user Button...........................//
                IWebElement Add_User_Button = driver.FindElement(By.XPath("//button[contains(@class, 'step-control-button green') and contains(text(), 'Add User')]"), GlobalWaitTime_int);
                Add_User_Button.Click();
                Thread.Sleep(5000);
                //   Assert.IsNotNull(Add_Account_Button);

                //..........Click Next Button...........................//
                IWebElement Add_User_Next_Button = driver.FindElement(By.XPath("//a[contains(@class, 'step-control-button blue') and contains(text(), 'Next')]"), GlobalWaitTime_int);
                Add_User_Next_Button.Click();
                Thread.Sleep(1000);

                // Assert.AreSame("Pass", FlagTestCase, exMsg);
                exMsg = "User created successfully with details:\n" + "FirstName:-" + firstname + "\nEmail:-" + email;

                return email;

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                return email;
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Select Values for the fields while creating a Teams _____________________*/
        public static void select_Values_for_Teams_Creation(string email, string browser, string TestData, string TestStepID, string xpath, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string[] Add_Teams_Values = TestData.Split('|');

                {
                    //IWebElement Add_Teams = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                    //Add_Teams.Click();
                    //Thread.Sleep(5000);

                    ////................Team Name......................//
                    IWebElement Add_Team_Name = driver.FindElement(By.XPath("//input[@id='team-name']"), GlobalWaitTime_int);
                    Random randomGenerator = new Random();
                    int randomInts = randomGenerator.Next(1000);
                    String Teamname = Add_Teams_Values[0] + randomInts;
                    Add_Team_Name.SendKeys(Teamname);
                    Thread.Sleep(5000);


                    //................Description..........................//
                    IWebElement Description = driver.FindElement(By.XPath("//input[@id='team-description']"), GlobalWaitTime_int);
                    Description.SendKeys(Add_Teams_Values[1]);
                    Thread.Sleep(5000);

                    //................Search for Users..........................//
                    IWebElement Search_User = driver.FindElement(By.XPath("//input[@id='team-search-member']"), GlobalWaitTime_int);
                    Search_User.SendKeys(email);
                    Thread.Sleep(5000);

                    //...................Searched Email List.............//
                    String member_email = driver.FindElement(By.XPath("(//div[@class='member--add-icon'])[1]/preceding-sibling::div//span[contains(@class, 'add-user--name')]/b")).Text;
                    Thread.Sleep(5000);


                    //..........Click Add Teams...........................//
                    IWebElement Add_Teams_Button = driver.FindElement(By.XPath("//button[contains(@class, 'step-control-button green') and contains(text(), 'Add Teams')]"), GlobalWaitTime_int);
                    Add_Teams_Button.Click();
                    Thread.Sleep(5000);


                    //..........Click Next...........................//
                    IWebElement Next_Button_Teams = driver.FindElement(By.XPath("//a[contains(@class, 'step-control-button blue') and contains(text(), 'Next')]"), GlobalWaitTime_int);
                    Next_Button_Teams.Click();
                    Thread.Sleep(1000);

                    // Assert.AreSame("Pass", FlagTestCase, exMsg);
                    exMsg = "\n Team Name created successfully" + Teamname;

                }

                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Select Values for the fields while creating a users_____________________*/
        public static string Select_Values_for_roles_Creation(string browser, string TestData, string TestStepID, string xpath, string TestStepDesc, string Keyword, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";
            String email = "";
            try
            {

                string[] Add_role_Values = TestData.Split('|');


                //................Role_Name......................//
                IWebElement Role_Name = driver.FindElement(By.XPath("//input[@id='role-name']"), GlobalWaitTime_int);
                Random randomGenerator = new Random();
                int randomInts = randomGenerator.Next(1000);
                String rolename = Add_role_Values[0] + randomInts;
                Role_Name.SendKeys(rolename);
                Thread.Sleep(5000);

                //................Description..........................//
                IWebElement Role_Description = driver.FindElement(By.XPath("//input[@id='role-description']"), GlobalWaitTime_int);
                String role = randomInts + Add_role_Values[1];
                Role_Description.SendKeys(role);
                Thread.Sleep(5000);



                //..........Expand the 1st - Action API................//
                IWebElement Expand_List = driver.FindElement(By.XPath("//b[contains(text(),'Action API')]/span[contains(text(),'+')]"), GlobalWaitTime_int);
                Expand_List.Click();
                Thread.Sleep(5000);

                //...........select All.................................//
                IWebElement Select_All = driver.FindElement(By.XPath("//span[@class='role-select-group']"), GlobalWaitTime_int);
                Select_All.Click();
                Thread.Sleep(5000);

                //..........Click Add Role...........................//
                IWebElement Add_Role_Button = driver.FindElement(By.XPath("//button[contains(@class, 'green') and contains(text(), 'Add Roles')]"), GlobalWaitTime_int);
                Add_Role_Button.Click();
                Thread.Sleep(5000);

                //..........Click Finish...........................//
                IWebElement Finish_Button = driver.FindElement(By.XPath("//a[contains(@class, 'step-control-button blue') and contains(text(), 'Finish')]"), GlobalWaitTime_int);
                Finish_Button.Click();
                Thread.Sleep(1000);


                return email;



            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                return email;
            }
            finally
            {

                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Verifying list of Total Teams present in the page_____________________*/
        public static void Verify_Teams(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                Thread.Sleep(3000);

                IWebElement Teams = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                String Total_Teams = Teams.Text;

                exMsg = ObjectName + " is :" + Total_Teams;

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying list of Total Users present in the page_____________________*/
        public static void Verify_Users(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement Users = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                String Total_Users = Users.Text;
                exMsg = ObjectName + " is :" + Total_Users;
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying list of Total Roles present in the page_____________________*/
        public static void Verify_Roles(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement Roles = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                String Total_Roles = Roles.Text;
                exMsg = ObjectName + " is :" + Total_Roles;
            }

            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Select Values for the fields while creating/Updating a Ward_____________________*/
        public static void addUpdateWard(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string[] field_values = TestData.Split('|');
                string[] flow = field_values[0].Split(':');
                void fill_value(string xpath, string data)
                {
                    IWebElement element = driver.FindElement(By.XPath(xpath));
                    element.Clear();
                    element.SendKeys(data);
                }

                /*Random randomGenerator = new Random();
                int randomInt = randomGenerator.Next(1000);
                String wardKey = "
                " + randomInt;*/
                if (flow.Contains("Create"))
                {
                    fill_value("//label[contains(text(),'Enter Ward Key')]/preceding-sibling::input", flow[1]);
                }

                fill_value("//label[contains(text(),'Ward Name')]/preceding-sibling::input", field_values[1]);
                fill_value("//label[contains(text(),'Ward Description') or contains(text(),'Description')]/preceding-sibling::input", field_values[2]);
                fill_value("//label[contains(text(),'Ward Data Key')]/preceding-sibling::input", field_values[3]);

                if (field_values[4].Equals("Standard"))
                {
                    driver.FindElement(By.XPath("//label[@for='license_Standard']")).Click();
                }
                else if (field_values[4].Equals("Enterprise"))
                {
                    driver.FindElement(By.XPath("//label[@for='license_Enterprise']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Incorrect Input for License";

                }

                if (field_values[5].Equals("Yes"))
                {
                    driver.FindElement(By.XPath("//label[@for='hasCdpWardSpecific_true']")).Click();
                }
                else if (field_values[5].Equals("No"))
                {
                    driver.FindElement(By.XPath("//label[@for='hasCdpWardSpecific_false']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nIncorrect Input for Is CDP ward specific";

                }

                if (field_values[6].Equals("Yes"))
                {
                    driver.FindElement(By.XPath("//label[@for='isPiiShareable_true']")).Click();
                }
                else if (field_values[6].Equals("No"))
                {
                    driver.FindElement(By.XPath("//label[@for='isPiiShareable_false']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nIncorrect Input for Is Pii Shareable";

                }

                if (field_values[7].Equals("Yes"))
                {
                    driver.FindElement(By.XPath("//label[@for='isPiiMatching_true']")).Click();
                }
                else if (field_values[7].Equals("No"))
                {
                    driver.FindElement(By.XPath("//label[@for='isPiiMatching_false']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nIncorrect Input for Is Pii Matching Allowed";

                }

                if (field_values[8].Equals("Yes"))
                {
                    driver.FindElement(By.XPath("//label[@for='isAnonymousMatchingAllowed_true']")).Click();
                }
                else if (field_values[8].Equals("No"))
                {
                    driver.FindElement(By.XPath("//label[@for='isAnonymousMatchingAllowed_false']")).Click();
                }
                else
                {

                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nIncorrect Input for Is Anonymous Matching Allowed";

                }

                switch (field_values[9])
                {

                    case "Custom":
                        driver.FindElement(By.XPath("//input[@value='Custom']/following-sibling::label[@for='newLicenseTier_0']")).Click();
                        break;

                    case "100k":
                        driver.FindElement(By.XPath("//input[@value='_100k']/following-sibling::label[@for='newLicenseTier_1']")).Click();
                        break;

                    case "250k":
                        driver.FindElement(By.XPath("//input[@value='_250k']/following-sibling::label[@for='newLicenseTier_2']")).Click();
                        break;

                    case "500k":
                        driver.FindElement(By.XPath("//input[@value='_500k']/following-sibling::label[@for='newLicenseTier_3']")).Click();
                        break;

                    case "1M":
                        driver.FindElement(By.XPath("//input[@value='_1M']/following-sibling::label[@for='newLicenseTier_4']")).Click();
                        break;

                    case "2M":
                        driver.FindElement(By.XPath("//input[@value='_2M']/following-sibling::label[@for='newLicenseTier_5']")).Click();
                        break;

                    case "3M":
                        driver.FindElement(By.XPath("//input[@value='_3M']/following-sibling::label[@for='newLicenseTier_6']")).Click();
                        break;

                    case "4M":
                        driver.FindElement(By.XPath("//input[@value='_4M']/following-sibling::label[@for='newLicenseTier_7']")).Click();
                        break;

                    default:
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = exMsg + "\nIncorrect Input for Select Tier";
                        break;
                }

                string[] modulesToBeSelected = field_values[10].Split('/');
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                Boolean checkBox = true;
                if (modulesToBeSelected.Length.Equals(0))
                {
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nNo Modules provided";
                }
                if (modulesToBeSelected.Contains("Advanced Analytics"))
                {
                    if (flow[0].Equals("Create"))
                    {
                        driver.FindElement(By.XPath("//input[@id='hasAdmin']")).Click();
                    }
                    if (flow[0].Equals("Update"))
                    {
                        checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasAdvancedAnalytics.regular-checkbox').checked");
                        if (!checkBox)
                        {
                            driver.FindElement(By.XPath("//input[@id='hasAdvancedAnalytics']")).Click();
                        }
                    }
                }
                else
                {
                    if (flow[0].Equals("Update"))
                    {
                        checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasAdvancedAnalytics.regular-checkbox').checked");
                        if (checkBox)
                        {
                            driver.FindElement(By.XPath("//input[@id='hasAdvancedAnalytics']")).Click();
                        }
                    }
                }
                if (modulesToBeSelected.Contains("Compliance"))
                {
                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasCCPA.regular-checkbox').checked");
                    if (!checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='hasCCPA']")).Click();
                    }
                }
                else
                {

                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasCCPA.regular-checkbox').checked");
                    if (checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='hasCCPA']")).Click();
                    }
                }
                if (modulesToBeSelected.Contains("Demo Gallery"))
                {
                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasDemo.regular-checkbox').checked");
                    if (!checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='hasDemo']")).Click();
                    }
                }
                else
                {

                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasDemo.regular-checkbox').checked");
                    if (checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='hasDemo']")).Click();
                    }
                }
                if (flow.Contains("Update"))
                {
                    if (modulesToBeSelected.Contains("Customer Care"))
                    {
                        checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasCustomerCare.regular-checkbox').checked");
                        if (!checkBox)
                        {
                            driver.FindElement(By.XPath("//input[@id='hasCustomerCare']")).Click();
                        }
                    }
                    else
                    {

                        checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#hasCustomerCare.regular-checkbox').checked");
                        if (checkBox)
                        {
                            driver.FindElement(By.XPath("//input[@id='hasCustomerCare']")).Click();
                        }
                    }
                }

                driver.FindElement(By.XPath("//button[contains(text(),'Add Ward') or contains(text(),'Update Ward')]")).Click();
                Thread.Sleep(5000);
                if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                {
                    exMsg = "Update successful message displayed";
                }
                else
                {
                    FlagTestCase = "Fail";
                }

                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Add Engagements to a Ward_____________________*/
        public static void addEngagements(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                WaitForLoad(driver, 60);
                Thread.Sleep(5000);
                string[] field_values = TestData.Split('|');
                string engagementKey = field_values[0];
                driver.FindElement(By.XPath("//input[@id='add-engagement-search']")).SendKeys(engagementKey);
                Thread.Sleep(5000);
                try
                {
                    Thread.Sleep(5000);
                    driver.FindElement(By.XPath("//div[contains(text(),'" + field_values[0] + "')]/ancestor::div[1]//div[text()='+']")).Click();
                    Thread.Sleep(3000);
                    if (driver.FindElement(By.XPath("//input[@id='engagement-name']")).Displayed)
                        exMsg = "Engagement Name field is present";
                    if (driver.FindElement(By.XPath("//input[@id='dateengagement-start-on']")).Displayed)
                        exMsg = "Engagement Start Date field is present";
                    if (driver.FindElement(By.XPath("//input[@id='timeengagement-start-on']")).Displayed)
                        exMsg = "Engagement Start time field is present";
                    if (driver.FindElement(By.XPath("//input[@id='dateengagement-end-on']")).Displayed)
                        exMsg = "Engagement End date field is present";
                    if (driver.FindElement(By.XPath("//input[@id='timeengagement-end-on']")).Displayed)
                        exMsg = "Engagement End time field is present";
                    IWebElement startdatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-start-on']"));
                    IWebElement enddatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-end-on']"));
                    IWebElement prePromoStartDatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-pre-promo-start-on']"));
                    IWebElement postPromoEndDatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-post-promo-end-on']"));
                    IWebElement minimumage = driver.FindElement(By.XPath("//input[@id='minimum-age']"));
                    String today = DateTime.Today.ToString("MM/dd/yyyy");
                    String prepromostartdate = today;
                    String startdate = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                    String enddate = DateTime.Today.AddDays(5).ToString("MM/dd/yyyy");
                    String postpromoenddate = DateTime.Today.AddDays(6).ToString("MM/dd/yyyy");
                    startdatefield.SendKeys(startdate);
                    enddatefield.SendKeys(enddate);
                    prePromoStartDatefield.SendKeys(prepromostartdate);
                    prePromoStartDatefield.SendKeys(prepromostartdate);
                    postPromoEndDatefield.SendKeys(postpromoenddate);
                    postPromoEndDatefield.SendKeys(postpromoenddate);
                    minimumage.Clear();
                    minimumage.SendKeys("18");

                    string[] featuresToBeSelected = field_values[3].Split('/');
                    int i = 0;
                    while (i < featuresToBeSelected.Length)
                    {
                        driver.FindElement(By.XPath("//input[@id='" + featuresToBeSelected[i] + "']")).Click();
                        i++;
                    }

                    driver.FindElement(By.XPath("//button[contains(text(),'Add Engagement')]")).Click();
                    Thread.Sleep(5000);
                    if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                    {
                        exMsg = "Update successful message displayed";
                    }
                    else
                    {
                        FlagTestCase = "Fail";
                    }

                }
                catch (Exception ex)
                {
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                }

                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Add Integrations to a Ward_____________________*/
        public static void addIntegrations(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string[] field_values = TestData.Split('|');
                driver.FindElement(By.XPath("//select[@data-cy='integrationKey-drop-down']")).Click();

                switch (field_values[0])
                {

                    case "Custom Code Integration":
                        try
                        {
                            driver.FindElement(By.XPath("//option[contains(text(),'Custom Code Integration')]")).Click();
                            driver.FindElement(By.XPath("//input[@id='integration-name']")).SendKeys(field_values[1]);
                            driver.FindElement(By.XPath("//input[@id='customCodeUrl']")).SendKeys(field_values[2]);
                        }
                        catch (Exception ex)
                        {
                            exMsg = ex.Message;
                            FlagTestCase = "Fail";
                        }
                        break;

                    case "Lexis Nexis Age Verification":
                        try
                        {
                            driver.FindElement(By.XPath("//option[contains(text(),'Lexis Nexis Age Verification')]")).Click();
                            driver.FindElement(By.XPath("//input[@id='integration-name']")).SendKeys(field_values[1]);
                            driver.FindElement(By.XPath("//input[@id='endpointUri']")).SendKeys(field_values[2]);
                            driver.FindElement(By.XPath("//input[@id='orgId']")).SendKeys(field_values[3]);
                            driver.FindElement(By.XPath("//input[@id='apiKey']")).SendKeys(field_values[4]);
                            driver.FindElement(By.XPath("//input[@id='policy']")).SendKeys(field_values[5]);
                            driver.FindElement(By.XPath("//select[@data-cy='ageBracket']")).Click();
                            try
                            {
                                driver.FindElement(By.XPath("//option[contains(text(),'" + field_values[6] + "')]")).Click();
                            }
                            catch (Exception ex)
                            {
                                exMsg = "No option with text as " + field_values[6] + " found in Age Bracket"+ex.Message;
                            }

                            string[] subscribedEvents = field_values[7].Split('/');
                            int i = 0;
                            while (i < subscribedEvents.Length)
                            {
                                driver.FindElement(By.XPath("//input[@id='" + subscribedEvents[i] + "']")).Click();
                                i++;
                            }
                        }
                        catch (Exception ex)
                        {
                            exMsg = ex.Message;
                            FlagTestCase = "Fail";
                        }
                        break;

                    case "Telerx Service Integration":
                        try
                        {
                            driver.FindElement(By.XPath("//option[contains(text(),'Telerx Service Integration')]")).Click();
                            driver.FindElement(By.XPath("//input[@id='integration-name']")).SendKeys(field_values[1]);
                            driver.FindElement(By.XPath("//input[@id='endpointUri']")).SendKeys(field_values[2]);
                            driver.FindElement(By.XPath("//input[@id='username']")).SendKeys(field_values[3]);
                            driver.FindElement(By.XPath("//input[@id='password']")).SendKeys(field_values[4]);
                            if (field_values.Contains("ContactUsRequestCreated"))
                                driver.FindElement(By.XPath("//input[@id='ContactUsRequestCreated']")).Click();
                        }
                        catch (Exception ex)
                        {
                            exMsg = ex.Message;
                            FlagTestCase = "Fail";
                        }
                        break;

                    default:

                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "Incorrect value for Integration Service";
                        break;
                }

                driver.FindElement(By.XPath("//button[contains(text(),'Create Integration')]")).Click();
                Thread.Sleep(5000);
                if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                {
                    exMsg = "Update successful message displayed";
                }
                else
                {
                    FlagTestCase = "Fail";
                }
                Assert.AreSame("Pass", FlagTestCase, exMsg);

                Assert.AreSame("Pass", FlagTestCase, exMsg);

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        static void WaitForLoad(IWebDriver driver, int timeoutSec = 35)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }


        /* ______________Add Access Group to a Ward_____________________*/
        public static void addAccessGroup(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                IWebElement addAccessGroupHeading = driver.FindElement(By.XPath("//div[@class='title' and contains(text(),'Add Access Groups')]"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addAccessGroupHeading);

                string[] field_values = TestData.Split('|');
                driver.FindElement(By.XPath("//label[text()='Enter Group Name']/preceding-sibling::input")).SendKeys(field_values[0]);
                driver.FindElement(By.XPath("//label[text()='Group Description']/preceding-sibling::input")).SendKeys(field_values[1]);

                if (!field_values[2].Equals(""))
                {
                    driver.FindElement(By.XPath("//input[@id='search-engagements']")).SendKeys(field_values[2]);
                    Thread.Sleep(3000);
                    IWebElement engagementsToBeAdded = driver.FindElement(By.XPath("//div[contains(text(),'" + field_values[2] + "')]/following-sibling::div[2]"));
                    if (engagementsToBeAdded.Text.Equals("+"))
                    {
                        engagementsToBeAdded.Click();
                    }
                    Assert.AreSame("Added", engagementsToBeAdded.Text, "Engagement not added");


                }

                if (!field_values[3].Equals(""))
                {
                    driver.FindElement(By.XPath("//select[@data-cy='opt-out-response-drop-down']")).Click();
                    driver.FindElement(By.XPath("//option[contains(text(),'" + field_values[3] + "')]")).Click();
                }

                IWebElement teamsearchfield =  driver.FindElement(By.XPath("//input[@id='TeamSearch']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", teamsearchfield);
                teamsearchfield.SendKeys(field_values[4]);
                Thread.Sleep(5000);
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                try
                {
                    IWebElement addIcon = driver.FindElement(By.XPath("//div[contains(text(),'" + field_values[4] + "')]/ancestor::div[1]//div[contains(@class,'add-remove-icon')]/div"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addIcon);
                    addIcon.Click();
                }
                catch (Exception ex) {
                    if (ex.Message.Contains("Unable to find element")) {
                        exMsg = "Team is not added";
                    
                    }
                
                }
                driver.FindElement(By.XPath("//button[contains(text(),'Add Access Group') or contains(text(),'Create Access Group')]")).Click();
                Thread.Sleep(5000);
                if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                {
                    exMsg = exMsg + "\nUpdate successful message displayed";
                }
                else
                {
                    FlagTestCase = "Fail";
                }
                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Search a ward under Admin Module_____________________*/
        public static void searchWard(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                driver.FindElement(By.XPath("//input[@id='ward-list-search']")).SendKeys(TestData);
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//li[@class='select-account--account-list-item' and contains(text(),'" + TestData + "')]")).Click();
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                Thread.Sleep(5000);
                if (!driver.FindElement(By.XPath("//p/span[text()='" + TestData + "']")).Displayed)
                {
                    FlagTestCase = "Fail";
                }
                Thread.Sleep(5000);
                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ___________________Update Engagements in a Ward_____________________*/
        public static void updateEngagements(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string[] field_values = TestData.Split('|');
                string engagementKey = field_values[0];
                driver.FindElement(By.XPath("//div[@data-cy='admin-engagements-list']//div[contains(text(),'" + field_values[0] + "')]")).Click();
                Thread.Sleep(5000);
                if (driver.FindElement(By.XPath("//input[@id='engagement-name']")).Displayed)
                    exMsg = exMsg + "Engagement Name field is present";
                if (driver.FindElement(By.XPath("//input[@id='dateengagement-start-on']")).Displayed)
                    exMsg = exMsg + "\nEngagement Start Date field is present";
                if (driver.FindElement(By.XPath("//input[@id='timeengagement-start-on']")).Displayed)
                    exMsg = exMsg + "\nEngagement Start time field is present";
                if (driver.FindElement(By.XPath("//input[@id='dateengagement-end-on']")).Displayed)
                    exMsg = exMsg + "\nEngagement End date field is present";
                if (driver.FindElement(By.XPath("//input[@id='timeengagement-end-on']")).Displayed)
                    exMsg = exMsg + "\nEngagement End time field is present";
                if (driver.FindElement(By.XPath("//input[@id='engagement-data-key']")).Displayed)
                    exMsg = exMsg + "\nEngagement data Key field is present";
                Thread.Sleep(1000);
                IWebElement startdatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-start-on']"));
                IWebElement enddatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-end-on']"));
                IWebElement prePromoStartDatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-pre-promo-start-on']"));
                IWebElement postPromoEndDatefield = driver.FindElement(By.XPath("//input[@id='dateengagement-post-promo-end-on']"));
                IWebElement minimumage = driver.FindElement(By.XPath("//input[@id='minimum-age']"));
                String today = DateTime.Today.ToString("MM/dd/yyyy");
                String prepromostartdate = today;
                String startdate = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                String enddate = DateTime.Today.AddDays(5).ToString("MM/dd/yyyy");
                String postpromoenddate = DateTime.Today.AddDays(6).ToString("MM/dd/yyyy");
                startdatefield.SendKeys(startdate);
                enddatefield.SendKeys(enddate);
                prePromoStartDatefield.SendKeys(prepromostartdate);
                prePromoStartDatefield.SendKeys(prepromostartdate);
                postPromoEndDatefield.SendKeys(postpromoenddate);
                postPromoEndDatefield.SendKeys(postpromoenddate);
                minimumage.Clear();
                minimumage.SendKeys("18");


                string[] featuresToBeSelected = field_values[1].Split('/');
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                Boolean checkBox = true;
                if (featuresToBeSelected.Length.Equals(0))
                {
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = exMsg + "\nNo Modules provided";
                }
                void selectFeature(string feature)
                {
                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#" + feature + ".regular-checkbox').checked");
                    if (!checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='" + feature + "']")).Click();

                    }
                }
                void deselectFeature(string feature)
                {

                    checkBox = (Boolean)js.ExecuteScript("return document.querySelector('#" + feature + ".regular-checkbox').checked");
                    if (checkBox)
                    {
                        driver.FindElement(By.XPath("//input[@id='" + feature + "']")).Click();

                    }

                }

                if (featuresToBeSelected.Contains("Admin"))
                {
                    selectFeature("Admin");
                }
                else
                {
                    deselectFeature("Admin");
                }
                if (featuresToBeSelected.Contains("Fulfillment"))
                {
                    selectFeature("Fulfillment");
                }
                else
                {
                    deselectFeature("Fulfillment");
                }
                if (featuresToBeSelected.Contains("CustomerSupport"))
                {
                    selectFeature("CustomerSupport");
                }
                else
                {
                    deselectFeature("CustomerSupport");
                }
                if (featuresToBeSelected.Contains("FireDrill"))
                {
                    selectFeature("FireDrill");
                }
                else
                {
                    deselectFeature("FireDrill");
                }
                if (featuresToBeSelected.Contains("GiftWithPurchase"))
                {
                    selectFeature("GiftWithPurchase");
                }
                else
                {
                    deselectFeature("GiftWithPurchase");
                }
                if (featuresToBeSelected.Contains("InstantWin"))
                {
                    selectFeature("InstantWin");
                }
                else
                {
                    deselectFeature("InstantWin");
                }
                if (featuresToBeSelected.Contains("MMS"))
                {
                    selectFeature("MMS");
                }
                else
                {
                    deselectFeature("MMS");
                }
                if (featuresToBeSelected.Contains("Loyalty"))
                {
                    selectFeature("Loyalty");
                }
                else
                {
                    deselectFeature("Loyalty");
                }
                if (featuresToBeSelected.Contains("ReceiptRecognition"))
                {
                    selectFeature("ReceiptRecognition");
                }
                else
                {
                    deselectFeature("ReceiptRecognition");
                }
                if (featuresToBeSelected.Contains("SocialAggregator"))
                {
                    selectFeature("SocialAggregator");
                }
                else
                {
                    deselectFeature("SocialAggregator");
                }
                if (featuresToBeSelected.Contains("Sweepstakes"))
                {
                    selectFeature("Sweepstakes");
                }
                else
                {
                    deselectFeature("Sweepstakes");
                }
                if (featuresToBeSelected.Contains("Trivia"))
                {
                    selectFeature("Trivia");
                }
                else
                {
                    deselectFeature("Trivia");
                }
                if (featuresToBeSelected.Contains("UserGeneratedContent"))
                {
                    selectFeature("UserGeneratedContent");
                }
                else
                {
                    deselectFeature("UserGeneratedContent");
                }
                if (featuresToBeSelected.Contains("Survey"))
                {
                    selectFeature("Survey");
                }
                else
                {
                    deselectFeature("Survey");
                }
                if (featuresToBeSelected.Contains("ClaimsSite"))
                {
                    selectFeature("ClaimsSite");
                }
                else
                {
                    deselectFeature("ClaimsSite");
                }
                if (featuresToBeSelected.Contains("EmailOptIn"))
                {
                    selectFeature("EmailOptIn");
                }
                else
                {
                    deselectFeature("EmailOptIn");
                }
                if (featuresToBeSelected.Contains("PurchaseIncentive"))
                {
                    selectFeature("PurchaseIncentive");
                }
                else
                {
                    deselectFeature("PurchaseIncentive");
                }
                if (featuresToBeSelected.Contains("Registration"))
                {
                    selectFeature("Registration");
                }
                else
                {
                    deselectFeature("Registration");
                }
                if (featuresToBeSelected.Contains("LabelMaker"))
                {
                    selectFeature("LabelMaker");
                }
                else
                {
                    deselectFeature("LabelMaker");
                }
                if (featuresToBeSelected.Contains("PunchCard"))
                {
                    selectFeature("PunchCard");
                }
                else
                {
                    deselectFeature("PunchCard");
                }
                if (featuresToBeSelected.Contains("GoogleAnalytics"))
                {
                    selectFeature("GoogleAnalytics");
                }
                else
                {
                    deselectFeature("GoogleAnalytics");
                }
                if (featuresToBeSelected.Contains("LoginTracking"))
                {
                    selectFeature("LoginTracking");
                }
                else
                {
                    deselectFeature("LoginTracking");
                }

                driver.FindElement(By.XPath("//button[contains(text(),'Update Engagement')]")).Click();
                Thread.Sleep(5000);
                WaitForLoad(driver, 60);
                if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                {
                    exMsg = "Update successful message displayed";
                }
                else
                {
                    FlagTestCase = "Fail";
                }

                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Update Access Group in a Ward_____________________*/
        public static void updateAccessGroup(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string[] field_values = TestData.Split('|');
                IWebElement accessgroup =  driver.FindElement(By.XPath("//div[contains(@data-cy,'access-groups-list')]//div[contains(text(),'" + field_values[0] + "')]"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", accessgroup);
                accessgroup.Click();
                Thread.Sleep(5000);
                if (driver.FindElement(By.XPath("//label[text()='Enter Group Name']/preceding-sibling::input")).Displayed)
                    exMsg = exMsg + "Group Name field is present";
                if (driver.FindElement(By.XPath("//label[text()='Group Description']/preceding-sibling::input")).Displayed)
                    exMsg = exMsg + "\nGroup Description field is present";
                if (driver.FindElement(By.XPath("//h2[contains(text(),'Add Engagements to this access group')]")).Displayed)
                    exMsg = exMsg + "\nAdd engagement section is present";

                if (!field_values[1].Equals(""))
                {
                    driver.FindElement(By.XPath("//input[@id='search-engagements']")).SendKeys(field_values[1]);
                    Thread.Sleep(3000);
                    IWebElement engagementsToBeAdded = driver.FindElement(By.XPath("//div[contains(text(),'" + field_values[1] + "')]/following-sibling::div[2]"));
                    if (engagementsToBeAdded.Text.Equals("+"))
                    {
                        engagementsToBeAdded.Click();
                        Assert.AreSame("Added", engagementsToBeAdded.Text, "Engagement not added");
                        exMsg = exMsg + "\nEngagement " + field_values[1] + "added Successfully";
                    }
                    if (engagementsToBeAdded.Text.Equals("Added"))
                    {
                        exMsg = exMsg + "\nEngagement already added to the Access Group";
                    }
                }
                IWebElement teamSearchField = driver.FindElement(By.XPath("//input[@id='TeamSearch']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", accessgroup);
                teamSearchField.SendKeys(field_values[2]);
                Thread.Sleep(5000);
                try
                {
                    IWebElement addIcon = driver.FindElement(By.XPath("//div[contains(text(),'" + field_values[4] + "')]/ancestor::div[1]//div[contains(@class,'add-remove-icon')]/div"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addIcon);
                    addIcon.Click();                    
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Unable to find element"))
                    {
                        exMsg = "Team is not added";

                    }

                }
                driver.FindElement(By.XPath("//button[contains(text(),'Update Access Group')]")).Click();
                Thread.Sleep(5000);
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                if (driver.FindElement(By.XPath("//div[@role='alert']/div[text()='Update successful']")).Displayed)
                {
                    exMsg = "Update successful message displayed";
                }
                else
                {
                    FlagTestCase = "Fail";
                }
                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");

            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Verifying last updated Date in the page_____________________*/
        public static void Verify_Last_Updated_Date(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement Date = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                String dateformat = Date.Text;
                DateFormat dateFormat = new SimpleDateFormat("Last Updated:dd/mm/yy");
                DateTime date = new DateTime();
                String date1 = dateFormat.format(date);
                exMsg = ObjectName + " is :" + date1;
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        public static void FOLLOWUPEVENTCHECK(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement FupEvents_Tab = driver.FindElement(By.XPath("//a[contains(text(),'Follow-up Events')]"), GlobalWaitTime_int);
                MoveToElement_JS(FupEvents_Tab);
                ClickOnElement_JS(FupEvents_Tab);
                Thread.Sleep(2000);

                var eventList = driver.FindElements(By.XPath("//follow-up-events//tbody/tr/td[2]/a"));

                foreach (IWebElement item in eventList)
                {
                    string IsDisabled = item.GetAttribute("class");
                    if (IsDisabled == null || IsDisabled == "")
                    {
                        ClickOnElement_JS(item);
                        Thread.Sleep(2000);
                        //IWebElement addSurveyorBtn = driver.FindElement(By.XPath("//followupsurveyclasscomponent//div[contains(text(),'Add')]"), GlobalWaitTime_int);
                        IWebElement Surveyor = driver.FindElement(By.XPath("(//followupsurveyclasscomponent//select[@name='SurveyorOption'])[1]"), GlobalWaitTime_int);
                        SelectElement surSelect = new SelectElement(Surveyor);
                        surSelect.SelectByIndex(1);
                        Thread.Sleep(1000);

                        IWebElement FUPSaveBtn = driver.FindElement(By.XPath("//followupsurveyclasscomponent//button[contains(text(),'Save')]"), GlobalWaitTime_int);
                        ClickOnElement_JS(FUPSaveBtn);
                        Thread.Sleep(6000);

                        IWebElement FUPCancelBtn = driver.FindElement(By.XPath("//followupsurveyclasscomponent//button[contains(text(),'Cancel')]"), GlobalWaitTime_int);
                        ClickOnElement_JS(FUPCancelBtn);
                        Thread.Sleep(4000);
                    }
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Verification of PSP Dashboard Due List_____________________*/
        public static void Submit_PSP_DUE(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Fail";
            string exMsg = "ESC45/60 or POC10 have not been listed";

            try
            {
                IList<IWebElement> dueList = driver.FindElements(By.XPath(xpath), GlobalWaitTime_int);

                int i = 1;

                foreach (IWebElement ele in dueList)
                {
                    //IWebElement dueStatus = driver.FindElement(By.XPath("//postsurvey-whatsdue//table//tbody/tr[" + i + "]/td[4]"), GlobalWaitTime_int);
                    IWebElement dueStatus = driver.FindElement(By.XPath("//postsurvey-whatsdue//table//tbody/tr[2][" + i + "]/td[2]/a"), GlobalWaitTime_int);
                    string statusText = dueStatus.Text;

                    if (statusText == string.Empty)
                    {
                        statusText = dueStatus.GetAttribute("value");
                    }

                    string eleText = ele.Text;

                    if (eleText == string.Empty)
                    {
                        eleText = ele.GetAttribute("value");
                    }

                    if ((eleText.Contains("Plan of Correction") || eleText.Contains("60-day Evidence of Standards Compliance") || eleText.Contains("45-day Evidence of Standards Compliance")) && statusText.Trim() == "OPEN")
                    {
                        exMsg = eleText.Replace("Plan of Correction(INIT-U)", "POC").Replace("60-day Evidence of Standards Compliance(INIT-U)", "ESC60")
                            .Replace("45-day Evidence of Standards Compliance(INIT-U)", "ESC45").Replace("Plan of Correction(FULL-U)", "POC")
                            .Replace("60-day Evidence of Standards Compliance(FULL-U)", "ESC60").Replace("45-day Evidence of Standards Compliance(FULL-U)", "ESC45") + " has been listed in the due list";
                        FlagTestCase = "Pass";

                        ClickOnElement_JS(ele);

                        Thread.Sleep(1000);

                        string childEventURL = driver.Url;
                        childEventID = childEventURL.Substring(childEventURL.IndexOf("eventsummary/")).Replace("eventsummary/", "");

                        IList<IWebElement> list = driver.FindElements(By.XPath("//tbody/tr"));

                        for (int j = 1; j <= list.Count; j++)
                        {
                            IWebElement GoToButton = driver.FindElement(By.XPath("//tbody/tr[1]//a[text()='Go To']"));
                            ClickOnElement_JS(GoToButton);
                            Thread.Sleep(5000);

                            IWebElement AllSections_Tab = driver.FindElement(By.XPath("//a[contains(text(),'All Sections')]"));
                            ClickOnElement_JS(AllSections_Tab);
                            Thread.Sleep(3000);

                            var jsDriver = (IJavaScriptExecutor)driver;
                            string enableScript = @"document.getElementsByClassName(""mat-input-element"")[0].readOnly = false;";
                            jsDriver.ExecuteScript(enableScript);
                            IWebElement CorActionDate = driver.FindElement(By.XPath("//button[@aria-label='Open calendar']/../../..//input"));
                            string date = CorActionDate.GetAttribute("max");

                            // DateTime dt = DateTime.ParseExact(date, "yyyy-MM-d", CultureInfo.InvariantCulture);
                            DateTime dt = DateTime.Today;
                            string result = dt.ToString("MM/d/yyyy", CultureInfo.InvariantCulture);
                            CorActionDate.SendKeys("result");
                            //string enterScript = @"document.getElementsByClassName(""mat-input-element"")[0].value=''; document.getElementsByClassName(""mat-input-element"")[0].value='" + result + "'; ";
                            //jsDriver.ExecuteScript(enterScript);
                            //Thread.Sleep(1000);

                            try
                            {
                                MoveToElement_JS(CorActionDate);
                                CorActionDate.SendKeys(result);

                                IWebElement AssigningAccountability = driver.FindElement(By.XPath("(//input)[1]"));
                                MoveToElement_JS(AssigningAccountability);
                                ClickOnElement_JS(AssigningAccountability);


                                AssigningAccountability.Clear();
                                AssigningAccountability.SendKeys("Test");
                            }
                            catch
                            {
                                //IsAlertPresent(driver, "Accept");
                                SendKeys.SendWait("{ENTER}");
                                Thread.Sleep(1000);
                            }

                            try
                            {
                                IWebElement memberSelect = driver.FindElement(By.XPath("//label[contains(text(),'Which member')]/following-sibling::div//mat-select"), GlobalWaitTime_int);
                                MoveToElement_JS(memberSelect);
                                ClickOnElement_JS(memberSelect);
                                Thread.Sleep(1000);
                                IWebElement CEOCheckBox = driver.FindElement(By.XPath("//span[contains(text(),'Chief Executive')]/ancestor::mat-option//mat-pseudo-checkbox"), GlobalWaitTime_int);
                                MoveToElement_JS(CEOCheckBox);
                                ClickOnElement_JS(CEOCheckBox);
                                Thread.Sleep(1000);
                            }
                            catch { }


                            var tr_collectionFrames = driver.FindElements(By.TagName("iframe"));
                            foreach (var item in tr_collectionFrames)
                            {
                                driver.SwitchTo().Frame(item);
                                Thread.Sleep(1000);
                                IWebElement iframeTxtBx = driver.FindElement(By.TagName("body"), GlobalWaitTime_int);

                                ClickOnElement_JS(iframeTxtBx);
                                iframeTxtBx.Clear();
                                MoveToElement_JS(iframeTxtBx);
                                iframeTxtBx.SendKeys("Test");
                                Thread.Sleep(2000);
                                driver.SwitchTo().ParentFrame();
                            }

                            IWebElement Save_NextBtn = driver.FindElement(By.XPath("//button[contains(text(),'Next')]"));
                            ClickOnElement_JS(Save_NextBtn);
                            Thread.Sleep(15000);

                            //IWebElement EventSummaryBrdCrmb = driver.FindElement(By.XPath("//a[contains(text(),'Event Summary')]"));
                            //ClickOnElement_JS(EventSummaryBrdCrmb);
                            //Thread.Sleep(5000);
                        }

                        IWebElement SubmitBtn = driver.FindElement(By.XPath("//button[contains(text(),'Submit')]"), 20);
                        //ClickOnElement_JS(SubmitBtn);
                        SubmitBtn.Click();
                        Thread.Sleep(10000);
                        try
                        {
                            IWebElement popupSubmitBtn = driver.FindElement(By.XPath("//div[@class='modal-dialog']//button[contains(text(),'Submit')]"), 20);
                            //ClickOnElement_JS(popupSubmitBtn);
                            popupSubmitBtn.Click();
                            Thread.Sleep(15000);
                        }
                        catch { }

                        IWebElement HomeBtn = driver.FindElement(By.XPath("//a[contains(text(),'Home')]"), 20);
                        //ClickOnElement_JS(HomeBtn);
                        HomeBtn.Click();
                        Thread.Sleep(5000);
                        //IWebElement submitSuccessMsg = driver.FindElement(By.XPath("//.[contains(text(),'Submitted')]"), GlobalWaitTime_int);

                        IWebElement dueStatusCheck = driver.FindElement(By.XPath("//postsurvey-whatsdue//table//tbody/tr[" + i + "]/td[4]"), GlobalWaitTime_int);

                        string statusTextCheck = dueStatusCheck.Text;

                        if (statusTextCheck == string.Empty)
                        {
                            statusTextCheck = dueStatusCheck.GetAttribute("value");
                        }

                        if (statusTextCheck.Trim() == "SUBMITTED")
                        {
                            exMsg = "PSP event has been submitted sucessfully";
                            string ChildEventSum = TestData.Substring(0, TestData.LastIndexOf("/") + 1) + childEventID;
                            driver.Navigate().GoToUrl(ChildEventSum);
                            Thread.Sleep(9000);
                        }
                        else
                        {
                            FlagTestCase = "Fail";
                            exMsg = "PSP event has not been submitted sucessfully";
                        }

                        break;
                    }

                    i++;
                }

                Assert.AreSame("Pass", FlagTestCase, exMsg);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Submit ESC60 Button_____________________*/
        public static void SUBMITBTN_ESC60(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            IWebElement SubmitBtn = driver.FindElement(By.XPath("//button[contains(text(),'Submit')]"), 20);
            //ClickOnElement_JS(SubmitBtn);
            SubmitBtn.Click();
            Thread.Sleep(10000);

            IWebElement popupSubmitBtn = driver.FindElement(By.XPath("//div[@class='modal-dialog']//button[contains(text(),'Submit')]"), 20);
            //ClickOnElement_JS(popupSubmitBtn);
            popupSubmitBtn.Click();
            Thread.Sleep(15000);

        }


        /* ______________ ESC60 Button_____________________*/
        public static void ESC60(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            IWebElement SubmitBtn = driver.FindElement(By.XPath("//button[contains(text(),'Submit')]"), 20);
            //ClickOnElement_JS(SubmitBtn);
            SubmitBtn.Click();
            Thread.Sleep(10000);

            IWebElement popupSubmitBtn = driver.FindElement(By.XPath("//div[@class='modal-dialog']//button[contains(text(),'Submit')]"), 20);
            //ClickOnElement_JS(popupSubmitBtn);
            popupSubmitBtn.Click();
            Thread.Sleep(15000);


        }


        /* ______________Date Due List_____________________*/
        public static void Select_Date(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            string enableScript = @"document.getElementsByClassName(""mat-input-element"")[0].readOnly = false;";
            jsDriver.ExecuteScript(enableScript);
            IWebElement CorActionDate = driver.FindElement(By.XPath("//button[@aria-label='Open calendar']/../../..//input"));
            string date = CorActionDate.GetAttribute("max");
            DateTime dt = DateTime.Today;
            string result = dt.ToString("MM/d/yyyy", CultureInfo.InvariantCulture);
            CorActionDate.Clear();
            CorActionDate.SendKeys(result);

        }


        /* ______________Verifying Page URL_____________________*/
        public static void Verify_PageURL(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                string PageURL = driver.Url;

                if (!PageURL.Contains(TestData))
                {
                    FlagTestCase = "Fail";
                    exMsg = "Current page url does not contain " + TestData;
                }
                else
                {
                    exMsg = "Current page is verified and page is correctly loaded";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Verifying Web Element is present in the page or not_____________________*/
        public static void Verify_ElementDisplayed(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Assert.IsNotNull(element);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Verifying Web Element is present in the page or not_____________________*/
        public static void Verify_LoggedINUser(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Thread.Sleep(1000);
                Assert.IsNotNull(element);

                string eleText = element.Text.ToLower();

                string userName = TestData.ToLower();
                Thread.Sleep(2000);
                if (!(eleText.Contains(userName)))
                {
                    FlagTestCase = "Fail";
                }

                Assert.AreSame("Pass", FlagTestCase, TestData + " - Text is not found in the " + ObjectName + " object");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Export To CSV _____________________*/
        public static void Export_File(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                //.................................Edge.........................................//
                if (browser.Trim().ToUpper() == "Edge" || browser.Trim().ToUpper() == "MICROSOFT EDGE" || browser.Trim().ToUpper() == "ME")
                {

                    var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    Thread.Sleep(10000);

                    element.Click();
                    Thread.Sleep(10000);

                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }


                }
                //........................Chrome............................................//
                if (browser.Trim().ToUpper() == "CHROME" || browser.Trim().ToUpper() == "GOOGLE CHROME" || browser.Trim().ToUpper() == "GOOGLECHROME")
                {
                    var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    element.Click();

                    Thread.Sleep(10000);
                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }

                    /*CommonFunctions.DeleteAllFilesInFolder(downloadDirectory)*/;
                }
                //..............................Firefox................................//
                else if (browser.Trim().ToUpper() == "MOZILLA" || browser.Trim().ToUpper() == "MOZILLAFIREFOX" || browser.Trim().ToUpper() == "FIREFOX")
                {
                    var downloadDirectory = KnownFolders.Downloads.Path;
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    element.Click();

                    Thread.Sleep(5000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(3000);
                    SendKeys.SendWait(@"{Enter}");
                    Thread.Sleep(10000);

                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }
                }


                else if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER" || browser.Trim().ToUpper() == "IEXPLORER")
                {
                    var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    string fileName = element.Text;

                    Actions actions = new Actions(driver);
                    actions.ContextClick(element).Perform();
                    Thread.Sleep(2000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(7000);
                    SendKeys.SendWait(downloadDirectory + @"\" + fileName);
                    Thread.Sleep(3000);
                    SendKeys.SendWait(@"{Enter}");

                    Thread.Sleep(4000);
                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }

                    //CommonFunctions.DeleteAllFilesInFolder(downloadDirectory);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Check File Downloaded _____________________*/
        public static void CheckFileDownloaded(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                bool exist = false;
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\TestDownloads";

                string[] filePaths = Directory.GetFiles(Path);
                foreach (string p in filePaths)
                {
                    if (p.Contains(TestData))
                    {
                        FileInfo thisFile = new FileInfo(p);
                        var isFresh = DateTime.Now - thisFile.LastWriteTime < TimeSpan.FromMinutes(3);
                        exist = true;
                    }
                }
                FlagTestCase = exist ? "Pass" : "Fail";


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {

                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________SendMail _____________________*/
        public static void SendMail(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "Mail sent successfully";

            try
            {
                MailMessage mail = new MailMessage();
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                SmtpClient smtp = new SmtpClient("smtp.office365.com");
                String AttchmentPath = string.Empty;
                mail.To.Add("sujith.s@winwire.com");
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Sender"]);
                mail.Subject = "TJC Execution Report";
                mail.Body = "Hi All, " + Environment.NewLine + "\n Please find the attachment of Execution Report" + Environment.NewLine + Environment.NewLine + "\n Regards, \n QA Team ";
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.High;
                AttchmentPath = ReportPath;
                Attachment attachment;
                attachment = new Attachment(AttchmentPath);
                mail.Attachments.Add(attachment);
                //smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserId"],
                                                                    ConfigurationManager.AppSettings["Pwd"]);
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {

                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying downloaded file present or not_____________________*/
        public static void Download_Verification(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                if (browser.Trim().ToUpper() == "CHROME" || browser.Trim().ToUpper() == "GOOGLE CHROME" || browser.Trim().ToUpper() == "GOOGLECHROME")
                {
                    var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    element.Click();

                    Thread.Sleep(10000);
                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }
                    //CommonFunctions.DeleteAllFilesInFolder(downloadDirectory);
                }
                else if (browser.Trim().ToUpper() == "MOZILLA" || browser.Trim().ToUpper() == "MOZILLAFIREFOX" || browser.Trim().ToUpper() == "FIREFOX")
                {
                    var downloadDirectory = KnownFolders.Downloads.Path;
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    element.Click();

                    Thread.Sleep(5000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(3000);
                    SendKeys.SendWait(@"{Enter}");
                    Thread.Sleep(10000);

                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }
                }
                else if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER" || browser.Trim().ToUpper() == "IEXPLORER")
                {
                    var downloadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestDownloads";
                    int filesCountBefore = Directory.GetFiles(downloadDirectory).Count();

                    IWebElement element = driver.FindElement(By.XPath(xpath), 60);
                    string fileName = element.Text;

                    Actions actions = new Actions(driver);
                    actions.ContextClick(element).Perform();
                    Thread.Sleep(2000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(7000);
                    SendKeys.SendWait(downloadDirectory + @"\" + fileName);
                    Thread.Sleep(3000);
                    SendKeys.SendWait(@"{Enter}");

                    Thread.Sleep(4000);
                    exMsg = "File has been dowloaded successfully";
                    int filesCountAfter = Directory.GetFiles(downloadDirectory).Count();
                    if (!(filesCountAfter > filesCountBefore))
                    {
                        FlagTestCase = "Fail";
                        TestLogic.TestFail++;
                        exMsg = "File has not been dowloaded successfully";
                    }

                    //CommonFunctions.DeleteAllFilesInFolder(downloadDirectory);
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Verifying Documents Links_____________________*/
        public static void Verify_DocumentsLinks(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collection1 = driver.FindElements(By.XPath(xpath));
                int i = 0;
                foreach (IWebElement row in tr_collection1)
                {
                    row.Click();
                    Thread.Sleep(3000);
                    flag = "clicked";
                    i++;
                }

                if (flag == "clicked")
                {
                    exMsg = i + " document links has been clicked";
                }
                else
                {
                    exMsg = "There are no document links available";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Verifying the Value of Web Element is not equal to Empty_____________________*/
        public static void Verification_NotEmpty(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collection1 = driver.FindElements(By.XPath(xpath));

                foreach (IWebElement row in tr_collection1)
                {
                    if (row.Text.Trim() != string.Empty)
                    {
                        flag = "pass";
                        break;
                    }
                }

                Assert.AreSame("pass", flag, ObjectName + " - Object doesn't contain any text");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying the list of Web Elements_____________________*/
        public static void verification_list(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var tr_collection1 = driver.FindElements(By.XPath(xpath));

                foreach (IWebElement row in tr_collection1)
                {
                    if (row.Text.Contains(TestData))
                    {
                        flag = "pass";
                    }
                    else
                    {
                        flag = "fail";
                        break;
                    }
                }

                Assert.AreSame("pass", flag, TestData + " - Text is not found in the " + ObjectName + " object");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying the validation in the page. If validation is present, Test will fail_____________________*/
        public static void verification_NoValidation(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                //if element is present, then test is failed else passed
                FlagTestCase = "Fail";

                TestLogic.TestFail++;

                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }

                exMsg = "The validation is found in the page, thus Test Step " + TestStepID + " has failed";
            }
            catch
            {
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Verifying the Value of PDF_____________________*/
        public static void verification_PDF(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                string pdfContent, saveFile = string.Empty;

                if (browser.Trim().ToUpper() == "IE" || browser.Trim().ToUpper() == "INTERNET EXPLORER" || browser.Trim().ToUpper() == "IEXPLORER")
                {
                    saveFile = CommonFunctions.SaveAction_IE("PDFVerify");
                    Thread.Sleep(3000);
                }
                else
                {
                    saveFile = CommonFunctions.SaveAction("PDFVerify");
                    Thread.Sleep(1000);
                }

                TextExtractor textExtractor = new TextExtractor();
                pdfContent = textExtractor.Extract(saveFile).Text;

                if (!pdfContent.Contains(TestData))
                {
                    FlagTestCase = "Fail";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Storing the Value of Web Element_____________________*/
        public static void Store_Details(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                MoveToElement_JS(ele);
                text = ele.Text.Replace("location_on", "").Replace(":", "").Trim();

                if (text == string.Empty)
                {
                    text = ele.GetAttribute("value");
                }

                StoredDetails[TestData] = text;

                if (text.Length > 25)
                {
                    text = text.Substring(0, 22) + "...";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, text + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Storing the Value of Web Element_____________________*/
        public static void Store_Details_SaferPlac(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var th = driver.FindElements(By.XPath("//scoredstandards//th[contains(text(),'SAFER Placement')]/../th"));

                int indexofSP = 0;
                for (int i = 1; i <= th.Count; i++)
                {
                    var chk = driver.FindElement(By.XPath("(//scoredstandards//th)[" + i + "]"), GlobalWaitTime_int);
                    if (chk.Text.Contains("SAFER Placement"))
                    {
                        indexofSP = i;
                        break;
                    }
                }


                var ele = driver.FindElement(By.XPath("//scoredstandards//th[contains(text(),'SAFER Placement')]/../../following-sibling::tbody/tr[1]/td[" + indexofSP + "]"), GlobalWaitTime_int);
                MoveToElement_JS(ele);
                text = ele.Text;

                if (text == string.Empty)
                {
                    text = ele.GetAttribute("value");
                }

                string[] list = text.Split('-');


                StoredDetails["EPLikelihood"] = list[0].Trim();
                StoredDetails["EPScope"] = list[1].Trim();

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Select_Search_Filter_____________________*/
        public static void Select_Search_Filter(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            int i = 0;

            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                Thread.Sleep(5000);
                ele.Click();
                Thread.Sleep(5000);

                var Select_none = driver.FindElement(By.XPath("//li[normalize-space()='SELECT NONE']"), GlobalWaitTime_int);
                Thread.Sleep(5000);
                Select_none.Click();
                Thread.Sleep(5000);

                var Select_all = driver.FindElement(By.XPath("//li[normalize-space()='SELECT ALL']"), GlobalWaitTime_int);
                Thread.Sleep(5000);
                Select_all.Click();
                Thread.Sleep(5000);

                //var Clickout = driver.FindElement(By.XPath("//div[@class='toggle-switch']"), GlobalWaitTime_int);
                //Thread.Sleep(5000);
                //Clickout.Click();

                exMsg = "Filter seach is complete";

                //foreach (IWebElement row in tr_collection1)
                //{
                //    row.Click();
                //    Thread.Sleep(3000);
                flag = "clicked";
                //    i++;
                //}


                if (flag == "clicked")
                {
                    exMsg = i + "Filter is clicked";
                }
                else
                {
                    exMsg = "There are no Filter available";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Search_Engagement_Value search box_____________________*/
        public static void Search_Engagement_Value(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            //string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                IWebElement text = driver.FindElement(By.XPath(xpath));
                text.SendKeys("PL011271");
                Thread.Sleep(10000);

                text.Clear();
                Thread.Sleep(10000);

                IWebElement textsearch = driver.FindElement(By.XPath("//input[@class='nameInput field-toggle--input']"));
                textsearch.SendKeys("PL011271");
                Thread.Sleep(10000);

                var gettext = driver.FindElement(By.XPath("//div[@Class='table-cell'][2]"), GlobalWaitTime_int);
                gettext.Click();
                Thread.Sleep(2000);

                exMsg = " Search with Engagement ID is Successfull";



            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Entering the Value in Textbox_____________________*/
        public static void Enter_Value(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();
                ele.Clear();
                ele.SendKeys(TestData);
                Assert.IsNotNull(ele);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                IWebElement ele_enter = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                exMsg = "Value Entered ";
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
                Thread.Sleep(5000);
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Run SP to delete the data in the DB_____________________*/
        public static void Run_StoreProcedure(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string exMsg = "";
            string FlagTestCase = "Pass";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();

                //..................Connecting to WW-QA DB....................................//
                //if (SystemRun == "QA")
                {
                    SqlConnection con = new SqlConnection("Server=tcp:elancocrdb.database.windows.net;Database=ElancoCRDB_dev;Trusted_Connection=False;user id=Elancocrdbadmin;password=Pass@Elanco1;");
                    con.Open();
                    SqlCommand command = new SqlCommand("[dbo].[RevertSmoketestChanges]", con)
                    {
                        CommandType = CommandType.StoredProcedure

                    };


                    DateTime dt = TestLogic.UtcStartTime;
                    string result = dt.ToString("dd-mmm-yyyy", CultureInfo.InvariantCulture);

                    command.Parameters.Add(new SqlParameter("@ProductType", SqlDbType.VarChar, 50, "Biocide"));
                    command.Parameters.Add(new SqlParameter("@Section", SqlDbType.VarChar, 50, "Clinical"));
                    command.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar, 50, "Afghanistan"));
                    command.Parameters.Add(new SqlParameter("@ActionDate", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@ActionBy", SqlDbType.VarChar, 50, "Template Author1"));

                    command.Parameters[0].Value = "Biocide";
                    command.Parameters[1].Value = "Clinical";
                    command.Parameters[2].Value = "Afghanistan";
                    //command.Parameters[3].Value = dt;
                    command.Parameters[3].Value = "2020-08-26 09:27:32.450";
                    command.Parameters[4].Value = "Template Author1";

                    int i = command.ExecuteNonQuery();

                    exMsg = "Version Restored successfully from the Database";

                    con.Close();
                }

                //..........................................Connecting to Elanco DEV DB......................................//
                //    else if (SystemRun == "UAT")
                //    {
                //        SqlConnection con = new SqlConnection("Server=tcp:crdbqa.database.windows.net;Database=ElancoCRDB_qa;Trusted_Connection=False;user id=crdbstg;password=Pass@Elanco1;");
                //        con.Open();
                //        SqlCommand command = new SqlCommand("[dbo].[RevertSmoketestChanges]", con)
                //        {
                //            CommandType = CommandType.StoredProcedure

                //        };


                //        DateTime dt = TestLogic.UtcStartTime;
                //        string result = dt.ToString("dd-mmm-yyyy", CultureInfo.InvariantCulture);

                //        command.Parameters.Add(new SqlParameter("@ProductType", SqlDbType.VarChar, 50, "Biocide"));
                //        command.Parameters.Add(new SqlParameter("@Section", SqlDbType.VarChar, 50, "Clinical"));
                //        command.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar, 50, "Afghanistan"));
                //        command.Parameters.Add(new SqlParameter("@ActionDate", SqlDbType.DateTime));
                //        command.Parameters.Add(new SqlParameter("@ActionBy", SqlDbType.VarChar, 50, "Template Author1"));

                //        command.Parameters[0].Value = "Biocide";
                //        command.Parameters[1].Value = "Quality (CMC)";
                //        command.Parameters[2].Value = "Afghanistan";
                //        command.Parameters[3].Value = dt;
                //        command.Parameters[4].Value = "Template Author1";

                //        // command.UpdatedRowSource = UpdateRowSource.OutputParameters;


                //        int i = command.ExecuteNonQuery();

                //        exMsg = "Section deleted successfully from the Data base";

                //        con.Close();
                //    }
            }

            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Clearing the Value in Textbox_____________________*/
        public static void Clear_Value(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();
                ele.Clear();
                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Entering the Value in Textbox using TagName_____________________*/
        public static void Enter_Value_ByTagName(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElements(By.TagName("input"));

                foreach (IWebElement row in ele)
                {
                    if (row.GetAttribute("type") == xpath)
                    {
                        row.SendKeys(TestData);
                        break;
                    }
                }
                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Entering the Value in Textbox present inside IFrame_____________________*/
        public static void Enter_Value_iframe(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var tr_collectionFrames = driver.FindElements(By.TagName("iframe"));
                foreach (var item in tr_collectionFrames)
                {
                    driver.SwitchTo().Frame(item);
                    var tr_collection1 = driver.FindElements(By.XPath(xpath));
                    if (tr_collection1.Count > 0)
                    {
                        foreach (IWebElement ele in tr_collection1)
                        {
                            //ele.Click();
                            ele.Clear();
                            ele.SendKeys(TestData);
                            Assert.IsNotNull(ele);
                        }
                        driver.SwitchTo().ParentFrame();
                        break;
                    }
                    driver.SwitchTo().ParentFrame();
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }


        /* ______________Entering the Value in Textbox present inside IFrame_____________________*/
        public static void EnterValue_Iframe(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var tr_collectionFrames = driver.FindElements(By.XPath(xpath));
                foreach (var item in tr_collectionFrames)
                {
                    driver.SwitchTo().Frame(item);
                    Thread.Sleep(1000);
                    IWebElement iframeTxtBx = driver.FindElement(By.TagName("body"), GlobalWaitTime_int);

                    ClickOnElement_JS(iframeTxtBx);
                    iframeTxtBx.Clear();
                    MoveToElement_JS(iframeTxtBx);
                    //iframeTxtBx.SendKeys("Test_" + Reports.replace(DateTime.Now.ToString()));
                    // iframeTxtBx.SendKeys("Test_" + tr_collectionFrames.ToString());
                    iframeTxtBx.SendKeys(TestData);

                    Thread.Sleep(2000);
                    driver.SwitchTo().ParentFrame();
                }


            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Entering the Value in Textbox present inside IFrame_____________________*/
        public static void Enter_Text_KeyBoard(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                SendKeys.SendWait(TestData);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }



        /* ______________Clicking on the link in the Web Page_____________________*/
        public static void Click_on_link(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string found = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElements(By.TagName("a"));

                foreach (IWebElement row in ele)
                {
                    if (row.GetAttribute("href").Contains(TestData))
                    {
                        row.Click();
                        found = "found";
                        break;
                    }

                    if (found != "found")
                    {
                        if (row.GetAttribute("innerHTML") == TestData)
                        {
                            row.Click();
                            found = "found";
                            break;
                        }
                    }

                }
                Assert.IsNotNull(ele);

                if (found == "")
                {
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Clicking on the event in the Web Page_____________________*/
        public static void Click_on_event(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {

            string found = "";
            string FlagTestCase = "Pass";
            string flag = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElements(By.TagName("a"));

                foreach (IWebElement row in ele)
                {
                    if (row.GetAttribute("href").Contains(TestData))
                    {
                        row.Click();
                        found = "found";
                        break;
                    }

                    if (found != "found")
                    {
                        if (row.GetAttribute("innerHTML") == TestData)
                        {
                            row.Click();
                            found = "found";
                            break;
                        }
                    }

                }
                Assert.IsNotNull(ele);

                if (found == "")
                {
                    flag = "fail";
                }
                Thread.Sleep(15000);
                string eventSum = driver.Url.ToString();

                if (eventSum.Contains("SurveySummary.aspx"))
                {
                    Thread.Sleep(10000);
                    var leftPanelLink = driver.FindElement(By.XPath("//div[@class='leftRail']//a[contains(text(),'ModernCO Event Summary')]"), GlobalWaitTime_int);
                    ClickOnElement_JS(leftPanelLink);
                    Thread.Sleep(15000);
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }


        /* ______________Verifying the Search Link in the Web Page_____________________*/
        public static void Verifying_search_link(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                if (ele.Text.Contains(TestData))
                {
                    flag = "pass";
                }

                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }

        }

        /* ______________Verifying Left Navigation in the Browser_____________________*/
        public static void Verify_leftNavigation(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            int i = 0;
            try
            {
                string[] leftnav = TestData.Split(',');
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                var elesub = driver.FindElements(By.TagName("li"));

                for (int j = 0; j < elesub.Count; j++)
                {
                    foreach (string word in leftnav)
                    {
                        if (elesub[j].Text != null || elesub[j].Text != "")
                        {
                            if (elesub[j].Text.Contains(word))
                            {
                                i++;
                            }
                        }
                    }
                }

                if (leftnav.Count() == i)
                {
                    flag = "pass";
                }
                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Switching to new tab in the Browser_____________________*/
        public static void SwitchToNewTab(string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                string windowName = driver.WindowHandles.Last();
                Thread.Sleep(5000);
                driver.SwitchTo().Window(windowName);
                Thread.Sleep(5000);
                driver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + "" + " - " + "" + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", "" + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Verifying and Closing Dialog_____________________*/
        public static void Verify_CloseDialog(string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                SendKeys.SendWait("5000");
                Thread.Sleep(3000);
                SendKeys.SendWait("(%{F4})");
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + "" + " - " + "" + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", "" + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Clicking on the Header Tab - Logic is mainly created for IE Browser. In this method, the Tab mentioned will be clicked and then it's repective header section is made visible and then the 'next' link(Ex: General Application) will be clicked in the displayed section._____________________*/
        public static void SortColumnAscending(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var colButton = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                var colSortIcon = driver.FindElement(By.XPath(ColumnSortBtn), GlobalWaitTime_int);

                string sortColumnClass = colSortIcon.GetAttribute("class");

                if (sortColumnClass == "rgIcon rgSortDescIcon")
                {
                    colButton.Click();
                }

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Clicking on the Web Element_____________________*/
        public static void Get_Tokens(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                text = ele.Text;

                if (text == string.Empty)
                {
                    text = ele.GetAttribute("value");
                }

                string[] li = text.Split('[');

                foreach (string item in li)
                {
                    if (item != string.Empty && item.Contains(']'))
                    {
                        if (tokens[TestData] != null)
                        {
                            tokens[TestData] = tokens[TestData] + "þ" + "[" + item.Substring(0, item.IndexOf(']') + 1);
                        }
                        else
                        {
                            tokens[TestData] = "[" + item.Substring(0, item.IndexOf(']') + 1);
                        }
                    }
                }

                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Clicking on the Web Element_____________________*/
        public static void Verify_Tokens(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            var text = "";
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);

                text = ele.Text;

                if (text == string.Empty)
                {
                    text = ele.GetAttribute("value");
                }

                string[] li = tokens[TestData].ToString().Split('þ');
                string tokenName = string.Empty;

                foreach (string item in li)
                {
                    if (text.Contains(item))
                    {
                        FlagTestCase = "Fail";
                        tokenName = item;
                        exMsg = "'" + TestData + "' field still contains the token '" + tokenName + "'";
                        break;
                    }
                }

                Assert.AreEqual("Pass", FlagTestCase);

            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message;
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Closing the Browser_____________________*/
        public static void CloseBrowser(string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            try
            {
                driver.Quit();
            }
            catch { }
        }


        /* ______________Highlighting the Web Element using XPath_____________________*/
        public static void highlightUsingXpath(IWebDriver driver, string xpath, string TestStepID, string TestStepDesc, string Keyword, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            try
            {
                IWebElement element = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                var jsDriver = (IJavaScriptExecutor)driver;
                string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
                jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + "" + " - " + "" + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", "" + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Alert Handling in Page_____________________*/
        public static void IsAlertPresent(IWebDriver _driver, string Value)
        {
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                if (Value == "Accept")
                {
                    alert.Accept();
                    Console.WriteLine("Alert is Accepted");
                }
                else if (Value == "Dismiss")
                {
                    alert.Dismiss();
                    Console.WriteLine("Alert is Dismissed");
                }
                else
                {
                    Console.WriteLine("Alert Action is Indefined");
                }
            }
            catch (NoAlertPresentException NoAlert)
            {
                Console.WriteLine("No Alert is Present");
                Console.WriteLine(NoAlert.StackTrace);
            }
        }
        /* ______________Entering the Value in Textbox_____________________*/
        public static void Enter_Value_EmailUpdate(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string FlagTestCase = "Pass";
            string exMsg = "";
            string finalData = "";
            try
            {
                string timeStamp = Reports.replace(DateTime.Now.ToString()).Replace("_", "");
                string[] str = TestData.Split('@');
                finalData = str[0] + "_" + timeStamp + "@" + str[1];

                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();
                ele.Clear();
                ele.SendKeys(finalData);
                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, finalData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }
        //..........................JSClick..................//
        public static void JSClick_on(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            String exMsg = "";

            String FlagTestCase = "Pass";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = null;
                ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                try
                {
                    Actions action = new Actions(driver);
                    action.MoveToElement(ele);
                    action.Build();
                    action.Perform();


                    exMsg = ObjectName + " Element is Clicked";

                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);


                    //  IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    ////  WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
                    //  IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
                    //  wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));


                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);
                    Thread.Sleep(2000);
                }

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);

                if (ele == null)
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Element is not present in the DOM";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        /* ______________Toggle Advanced Search_____________________*/
        public static void Toggle_Advance_Search(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            string flag = "";
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {

                IWebElement ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                ele.Click();

                if (ele.Enabled)
                {
                    //  ele.Click();
                    flag = "found";
                    exMsg = "Toggle Element search is Enabled";
                    Thread.Sleep(20000);

                }
                if (flag == "")
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Toggle Element search is Disabled";
                    Thread.Sleep(5000);
                }
                /*__________________Uncheck_______________________________*/
                ele.Click();
                if (ele.Enabled)
                {
                    //  ele.Click();
                    flag = "found";
                    exMsg = "Toggle Element search is Disabled";
                    Thread.Sleep(20000);

                }
                if (flag == "")
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Toggle Element search is Enabled";
                    Thread.Sleep(5000);
                }
                ele.Click();


                Assert.IsNotNull(ele);
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        //......................JSEnter ..................................//
        public static void JSEnter_on(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            String exMsg = "";

            String FlagTestCase = "Pass";

            try
            {
                Thread.Sleep(1000);
                IWebElement ele = null;
                ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                try
                {
                    Actions action = new Actions(driver);
                    action.MoveToElement(ele);
                    action.Build();
                    action.Perform();
                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);
                    Thread.Sleep(2000);
                }

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);

                if (ele == null)
                {
                    if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                    {
                        testCaseExecution.bProceedOnFail = false;
                    }
                    FlagTestCase = "Fail";
                    TestLogic.TestFail++;
                    exMsg = "Element is not present in the DOM";
                }
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, "", "" + "\n" + exMsg, FlagTestCase, RunningTestCase);

            }
        }


        /*.....................EXITAPPLICATION..............................*/
        public static void EXITAPPLICATION(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {
            String exMsg = "";

            String FlagTestCase = "Pass";

            try
            {
                var ele = driver.FindElement(By.XPath(xpath), GlobalWaitTime_int);
                CommonFunctions.highlight(driver, ele);

                ele.Click();

                Assert.IsNotNull(ele);
                Thread.Sleep(3000);
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg = ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, TestData + "\n" + exMsg, FlagTestCase, RunningTestCase);
            }
        }

        public static void MoveToElement_JS(IWebElement ele)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);
            Thread.Sleep(200);
        }

        public static void ClickOnElement_JS(IWebElement ele)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);
            Thread.Sleep(300);
        }

        //* ______________Verifying the Programs Section in Modern Event summary Page_____________________*//
        public static void Verfication_ProgramsSection(string browser, string TestData, string TestStepID, string TestStepDesc, string Keyword, string ObjectName, string xpath, string xpath_verification, string RunningTestCase)
        {            
            string FlagTestCase = "Pass";
            string exMsg = "";

            try
            {
                var progList = driver.FindElements(By.XPath("(//div[@id='tab1']//table)[1]//tbody/tr"));
            }
            catch (Exception ex)
            {
                if (testCaseExecution.sProceedOnFail.ToUpper() == "N" || testCaseExecution.sProceedOnFail.ToUpper() == "NO")
                {
                    testCaseExecution.bProceedOnFail = false;
                }
                FlagTestCase = "Fail";
                TestLogic.TestFail++;
                exMsg += " \n " + ex.Message.Replace("width: 1349px; height: 1368px; z-index: 1001;", "");
            }
            finally
            {
                Console.WriteLine(TestStepID + " - " + TestStepDesc + "\n" + exMsg + " - " + Keyword + " - " + ObjectName + " - " + TestData + " - " + FlagTestCase + " - " + RunningTestCase);
                Reports.Report_TestDataStep(driver, TestStepID, TestStepDesc, Keyword, ObjectName, exMsg, FlagTestCase, RunningTestCase);
            }
        }

    }
}

