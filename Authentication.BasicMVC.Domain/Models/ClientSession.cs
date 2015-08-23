using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain.Models
{
  public class ClientSession
  {
    public Guid Id { get; set; }
    public string LocalSessionID { get; set; }
    public Guid ClientSessionID { get; set; }
    public Guid? LoginID { get; set; }
  }
}
