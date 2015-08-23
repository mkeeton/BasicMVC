using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain.Models
{
  public class AuthenticationResponse
  {
    public Guid Id { get; set; }
    public Guid SessionToken { get; set; }
    public AuthenticationResponseCode ResponseCode { get; set;}
    public Authentication.BasicMVC.Domain.Models.Login UserLogin { get; set;}
    public string RedirectURL { get; set; }

    public enum AuthenticationResponseCode
    {  
      Unknown,
      LoggedIn,
      NotLoggedIn,
    };
  }
}
