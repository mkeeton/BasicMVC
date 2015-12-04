using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Domain.Repositories;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using System.Data;
using Dapper;
using Microsoft.AspNet.Identity;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{

  public class UserStore<T> : IUserStore<User, Guid>, IUserLoginStore<User,Guid>, IUserPasswordStore<User,Guid>, IUserSecurityStampStore<User,Guid>, IUserEmailStore<User,Guid>
  {
    private readonly IDbContext CurrentContext;

    public UserStore(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public void Dispose()
    {

    }

    #region IUserStore
    public virtual Task CreateAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");
      var owner = this.FindByNameAsync(user.UserName);
      if((owner==null)||(owner.Result==null))
      {
        return Task.Factory.StartNew(() =>
        {
          user.Id = Guid.NewGuid();
          using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("insert into auth_Users(Id, UserName, PasswordHash, SecurityStamp, Email, EmailConfirmed) values(@Id, @userName, @passwordHash, @securityStamp, @email, @emailConfirmed)", user);
        });
      }
      else
      {
        user.Id = owner.Result.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task DeleteAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_Users where Id = @Id", new { user.Id });
      });
    }

    public virtual Task<User> FindByIdAsync(Guid userId)
    {
      if (userId==Guid.Empty)
        throw new ArgumentNullException("userId");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where Id = @Id", new { Id = userId }).SingleOrDefault();
      });
    }

    public virtual Task<User> FindByNameAsync(string userName)
    {
      if (string.IsNullOrWhiteSpace(userName))
        throw new ArgumentNullException("userName");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where lower(UserName) = lower(@userName)", new { userName }).SingleOrDefault();
      });
    }

    public virtual Task UpdateAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update auth_Users set UserName = @userName, PasswordHash = @passwordHash, SecurityStamp = @securityStamp where Id = @Id", user);
      });
    }
    #endregion

    #region IUserLoginStore
    public virtual Task AddLoginAsync(User user, UserLoginInfo login)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      if (login == null)
        throw new ArgumentNullException("login");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("INSERT INTO auth_ExternalLogins(Id, UserId, LoginProvider, ProviderKey) values(@Id, @userId, @loginProvider, @providerKey)",
              new { Id = Guid.NewGuid(), userId = user.Id, loginProvider = login.LoginProvider, providerKey = login.ProviderKey });
      });
    }

    public virtual Task<User> FindAsync(UserLoginInfo login)
    {
      if (login == null)
        throw new ArgumentNullException("login");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("SELECT u.* FROM auth_Users u INNER JOIN auth_ExternalLogins l ON l.UserId = u.Id WHERE l.LoginProvider = @loginProvider AND l.ProviderKey = @providerKey",
              login).SingleOrDefault();
      });
    }

    public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<UserLoginInfo>)connection.Query<UserLoginInfo>("SELECT LoginProvider, ProviderKey FROM auth_ExternalLogins WHERE UserId = @userId", new { userId = user.Id }).ToList();
      });
    }

    public virtual Task RemoveLoginAsync(User user)
    {
      return RemoveLoginAsync(user,new UserLoginInfo("",""));
    }

    public virtual Task RemoveLoginAsync(User user, UserLoginInfo login)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      if (login == null)
        throw new ArgumentNullException("login");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_ExternalLogins where UserId = @userId and ((@loginProvider='' AND @providerKey='') OR (LoginProvider = @loginProvider and ProviderKey = @providerKey))",
              new { userId=user.Id, login.LoginProvider, login.ProviderKey });
      });
    }

    #endregion

    #region LoginSession

    public virtual Task CreateLoginSessionAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("user");
      var owner = this.FindLoginSessionByIdAsync(login.Id);
      if ((owner == null) || (owner.Result == null))
      {
        return Task.Factory.StartNew(() =>
        {
          login.Id = Guid.NewGuid();
          using(IDbConnection connection = CurrentContext.OpenConnection())
            connection.Execute("insert into auth_Users(Id, UserName, PasswordHash, SecurityStamp, Email, EmailConfirmed) values(@Id, @userName, @passwordHash, @securityStamp, @email, @emailConfirmed)", login);
        });
      }
      else
      {
        login.Id = owner.Result.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task DeleteLoginSessionAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_Users where Id = @Id", new { login.Id });
      });
    }

    public virtual Task<User> FindLoginSessionByIdAsync(Guid loginId)
    {
      if (loginId == Guid.Empty)
        throw new ArgumentNullException("loginId");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where Id = @Id", new { Id = loginId }).SingleOrDefault();
      });
    }

    public virtual Task<User> FindLoginSessionByUserAsync(Guid userId)
    {
      if (userId == Guid.Empty)
        throw new ArgumentNullException("userName");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where lower(UserName) = lower(@userName)", new { userId }).SingleOrDefault();
      });
    }

    public virtual Task UpdateLoginSessionAsync(Login login)
    {
      if (login == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update auth_Users set UserName = @userName, PasswordHash = @passwordHash, SecurityStamp = @securityStamp where Id = @Id", login);
      });
    }
    #endregion

    #region IUserPasswordStore
    public virtual Task<string> GetPasswordHashAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.PasswordHash);
    }

    public virtual Task<bool> HasPasswordAsync(User user)
    {
      return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public virtual Task SetPasswordHashAsync(User user, string passwordHash)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.PasswordHash = passwordHash;

      return Task.FromResult(0);
    }

    #endregion

    #region IUserSecurityStampStore
    public virtual Task<string> GetSecurityStampAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.SecurityStamp);
    }

    public virtual Task SetSecurityStampAsync(User user, string stamp)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.SecurityStamp = stamp;

      return Task.FromResult(0);
    }

    #endregion

    public Task<User> FindByEmailAsync(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentNullException("email");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where lower(Email) = lower(@Email)", new { email }).SingleOrDefault();
      });
    }

    public Task<string> GetEmailAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailAsync(User user, string email)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.Email= email;

      return Task.FromResult(0);
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.EmailConfirmed = confirmed;
      return Task.FromResult(0);
    }
  }
}
