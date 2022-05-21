using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engage.Automation.Utilities;

namespace Engage.Automation.Modules
{

    [TestClass]
    public class Engagements
    {
        public Engagements()
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


        //.................... Validate Configurations section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_01_Verify_Engagements_Configuration()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Validate Change Engagements.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_02_Verify_Change_Engagements()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Validate Back button.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_03_Verify_Backbtn_Engagements()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Registration report in Engagement Reports.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_04_Verify_Registration_Report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the OptIn report in Engagement Reports.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_05_Verify_OptIn_Report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagement Summary in Details section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_06_Verify_Details_Summary()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Details section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_07_Verify_Details_Section()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Search for Featues List of Dropdown.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_08_Verify_Features_Dropdown()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search by selecting available Features.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_09_Verify_Search_By_Feature()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search with Column Search Filter with ID.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_10_Verify_Search_By_Filter_ID()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search with Column Search Filter with Feature.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_11_Verify_Search_By_Filter_Feature()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search with Column Search Filter with All filters.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_12_Verify_Search_By_Filter_All()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search with Column Search Filter with no filters.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_13_Verify_Search_By_Filter_None()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagement Action Configuration in Configuration section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_14_Verify_Configuration_Action()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagement Attributes Configuration in Configuration section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_15_Verify_Configuration_Attributes()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagement Integration Configuration in Configuration section.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_16_Verify_Configuration_Integration()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagements page with search fields.........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_17_Verify_Engagement_SearchFields()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify the Engagement search with Engagement ID..........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_18_Verify_Search_By_ID()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //....................Verify Engagement Reports in Report section..........................//
        [TestMethod]
        [TestCategory("Engagements")]
        public void Engagements_19_Verify_Engagements_Reports()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }
    }
}