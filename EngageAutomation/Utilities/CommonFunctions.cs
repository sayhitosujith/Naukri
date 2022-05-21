using Engage.Automation.Operations;
using java.awt;
using java.awt.@event;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Configuration;

namespace Engage.Automation.Utilities
{
    class CommonFunctions
    {
        static string SavedFiles = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\SavedFiles";
        //private object UriFactory;

        public DocumentClient documentClient { get; private set; }
        public object collectionUri { get; private set; }

        /* ______________Killing the process/Ending the Thread_____________________*/
        public static void KillProcess(string instance)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Equals(instance))
                {
                    clsProcess.Kill();
                }
            }
        }

        /* ______________Dynamic Folder Path of the Project_____________________*/
        public static string FolderPath()
        {
            string Path = Directory.GetCurrentDirectory();
            string[] words = Path.Split('\\');
            Path = "";
            for (int i = 0; i < words.Length - 3; i++)
            {
                Path += words[i] + @"\";
            }

            Path += words[words.Length - 4];
            return Path;
        }

        /* ______________Highlighting the Web Element_____________________*/
        public static void highlight(IWebDriver driver, IWebElement element)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }

        public static string SaveAction(string name)
        {
            Robot r = new Robot();

            r.keyPress(KeyEvent.VK_CONTROL);
            Thread.Sleep(2000);
            r.keyPress(KeyEvent.VK_S);

            Thread.Sleep(3000);

            r.keyRelease(KeyEvent.VK_CONTROL);
            r.keyRelease(KeyEvent.VK_S);

            if (!Directory.Exists(SavedFiles))
            {
                Directory.CreateDirectory(SavedFiles);
            }

            SavedFiles = SavedFiles.Replace(@"\\", @"\");

            Thread.Sleep(2000);

            DeleteAllFilesInFolder(SavedFiles);

            Thread.Sleep(2000);
            string fileName = name + "_" + Reports.replace(DateTime.Now.ToString("dd MMM yyyy")) + "_" + Reports.replace(DateTime.Now.ToString("T")) + ".pdf";

            SendKeys.SendWait(@SavedFiles + @"\" + fileName);
            Thread.Sleep(5000);
            SendKeys.SendWait(@"{Enter}");

            return SavedFiles + @"\" + fileName;
        }

        public static string SaveAction_IE(string name)
        {
            Robot r = new Robot();
            r.keyPress(KeyEvent.VK_ESCAPE);
            r.keyRelease(KeyEvent.VK_ESCAPE);

            Thread.Sleep(3000);

            r.keyPress(KeyEvent.VK_CONTROL);
            r.keyPress(KeyEvent.VK_SHIFT);
            r.keyPress(KeyEvent.VK_S);

            Thread.Sleep(1000);

            r.keyRelease(KeyEvent.VK_CONTROL);
            r.keyRelease(KeyEvent.VK_SHIFT);
            r.keyRelease(KeyEvent.VK_S);

            if (!Directory.Exists(SavedFiles))
            {
                Directory.CreateDirectory(SavedFiles);
            }

            SavedFiles = SavedFiles.Replace(@"\\", @"\");

            Thread.Sleep(3000);

            DeleteAllFilesInFolder(SavedFiles);

            Thread.Sleep(3000);
            string fileName = name + "_" + Reports.replace(DateTime.Now.ToString("dd MMM yyyy")) + "_" + Reports.replace(DateTime.Now.ToString("T")) + ".pdf";

            SendKeys.SendWait(@SavedFiles + @"\" + fileName);
            Thread.Sleep(5000);
            SendKeys.SendWait(@"{Enter}");

            return SavedFiles + @"\" + fileName;
        }

        //__________________________DeleteAllFilesInFolder____________________________//
        public static void DeleteAllFilesInFolder(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }


        //_____________________________GetMonthPosition________________________________//
        public static int GetMonthPosition(string monthToCheck)
        {
            int pos = 0;
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for (int i = 0; i < monthNames.Length; i++)
            {
                if (monthNames[i].ToLower().Trim() == monthToCheck.ToLower().Trim())
                {
                    pos = i + 1;
                }
            }

            return pos;
        }

        /* ______________Highlighting the Web Element using XPath_____________________*/
        public static void highlightUsingXpath(IWebDriver driver, string xpath)
        {
            IWebElement element = driver.FindElement(By.XPath(xpath));
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }


        /*________________________GetCommandLineargs___________________________*/
        public static bool GetCommandLineargs()
        {
            bool flag = false;
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                string FinalCategory = "";

                if (args.Length > 0)
                {
                    foreach (string str in args)
                    {
                        if (str.Contains("TestCaseFilter"))
                        {
                            //Console.WriteLine("\ncategory fetched " + str);
                            FinalCategory = str;
                            break;
                        }
                    }

                    string[] category = FinalCategory.Split('=');

                    if (category[1] == "PatchTesting")
                    {
                        flag = true;
                    }
                }
            }
            catch { }
            return flag;
        }

        //_________________________________SendEmail_Via_SMTP_______________________________//
        public static void SendEmail(string ReportPath)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
           // SmtpClient smtp = new SmtpClient("smtp.office365.com");
            String AttchmentPath = string.Empty;
          //  mail.To.Add("sujith.s@winwire.com");
            mail.To.Add("sayhitosujith@gmail.com");
            mail.From = new MailAddress(ConfigurationManager.AppSettings["Sender"]);
            mail.Subject = "Execution Report";
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

    }
}
