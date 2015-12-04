using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Authentication.BasicMVC.Models;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Infrastructure;
using Authentication.BasicMVC.Infrastructure.Repositories;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static LoginList currentLogins;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            currentLogins = new LoginList(360);
        }

        protected void Application_BeginRequest()
        {
          
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
          
        }

        protected void Application_End()
        {
          currentLogins.Dispose();
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
          Session["init"] = 0;
        }

        protected void Session_End(object sender, EventArgs e)
        {
          
          var manager = new LoginRepository(HttpContext.Current.GetOwinContext().Get<IDbContext>());
          manager.EndSessionLoginRecordsAsync(Session.SessionID);
          var currentLogin = currentLogins.Logins.Where(x => x.SessionId==Session.SessionID && x.LogoutDate==null ).FirstOrDefault();
          if(currentLogin!=null)
          {
            currentLogins.Logins.Remove(currentLogin);
          }
        }

        public static LoginList GetCurrentLogins()
        {
          return currentLogins;
        }
    }
}
