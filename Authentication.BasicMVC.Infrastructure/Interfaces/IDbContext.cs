﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Authentication.BasicMVC.Infrastructure.Interfaces
{
  public interface IDbContext : IDisposable
  {
    
    IDbConnection OpenConnection();
    IDbConnection OpenConnection(IDbTransaction transaction);
    IDbTransaction CurrentTransaction { get;}
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();

  }
}
