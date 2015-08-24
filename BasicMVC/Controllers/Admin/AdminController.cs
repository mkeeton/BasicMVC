using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasicMVC.Controllers.Admin
{
    public class AdminController : BaseController
    {
        public AdminController() : base()
        {

        }

        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}