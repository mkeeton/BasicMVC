using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Client.Models
{
  public class AuthenticationResponse
  {
    public Guid Id { get; set; }
    public Guid SessionToken { get; set; }
    public AuthenticationResponseCode ResponseCode { get; set; }
    public Guid UserId { get; set; }
    public string RedirectURL { get; set; }

    public enum AuthenticationResponseCode
    {
      Unknown,
      LoggedIn,
      NotLoggedIn,
      Error,
    };
  }
}
