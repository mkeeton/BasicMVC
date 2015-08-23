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
using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Web.Http.Cors;

namespace Authentication.BasicMVC.Controllers.API
{
    [RequireHttps]
    [EnableCorsAttribute("http://localhost:50785/", "*", "*", SupportsCredentials = true)]
    public class AuthenticationController : ApiController
    {

        private ApplicationUserManager _userManager;
        private SessionRepository _sessionRepository;
        private LoginRepository _loginRepository;

        public AuthenticationController()
        {
        }

        public AuthenticationController(ApplicationUserManager userManager)
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

        public SessionRepository SessionManager
        {
          get
          {
            return _sessionRepository ?? new SessionRepository(HttpContext.Current.GetOwinContext().Get<IDbContext>());
          }
          private set
          {
            _sessionRepository = value;
          }
        }

        public LoginRepository LoginManager
        {
          get
          {
            return _loginRepository ?? new LoginRepository(HttpContext.Current.GetOwinContext().Get<IDbContext>());
          }
          private set
          {
            _loginRepository = value;
          }
        }

      // GET api/<controller>
      public IEnumerable<string> Get()
      {
        return new string[] { "value1", "value2" };
      }

      // GET api/<controller>/5
      public Authentication.BasicMVC.Domain.Models.AuthenticationResponse Get(Guid id)
      {
        string strSource = Request.RequestUri.PathAndQuery;
        Authentication.BasicMVC.Domain.Models.AuthenticationResponse objReturn = new Authentication.BasicMVC.Domain.Models.AuthenticationResponse();
        objReturn.Id = Guid.NewGuid();
        objReturn.ResponseCode = Domain.Models.AuthenticationResponse.AuthenticationResponseCode.Unknown;
        objReturn.RedirectURL = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Account/ConfirmLogin/";
        ClientSession _clientSession = SessionManager.FindByClientAsync(id).Result;
        if(_clientSession==null)
        {
          Login _Login = LoginManager.FindOpenByClientIdAsync(id).Result;
          if(_Login!=null)
          {
            objReturn.ResponseCode = Domain.Models.AuthenticationResponse.AuthenticationResponseCode.LoggedIn;
            objReturn.RedirectURL = "";
          }
        }
        else
        { 
          objReturn.ResponseCode = Domain.Models.AuthenticationResponse.AuthenticationResponseCode.NotLoggedIn;
          objReturn.RedirectURL = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Account/Login/";
        }
        //if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated == true)
        //{
        //  //objReturn.UserLogin = UserManager.Find
        //  objReturn.ResponseCode = Domain.Models.AuthenticationResponse.AuthenticationResponseCode.LoggedIn;
        //  objReturn.RedirectURL = "";
        //}
        //else
        //{
          
        //  objReturn.RedirectURL = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Account/Login/";
        //}
        
        return objReturn;
      }

      // POST api/<controller>
      public Authentication.BasicMVC.Domain.Models.AuthenticationResponse Post([FromBody]Authentication.BasicMVC.Domain.Models.AuthenticationResponse value)
      {
        return new Authentication.BasicMVC.Domain.Models.AuthenticationResponse();
      }

      // PUT api/<controller>/5
      public Authentication.BasicMVC.Domain.Models.AuthenticationResponse Put(int id, [FromBody]Authentication.BasicMVC.Domain.Models.AuthenticationResponse value)
      {
        return new Authentication.BasicMVC.Domain.Models.AuthenticationResponse();
      }

      // DELETE api/<controller>/5
      public bool Delete(Guid id)
      {
        return true;
      }
    }


}
