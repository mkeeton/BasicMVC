using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Authentication.BasicMVC.Client.Domain.Models;

namespace Authentication.BasicMVC.Client
{
  public class Authentication
  {

    public static Guid GetUserId()
    {
      AuthenticationResponse _Response = Authentication.GetAuthenticationResponse();
      if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.LoggedIn)
      {
        return _Response.UserId;
      }
      else if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.Unknown)
      {
        HttpContext.Current.Response.Redirect(_Response.RedirectURL + "?sessionID=" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()) + "&returnURL=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.PathAndQuery));
      }
      return new Guid();
    }

    public static void ValidateAuthentication()
    {
      AuthenticationResponse _Response = Authentication.GetAuthenticationResponse();
      if (_Response.ResponseCode == AuthenticationResponse.AuthenticationResponseCode.Unknown)
      {
        new RedirectResult(_Response.RedirectURL + "?sessionID=" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()) + "&returnURL=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.PathAndQuery));
      }
    }

    public static AuthenticationResponse GetAuthenticationResponse()
    {
      AuthenticationResponse _Response;
      try
      { 
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["SSOURL"]);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // New code:
        HttpResponseMessage response = client.GetAsync("API/Authentication/" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString())).Result;
        if (response.IsSuccessStatusCode)
        {
          _Response  = response.Content.ReadAsAsync<AuthenticationResponse>().Result;
        }
        else
        {
          _Response = new AuthenticationResponse();
          _Response.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.Error;
        }
      }
      }
      catch(Exception)
      {
        _Response = new AuthenticationResponse();
        _Response.ResponseCode = AuthenticationResponse.AuthenticationResponseCode.Error;
      }
      return _Response;
    }

    public static ActionResult Logout(string returnURL)
    {
      string DestinationURL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSOURL"] + "Account/LogOut/";
      if(returnURL=="")
      {
        returnURL = HttpUtility.UrlEncode(HttpContext.Current.Request.UrlReferrer.Scheme + "://" + HttpContext.Current.Request.UrlReferrer.Authority + HttpContext.Current.Request.UrlReferrer.PathAndQuery);
      }
      return new RedirectResult(DestinationURL + "?returnURL=" + returnURL);
    }
  }
}
