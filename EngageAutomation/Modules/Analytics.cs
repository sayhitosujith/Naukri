using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engage.Automation.Utilities;

namespace Engage.Automation.Modules
{

    [TestClass]
    public class Analytics
    {
        public Analytics()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        //DateTime dt = TestLogic.UtcStartTime;

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

        public void testCaseExecution(string testName, string environment, string browser)
        {
            Automation.testCaseExecution.HybridMethodNew(testName, environment, browser);

            if (Reports.currentTestCaseFail)
            {
                Reports.currentTestCaseFail = false;
                Assert.Fail("Failed: " + Reports.failedMessage);
            }
        }
        /*______________Testcases with Description__________________________________________________*/
        [TestMethod]
        [TestCategory("Analytics")]
        public void Analytics_01_Verify_Analytics_ClientOverview_Report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

    }
}
