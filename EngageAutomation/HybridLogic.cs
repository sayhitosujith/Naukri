using System;
using System.Linq;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;
using Engage.Automation.Utilities;
using Engage.Automation.Operations;
using System.Configuration;
using Microsoft.CSharp.RuntimeBinder;
using OpenQA.Selenium;
using com.sun.tools.@internal.xjc;
using OpenQA.Selenium.Support.UI;

namespace Engage.Automation
{
    class testCaseExecution
    {
        static int numTotalTestCases = 0;
        public static string sBrowserName = string.Empty;
        public static string sModuleName = string.Empty;
        public static string sModuleDesc = string.Empty;
        public static string sRunningTestCaseName = string.Empty;
        public static string sRunningTestCaseDesc = string.Empty;

        public static string sTestDataID = string.Empty;
        public static string sTestStepID = string.Empty;
        static string sTestStepDesc = string.Empty;
        static string sKeyword = string.Empty;
        static string sDataColumnName = string.Empty;

        internal static void HybridMethodNew(string testName)
        {
            throw new NotImplementedException();
        }

        static string sDataColumnValue = string.Empty;
        static string sVerification = string.Empty;
        public static string sProceedOnFail = string.Empty;
        public static bool bProceedOnFail = true;
        static bool bObjectFound = false;
        public static string TestCaseFound = string.Empty;

        static string sObject = string.Empty;
        static string sXPathValue = string.Empty;
        static string sXPathValueVerification = string.Empty;

        static string Browser_StartTime;
        static string Browser_EndTime;
        static string TestModule_StartTime;
        static string TestModule_EndTime;
        static string TestCase_StartTime;
        static string TestCase_EndTime;
        static string TestData_StartTime;
        static string TestData_EndTime;

        static DataTable dtTestCases = new DataTable();
        static DataTable dtTestData = new DataTable();
        static DataTable dtTestObjects = new DataTable();
        public static string SystemRun;
        static Stopwatch stopWatch = new Stopwatch();
        private static IJavaScriptExecutor driver;
        public static string email = "";
        public static string accountname = "";

