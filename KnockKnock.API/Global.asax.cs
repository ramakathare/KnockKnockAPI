using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace KnockKnock.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected TelemetryClient telemetry = new TelemetryClient();
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                telemetry.TrackException(exception);
            }
        }
    }
}
