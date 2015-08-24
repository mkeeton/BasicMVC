using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMVC.Core.Domain.Models.Authentication
{
    public class User : Interfaces.Authentication.ICoreUser<Guid>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed {  get; set;}

        public class SearchParameters
        {
          public string UserName { get; set; }
          public string Password { get; set; }
        }
    }
}
