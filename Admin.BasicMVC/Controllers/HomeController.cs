using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.BasicMVC.Client;
using Authentication.BasicMVC.Client.Attributes;
using System.Threading.Tasks;


namespace Admin.BasicMVC.Controllers
{
  public class HomeController : BaseController
  {
    public async Task<ActionResult> Index()
    {
      bool _set = await LoginProperties.SetLoginPropertyValue("CentreId", "Suck My Ass");
      string _CentreId = await LoginProperties.GetLoginPropertyValue("CentreId", "Unknown");
      return View();
    }

    [SSOAuthentication]
    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}