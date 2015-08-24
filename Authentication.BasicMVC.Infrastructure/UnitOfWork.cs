using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;

namespace Authentication.BasicMVC.Infrastructure
{
  public class UnitOfWork : IDisposable
  {
    private IDbContext _dbContext;
    private SessionRepository _sessionRepository;
    private LoginRepository _loginRepository;

    public UnitOfWork(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this._dbContext = context;
    }

    public UnitOfWork()
    {
      this._dbContext = DbContext.Create();
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
          _loginRepository = new LoginRepository(_dbContext);
        }
        return _loginRepository;
      }
      private set
      {
        _loginRepository = value;
      }
    }

  }
}
