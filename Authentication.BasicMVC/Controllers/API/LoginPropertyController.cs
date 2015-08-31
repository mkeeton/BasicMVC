using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Authentication.BasicMVC.Domain;
using Authentication.BasicMVC.Models;
using Authentication.BasicMVC.Infrastructure;
using Authentication.BasicMVC.Domain.Models;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;
using Authentication.BasicMVC.Client.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Web.Http.Cors;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Controllers.API
{
    [RequireHttps]
    [EnableCorsAttribute("http://localhost:50785/", "*", "*", SupportsCredentials = true)]
    public class LoginPropertyController : ApiController
    {

      private ApplicationUserManager _userManager;

      public LoginPropertyController()
      {
      }

      public LoginPropertyController(ApplicationUserManager userManager)
      {
          UserManager = userManager;
      }

      public ApplicationUserManager UserManager {
          get
          {
              return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
          }
          private set
          {
              _userManager = value;
          }
      }

      public async Task<HttpResponseMessage> Get(string propertyName, string defaultValue)
      {
        HttpResponseMessage _return = null;
        try
        {
          Login _login = await HttpContext.Current.GetOwinContext().Get<UnitOfWork>().LoginManager.FindOpenBySessionAsync("");
          if(_login!=null)
          {
            LoginProperty _prop = await HttpContext.Current.GetOwinContext().Get<UnitOfWork>().LoginPropertyManager.FindByNameAsync(_login.Id,propertyName);
            if(_prop!=null)
            {
              _return = Request.CreateResponse<string>(HttpStatusCode.OK, _prop.PropertyValue);
            }
          }
          else
          {
            _return = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, defaultValue);
          }
        }
        catch (Exception ex)
        {
          _return = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, defaultValue);
        }
        if(_return==null)
        {
          _return = Request.CreateResponse<string>(HttpStatusCode.OK, defaultValue);
        }
        return _return;
      }

      public bool Post(string propertyName, string propertyValue)
      {
        return true;
      }

    }
}
