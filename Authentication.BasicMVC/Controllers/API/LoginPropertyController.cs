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

      public LoginPropertyController(ApplicationUserManager userManager, UnitOfWork unitOfWork)
      {
          UserManager = userManager;
          WorkManager = unitOfWork;
      }

      public ApplicationUserManager UserManager {get;set;}

      public UnitOfWork WorkManager { get;set;}

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
          Login _login = await WorkManager.LoginManager.FindOpenByClientIdAsync(clientId);
          if(_login!=null)
          {
            LoginProperty _prop = await WorkManager.LoginManager.FindPropertyByNameAsync(_login, propertyName);
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
          Login _login = await WorkManager.LoginManager.FindOpenByClientIdAsync(_property.SessionToken);
          if(_login!=null)
          {
            LoginProperty _loginProperty = new LoginProperty();
            _loginProperty.LoginId = _login.Id;
            _loginProperty.PropertyName = _property.PropertyName;
            _loginProperty.PropertyValue = _property.PropertyValue;
            await WorkManager.LoginManager.UpdatePropertyAsync(_login, _loginProperty);
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
