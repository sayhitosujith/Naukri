using System;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using System.Data;
using System.Linq;
using System.Collections;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Engage.Automation.Utilities
{
    public class Reports
    {
        static int intCount = 1;
        public static int detailedReportFail = 0;
        public static bool currentTestCaseFail = false;
        public static string failedMessage = "";
        static int numModulesPass = 0;
        static int numModulesFail = 0;
        static int numTestCasePass = 0;
        static int numTestCaseFail = 0;
        static int FinalTestCasePass = 0;
        static int FinalTestCaseFail = 0;
        static int numTestDataFail = 0;
        static int numTestDataPass = 0;

        static int numOfModulesPass = 0;
        static int numOfModulesFail = 0;

        static int FinalStatus = 0;
        static TimeSpan TotalTestRanTime;
        public static Hashtable BrowserRunTime = new Hashtable();
        public static Hashtable BrowserModuleStartTime = new Hashtable();

        static string TestStartTime = "";
        static string EndTime;

        static string commonTime = replace(DateTime.Now.ToString("dd MMM yyyy")) + "_" + replace(DateTime.Now.ToString("T"));
        static string commonDate = replace(DateTime.Now.ToString("dd MMM yyyy"));
        static string HighReport_ResultsFolder = FolderPath() + "\\TestReports";
        public static string TestReport = HighReport_ResultsFolder + "\\TestReport_" + commonTime;
        static string finalName = "";

        static string file_Name = "";

        //hightreport file name
        static string HighReport_filename = TestReport + "\\TestReport.html";
        static string MasterReportPath = GetMasterFolderPath() + "\\MasterReports";
        static string MasterReportFile = MasterReportPath + "\\MasterReport_" + commonDate + ".html";

        static string html_header_string = "";
        public static string html_body_string = "";
        static string MasterStatusBGcolor = Status_Color("pass");
        static string MasterStatus = "Pass";
        static string StatusHighbgcolor;
        static string Status;
        static string browserfilepath = "";
        static string Report_Browser_Filename = "";
        static string testmodulefilepath = "";
        static string Report_TestModule_Filename = "";
        static string testdatafilepath = "";
      //  static string Report_TestData_Filename = "";
        static string testcasefilepath = "";
        static string Report_TestCase_Filename = "";
        //static string testdatastepsfilepath = "";
        static string DetailedReport_filename = "";

        static string StatusDetbgcolor = "#BCE954";
        static string module = "";

        static string stylesString = "<style>" + 
                                    "table {border-collapse: collapse;width: 100%;}" +
                                    "td, th {height: 2rem;border: 1px solid #ccc;text-align: center;}" +
                                    "th {background: lightblue;border-color: white;}" +
                                    "tr:nth-child(even){background-color: #f2f2f2}" +
                                    "body {padding: 1rem;}" +
                                    "</style>";

    

        static string homepageStyles = "<style>" +
                                        ".TestDetails {border-collapse: collapse;margin-top: -20px !important;height: 235px;}" +
                                        ".TestDetails td {border: 1px solid #ccc;text-align: left;padding: 7px;}" +
                                        ".TestDetails th {border: 1px solid #ccc;text-align: left;padding: 7px;}" +
                                        ".TestDetails td:first-child {width: 150px;}" +
                                        ".TestDetails th {background: lightblue;border-color: white;}" +
                                        ".TestDetails tr:nth-child(even) {background-color: #f2f2f2}" +
                                        "body {padding: 1rem;}" +
                                        ".DetailedReport {border-collapse: collapse;}" +
                                        ".TopBottom {display: block;clear: both;padding-top: 5px;}" +
                                        ".DetailedReport td, th {width: 4rem;height: 2rem;border: 1px solid #ccc;text-align: center;}" +
                                        ".DetailedReport th {background: lightblue;border-color: white;}" +
                                        ".DetailedReport tr:nth-child(even) {background-color: #f2f2f2}" +
                                        "body {padding: 1rem;}" +
                                        "h3.heading {padding: 0px;margin: 0px;margin-bottom: 15px;}" +
                                        ".topLeft {width: 460px;float: left;}" +
                                        ".topRight {margin-left: 480px;}" +
                                        ".Passdiv {background-color: #006633;width: 30px;vertical-align: middle;text-align: center;display: inherit;}" +
                                        ".Faildiv {background-color: #FF0000;width: 30px;vertical-align: middle;text-align: center;display: inherit;}" +
                                        ".ResultSummary {border-collapse: collapse;width: 100%;margin-top: -36px !important;border: 1px solid #d0d0d0;}" +
                                        ".td02 {padding-left: 80px;float: left;height: 195px;}" +
                                        ".t08 {padding-left: 20px;float: left;vertical-align: bottom;height: 195px;}" +
                                        ".PercentageBarFont {color: white;font-weight: bold;}" +
                                        ".t06 {float: left;background-color: antiquewhite;padding: 5px;margin-left: 10px;}" +
                                        "</style>";
        public static string ProjectName;
        public static string Release1;
        public static string UserReq;
        public static string TestEnv;

        public static string ChromeWaitPercentage;
        public static string FirefoxWaitPercentage;

        public static Hashtable ModulesDetails = new Hashtable();
        public static Hashtable ModulesBrowser = new Hashtable();
        public static Hashtable BrowserList = new Hashtable();
        public static Hashtable BrowserSummaryList = new Hashtable();

        /* ______________Setting Report Details_____________________*/
        public static string setReportDetails(DataTable dt, string settingsKey)
        {
            var settingsValue = (dt.AsEnumerable().Where(s => s.Field<string>("Name") == settingsKey).Select(s => s.Field<string>("Value"))).FirstOrDefault();
            return settingsValue.ToString();
        }

        /* ______________Dynamic Folder Path of the Project_____________________*/
        private static string FolderPath()
        {
            //string path = ConfigurationManager.AppSettings["report_location"];

            //if (path.Equals(""))
            //path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            Console.WriteLine("FolderPath = " + path);

            return path;
        }

        /* ______________Dynamic Folder Path of the Project_____________________*/
        private static string GetMasterFolderPath()
        {
            //string path = ConfigurationManager.AppSettings["masterreport_location"];

            //if (path.Equals(""))
            //path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            Console.WriteLine("GetMasterFolderPath = "+ path);

            return path;
        }

        /* ______________Replacing special characters in DateTime string_____________________*/
        public static string replace(string date)
        {
            StringBuilder b = new StringBuilder(date);

            b.Replace("-", "_");
            b.Replace(" ", "_");
            b.Replace(":", "_");
            b.Replace("/", "_");
            return b.ToString();
        }

        /* ______________Creating the folders structure for saving the report_____________________*/
        public static void folders()
        {
            if (!Directory.Exists(HighReport_ResultsFolder))
            {
                Directory.CreateDirectory(HighReport_ResultsFolder);
            }

            if (!Directory.Exists(TestReport))
            {
                Directory.CreateDirectory(TestReport);
            }
        }

        /* ______________Capturing the Screenshot_____________________*/
        public static string CaptureTestImage(string browser, IWebDriver driver, string tcid, string tsid)
        {
            folders();
            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

                file_Name = browser + "_" + testCaseExecution.sTestDataID + "_" + testCaseExecution.sTestStepID + ".Gif";

                finalName = TestReport + "\\" + file_Name;
                ss.SaveAsFile(finalName, OpenQA.Selenium.ScreenshotImageFormat.Gif);
            }
            catch { }
            return finalName;
        }

        /* ______________Creating the Dashboard of the Report_____________________*/
        public static void HightReport()
        {
            folders();
            DateTime time = DateTime.Now;
            string format = "d MMM yyyy";
            string date = time.ToString(format);

            double successRate = 0;
            double failRate = 0;

            double passwidth = 0;
            double failwidth = 0;



            int NumTotal = FinalTestCasePass + FinalTestCaseFail;
            if (NumTotal != 0)
            {
                successRate = (FinalTestCasePass * 100 / (NumTotal));
                successRate = Math.Round(successRate, 2);

                failRate = 100 - successRate;
                passwidth = (150 * successRate) / 100;
                failwidth = 150 - passwidth;
            }

            double successRateMod = 0;
            double failRateMod = 0;

            double passwidthMod = 0;
            double failwidthMod = 0;

            foreach (var item in ModulesDetails.Values)
            {
                string moduleStatus = item.ToString().Split('$')[8];
                if (moduleStatus == "Pass")
                {
                    numOfModulesPass++;
                }
                else
                {
                    numOfModulesFail++;
                }
            }

            int NumTotalMod = numOfModulesPass + numOfModulesFail;
            if (NumTotalMod != 0)
            {
                successRateMod = (numOfModulesPass * 100 / (NumTotalMod));
                successRateMod = Math.Round(successRateMod, 2);

                failRateMod = 100 - successRateMod;
                passwidthMod = (150 * successRateMod) / 100;
                failwidthMod = 150 - passwidthMod;
            }

            if (FinalStatus > 0)
            {
                Status = "Fail";
                StatusHighbgcolor = Status_Color("fail");
            }
            else
            {
                Status = "Pass";
                StatusHighbgcolor = Status_Color("pass");
            }

            html_header_string = "<html><HEAD><TITLE>Test Automation Report - " + ProjectName + "</TITLE>" + homepageStyles +     
                                 "</HEAD><body><h4 align=\"center\"><FONT COLOR=\"435ecc\" FACE=\"Arial\" SIZE=6><b> Test Automation Report - " + ProjectName + 
                                 "</b></h4>" +
                                 "<div class=\"topLeft\"><span></span> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=4.5> <h4>Test Details :</h4> </FONT>" +
                                 "<table class=\"TestDetails\" width=\"100%\" border=1 cellspacing=1 cellpadding=1><tr><td bgcolor=\"#153E7E\" style=\"padding-top:-30px;\">" +
                                 "<FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Run Date</b></td><td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" +
                                 date + "</b></td></tr>";

             

            if (!File.Exists(HighReport_filename))
            {
                FileStream fs = File.Create(HighReport_filename);
                fs.Close();

                using (TextWriter tw = new StreamWriter(HighReport_filename, true))
                {
                    tw.WriteLine(html_header_string);

                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>User Requested</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + UserReq + "</b></td></tr>");

                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Environment</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + TestEnv + "</b></td></tr>");

                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Test Start Time</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + TestStartTime + "</b></td></tr>");

                    EndTime = DateTime.Now.ToString();
                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Test End Time</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + EndTime + "</b></td></tr>");

                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Total Test Ran Time</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + TotalTestRanTime.Hours.ToString("00") + ":" + TotalTestRanTime.Minutes.ToString("00") + ":" + TotalTestRanTime.Seconds.ToString("00") + "." + TotalTestRanTime.Milliseconds.ToString("000") + "</b></td></tr>");

                    tw.WriteLine(" <tr><td bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2.75><b>Release</b></td>");
                    tw.WriteLine(" <td><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2.75><b>" + Release1 + "</b></td></tr></table></div>");

                    tw.WriteLine("<div class=\"topRight\"><h4> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=4.5> Test Result Summary :</h4>");

                    tw.WriteLine("<table class =\"ResultSummary\">");
                    tw.WriteLine("<tr>");
                    tw.WriteLine("<td width=50% height=\"234px\" style=\"padding: 3px; border-right: double 2px lightgrey; vertical-align:text-top; \" align=\"center\">");

                    tw.WriteLine("<h3 class=\"heading\"> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=3.5> Test Modules Summary</h3>");

                    tw.WriteLine("<table class=\"t06\" border=0 cellspacing=1 cellpadding=1 width=\"50\">");

                    tw.WriteLine(" <tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Modules</b></td>");
                    tw.WriteLine(" <td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">: ");
                    tw.WriteLine(NumTotalMod + "</b></td></tr>");

                    tw.WriteLine(" <tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Pass</b></td>");
                    tw.WriteLine(" <td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">: ");
                    tw.WriteLine(numOfModulesPass + "</b></td></tr>");

                    tw.WriteLine(" <tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Fail</b></td>");
                    tw.WriteLine(" <td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">:  ");
                    tw.WriteLine(numOfModulesFail + "</b></td></tr></table>");

                    tw.WriteLine(" <table class=\"t07 td02\" border=0 cellspacing=1 cellpadding=1><tr><td valign=\"bottom\"><b>" + successRateMod + "%</b><div class=\"Passdiv\" style=\"height: " + passwidthMod + "px; \"><span class=\"PercentageBarFont\"> " + numOfModulesPass + "</span></div></td></tr>");

                    tw.WriteLine(" <tr><td style=\"height:1px;\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Pass</b></td></tr></table>");

                    tw.WriteLine(" <table class=\"t08\" border=0 cellspacing=1 cellpadding=1><tr><td align = \"center\" valign=\"bottom\"><b>" + failRateMod + "%</b><div class=\"Faildiv\" style=\"height: " + failwidthMod + "px; \"><span class=\"PercentageBarFont\">" + numOfModulesFail + "</span></div></td></tr>");

                    tw.WriteLine("<tr><td style=\"height:1px;\" align=\"center\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Fail</b></td></tr></table>");

                    tw.WriteLine("</td>");

                    tw.WriteLine("<td width=50% style=\"padding: 3px; vertical-align:text-top;\" align=\"center\"><h3 class=\"heading\"> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=3.5> Test Cases Summary</h3>");
                    tw.WriteLine("<table class=\"t06\" border=0 cellspacing=1 cellpadding=1 width=\"50\"><tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Cases</b></td>");
                    tw.WriteLine("<td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">: " + NumTotal + "</b></td></tr>");
                    tw.WriteLine("<tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Pass</b></td>");
                    tw.WriteLine("<td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">: " + FinalTestCasePass + "</b></td></tr>");
                    tw.WriteLine("<tr><td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Total Fail</b></td>");
                    tw.WriteLine("<td align=\"left\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">: " + FinalTestCaseFail + "</b></td></tr></table>");
                    tw.WriteLine("<table class=\"td02\" border=0 cellspacing=1 cellpadding=1><tr><td valign=\"bottom\" align=\"center\"><b>" + successRate + "%</b>");
                    tw.WriteLine("<div class=\"Passdiv\" style=\"height: " + passwidth + "px; \"><span class=\"PercentageBarFont\">" + FinalTestCasePass + "</span></div></td></tr>");
                    tw.WriteLine("<tr><td valign=\"bottom\" style=\"height: 1px; \" align=\"center\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Pass</b></td></tr></table>");
                    tw.WriteLine("<table class=\"t08\" border=0 cellspacing=1 cellpadding=1><tr><td valign=\"bottom\" align=\"center\"><b>" + failRate + "%</b>");
                    tw.WriteLine("<div class=\"Faildiv\" style=\"height: " + failwidth + "px; \"><span class=\"PercentageBarFont\">" + FinalTestCaseFail + "</span></div></td></tr>");
                    tw.WriteLine("<tr><td valign=\"bottom\" style=\"height: 1px; \" align=\"center\"><FONT COLOR=\"#000066\" FACE=\"Arial\" SIZE=2.75><b style=\"white-space:nowrap;\">Fail</b></td></tr></table></td></tr></table></div>");

                    tw.WriteLine(" <div class=\"TopBottom\"> <h4> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=4.5>  Detailed Test Report :</h4>");

                    tw.WriteLine("<table class =\"DetailedReport\" border=1 cellspacing=1 cellpadding=1 width=100% align='center'><tr>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Browser</b></td>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Execution Time</b></td>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Status</b></td>");

                    tw.WriteLine(" </tr>");

                    tw.WriteLine(html_body_string);

                    tw.WriteLine("</table></div></body></html>");

                    tw.Close();
                }
            }

        }

        /* ______________Creating the Report Summary of the Report_____________________*/
        public static void Report_Summary(string TestType_StartTime, string TestType_EndTime, string tsr)
        {
            if (TestStartTime == "")
            {
                TestStartTime = TestType_StartTime;
            }

            if (Convert.ToInt16(BrowserList[testCaseExecution.sBrowserName]) > 0)
            {
                Status = "Fail";
                StatusHighbgcolor = Status_Color("fail");

                MasterStatus = "Fail";
                MasterStatusBGcolor = Status_Color("fail");
            }
            else
            {
                Status = "Pass";
                StatusHighbgcolor = Status_Color("pass");
            }

            string browserHTMLBody = "<tr><td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href=" + testCaseExecution.sBrowserName + ".html" + ">" + testCaseExecution.sBrowserName + "</a></b></td>" +
                                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + BrowserRunTime[testCaseExecution.sBrowserName] + "</b></td>" +
                                    "<td width=20% align=\"center\" bgcolor=" + StatusHighbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td></tr>";
            BrowserSummaryList[testCaseExecution.sBrowserName] = browserHTMLBody;


            //html_body_string = html_body_string + "<tr><td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href=" + HybridLogic.sBrowserName + "_" + commonTime + ".html" + ">" + HybridLogic.sBrowserName + "</a></b></td>" +
            //                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + tsr + "</b></td>" +
            //                    "<td width=20% align=\"center\" bgcolor=" + StatusHighbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td></tr>";

            TotalTestRanTime = TotalTestRanTime + TimeSpan.Parse(tsr);
        }

        /* ______________Creating the Report Browser Page of the Report_____________________*/
        public static void Report_Browser(string TestModule_StartTime, string TestModule_EndTime)
        {
            string rstatus = "Pass";

            folders();
            browserfilepath = testCaseExecution.sBrowserName + ".html";
            Report_Browser_Filename = TestReport + "\\" + browserfilepath;

            int tc_PassPerModule = Convert.ToInt16(ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Pass"]);
            int tc_FailPerModule = Convert.ToInt16(ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"]);

            if (Convert.ToInt16(ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"]) > 0)
            {
                rstatus = "Fail";
                StatusDetbgcolor = Status_Color("fail");
                numModulesFail++;
                FinalStatus++;
                BrowserList[testCaseExecution.sBrowserName] = 1;
            }
            else
            {
                StatusDetbgcolor = Status_Color("pass");
                numModulesPass++;
            }

            if (!File.Exists(Report_Browser_Filename))
            {
                FileStream fs = File.Create(Report_Browser_Filename);
                fs.Close();

                using (TextWriter rb = new StreamWriter(Report_Browser_Filename, true))
                {
                    rb.WriteLine("<html><HEAD><TITLE> Browser Report</TITLE>" + stylesString + "</HEAD><body>");
                    rb.WriteLine("<h4> <FONT COLOR=660000 FACE = Arial  SIZE = 4.5>" + testCaseExecution.sBrowserName + "</h4>");
                    rb.WriteLine("<table  border=1 cellspacing=1 cellpadding=1 width=100%>");
                    rb.WriteLine("<tr>");
                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Test Module</b></td>");
                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Description</b></td>");
                    rb.WriteLine("<td width=10% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Pass-TestCase</b></td>");
                    rb.WriteLine("<td width=10% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Fail-TestCase</b></td>");
                    rb.WriteLine("<td width=15% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run Start Time</b></td>");
                    rb.WriteLine("<td width=15% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run End Time</b></td>");
                    rb.WriteLine("<td width=10% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Status</b></td>");
                    rb.WriteLine("</tr >");
                    rb.Close();
                }
            }

            if (BrowserModuleStartTime[testCaseExecution.sModuleName + "_" + testCaseExecution.sBrowserName] == null)
            {
                BrowserModuleStartTime[testCaseExecution.sModuleName + "_" + testCaseExecution.sBrowserName] = TestModule_StartTime;
            }
            string moduleStartTime = BrowserModuleStartTime[testCaseExecution.sModuleName + "_" + testCaseExecution.sBrowserName].ToString();


            TimeSpan testRunTime = Convert.ToDateTime(TestModule_EndTime).TimeOfDay - Convert.ToDateTime(TestModule_StartTime).TimeOfDay;
            if (BrowserRunTime[testCaseExecution.sBrowserName] == null)
            {
                BrowserRunTime[testCaseExecution.sBrowserName] = testRunTime;
            }
            else
            {
                BrowserRunTime[testCaseExecution.sBrowserName] = (TimeSpan)BrowserRunTime[testCaseExecution.sBrowserName] + testRunTime;
            }


            ModulesDetails[testCaseExecution.sModuleName + "_" + testCaseExecution.sBrowserName] = testCaseExecution.sModuleName + "$" + testmodulefilepath + "$" + testCaseExecution.sModuleDesc + "$" + tc_PassPerModule + "$" + tc_FailPerModule + "$" + moduleStartTime + "$" + TestModule_EndTime + "$" + StatusDetbgcolor + "$" + rstatus + "$" + Report_Browser_Filename;

            //ModulesDetails[HybridLogic.sModuleName + "_" + HybridLogic.sBrowserName] = HybridLogic.sModuleName + "$" + testmodulefilepath + "$" + HybridLogic.sModuleDesc + "$" + tc_PassPerModule + "$" + tc_FailPerModule + "$" + TestModule_StartTime + "$" + TestModule_EndTime + "$" + StatusDetbgcolor + "$" + rstatus + "$" + Report_Browser_Filename;
        }

        public static void TestModulesAddReport(string[] detailsList)
        {
            TextWriter rbw = new StreamWriter(detailsList[9], true);
            rbw.WriteLine("<tr>");
            rbw.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b><a href=" + detailsList[1] + ">" + detailsList[0] + "</a></b></td>");
            rbw.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[2] + "</b></td>");
            rbw.WriteLine("<td width=10% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[3] + "</b></td>");
            rbw.WriteLine("<td width=10% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[4] + "</b></td>");
            rbw.WriteLine("<td width=15% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[5] + "</b></td>");
            rbw.WriteLine("<td width=15% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[6] + "</b></td>");
            rbw.WriteLine("<td width=10% align=center bgcolor=" + detailsList[7] + "><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + detailsList[8] + "</b></td>");
            rbw.WriteLine("</tr>");
            rbw.Close();
        }

        /* ______________Creating the Test Module Page of the Report_____________________*/
        public static void Report_TestModule(string TestCase_StartTime, string TestCase_EndTime)
        {
            if (module != testCaseExecution.sModuleName || module == "")
            {
                numTestCaseFail = 0;
                numTestCasePass = 0;
                module = testCaseExecution.sModuleName;
            }

            string rstatus = "";
            folders();
            testmodulefilepath = testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + ".html";

            Report_TestModule_Filename = TestReport + "\\" + testmodulefilepath;

            if (ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"] == null)
            {
                ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"] = 0;
            }
            if (ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Pass"] == null)
            {
                ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Pass"] = 0;
            }

            if (numTestDataFail > 0)
            {
                rstatus = "Fail";
                StatusDetbgcolor = Status_Color("fail");
                numTestCaseFail++;
                FinalTestCaseFail++;
                ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"] = Convert.ToInt16(ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Fail"]) + 1;
            }
            else
            {
                rstatus = "Pass";
                StatusDetbgcolor = Status_Color("pass");
                numTestCasePass++;
                FinalTestCasePass++;
                ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Pass"] = Convert.ToInt16(ModulesBrowser[testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_Pass"]) + 1;
            }

            if (!File.Exists(Report_TestModule_Filename))
            {
                FileStream fs = File.Create(Report_TestModule_Filename);
                fs.Close();

                using (TextWriter rb = new StreamWriter(Report_TestModule_Filename))
                {
                    rb.WriteLine("<html><HEAD><TITLE> Test Module Report</TITLE>" + stylesString + "</HEAD><body>");
                    rb.WriteLine("<h4> <FONT COLOR=660000 FACE = Arial  SIZE = 4.5>" + testCaseExecution.sBrowserName + " - " + testCaseExecution.sModuleName + "</h4>");
                    rb.WriteLine("<table  border=1 cellspacing=1 cellpadding=1 width=100%>");
                    rb.WriteLine("<tr>");
                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Test Case</b></td>");
                    rb.WriteLine("<td width=30% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Description</b></td>");

                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run Start Time</b></td>");
                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run End Time</b></td>");
                    rb.WriteLine("<td width=10% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Status</b></td>");
                    rb.WriteLine("</tr >");

                    rb.WriteLine("<tr>");
                    rb.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b><a href=" + testcasefilepath + ">" + testCaseExecution.sRunningTestCaseName + "</a></b></td>");  //DetailedReport_filename
                    rb.WriteLine("<td width=30% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + testCaseExecution.sRunningTestCaseDesc + "</b></td>");
                    rb.WriteLine("<td width=10% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestCase_StartTime + "</b></td>");

                    rb.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestCase_EndTime + "</b></td>");
                    rb.WriteLine("<td width=10% align=center bgcolor=" + StatusDetbgcolor + "><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + rstatus + "</b></td>");
                    rb.WriteLine("</tr>");
                    rb.Close();
                }
            }
            else
            {
                TextWriter rbw = new StreamWriter(Report_TestModule_Filename, true);
                rbw.WriteLine("<tr>");
                rbw.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b><a href=" + testcasefilepath + ">" + testCaseExecution.sRunningTestCaseName + "</a></b></td>");

                rbw.WriteLine("<td width=30% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + testCaseExecution.sRunningTestCaseDesc + "</b></td>");
                rbw.WriteLine("<td width=10% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestCase_StartTime + "</b></td>");

                rbw.WriteLine("<td width=20% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestCase_EndTime + "</b></td>");
                rbw.WriteLine("<td width=10% align=center bgcolor=" + StatusDetbgcolor + "><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + rstatus + "</b></td>");
                rbw.WriteLine("</tr>");
                rbw.Close();
            }
            numTestDataFail = 0;
            numTestDataPass = 0;
            detailedReportFail = 0;
        }

        ///* ______________Creating the Test Case page of the Report_____________________*/
        public static void Report_TestData(string TestData_StartTime, string TestData_EndTime)
        {
            string rstatus = "Pass";
            folders();
            testcasefilepath = testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_" + testCaseExecution.sRunningTestCaseName + ".html";
            Report_TestCase_Filename = TestReport + "\\" + testcasefilepath;

            if (detailedReportFail > 0)
            {
                rstatus = "Fail";
                StatusDetbgcolor = Status_Color("fail");
                numTestDataFail++;
            }
            else
            {
                StatusDetbgcolor = Status_Color("pass");
                numTestDataPass++;
            }

            if (!File.Exists(Report_TestCase_Filename))
            {
                FileStream fs = File.Create(Report_TestCase_Filename);
                fs.Close();

                using (TextWriter rb = new StreamWriter(Report_TestCase_Filename))
                {
                    rb.WriteLine("<html><HEAD><TITLE> Test Data Report </TITLE>" + stylesString + "</HEAD><body>");
                    rb.WriteLine("<h4> <FONT COLOR=660000 FACE = Arial  SIZE = 4.5>" + testCaseExecution.sBrowserName + " - " + testCaseExecution.sModuleName + " - " + testCaseExecution.sRunningTestCaseName + "</h4>");
                    rb.WriteLine("<table  border=1 cellspacing=1 cellpadding=1 width=100%>");
                    rb.WriteLine("<tr>");
                    rb.WriteLine("<td width=30% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Test Case Name</b></td>");
                    rb.WriteLine("<td width=25% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run Start Time</b></td>");
                    rb.WriteLine("<td width=25% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Run End Time</b></td>");
                    rb.WriteLine("<td width=20% align=center  bgcolor=#153E7E><FONT COLOR=#E0E0E0 FACE= Arial SIZE=2><b>Status</b></td>");
                    rb.WriteLine("</tr >");

                    rb.WriteLine("<tr>");
                    rb.WriteLine("<td width=30% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b><a href=" + testdatafilepath + ">" + testCaseExecution.sTestDataID + "</a></b></td>");  //DetailedReport_filename
                    rb.WriteLine("<td width=25% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestData_StartTime + "</b></td>");
                    rb.WriteLine("<td width=25% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestData_EndTime + "</b></td>");
                    rb.WriteLine("<td width=20% align=center bgcolor=" + StatusDetbgcolor + "><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + rstatus + "</b></td>");
                    rb.WriteLine("</tr>");
                    rb.Close();
                }
            }
            else
            {
                TextWriter rbw = new StreamWriter(Report_TestCase_Filename, true);
                rbw.WriteLine("<tr>");
                rbw.WriteLine("<td width=30% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b><a href=" + testdatafilepath + ">" + testCaseExecution.sTestDataID + "</a></b></td>");
                rbw.WriteLine("<td width=25% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestData_StartTime + "</b></td>");
                rbw.WriteLine("<td width=25% align=center><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + TestData_EndTime + "</b></td>");
                rbw.WriteLine("<td width=20% align=center bgcolor=" + StatusDetbgcolor + "><FONT COLOR=#153E7E FACE=Arial SIZE=2><b>" + rstatus + "</b></td>");
                rbw.WriteLine("</tr>");
                rbw.Close();
            }

            detailedReportFail = 0;
        }

        /* ______________Creating the Test Case Steps Page of the Report_____________________*/
        public static void Report_TestDataStep(IWebDriver driver, string TestStepID, string TestStepDesc, string Keyword, string ObjectValue, string TestData, string Status, string RunningTestCase)
        {
            folders();
            testdatafilepath = testCaseExecution.sBrowserName + "_" + testCaseExecution.sModuleName + "_" + testCaseExecution.sRunningTestCaseName + "_" + testCaseExecution.sTestDataID + ".html";
            DetailedReport_filename = TestReport + "\\" + testdatafilepath;

            /*..............
            if (Status == "Fail")
            {
                Status = "Pass";
                TestData = "";
            }..............*/

            if (Status == "Fail")
            {
                detailedReportFail++;
                currentTestCaseFail = true;
                failedMessage = TestData;
                CaptureTestImage(testCaseExecution.sBrowserName, driver, RunningTestCase, TestStepID);
            }

            //Assign Value for all the variables used in the Report
            string ScriptStartTime = "";
            string ScriptDate = "";

            string date2 = "";
            string time2 = "";

            string RunStartTime = "";
            string RunStartDate = "";

            StatusDetbgcolor = Status_Color(Status);

            int StartTimeFlag = 0;
            if (StartTimeFlag == 0)
            {
                DateTime time = DateTime.Now;
                string format = "d MMM yyyy";
                date2 = time.ToString(format);

                time2 = time.ToString("T");
                ScriptStartTime = time2;
                ScriptDate = date2;
                StartTimeFlag = 1;
            }

            if (intCount == 1)
            {
                RunStartTime = time2;
                RunStartDate = date2;
            }

            if (!File.Exists(DetailedReport_filename))
            {
                FileStream fs = File.Create(DetailedReport_filename);
                fs.Close();

                using (TextWriter tw = new StreamWriter(DetailedReport_filename))
                {
                    tw.WriteLine("<html><HEAD><TITLE>Detailed Test Results</TITLE>" + stylesString + "</HEAD><body><h4 align=\"center\"><FONT COLOR=\"660066\" FACE=\"Arial\"SIZE=5><b>Detailed Test Report</b></h4>");
                    tw.WriteLine("<h4> <FONT COLOR=\"660000\" FACE=\"Arial\" SIZE=4.5>" + testCaseExecution.sBrowserName + " - " + testCaseExecution.sModuleName + " - " + testCaseExecution.sRunningTestCaseName + " - " + testCaseExecution.sTestDataID + "</h4> ");
                    tw.WriteLine("<table cellspacing=1 cellpadding=1 border=1 width=100%> <tr>");

                    tw.WriteLine("<td width=10%  align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Test Step ID</b></td>");
                    tw.WriteLine("<td width=20% align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Description</b></td>");
                    tw.WriteLine("<td width=10% align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Keyword</b></td>");
                    tw.WriteLine("<td width=10% align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Object</b></td>");
                    tw.WriteLine("<td width=10%  align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Data</b></td>");
                    tw.WriteLine("<td width=15% align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Status</b></td>");
                    tw.WriteLine("<td width=15% align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Screenshot(s)</b></td></tr>");

                    if (Status == "Fail")
                    {
                        tw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                        tw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                        tw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");

                        tw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href = " + file_Name + ">Screen Shot</a></b></td></tr>");
                    }
                    else if (Status == "Not Executed")
                    {
                        tw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                        tw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                        tw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");
                        tw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> </a></b></td></tr>");
                    }
                    else
                    {
                        tw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                        tw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                        tw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                        tw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");
                        tw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> </a></b></td></tr>");

                    }
                    tw.Close();
                }
            }
            else
            {
                TextWriter tsw = new StreamWriter(DetailedReport_filename, true);
                if (Status == "Fail")
                {
                    tsw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                    tsw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                    tsw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");
                    tsw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href = " + file_Name + ">Screen Shot</a></b></td></tr>");
                }
                else if (Status == "Not Executed")
                {
                    tsw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                    tsw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                    tsw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");
                    tsw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> </a></b></td></tr>");
                }
                else
                {
                    tsw.WriteLine("<tr><td width=10% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepID + "</b></td>");
                    tsw.WriteLine("<td width=20% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestStepDesc + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Keyword + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + ObjectValue + "</b></td> ");
                    tsw.WriteLine("<td width=10% align=\"left\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + TestData + "</b></td> ");
                    tsw.WriteLine("<td width=15% align=\"center\" bgcolor=" + StatusDetbgcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + Status + "</b></td>");
                    tsw.WriteLine("<td width=15% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> </a></b></td></tr>");
                }
                tsw.Close();
            }

           
        }

        /* ______________Creating a Master Report_____________________*/
        public static void CreateMasterReport()
        {
            if (!(Directory.Exists(MasterReportPath)))
            {
                Directory.CreateDirectory(MasterReportPath);
            }

            string html_Master_header_string = "<html><HEAD><TITLE>Master Report</TITLE>" + homepageStyles + "</HEAD><body><h4 align=\"center\"><FONT COLOR=\"660066\" FACE=\"Arial\" SIZE=6><b>Master Report</b></h4>";

            if (!File.Exists(MasterReportFile))
            {
                FileStream fs = File.Create(MasterReportFile);
                fs.Close();

                using (TextWriter tw = new StreamWriter(MasterReportFile, true))
                {
                    tw.WriteLine(html_Master_header_string);

                    tw.WriteLine("<table class =\"DetailedReport\" border=1 cellspacing=1 cellpadding=1 width=100% align='center'><tr>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Project Name</b></td>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Execution Start Time</b></td>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Execution End Time</b></td>");
                    tw.WriteLine(" <td align=\"center\" bgcolor=\"#153E7E\"><FONT COLOR=\"#E0E0E0\" FACE=\"Arial\" SIZE=2><b>Status</b></td>");

                    tw.WriteLine(" </tr>");


                    tw.WriteLine("<tr><td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href='" + HighReport_filename + "'>" + ProjectName + "_" + commonDate + "</a></b></td>" +
                                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + TestStartTime + "</b></td>" +
                                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + EndTime + "</b></td>" +
                                    "<td width=20% align=\"center\" bgcolor=" + MasterStatusBGcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + MasterStatus + "</b></td></tr>");

                    tw.Close();
                }
            }
            else
            {
                using (TextWriter tw = new StreamWriter(MasterReportFile, true))
                {
                    tw.WriteLine("<tr><td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b><a href='" + HighReport_filename + "'>" + ProjectName + "_" + commonTime + "</a></b></td>" +
                                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + TestStartTime + "</b></td>" +
                                    "<td width=20% align=\"center\"><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b> " + EndTime + "</b></td>" +
                                    "<td width=20% align=\"center\" bgcolor=" + MasterStatusBGcolor + "><FONT COLOR=\"#153E7E\" FACE=\"Arial\" SIZE=2><b>" + MasterStatus + "</b></td></tr>");
                    tw.Close();
                }
            }
        }

        /* ______________Status Color Styles_____________________*/
        public static string Status_Color(string Status)
        {
            //maps the variable in CSS File
            //Case for the pass/fail criteria to decide the font formats in the HTML report
            switch (Status.ToLower())
            {

                case "pass":
                    StatusDetbgcolor = "#a7f432";
                    break;
                case "fail":
                    StatusDetbgcolor = "#f77331";
                    break;
                case "not executed":
                    StatusDetbgcolor = "#FBD105";
                    break;
                case "done":
                    StatusDetbgcolor = "#BCE954";
                    break;

                default:
                    break;
            }
            return StatusDetbgcolor;

        }
    }
}
