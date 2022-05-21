using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engage.Automation.Utilities;

namespace Engage.Automation.Modules
{

    [TestClass]
    public class Compliance
    {
        public Compliance()
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
            HybridLogic.HybridMethodNew(testName, environment, browser);

            if (Reports.currentTestCaseFail)
            {
                Reports.currentTestCaseFail = false;
                Assert.Fail("Failed: " + Reports.failedMessage);
            }
        }
        /*______________Testcases with Description__________________________________________________*/
        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_01_Verify_ComplianceModule_Presence()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_02_Verify_ComplianceModule_Absence()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_03_Verify_PendingRequests_In_PreviousRequests()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_04_Verify_CompletedRequests_In_PreviousRequests()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_05_Verify_Download_MarketersGuide()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_06_Verify_AbleToFind_ParticipantRecord()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_07_Verify_UnableToFind_ParticipantRecord()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_08_Verify_SubmitRequest_Removal()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        [TestMethod]
        [TestCategory("Compliance")]
        public void Compliance_09_Verify_BulkUpdate_ReviewPendingRequest()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


    }
}
