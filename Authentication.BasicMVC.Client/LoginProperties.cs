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

using System.Net.Http.Formatting;

using Authentication.BasicMVC.Client.Domain.Models;

namespace Authentication.BasicMVC.Client
{
  public class LoginProperties
  {

    public static async Task<string> GetLoginPropertyValue(string propertyName, string defaultValue)
    {
      string _Response;
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["SSOURL"]);
          client.DefaultRequestHeaders.Accept.Clear();
          client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          // New code:
          HttpResponseMessage response = await client.GetAsync("API/LoginProperty/" + Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()) + "/" + propertyName + "/" + defaultValue);
          if (response.IsSuccessStatusCode)
          {
            _Response = response.Content.ReadAsAsync<string>().Result;
            return _Response;
          }
          else
          {
            throw new Exception("Service Response Error");
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Service Response Error");
      }
    }

    public static async Task<bool> SetLoginPropertyValue(string propertyName, string propertyValue)
    {
      bool _Response;
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["SSOURL"]);
          client.DefaultRequestHeaders.Accept.Clear();
          client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          // New code:
          LoginPropertyModel _data = new LoginPropertyModel();
          _data.SessionToken = new Guid(Repositories.CookieRepository.GetCookieValue("SessionID", Guid.NewGuid().ToString()));
          _data.PropertyName = propertyName;
          _data.PropertyValue = propertyValue;
          var content = new ObjectContent<LoginPropertyModel>(_data, new JsonMediaTypeFormatter());
          HttpResponseMessage response = await client.PostAsync("API/LoginProperty",content);
          if (response.IsSuccessStatusCode)
          {
            _Response = response.Content.ReadAsAsync<bool>().Result;
            return _Response;
          }
          else
          {
            return false;
          }
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

  }
}
