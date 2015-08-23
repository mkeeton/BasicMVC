using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BasicMVC.Controllers
{
    public abstract partial class BaseController : Controller
    {
      public BaseController()
      {
        //WebClient client = new WebClient();
        //client.Headers["Accept"] = "application/json";

        //string returnedString = client.DownloadString(new Uri("https://localhost:44300/Api/Authentication/Authenticated"));
        
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:44300/");

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = "Api/Authentication/Authenticated";

        HttpResponseMessage response = client.GetAsync(url).Result;

        if (response.IsSuccessStatusCode)
        {
          bool blnAuth = response.Content.ReadAsAsync<bool>().Result;
          if (true)
          {
           

          }
          else
          {
            //Not logged in
          }
        }
        else
        {
        }
      }
    }
}