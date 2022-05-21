using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CRDB.Automation.Modules
{
   
    [TestClass]
    public class CRDB
    {
        public CRDB()
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




        //...........Automation of Flipkart Application..................................//
        [TestMethod]
        [Priority(0)]
        [TestCategory("SmokeTesting")]
        public void CRDB_SmokeTestcase1()
        {
            HybridLogic.HybridMethodNew(TestContext.TestName);

        }


    }
}
