using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain.Models
{
  public class Login
  {
    public Guid Id { get; set; }
    public string SessionId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LoginDate { get; set; }
    public DateTime LastAccessed { get;set;}
    public DateTime? LogoutDate { get; set; }
    public List<LoginProperty> LoginProperties { get;set;}

    public Login()
    {
      LoginProperties = new List<LoginProperty>();
    }
  }
}
