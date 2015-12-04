using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain.Models
{
  public class LoginList : IDisposable
  {

    public List<ClientSession> ClientSessions { get;set;}
    public List<Login> Logins { get;set;}
    public DateTime LastCleansed { get;set;}
    public int CleansePeriod { get;set;}

    public LoginList(int cleansePeriod)
    {
      ClientSessions = new List<ClientSession>();
      Logins = new List<Login>();
      LastCleansed = System.DateTime.Now;
      CleansePeriod = cleansePeriod;
    }

    public void Dispose()
    {
      ClientSessions = null;
      Logins = null;
    }
  }
}
