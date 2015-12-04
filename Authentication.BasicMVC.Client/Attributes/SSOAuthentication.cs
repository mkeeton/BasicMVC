using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading.Tasks;

using Authentication.BasicMVC.Client.Domain.Models;

namespace Authentication.BasicMVC.Client.Attributes
{
  public class SSOAuthentication : AuthorizeAttribute
  {
    public SSOAuthentication()
    {

    }

    public override void OnAuthorization(AuthorizationContext context)
    {
      
      AuthenticationResponse _Response = Authentication.GetAuthenticationResponse();
      if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.Unknown)
      {
        context.Result = new RedirectResult(_Response.RedirectURL + "?sessionID=" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()) + "&returnURL=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.PathAndQuery));
        return;
      }
      else if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.NotLoggedIn)
      {
        context.Result = new RedirectResult(_Response.RedirectURL + "?sessionID=" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()) + "&returnURL=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.PathAndQuery));
        return;
      }
      else if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.Error)
      {
        context.Result = new RedirectResult(System.Web.Configuration.WebConfigurationManager.AppSettings["SSOErrorURL"]);
        return;
      }
    }
  }
}
