using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using System.Net.Http;
using Authentication.BasicMVC.Domain;
using Authentication.BasicMVC.Models;
using Authentication.BasicMVC.Infrastructure;
using Authentication.BasicMVC.Domain.Models;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;
using Authentication.BasicMVC.Client.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Web.Http.Cors;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Controllers.API
{
    [RequireHttps]
    [EnableCorsAttribute("*", "*", "*")]
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

      // GET api/<controller>
      public IEnumerable<string> Get()
      {
        return new string[] { "value1", "value2" };
      }

      // GET api/<controller>/5
      [System.Web.Http.Route("API/LoginProperty/{clientId}/{propertyName}/{defaultValue}")]
      public async Task<HttpResponseMessage> Get(Guid clientId, string propertyName, string defaultValue)
      {
        HttpResponseMessage _return = null;
        try
        {
          Login _login = await HttpContext.Current.GetOwinContext().Get<UnitOfWork>().LoginManager.FindOpenByClientIdAsync(clientId);
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
            _return = Request.CreateResponse<string>(HttpStatusCode.Unauthorized, defaultValue);
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

      [System.Web.Http.Route("API/LoginProperty")]
      [System.Web.Http.HttpPost]
      public async Task<HttpResponseMessage> Post([FromBody] LoginPropertyModel _property)
      {
        HttpResponseMessage _return = null;
        try
        {
          Login _login = await HttpContext.Current.GetOwinContext().Get<UnitOfWork>().LoginManager.FindOpenByClientIdAsync(_property.SessionToken);
          if(_login!=null)
          {
            LoginProperty _loginProperty = new LoginProperty();
            _loginProperty.LoginId = _login.Id;
            _loginProperty.PropertyName = _property.PropertyName;
            _loginProperty.PropertyValue = _property.PropertyValue;
            await HttpContext.Current.GetOwinContext().Get<UnitOfWork>().LoginPropertyManager.UpdateAsync(_loginProperty);
            _return = Request.CreateResponse<bool>(HttpStatusCode.OK, true);
          }
          else
          {
            _return = Request.CreateResponse<bool>(HttpStatusCode.Unauthorized, false);
          }
        }
        catch(Exception ex)
        {
          _return = Request.CreateResponse<bool>(HttpStatusCode.InternalServerError, false);
        }
        if (_return == null)
        {
          _return = Request.CreateResponse<bool>(HttpStatusCode.InternalServerError, false);
        }
        return _return;
      }

    }
}
