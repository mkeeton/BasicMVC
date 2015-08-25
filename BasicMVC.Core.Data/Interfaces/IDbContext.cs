using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BasicMVC.Core.Data.Interfaces
{
  public interface IDbContext : IDisposable
  {

    IDbConnection OpenConnection();
    IDbConnection OpenConnection(IDbTransaction transaction);
    IDbTransaction CurrentTransaction { get; }
    string ConnectionString { get; set; }
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();

  }
}
