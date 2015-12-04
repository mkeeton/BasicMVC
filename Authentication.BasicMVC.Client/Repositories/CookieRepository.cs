using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.BasicMVC.Client.Repositories
{
  public class CookieRepository
  {
    public static string GetCookieValue(string cookieName, string defaultValue)
    {
      var langCookie = HttpContext.Current.Request.Cookies[cookieName];
      if (langCookie == null)
      {
        // cookie doesn't exist, either pull preferred lang from user profile
        // or just setup a cookie with the default language
        langCookie = new HttpCookie(cookieName, defaultValue);
        HttpContext.Current.Response.Cookies.Add(langCookie);
      }
      return langCookie.Value;
    }
  }
}
