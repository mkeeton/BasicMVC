using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Domain.Repositories;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class LoginRepository : ILoginRepository
  {

    private readonly LoginList logins;

    public LoginRepository(LoginList currentLogins)
    {
      if (currentLogins==null)
        throw new ArgumentNullException("connectionString");

      this.logins = currentLogins;
    }

    public void Dispose()
    {

    }

    public async Task<Login> AddLoginRecordAsync(User user, String sessionId)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      Login _login = new Login();
      _login.SessionId = sessionId;
      _login.UserId=user.Id;
      _login.LoginDate = System.DateTime.Now;
      _login = await AddLoginRecordAsync(_login);
      return _login;
    }

    public async Task<Login> AddLoginRecordAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("login");
      Login owner = null;
      if(login.Id==Guid.Empty)
      {
        owner = await this.FindOpenBySessionAsync(login.SessionId);
      }
      else
      {
        owner = login;//this.FindById(login.Id);
      }
      if ((owner == null))// || (owner.Result == null))
      {
        login.Id = Guid.NewGuid();
        await Task.Factory.StartNew(() =>
        {
          logins.Logins.Add(new Login(){Id=login.Id, SessionId=login.SessionId, UserId=login.UserId, LoginDate=login.LoginDate});
          //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
          //connection.Execute("INSERT INTO auth_Logins(Id, sessionId, UserId, LoginDate) values(@Id, @sessionId, @userId, @loginDate)", login, CurrentContext.CurrentTransaction);
        }); 
      }
      else
      {
        login.Id = owner.Id;
      }
      return login;
    }

    public virtual Task EndSessionLoginRecordsAsync(String sessionId)
    {
      if (sessionId == null)
        throw new ArgumentNullException("session");

      return Task.Factory.StartNew(() =>
      {
        List<Login> updateLogins = logins.Logins.Where(x => x.SessionId==sessionId && x.LogoutDate==null).ToList<Login>();
        foreach(Login currentLogin in updateLogins)
        {
          currentLogin.LogoutDate = DateTime.Now;
        }
        //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //  connection.Execute("UPDATE auth_Logins SET LogoutDate=@LogoutDate WHERE (SessionID=@sessionId OR SessionID IS NULL) AND LogoutDate IS NULL",
        //      new { LogoutDate = DateTime.Now, sessionId = sessionId }, CurrentContext.CurrentTransaction);
      });
    }
    public virtual Task LogoutAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("login");

      return Task.Factory.StartNew(() =>
      {
        List<Login> updateLogins = logins.Logins.Where(x => x.Id==login.Id).ToList<Login>();
        foreach (Login currentLogin in updateLogins)
        {
          login.LogoutDate = DateTime.Now;
        }
        //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //connection.Execute("Update auth_Logins SET LogoutDate = GETDATE() where Id = @loginId", new { loginId = login.Id }, CurrentContext.CurrentTransaction);
      });
    }

    public virtual Task<Login> FindOpenByIdAsync(Guid Id)
    {
      if (Id == Guid.Empty)
        throw new ArgumentNullException("ID");

      return Task.Factory.StartNew(() =>
      {
        return logins.Logins.Where(x => x.Id == Id).SingleOrDefault<Login>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //  return connection.Query<Login>("select L.* from auth_Logins L where L.ID = @Id AND L.LogoutDate IS NULL", new { Id = Id }).SingleOrDefault();
      });
    }

    public virtual Task<Login> FindOpenByClientIdAsync(Guid clientSessionID)
    {
      if (clientSessionID == Guid.Empty)
        throw new ArgumentNullException("clientSessionID");

      return Task.Factory.StartNew(() =>
      {
        return logins.Logins.Where(x => x.LogoutDate == null && logins.ClientSessions.Where(cs => cs.ClientSessionID == clientSessionID).Select(cs => cs.LoginID).Contains(x.Id)).FirstOrDefault<Login>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //  return connection.Query<Login>("select DISTINCT L.* from auth_ClientSessions CS INNER JOIN auth_Logins L ON CS.LoginID=L.ID where CS.ClientSessionID = @ClientSessionId AND L.LogoutDate IS NULL", new { ClientSessionId = clientSessionID }).SingleOrDefault();
      });
    }

    public virtual Task<Login> FindOpenBySessionAsync(string localSessionID)
    {
      if (localSessionID == "")
        throw new ArgumentNullException("localSessionID");

      return Task.Factory.StartNew(() =>
      {
        return logins.Logins.Where(x => x.SessionId==localSessionID && x.LogoutDate==null).SingleOrDefault<Login>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //  return connection.Query<Login>("select L.* from auth_Logins L where L.SessionID = @LocalSessionId AND L.LogoutDate IS NULL", new { LocalSessionId = localSessionID }).SingleOrDefault();
      });
    }

    public virtual Task<LoginProperty> FindPropertyByNameAsync(Login login, string propertyName)
    {
      if (login == null)
        throw new ArgumentNullException("login");

      if (propertyName == "")
        throw new ArgumentNullException("propertyName");

      return Task.Factory.StartNew(() =>
      {
        return login.LoginProperties.Where(x => x.PropertyName == propertyName).SingleOrDefault<LoginProperty>();
        //using (IDbConnection connection = CurrentContext.OpenConnection())
        //  return connection.Query<LoginProperty>("select DISTINCT * FROM auth_LoginProperties WHERE LoginId=@LoginId AND PropertyName LIKE @PropertyName", new { LoginId = loginId, PropertyName = propertyName }).SingleOrDefault();
      });
    }

    public async Task<LoginProperty> UpdatePropertyAsync(Login login, LoginProperty loginProperty)
    {
      if (loginProperty == null)
        throw new ArgumentNullException("loginProperty");
      LoginProperty _prop = await FindPropertyByNameAsync(login, loginProperty.PropertyName);
      if (_prop == null)
      {
        loginProperty.Id = Guid.NewGuid();
        await Task.Factory.StartNew(() =>
        {
          login.LoginProperties.Add(loginProperty);
          //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
          //connection.Execute("INSERT INTO auth_LoginProperties(Id, LoginId, PropertyName, PropertyValue) VALUES(@Id, @LoginId, @PropertyName, @PropertyValue)", new { Id = loginProperty.Id, LoginId = loginProperty.LoginId, PropertyName = loginProperty.PropertyName, PropertyValue = loginProperty.PropertyValue }, CurrentContext.CurrentTransaction);
        });
      }
      else
      {
        //loginProperty.Id = _prop.Id;
        await Task.Factory.StartNew(() =>
        {
          _prop.PropertyValue = loginProperty.PropertyValue;
        //  IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //  connection.Execute("Update auth_LoginProperties SET PropertyName=@PropertyName, PropertyValue=@PropertyValue WHERE Id=PropertyId", new { PropertyName = loginProperty.PropertyName, PropertyValue = loginProperty.PropertyValue, PropertyId = _prop.Id }, CurrentContext.CurrentTransaction);
        });
      }
      return loginProperty;
    }
  }
}
