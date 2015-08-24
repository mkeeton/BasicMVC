using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BasicMVC.Controllers
{
    public abstract partial class BaseController : Controller
    {
      protected void Application_OnAuthenticateRequest(Object sender,EventArgs e)
      {

      }
    }
}