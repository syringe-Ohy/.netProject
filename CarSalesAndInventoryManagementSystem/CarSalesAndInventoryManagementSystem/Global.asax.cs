using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Http;

using System.Web.Mvc;

using System.Web.Optimization;

using System.Web.Routing;

using System.Web.SessionState;

namespace CarSalesAndInventoryManagementSystem

{

    public class MvcApplication : System.Web.HttpApplication

    {

        protected void Application_Start()

        {

            AreaRegistration.RegisterAllAreas();

            // This line MUST come AFTER AreaRegistration and BEFORE other configs

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        // This method enables session state for all Web API requests.

        // It must be a separate method within the class, not inside Application_Start.

        protected void Application_PostAuthorizeRequest()

        {

            System.Web.HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);

        }

    }

}

