using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMVC.Core.Domain.Interfaces.Authentication
{
  public interface ICoreUser<T>
  {
    T Id { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
    bool EmailConfirmed { get; set; }
  }
}