        /*______________Method for running the test logic for all test methods_____________________*/
        public static void HybridMethodNew(string testName, string environment, string browser)
        {
            try
            {
                string[] module_testcasename = testName.Split(new[] { '_' }, 2);

                DataRow drModule = TestLogic.dsData.Tables["TestModules"].AsEnumerable()
                                              .Where(a => a.Field<string>("Name").Trim() == module_testcasename[0] &&
                                                     (a.Field<string>("Run").Trim().ToUpper() == "YES" ||
                                                     a.Field<string>("Run").Trim().ToUpper() == "Y" ||
                                                     a.Field<string>("Run").Trim().ToUpper() == "R"))
                                                     .Select(m => m).FirstOrDefault();

                TestLogic.UtcStartTime = DateTime.UtcNow;

                sModuleName = drModule.Field<string>("Name").Trim();

                if (!string.IsNullOrEmpty(sModuleName))
                {
                    dtTestCases = TestLogic.dsData.Tables["TestCases"];

                    sRunningTestCaseName = dtTestCases.AsEnumerable()
                                            .Where(a => a.Field<string>("Name").Trim() == module_testcasename[1] &&
                                                   (a.Field<string>("Run").Trim().ToUpper() == "YES" ||
                                                    a.Field<string>("Run").Trim().ToUpper() == "Y" ||
                                                    a.Field<string>("Run").Trim().ToUpper() == "R"))
                                            .Select(c => c.Field<string>("Name").Trim()).FirstOrDefault();


                    if (!string.IsNullOrEmpty(sRunningTestCaseName))
                    {
                        sModuleDesc = drModule.Field<string>("Description").Trim();

                        DataTable dtBrowsers = TestLogic.dsData.Tables["TestBrowsers"];
                        DataTable dtReportSettings = TestLogic.dsData.Tables["TestSettings"];

                        //var browserList = getListPostFiltering(dtBrowsers, "Run");

                        Reports.ProjectName = Reports.setReportDetails(dtReportSettings, "ProjectName");
                        Reports.UserReq = Reports.setReportDetails(dtReportSettings, "UserRequested");
                        Reports.TestEnv = Reports.setReportDetails(dtReportSettings, "Environment");
                        Reports.Release1 = Reports.setReportDetails(dtReportSettings, "Release");
                        SystemRun = Reports.setReportDetails(dtReportSettings, "SystemRun");
                        Reports.ChromeWaitPercentage = Reports.setReportDetails(dtReportSettings, "ChromeWaitPercentage");
                        Reports.FirefoxWaitPercentage = Reports.setReportDetails(dtReportSettings, "FirefoxWaitPercentage");

                        // .....................foreach browser........................//
                        //foreach (var browser in browserList)
                        //{
                            sProceedOnFail = string.Empty;
                            bProceedOnFail = true;

                            stopWatch.Start();
                            TestCaseFound = "";

                            Browser_StartTime = "";
                            Browser_StartTime = DateTime.Now.ToString();
                            //sBrowserName = browser["Name"].ToString().Trim();
                            sBrowserName = browser;

                        sRunningTestCaseDesc = dtTestCases.AsEnumerable()
                                                .Where(a => a.Field<string>("Name").Trim() == sRunningTestCaseName &&
                                                       (a.Field<string>("Run").Trim().ToUpper() == "YES" ||
                                                        a.Field<string>("Run").Trim().ToUpper() == "Y" ||
                                                        a.Field<string>("Run").Trim().ToUpper() == "R"))
                                                .Select(c => c.Field<string>("Description").Trim()).FirstOrDefault();

                            TestModule_StartTime = "";
                            TestModule_StartTime = DateTime.Now.ToString();

                            dtTestObjects = TestLogic.dsData.Tables["TestObjects"];

                            numTotalTestCases++;
                            TestCase_StartTime = "";
                            TestCase_StartTime = DateTime.Now.ToString();

                            /*.............................................Enviornment Setup..................................*/
                            TestCaseFound = "found";

                            if (SystemRun == "UAT")
                            {
                                dtTestData = TestLogic.dsData.Tables["TestData_UAT"].AsEnumerable()
                                                            .Where(a => a.Field<string>("TestCaseName").Trim().Equals(sRunningTestCaseName))
                                                            .Select(a => a).CopyToDataTable();
                            }
                            else if (SystemRun == "QA")
                            {
                                dtTestData = TestLogic.dsData.Tables["TestData_UATLab"].AsEnumerable()
                                                            .Where(a => a.Field<string>("TestCaseName").Trim().Equals(sRunningTestCaseName))
                                                            .Select(a => a).CopyToDataTable();
                            }
                            else
                            {
                                dtTestData = TestLogic.dsData.Tables["TestData_DEV"].AsEnumerable()
                                                            .Where(a => a.Field<string>("TestCaseName").Trim().Equals(sRunningTestCaseName))
                                                            .Select(a => a).CopyToDataTable();
                            }


                            var dataList = getListPostFiltering(dtTestData, "Run");

                            // foreach test data
                            foreach (var dataRow in dataList)
                            {
                                TestData_StartTime = DateTime.Now.ToString();
                                sTestDataID = dataRow["ID"].ToString();

                                DataTable dtTestDataDetails;
                                if (SystemRun == "UAT")
                                {
                                    dtTestDataDetails = TestLogic.dsData.Tables["TestDataDetails_UAT"].AsEnumerable()
                                                           .Where(a => a.Field<string>("ID").Trim().Equals(sTestDataID.Trim()))
                                                           .Select(a => a).CopyToDataTable();
                                }
                                else if (SystemRun == "QA")
                                {
                                    dtTestDataDetails = TestLogic.dsData.Tables["TestDataDetails_UATLab"].AsEnumerable()
                                                           .Where(a => a.Field<string>("ID").Trim().Equals(sTestDataID.Trim()))
                                                           .Select(a => a).CopyToDataTable();
                                }
                                else
                                {
                                    dtTestDataDetails = TestLogic.dsData.Tables["TestDataDetails_DEV"].AsEnumerable()
                                                           .Where(a => a.Field<string>("ID").Trim().Equals(sTestDataID.Trim()))
                                                           .Select(a => a).CopyToDataTable();
                                }
                                DataTable dtTestCaseSteps = TestLogic.dsData.Tables["TestSteps"].AsEnumerable()
                                                        .Where(a => a.Field<string>("Name").Trim().Equals(sRunningTestCaseName))
                                                        .Select(a => a).CopyToDataTable();

                                var stepsList = getListPostFiltering(dtTestCaseSteps, "Run");

                                // foreach test step
                                foreach (var step in stepsList)
                                {
                                    sTestStepID = step["ID"].ToString().Trim();
                                    sTestStepDesc = step["Description"].ToString().Trim();
                                    sKeyword = step["Keyword"].ToString().ToUpper().Trim();
                                    sObject = step["Object"].ToString().Trim();
                                    sDataColumnName = step["DataColumnName"].ToString().Trim();
                                    //sVerification = step["Verification"].ToString().Trim();
                                    sProceedOnFail = step["ProceedOnFail"].ToString().Trim();
                                    sXPathValue = string.Empty;
                                    sXPathValueVerification = string.Empty;
                                    bObjectFound = false;

                                    DataRow[] rows = dtTestDataDetails.Select("Name LIKE '" + step["DataColumnName"].ToString().Trim() + "'");
                                    if (rows != null)
                                    {
                                        if (rows.Count() > 0)
                                        {
                                            switch (rows[0]["Value"].ToString())
                                            {                                                
                                                case "Username":
                                                    sDataColumnValue = ConfigurationManager.AppSettings["username"];
                                                    break;

                                                case "Password":
                                                    sDataColumnValue = ConfigurationManager.AppSettings["password"];
                                                    break;                                                

                                                default:
                                                    sDataColumnValue = rows[0]["Value"].ToString().Trim();
                                                    break;
                                            }
                                        }
                                    }

                                    for (int count = 0; count < dtTestObjects.Rows.Count; count++)
                                    {
                                        if (dtTestObjects.Rows[count]["Name"].ToString().Trim() == sObject)
                                        {
                                            sXPathValue = dtTestObjects.Rows[count]["XPathValue"].ToString();
                                            bObjectFound = true;
                                        }

                                        if (bObjectFound)
                                            break;
                                    }

                                    if (bProceedOnFail)
                                    {
                                        if (bObjectFound == false && sKeyword != "WAIT" && sKeyword != "WAITEQUALLY" && sKeyword != "ENTER_TEXT" && sKeyword != "ENTERKEY" && sKeyword != "BROWSER" && sKeyword != "NAVIGATETO" && sKeyword != "REFRESH" && sKeyword != "SWITCH_TO_NEWTAB" && sKeyword != "CLOSE_TAB" && sKeyword != "CLOSE_BROWSER" && sKeyword != "VERIFYPDF" && sKeyword != "VERIFYNOVALIDATION" && sKeyword != "VERIFY_CLOSEDIALOG" && sKeyword != "VERIFYREVIEWERS" && sKeyword != "VERIFYDUEDATEASC" && sKeyword != "VERIFYDUEDATECHK" && sKeyword != "VERIFYEVENTDETAILS" && sKeyword != "VERIFYHCODETAILS" && sKeyword != "VERIFYPROGRAMDETAILS" && sKeyword != "VERIFYRWROPTIONS" && sKeyword != "VERIFYADDRWR"
                                            && sKeyword != "STARTREVIEW_AE" && sKeyword != "AEOBSCHANGES" && sKeyword != "SIGOBSCHANGES" && sKeyword != "SUBMIT_PSP_DUE_CHIP" && sKeyword != "SUBMIT_PSP_DUE_CHOC" && sKeyword != "FOLLOWUPEVENTCHECK" && sKeyword != "ACCEPTSAVENON_EPS" && sKeyword != "SELECTFIRSTVALUE" && sKeyword != "SUBMIT_PSP_DUE_AD" && sKeyword != "SUBMIT_PSP_DUE" && sKeyword != "BEMYSELF" && sKeyword != "IMPERSONATE_ASSIGNEDSTAFF" && sKeyword != "IMPERSONATE" && sKeyword != "ENTER_SENDQUESTION" && sKeyword != "VERIFY_MANUALRULES" && sKeyword != "CLICK_ON_LINK" && sKeyword != "CLICK_ON_EVENT" && sKeyword != "CLICKONMEVTID" && sKeyword != "NAVIGATETOSTOREDURL" && sKeyword != "VERIFYREVIEWERS_LK" && sKeyword != "VERIFYRWROPTIONS_LK" && sKeyword != "VERIFYACOPTIONS" && sKeyword != "VERIFYMANRULES" && sKeyword != "VERIFYPAGEURL" && sKeyword != "STOREPROGDETAILS_ES2" && sKeyword != "STOREPROGDETAILS_ES" && sKeyword != "VERIFYPROGRAMS_DA" && sKeyword != "VERIFYPROGRAMS_MR" && sKeyword != "STORE_DETAILS_SP" && sKeyword != "VERIFYEVENTMSG_CONOTES")
                                            Reports.Report_TestDataStep(SeleniumActions.driver, sTestStepID, "'" + sObject + "' - object does not exist in the ObjRep sheet '" + sModuleName + "'", sKeyword, sDataColumnName, sDataColumnValue, "Fail", sRunningTestCaseName);

                                        if (sTestStepDesc.Length <= 0)
                                        {
                                            if (step["DataColumnName"].ToString().Trim().Length > 0)
                                                sTestStepDesc = "Performed " + sKeyword + " operation on " + step["Object"].ToString().Trim() + " with " + step["DataColumnName"].ToString().Trim();
                                            else
                                                sTestStepDesc = "Performed " + sKeyword + " operation on " + step["Object"].ToString().Trim();
                                        }

                                        /*------------------Keywords/Methods-----------------------------*/
                                        switch (sKeyword)
                                        {
                                        case "BROWSER":
                                            switch (environment)
                                            {
                                                case "QA":
                                                    sDataColumnValue = ConfigurationManager.AppSettings["qa_url"];
                                                    break;
                                                case "CI":
                                                    sDataColumnValue = ConfigurationManager.AppSettings["ci_url"];
                                                    break;
                                                case "STG":
                                                    sDataColumnValue = ConfigurationManager.AppSettings["stg_url"];
                                                    break;

                                                default:
                                                    Reports.currentTestCaseFail = true;
                                                    Reports.failedMessage = "Provided invalid environment: "+environment;
                                                    throw new Exception("Provided invalid environment: " + environment);                                                    

                                            }                                            
                                            SeleniumActions.InvokeBrowser(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "NAVIGATETO":
                                            SeleniumActions.NavigateTo(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;                                        

                                        case "CLICK":
                                            SeleniumActions.Click_on(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICK_CLIENTCONTROL":
                                            SeleniumActions.Click_ClientControl(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYIFRAMEELEMENT":
                                            SeleniumActions.verify_element_iframe(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "DOWNLOAD_FILE":
                                            SeleniumActions.Download_File(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ENTERKEY":
                                            SeleniumActions.ENTERKEY(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SCROLLUP":
                                            SeleniumActions.Scroll_Up(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SCROLL_DOWN":
                                            SeleniumActions.Scroll_Down(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICKFRAMEELEMENT":
                                            SeleniumActions.Click_on_frameelement(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "JSCLICK":
                                            SeleniumActions.JSClick_on(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICKTAB":
                                            SeleniumActions.ClickTab_NextLink = getObjectValues(sObject + "1");
                                            SeleniumActions.ClickTab_TabToDisplay = getObjectValues(sObject + "2");

                                            SeleniumActions.Click_Tab(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICKTAB2":
                                            SeleniumActions.ClickTab_NextLink = getObjectValues(sObject + "1");
                                            SeleniumActions.ClickTab_TabToDisplay = getObjectValues(sObject + "2");                                            
                                            break;                                        


                                        case "GETTEXT":
                                            SeleniumActions.Get_Text(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "MOUSE_HOVER":
                                            SeleniumActions.Mouse_Hover(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "WAIT":
                                            SeleniumActions.Wait_For(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "WAIT_FOR_CONDITION":
                                            SeleniumActions.Wait_For_Condition(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "WAITEQUALLY":
                                            SeleniumActions.Wait_ForEqually(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SELECT":
                                            SeleniumActions.Select_value(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SELECT_FEATURE":
                                            SeleniumActions.Select_Feature(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SEARCH_ENGAGE_ACCOUNTS":
                                            SeleniumActions.Search_engage_Accounts(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VIEW_ENGAGE_ACCOUNTS_IS_ACCOUNT_ACTIVE":
                                            SeleniumActions.View_engage_Accounts_Is_Account_Active(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VIEW_ENGAGE_ACCOUNTS_WHATLOGIN_THEY_USE":
                                            SeleniumActions.View_engage_Whatlogin_do_they_use(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ACCOUNT_PASSWORD_EXPIRATION":
                                            SeleniumActions.Account_Password_Expiration(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SEARCH_LIST_OF_FEATURES":
                                            SeleniumActions.Search_list_of_Features(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY":
                                            SeleniumActions.verification_value(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "RADIO_BUTTON_IS_SELECTED":
                                            SeleniumActions.Radio_button_IsSelected(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "VERIFYTEXTINFRAME":
                                            SeleniumActions.verification_value_iframe(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "VERIFYELEMENT":
                                            SeleniumActions.Verify_Element(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_LIST_OF_FEATURES":
                                            SeleniumActions.Verify_list_of_Features(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLOSE_MULTISELECT_DROPDOWN":
                                            SeleniumActions.JSClose_multiselect_dropdown(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYNOELEMENT":
                                            SeleniumActions.Verify_No_Element(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "EDIT_ACCOUNT":
                                            SeleniumActions.Edit_Account(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ADD_USER":
                                            SeleniumActions.Add_User(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ADD_USER_LIST":
                                            SeleniumActions.Add_User_List(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ADD_TEAM_LIST":
                                            SeleniumActions.Add_Team_List(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ADD_ROLES_LIST":
                                            SeleniumActions.Add_Role_List(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ADDUPDATEWARD":
                                            SeleniumActions.addUpdateWard(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "ADDENGAGEMENTS":
                                            SeleniumActions.addEngagements(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "ADDACCESSGROUP":
                                            SeleniumActions.addAccessGroup(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "UPDATEACCESSGROUP":
                                            SeleniumActions.updateAccessGroup(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "SEARCHWARD":
                                            SeleniumActions.searchWard(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "UPDATEENGAGEMENTS":
                                            SeleniumActions.updateEngagements(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "ADDINTEGRATION":
                                            SeleniumActions.addIntegrations(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;


                                        case "CREATE_ACOUNT":
                                            accountname = SeleniumActions.select_Values_for_AccountCreation(sBrowserName, sDataColumnValue, sTestStepID, sXPathValue, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "ENTER_NEWLY_ADDED_ACCOUNT":
                                            SeleniumActions.Enter_Newly_Added_Account(accountname, sBrowserName, sDataColumnValue, sTestStepID, sXPathValue, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "CREATE_USER":
                                            email = SeleniumActions.Select_Values_for_Users_Creation(sBrowserName, sDataColumnValue, sTestStepID, sXPathValue, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;


                                        case "CREATE_TEAM":
                                            SeleniumActions.select_Values_for_Teams_Creation(email, sBrowserName, sDataColumnValue, sTestStepID, sXPathValue, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "CREATE_ROLE":
                                            email = SeleniumActions.Select_Values_for_roles_Creation(sBrowserName, sDataColumnValue, sTestStepID, sXPathValue, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_LASTUPDATED_DATE":
                                            SeleniumActions.Verify_Last_Updated_Date(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_USERS":
                                            SeleniumActions.Verify_Users(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "VERIFY_TOTAL_TEAMS":
                                            SeleniumActions.Verify_Teams(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_TOTAL_ROLES":
                                            SeleniumActions.Verify_Roles(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;





                                        case "FOLLOWUPEVENTCHECK":
                                            SeleniumActions.FOLLOWUPEVENTCHECK(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SUBMIT_PSP_DUE":
                                            SeleniumActions.Submit_PSP_DUE(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SUBMITBTN_ESC60":
                                            SeleniumActions.SUBMITBTN_ESC60(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ESC60":
                                            SeleniumActions.ESC60(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SELECT_DATE":
                                            SeleniumActions.Select_Date(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "VERIFYPAGEURL":
                                            SeleniumActions.Verify_PageURL(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYUSERNAME":
                                            SeleniumActions.Verify_LoggedINUser(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYNOTEMPTY":
                                            SeleniumActions.Verification_NotEmpty(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYLIST":
                                            SeleniumActions.verification_list(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYNOVALIDATION":
                                            SeleniumActions.verification_NoValidation(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYPDF":
                                            SeleniumActions.verification_PDF(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFYDOCUMENTSLINKS":
                                            SeleniumActions.Verify_DocumentsLinks(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "STORE_DETAILS":
                                            SeleniumActions.Store_Details(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "STORE_DETAILS_SP":
                                            SeleniumActions.Store_Details_SaferPlac(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "SELECT_SEARCH_FILTER":
                                            SeleniumActions.Select_Search_Filter(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SEARCH_ENGAGEMENT_WITH_ID":
                                            SeleniumActions.Search_Engagement_Value(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;



                                        case "TOGGLE_ADVANCED_SEARCH":
                                            SeleniumActions.Toggle_Advance_Search(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "ENTER":
                                            SeleniumActions.Enter_Value(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "RUN_SP":
                                            SeleniumActions.Run_StoreProcedure(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ENTER_TEXT":
                                            SeleniumActions.Enter_Text_KeyBoard(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ENTERIFRAME":
                                            SeleniumActions.Enter_Value_iframe(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ENTERVALUEIFRAME":
                                            SeleniumActions.EnterValue_Iframe(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "ENTER_EMAIL":
                                            SeleniumActions.Enter_Value_EmailUpdate(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLEAR":
                                            SeleniumActions.Clear_Value(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SORTASCEND":
                                            SeleniumActions.ColumnSortBtn = getObjectValues(sObject + "1");

                                            SeleniumActions.SortColumnAscending(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "ENTER_VALUE_BYTAGNAME":
                                            SeleniumActions.Enter_Value_ByTagName(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICK_ON_LINK":
                                            SeleniumActions.Click_on_link(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLICK_ON_EVENT":
                                            SeleniumActions.Click_on_event(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "VERIFY_SEARCH_LINK":
                                            SeleniumActions.Verifying_search_link(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_LEFTNAV":
                                            SeleniumActions.Verify_leftNavigation(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SWITCH_TO_NEWTAB":
                                            SeleniumActions.SwitchToNewTab(sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_CLOSEDIALOG":
                                            SeleniumActions.Verify_CloseDialog(sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "GET_TOKENS":
                                            SeleniumActions.Get_Tokens(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "VERIFY_TOKENS":
                                            SeleniumActions.Verify_Tokens(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLOSE_TAB":
                                            SeleniumActions.Close_Tab(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "CLOSE_BROWSER":
                                            SeleniumActions.CloseBrowser(sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "HIGHLIGHT":
                                            SeleniumActions.highlightUsingXpath(SeleniumActions.driver, sXPathValue, sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                            break;

                                        case "DOWNLOAD_VERIFICATION":
                                            SeleniumActions.Download_Verification(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "LASTSECTIONCLICK":
                                            SeleniumActions.Lastsection_Click(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "EXPORT_TO_CSV":
                                            SeleniumActions.Export_File(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;


                                        case "CHECKFILEDOWNLOADED":
                                            SeleniumActions.CheckFileDownloaded(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "SENDMAIL":
                                            SeleniumActions.SendMail(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        //-------------------------------------------------------------------------------------//

                                        case "FILE_UPLOAD_SUJITH":
                                            SeleniumActions.File_Upload_SUJITH(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_DEEKSHITA":
                                            SeleniumActions.File_Upload_Deekshita(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_VINAY":
                                            SeleniumActions.File_Upload_Vinay(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_VIDHYA":
                                            SeleniumActions.File_Upload_VIDHYA(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_SHIV":
                                            SeleniumActions.File_Upload_SHIV(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_PAVAN":
                                            SeleniumActions.File_Upload_PAVAN(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_KARTHIK_MCA":
                                            SeleniumActions.File_Upload_KARTHIK_MCA(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_GOVIND_SAGAR":
                                            SeleniumActions.File_Upload_Govind_Sagar(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        case "FILE_UPLOAD_ANIL":
                                            SeleniumActions.File_Upload_Anil(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;
                                        //---------------------------------------------------------------------------------------//

                                        case "EXITAPPLICATION":
                                            SeleniumActions.EXITAPPLICATION(sBrowserName, sDataColumnValue, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValue, sXPathValueVerification, sRunningTestCaseName);
                                            break;

                                        default:
                                            Reports.Report_TestDataStep(SeleniumActions.driver, sTestStepID, "'" + sKeyword + "' does not exist, please provide correct Keyword.", sKeyword, sObject, sDataColumnValue, "Fail", sRunningTestCaseName);
                                            break;
                                    }

                                    /*--------------------------Java script to load page completely after each action-------------------------*/
                                    if (!(sKeyword == "CLOSE_BROWSER"))
                                    {
                                        IJavaScriptExecutor js = (IJavaScriptExecutor)SeleniumActions.driver;   
                                        WebDriverWait wait = new WebDriverWait(SeleniumActions.driver, new TimeSpan(0, 0, 40));
                                        wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
                                    }


                                    /*----------------------------------------------------------------------------------------------------------*/
                                }
                                    else
                                    {
                                        if (!(sKeyword == "CLOSE_BROWSER"))
                                        {
                                            Reports.Report_TestDataStep(SeleniumActions.driver, sTestStepID, sTestStepDesc, sKeyword, sObject, sXPathValueVerification, "Not Executed", sRunningTestCaseName);
                                        }
                                    }
                                    sDataColumnValue = "";
                                    sTestStepID = "";
                                    sTestStepDesc = "";
                                    sKeyword = "";
                                    sObject = "";
                                    sDataColumnName = "";
                                    sVerification = "";
                                    sProceedOnFail = "";
                                }

                                if (!bProceedOnFail)
                                {
                                    SeleniumActions.CloseBrowser(sTestStepID, sTestStepDesc, sKeyword, sRunningTestCaseName);
                                }

                                TestData_EndTime = DateTime.Now.ToString();
                                Reports.Report_TestData(TestData_StartTime, TestData_EndTime);
                            }

                            TestCase_EndTime = DateTime.Now.ToString();
                            Reports.Report_TestModule(TestCase_StartTime, TestCase_EndTime);

                            TestModule_EndTime = DateTime.Now.ToString();
                            Reports.Report_Browser(TestModule_StartTime, TestModule_EndTime);

                            Browser_EndTime = DateTime.Now.ToString();

                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            string tsr = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("000");

                            Reports.Report_Summary(Browser_StartTime, Browser_EndTime, tsr);

                            stopWatch.Restart();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Reports.Report_TestDataStep(SeleniumActions.driver, sTestStepID, ex.ToString(), sKeyword, sDataColumnName, sDataColumnValue, "Fail"+ex.Message, sRunningTestCaseName);
            }
        }

        /* ______________Filtering the Datasets_____________________*/
        static dynamic getListPostFiltering(DataTable dt, string searchByColumn)
        {
            var filteredList = dt.AsEnumerable().
                                Where(t => t.Field<string>(searchByColumn).Trim().ToUpper() == "YES" || t.Field<string>(searchByColumn).Trim().ToUpper() == "Y" || t.Field<string>(searchByColumn).Trim().ToUpper() == "R");
            return filteredList;
        }

        static string getObjectValues(string object_Name)
        {
            string objectValue = dtTestObjects.AsEnumerable().
                          Where(t => t.Field<string>("Name").ToString().Trim() == object_Name).Select(t => t.Field<string>("XPathValue")).FirstOrDefault();

            return objectValue;
        }

    }

    internal interface IWait<T>
    {
        void Until(Func<object, bool> p);
    }
}
