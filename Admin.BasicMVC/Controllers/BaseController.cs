using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.BasicMVC.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult LogOut()
        {

          return Authentication.BasicMVC.Client.Authentication.Logout("");
        }
    }
}