using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web;
using System.Web.Configuration;
using Authentication.BasicMVC.Infrastructure;
using Authentication.BasicMVC.Infrastructure.Repositories;
using BasicMVC.Core.Data.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace Authentication.BasicMVC.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Classes.FromThisAssembly()
          .BasedOn<IController>()
          .LifestyleTransient());
      container.Register(
          Component.For<IDbContext>()
              .UsingFactoryMethod(_ => DbContext.Create(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<BasicMVC.Domain.Models.LoginList>()
                .UsingFactoryMethod(_ => new BasicMVC.Domain.Models.LoginList(360))
                .LifeStyle.Singleton
      );

      container.Register(
                Component.For<UserStore<BasicMVC.Domain.Models.User>>()
                .ImplementedBy<UserStore<BasicMVC.Domain.Models.User>>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<LoginRepository>()
          .ImplementedBy<LoginRepository>()
          .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<SessionRepository>()
          .ImplementedBy<SessionRepository>()
          .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<UnitOfWork>()
                .ImplementedBy<UnitOfWork>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<ApplicationUserManager>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()).LifestylePerWebRequest()
      );

    }

  }
}