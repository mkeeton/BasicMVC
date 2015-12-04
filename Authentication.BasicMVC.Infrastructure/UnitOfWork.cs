using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BasicMVC.Domain.Models;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;
using BasicMVC.Core.Data.DbContext;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure
{
  public class UnitOfWork : IDisposable
  {
    private IDbContext _dbContext;
    private SessionRepository _sessionRepository;
    private LoginRepository _loginRepository;
    private LoginPropertyRepository _loginPropertyRepository;
    private UserStore<User> _userRepository;

    public static UnitOfWork Create(IDbContext context, LoginList currentLogins)
    {
      return new UnitOfWork(context,currentLogins);
    }

    public IDbContext DbContext
    {
      get
      {
        return _dbContext;
      }
      set
      {
        _dbContext = value;
      }
    }

    public LoginList CurrentLogins { get;set;}

    public UnitOfWork(IDbContext context, LoginList currentLogins)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this._dbContext = context;
      CurrentLogins = currentLogins;
    }

    public void Dispose()
    {

    }

    public void BeginWork()
    {
      _dbContext.BeginTransaction();
    }

    public void CommitWork()
    {
      _dbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
      _dbContext.RollbackTransaction();
    }
    public SessionRepository SessionManager
    {
      get
      {
        if (_sessionRepository == null)
        {
          _sessionRepository = new SessionRepository(_dbContext);
        }
        return _sessionRepository;
      }
      private set
      {
        _sessionRepository = value;
      }
    }

    public LoginRepository LoginManager
    {
      get
      {
        if (_loginRepository == null)
        {
          _loginRepository = new LoginRepository(CurrentLogins);
        }
        return _loginRepository;
      }
      private set
      {
        _loginRepository = value;
      }
    }

    //public LoginPropertyRepository LoginPropertyManager
    //{
    //  get
    //  {
    //    if (_loginPropertyRepository == null)
    //    {
    //      _loginPropertyRepository = new LoginPropertyRepository(_dbContext);
    //    }
    //    return _loginPropertyRepository;
    //  }
    //  private set
    //  {
    //    _loginPropertyRepository = value;
    //  }
    //}

    public UserStore<User> UserManager
    {
      get
      {
        if (_userRepository == null)
        {
          _userRepository = new UserStore<User>(_dbContext);
        }
        return _userRepository;
      }
      private set
      {
        _userRepository = value;
      }
    }

  }
}
