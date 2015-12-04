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
    [EnableCorsAttribute("http://localhost:50785/", "*", "*", SupportsCredentials = true)]
    public class AuthenticationController : ApiController
    {

        public AuthenticationController(ApplicationUserManager userManager, UnitOfWork unitOfWork)
        {
            UserManager = userManager;
            WorkManager = unitOfWork;
        }

        public UnitOfWork WorkManager{get;set;}

        public ApplicationUserManager UserManager {get;set;}

      // GET api/<controller>
      public IEnumerable<string> Get()
      {
        return new string[] { "value1", "value2" };
      }

      // GET api/<controller>/5
      public async Task<AuthenticationResponse> Get(Guid id)
      {
        AuthenticationResponse objReturn = new AuthenticationResponse();
        objReturn.Id = Guid.NewGuid();
        try
        { 
          string strSource = Request.RequestUri.PathAndQuery; 
          objReturn.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.Unknown;
          objReturn.RedirectURL = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Account/ConfirmLogin/";
          ClientSession _clientSession = await WorkManager.SessionManager.FindByClientAsync(id);
          if(_clientSession==null)
          {
            Login _Login = await WorkManager.LoginManager.FindOpenByClientIdAsync(id);
            if(_Login!=null)
            {
              objReturn.UserId = _Login.UserId;
              objReturn.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.LoggedIn;
              objReturn.RedirectURL = "";
            }
          }
          else
          { 
            objReturn.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.NotLoggedIn;
            objReturn.RedirectURL = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Account/Login/";
          }
        }
        catch(Exception ex)
        {
          objReturn.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.Error;
          objReturn.RedirectURL = "";
        }        
        return objReturn;
      }

      // POST api/<controller>
      public AuthenticationResponse Post([FromBody]AuthenticationResponse value)
      {
        return new AuthenticationResponse();
      }

      // PUT api/<controller>/5
      public AuthenticationResponse Put(int id, [FromBody]AuthenticationResponse value)
      {
        return new AuthenticationResponse();
      }

      // DELETE api/<controller>/5
      public bool Delete(Guid id)
      {
        return true;
      }
    }


}
