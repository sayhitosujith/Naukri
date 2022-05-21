using Engage.Automation.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engage.Automation.Modules
{

    [TestClass]
    public class Naukri
    {
        public Naukri()
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

        //...........Automation of sujith Naukri Profile..................................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase1()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Vidhya Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase2()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }


        //...........Automation of Shiv Jojan - Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase3()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Pavan K P - Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase4()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Karthik MCA - Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase5()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Govinda Sagar - Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase6()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Vinay MCA- Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase7()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Deekshita- Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase8()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

        //...........Automation of Anil- Naukri Profile............................//
        [TestMethod]
        [TestCategory("SmokeTesting")]
        public void Naukri_SmokeTestcase9()
        {
            testCaseExecution(TestContext.TestName, TestContext.Properties["environment"].ToString(), TestContext.Properties["browser"].ToString());

        }

    }
}



  


    