using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using BasicMVC.Core.Data.DbContext;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class DbContext
  {

    public static IDbContext Create()
    {
      return new DapperDbContext();
    }
  }
}
