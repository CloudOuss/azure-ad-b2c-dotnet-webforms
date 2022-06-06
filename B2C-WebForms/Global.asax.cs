using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace B2C_WebForms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError();
            if (objErr == null)
                return;
            int cnt = 0;
            string outerException = null;
            while (objErr != null && objErr.InnerException != null && cnt++ < 3)
            {
                if (!String.IsNullOrEmpty(outerException))
                {
                    outerException += "\n\t\t";
                }
                outerException += objErr.Message;
                objErr = objErr.InnerException;
            }
            try
            {
                string err = " Error in: " + Request.Url.ToString() +
                "\nError Message: " + objErr.Message +
                "\nStack Trace: " + objErr.StackTrace;

                Dictionary<string, string> logAttributes = new Dictionary<string, string>();
                // NP Removed iv-user - it was getting added twice (once in logger.cs) and causing an exception
                //logAttributes.Add("User Id", Request.Headers["iv-user"]);
                if (!String.IsNullOrEmpty(outerException))
                {
                    logAttributes.Add("Additional Info", outerException);
                }
                Console.WriteLine(err, TraceEventType.Error, logAttributes);
            }
            catch (Exception ex)
            {
                // Ignore errors here so we don't wind up in an infinite loop
                try
                {
                    Console.WriteLine("Error writing exception" + ex.StackTrace, TraceEventType.Warning);
                }
                catch (Exception)
                { }

            }
            Server.Transfer("~/Error.aspx?strErr=" + objErr.Message + ".  " + DateTime.Now);
        }
    }
}