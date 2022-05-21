using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engage.Automation.Utilities;

namespace Engage.Automation.Modules
{

    [TestClass]
    public class Admin
    {
        public Admin()
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



        /*****************************Verify Engage Accounts Tab under Administration Module************/


        //...................Verify Add User Accounts in Administration..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_01_Create_Account()
        {
            testCaseExecution(TestContext.TestName,TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        //...................Verify search Account..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_02_Search_Account()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());
        }


        //...................Verify Add User Accounts in Administration..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_03_Add_User_In_Edit_Engage_Account()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());
        }


        //...................Verify Add User Accounts in Administration..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_04_Add_Teams_in_Engage_Account()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Add User Accounts in Administration..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_05_CreateRoles_in_Edit_Engage_Account()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify EDIT Engage Accounts in Administration..................//
        [TestMethod]        
        [TestCategory("Admin")]
        public void Admin_06_Edit_Engage_Account()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        //...................Verify View Engage Accounts in Administration..................//
        [TestMethod]       
        [TestCategory("Admin")]
        public void Admin_07_view_Engage_Accounts_Administration()
        {
            string environment = TestContext.Properties["environment"].ToString();
            string browser = TestContext.Properties["browser"].ToString();
            Automation.testCaseExecution.HybridMethodNew(TestContext.TestName, environment, browser);

            if (Reports.currentTestCaseFail)
            {
                Reports.currentTestCaseFail = false;
                Assert.Fail("Failed: " + Reports.failedMessage);
            }

        }

        /**************************Verify Ward tab under Administration Module****************************/


        //...................Verify creation of a new ward..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_08_Create_Ward()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Update Ward..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_09_Update_Ward()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Add and Update Engagements in an existing ward..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_10_Verify_Engagements()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Add and Update Access Group in an existing ward..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_11_Verify_Access_Groups()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        //...................Verify Verify add Integrations in existing Ward..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_12_Verify_Add_Integrations()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        /***********************Verify Reports tab under Administration module************************/

        //...................Verify Admin Fulfillment Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_13_Verify_Admin_fulfillment_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Usage Metrics Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_14_Verify_Usage_Metrics_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Impact Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_15_Verify_Impact_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Engagement benchmark Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_16_Verify_Engagement_benchmark_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Engagement details Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_17_Verify_Engagement_Details_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Forced Prize details Report..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_18_Verify_Forced_prize_report()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //..........................Verify_Admin_Reports_Section.....................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_19_Verify_Admin_Reports_Section()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        /***********************Verify User Look Up Tab under Administration Module*************/

        //...................Verify that all tabs present under Administration Module..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_20_Verify_tabs_presence()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify User Look Up Tab under Administration module..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_21_Verify_Administration_UserLookup()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify User Look Up by Account and Email..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_22_Verify_UserLookup_by_Account_and_Email()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...................Verify Add User Validations..................//
        [TestMethod]
        [TestCategory("Admin")]
        public void Admin_23_Verify_Add_User_Validations()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //..................Verify User Account Settings....................//
        [TestMethod]
        [TestCategory("General")]
        public void Admin_24_Verify_Account_Settings()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //.....................Verify list of wards in client selection..........................................................//
        [TestMethod]
        [TestCategory("General")]
        public void Admin_25_Verify_ListOfWards()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //.....................Verify left navigation panel should be updated based on client/ward selection.....................//
        [TestMethod]
        [TestCategory("General")]
        public void Admin_26_Verify_Client_NavigationMenu()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

       
    }
}
