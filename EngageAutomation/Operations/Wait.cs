using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Engage.Automation.Operations
{
    static class Wait
    {
        private static WebDriverWait jsWait;
        private static IJavaScriptExecutor jsExec;

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            try
            {
                waitPageLoad(driver, 20);
                int Totaltimeout = 0;
                if (timeoutInSeconds > 0)
                {
                    int jQueryWaitInt = waitUntilJQueryReady(driver, timeoutInSeconds);
                    Totaltimeout = Totaltimeout + jQueryWaitInt;
                    if (Totaltimeout < timeoutInSeconds)
                    {
                        int AngJSWaitInt = waitUntilAngularReady(driver, timeoutInSeconds - Totaltimeout);
                        Totaltimeout = Totaltimeout + AngJSWaitInt;
                        if (Totaltimeout < timeoutInSeconds)
                        {
                            int dataLoadWait = waitForDataLoad(driver, timeoutInSeconds - Totaltimeout);
                            Totaltimeout = Totaltimeout + dataLoadWait;
                            if (Totaltimeout < timeoutInSeconds)
                            {
                                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
                                wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds - Totaltimeout);
                                wait.PollingInterval = TimeSpan.FromMilliseconds(100);
                                wait.IgnoreExceptionTypes(typeof(Exception));
                                IWebElement element = wait.Until<IWebElement>((d) =>
                                {
                                    return d.FindElement(by);
                                });
                            }
                            else
                                ThrowExpection(by, timeoutInSeconds);
                        }
                        else
                            ThrowExpection(by, timeoutInSeconds);
                    }
                    else
                        ThrowExpection(by, timeoutInSeconds);
                }
            }
            catch
            {

            }
            return driver.FindElement(by);
        }

        public static IList<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            try
            {
                waitPageLoad(driver, 20);
                int Totaltimeout = 0;
                if (timeoutInSeconds > 0)
                {
                    int jQueryWaitInt = waitUntilJQueryReady(driver, timeoutInSeconds);
                    Totaltimeout = Totaltimeout + jQueryWaitInt;
                    if (Totaltimeout < timeoutInSeconds)
                    {
                        int AngJSWaitInt = waitUntilAngularReady(driver, timeoutInSeconds - Totaltimeout);
                        Totaltimeout = Totaltimeout + AngJSWaitInt;
                        if (Totaltimeout < timeoutInSeconds)
                        {
                            int dataLoadWait = waitForDataLoad(driver, timeoutInSeconds - Totaltimeout);
                            Totaltimeout = Totaltimeout + dataLoadWait;
                            if (Totaltimeout < timeoutInSeconds)
                            {
                                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
                                wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds - Totaltimeout);
                                wait.PollingInterval = TimeSpan.FromMilliseconds(100);
                                wait.IgnoreExceptionTypes(typeof(Exception));
                                IList<IWebElement> element = wait.Until<IList<IWebElement>>((d) =>
                                {
                                    return d.FindElements(by);
                                });
                            }
                            else
                                ThrowExpection(by, timeoutInSeconds);
                        }
                        else
                            ThrowExpection(by, timeoutInSeconds);
                    }
                    else
                        ThrowExpection(by, timeoutInSeconds);
                }
            }
            catch
            {

            }
            return driver.FindElements(by);
        }

        public static void ThrowExpection(By by, int timeoutInSeconds)
        {
            throw new System.TimeoutException("Could not find specified element '" + by + "' in '" + timeoutInSeconds + "' seconds");
        }

        //Wait Until JQuery and JS Ready
        public static int waitUntilJQueryReady(IWebDriver driver, int timeoutInSeconds)
        {
            int JQJSCount = 0;
            try
            {
                IJavaScriptExecutor jsExec = (IJavaScriptExecutor)driver;

                //First check that JQuery is defined on the page. If it is, then wait AJAX
                Boolean jQueryDefined = (Boolean)jsExec.ExecuteScript("return typeof jQuery != 'undefined'");
                if (jQueryDefined == true)
                {
                    //Pre Wait for stability (Optional)
                    Thread.Sleep(100);

                    //Wait JQuery Load
                    JQJSCount = JQJSCount + waitForJQueryLoad(driver, timeoutInSeconds);

                    //Wait JS Load
                    JQJSCount = JQJSCount + waitUntilJSReady(driver, timeoutInSeconds);

                    //Post Wait for stability (Optional)
                    Thread.Sleep(100);
                }
                else
                {
                    Console.WriteLine("jQuery is not defined on this site!");
                }
            }
            catch { }
            return JQJSCount;
        }

        //Wait Until Angular and JS Ready
        public static int waitUntilAngularReady(IWebDriver driver, int timeoutInSeconds)
        {
            int AGJSCount = 0;
            try
            {
                IJavaScriptExecutor jsExec = (IJavaScriptExecutor)driver;

                //First check that ANGULAR is defined on the page. If it is, then wait ANGULAR
                Boolean angularUnDefined = (Boolean)jsExec.ExecuteScript("return window.angular === undefined");
                if (!angularUnDefined)
                {
                    Boolean angularInjectorUnDefined = (Boolean)jsExec.ExecuteScript("return angular.element(document).injector() === undefined");
                    if (!angularInjectorUnDefined)
                    {
                        //Pre Wait for stability (Optional)
                        Thread.Sleep(100);

                        //Wait Angular Load
                        AGJSCount = AGJSCount + waitForAngularLoad(driver, timeoutInSeconds);

                        //Wait JS Load
                        AGJSCount = AGJSCount + waitUntilJSReady(driver, timeoutInSeconds);

                        //Post Wait for stability (Optional)
                        Thread.Sleep(100);
                    }
                    else
                    {
                        Console.WriteLine("Angular injector is not defined on this site!");
                    }
                }
            }
            catch { }
            return AGJSCount;
        }

        //waitForJQueryLoad
        public static int waitForJQueryLoad(IWebDriver driver, int timeoutInSeconds)
        {
            int i = 0;
            try
            {
                for (i = 0; i < timeoutInSeconds; i++)
                {
                    bool ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                        return i;
                    Thread.Sleep(1000);
                }
            }
            catch { }
            return i;
        }

        public static int waitUntilJSReady(IWebDriver driver, int timeoutInSeconds)
        {
            int i = 0;
            try
            {

                for (i = 0; i < timeoutInSeconds; i++)
                {
                    bool jsReady = jsExec.ExecuteScript("return document.readyState").ToString().Equals("complete");

                    if (jsReady)
                        return i;
                    Thread.Sleep(1000);
                }
            }
            catch { }
            return i;
        }

        //waitForAngularLoad
        public static int waitForAngularLoad(IWebDriver driver, int timeoutInSeconds)
        {
            int i = 0;
            try
            {
                string angularReadyScript = "return angular.element(document).injector().get('$http').pendingRequests.length === 0";
                for (i = 0; i < timeoutInSeconds; i++)
                {
                    bool AngularJSComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript(angularReadyScript);
                    if (AngularJSComplete)
                        return i;
                    Thread.Sleep(1000);
                }
            }
            catch { }
            return i;
        }

        //waitForDataLoad
        public static int waitForDataLoad(IWebDriver driver, int timeoutInSeconds)
        {
            int i = 0;
            try
            {
                string angularReadyScript = "return $('.spinner').is(':visible') == false";
                do
                {
                    for (i = 0; i < timeoutInSeconds; i++)
                    {
                        bool AngularJSComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript(angularReadyScript);
                        if (AngularJSComplete)
                            return i;
                        Thread.Sleep(1000);
                    }
                } while (!(bool)((IJavaScriptExecutor)driver).ExecuteScript("return jQuery.active == 0") || i < timeoutInSeconds);
            }
            catch { }
            return i;
        }

        //this function is not included in Timout seconds because sometimes page will be loaded by it will indicate as not loaded
        public static int waitPageLoad(IWebDriver driver, int timeoutInSeconds)
        {
            int i = 0;
            try
            {
                for (i = 0; i < timeoutInSeconds; i++)
                {
                    bool wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                    if (wait)
                        return i;
                }
            }
            catch { }
            return i;
        }

        internal static IWebElement Until(object p)
        {
            throw new NotImplementedException();
        }

        internal static void Until(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
