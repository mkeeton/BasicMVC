using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.Owin;

using BasicMVC.Core.Data.Interfaces;
using Authentication.BasicMVC.Infrastructure;

using System.Web.Configuration;

namespace Authentication.BasicMVC.App_Start
{
  public class OwinSettings : IDisposable
  {

    public static OwinSettings Create()
    {
      HttpContext.Current.GetOwinContext().Get<IDbContext>().ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      HttpContext.Current.GetOwinContext().Get<UnitOfWork>().DbContext = HttpContext.Current.GetOwinContext().Get<IDbContext>();
      return new OwinSettings();
    }

    public void Dispose()
    {

    }
  }
}