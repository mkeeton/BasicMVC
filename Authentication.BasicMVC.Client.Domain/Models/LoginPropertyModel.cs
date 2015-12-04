using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Client.Domain.Models
{
  public class LoginPropertyModel
  {
    public Guid SessionToken { get; set; }
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }
  }
}
