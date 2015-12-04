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

    public static UnitOfWork Create(IDbContext context, LoginList currentLogins)
    {
      return new UnitOfWork(context,currentLogins);
    }

    public IDbContext DbContext{ get;set;}

    public LoginList CurrentLogins { get;set;}

    public UnitOfWork(IDbContext context, LoginList currentLogins)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.DbContext = context;
      CurrentLogins = currentLogins;
    }

    public void Dispose()
    {

    }

    public void BeginWork()
    {
      DbContext.BeginTransaction();
    }

    public void CommitWork()
    {
      DbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
      DbContext.RollbackTransaction();
    }
    public SessionRepository SessionManager
    {
      get;
      set;
    }

    public LoginRepository LoginManager
    {
      get;
      set;
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
      get;
      set;
    }

  }
}
