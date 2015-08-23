using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Domain.Repositories;
using Authentication.BasicMVC.Infrastructure.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class LoginRepository
  {

    private readonly IDbContext CurrentContext;

    public LoginRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public LoginRepository()
    {
      this.CurrentContext = DbContext.Create();
    }

    public void Dispose()
    {

    }

    public virtual Login AddLoginRecordAsync(User user, String sessionId)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      Login _login = new Login();
      _login.SessionId = sessionId;
      _login.UserId=user.Id;
      _login.LoginDate = System.DateTime.Now;
      AddLoginRecordAsync(_login);
      return _login;
    }

    public virtual Task AddLoginRecordAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("login");
      Login owner = null;
      if(login.Id==Guid.Empty)
      {
        this.FindOpenBySessionAsync(login.SessionId);
      }
      else
      {
        //this.FindById(login.Id);
      }
      if ((owner == null))// || (owner.Result == null))
      {
        return Task.Factory.StartNew(() =>
        {
          login.Id = Guid.NewGuid();
          using (IDbConnection connection = CurrentContext.OpenConnection())
            connection.Execute("INSERT INTO auth_Logins(Id, sessionId, UserId, LoginDate) values(@Id, @sessionId, @userId, @loginDate)", login);
        });
      }
      else
      {
        login.Id = owner.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task EndSessionLoginRecordsAsync(String sessionId)
    {
      if (sessionId == null)
        throw new ArgumentNullException("session");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("UPDATE auth_Logins SET LogoutDate=@LogoutDate WHERE (SessionID=@sessionId OR SessionID IS NULL) AND LogoutDate IS NULL",
              new { LogoutDate = DateTime.Now, sessionId = sessionId });
      });
    }
    public virtual Task LogoutAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("login");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("Update auth_Logins SET LogoutDate = GETDATE() where Id = @loginId", new { loginId=login.Id });
      });
    }

    public virtual Task<Login> FindOpenByIdAsync(Guid Id)
    {
      if (Id == Guid.Empty)
        throw new ArgumentNullException("ID");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Login>("select L.* from auth_Logins L where L.ID = @Id AND L.LogoutDate IS NULL", new { Id = Id }).SingleOrDefault();
      });
    }

    public virtual Task<Login> FindOpenByClientIdAsync(Guid clientSessionID)
    {
      if (clientSessionID == Guid.Empty)
        throw new ArgumentNullException("clientSessionID");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Login>("select DISTINCT L.* from auth_ClientSessions CS INNER JOIN auth_Logins L ON CS.LoginID=L.ID where CS.ClientSessionID = @ClientSessionId AND L.LogoutDate IS NULL", new { ClientSessionId = clientSessionID }).SingleOrDefault();
      });
    }

    public virtual Task<Login> FindOpenBySessionAsync(string localSessionID)
    {
      if (localSessionID == "")
        throw new ArgumentNullException("localSessionID");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Login>("select L.* from auth_Logins L where L.SessionID = @LocalSessionId AND L.LogoutDate IS NULL", new {LocalSessionId = localSessionID }).SingleOrDefault();
      });
    }
  }
}
