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
    public IDbConnection OpenConnection()
    {
      IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
      connection.Open();
      return connection;
    }

    public void Dispose()
    {
      
    }
  }
}
