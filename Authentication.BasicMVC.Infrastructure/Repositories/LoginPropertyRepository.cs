using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Domain.Repositories;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class LoginPropertyRepository
  {

    private readonly IDbContext CurrentContext;

    public LoginPropertyRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    //public LoginPropertyRepository()
    //{
    //  this.CurrentContext = DbContext.Create();
    //}

    public void Dispose()
    {

    }

    public virtual Task<LoginProperty> FindByIdAsync(Guid propertyID)
    {
      if (propertyID == Guid.Empty)
        throw new ArgumentNullException("propertyID");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<LoginProperty>("select DISTINCT * FROM auth_LoginProperties WHERE Id=@PropertyId", new { PropertyId = propertyID }).SingleOrDefault();
      });
    }

    public virtual Task<LoginProperty> FindByNameAsync(Guid loginId, string propertyName)
    {
      if (loginId == Guid.Empty)
        throw new ArgumentNullException("loginId");

      if (propertyName == "")
        throw new ArgumentNullException("propertyName");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<LoginProperty>("select DISTINCT * FROM auth_LoginProperties WHERE LoginId=@LoginId AND PropertyName LIKE @PropertyName", new { LoginId = loginId, PropertyName = propertyName }).SingleOrDefault();
      });
    }

    public async Task<LoginProperty> UpdateAsync(LoginProperty loginProperty)
    {
      if (loginProperty == null)
        throw new ArgumentNullException("loginProperty");
      LoginProperty _prop = await FindByNameAsync(loginProperty.LoginId,loginProperty.PropertyName);
      if(_prop==null)
      {
        loginProperty.Id = Guid.NewGuid();
        await Task.Factory.StartNew(() =>
        {
          IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
          connection.Execute("INSERT INTO auth_LoginProperties(Id, LoginId, PropertyName, PropertyValue) VALUES(@Id, @LoginId, @PropertyName, @PropertyValue)", new { Id=loginProperty.Id, LoginId=loginProperty.LoginId, PropertyName=loginProperty.PropertyName, PropertyValue=loginProperty.PropertyValue }, CurrentContext.CurrentTransaction);
        });
      }
      else
      { 
        loginProperty.Id=_prop.Id;
        await Task.Factory.StartNew(() =>
        {
          IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
          connection.Execute("Update auth_LoginProperties SET PropertyName=@PropertyName, PropertyValue=@PropertyValue WHERE Id=PropertyId", new { PropertyName=loginProperty.PropertyName, PropertyValue=loginProperty.PropertyValue, PropertyId=_prop.Id }, CurrentContext.CurrentTransaction);
        });
      }
      return loginProperty;
    }
  }
}
