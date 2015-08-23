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
    public class DbContext
    {

        public static Interfaces.IDbContext Create()
        {
          return new DapperDbContext();
        }
    }
}
