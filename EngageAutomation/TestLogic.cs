using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engage.Automation.Operations;
using Engage.Automation.Utilities;
using System.Configuration;
using System.Data;
using System;

namespace Engage.Automation
{
    /// <summary>
    /// Summary description for TestLogic
    /// </summary>
    [TestClass]
    public class TestLogic
    {
        public static DataSet dsData = new DataSet();
        public static int TestFail = 0;
        public static int Flag = 0;
        public static bool TestCategoryValue = false;
        static CommonFunctions commonFunctions = new CommonFunctions();
   

        public TestLogic()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        //................Run SP.......................................................//
        public static DateTime UtcStartTime { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testcontext)
        {
                         
            TestCategoryValue = CommonFunctions.GetCommandLineargs();
            dsData = XmlReader.ReadXml(ConfigurationManager.AppSettings["appName"]);
            //dsData = ExcelReader.Excel_Reader(ConfigurationManager.AppSettings["appName"]);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            foreach (var item in Reports.ModulesDetails.Values)
            {
                Reports.TestModulesAddReport(item.ToString().Split('$'));
            }

            foreach (var item in Reports.BrowserSummaryList.Values)
            {
                Reports.html_body_string += item.ToString();
            }

            Reports.HightReport();

            if (TestCategoryValue)
            {
                Reports.CreateMasterReport();
            }

            string startPath = Reports.TestReport;
            string zipPath = Reports.TestReport + ".zip";

            //--------------------------Send Mail functionality--------------------------//
            //ZipFile.CreateFromDirectory(startPath, zipPath);
            // CommonFunctions.SendEmail(zipPath);
            //CommonFunctions.SendEmail("startPath");

            //SeleniumActions.driver.Quit();
            //CommonFunctions.KillProcess("IEDriverServer");
            //CommonFunctions.KillProcess("chromedriver");

            
        }
    }
}
