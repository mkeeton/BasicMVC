using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain.Models
{
  public class LoginProperty
  {
    public Guid Id { get; set; }
    public Guid LoginId { get; set; }
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }
  }
}
