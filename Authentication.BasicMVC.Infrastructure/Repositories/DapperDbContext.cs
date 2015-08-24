using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class DapperDbContext : Interfaces.IDbContext
  {

    IDbTransaction _transaction = null;

    public IDbConnection OpenConnection()
    {
      IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
      connection.Open();
      return connection;
    }

    public IDbConnection OpenConnection(IDbTransaction transaction)
    {
      if(transaction == null || transaction.Connection == null)
      {
        return OpenConnection();
      }
      else
      { 
        return transaction.Connection;
      }
    }

    public IDbTransaction CurrentTransaction
    {
      get
      {
        return _transaction;
      }
    }

    public void BeginTransaction()
    {
      _transaction = OpenConnection().BeginTransaction();
    }

    public void CommitTransaction()
    {
      _transaction.Commit();
      _transaction = null;
    }

    public void RollbackTransaction()
    {
      _transaction.Rollback();
      _transaction = null;
    }

    public void Dispose()
    {
      
    }
  }
}
