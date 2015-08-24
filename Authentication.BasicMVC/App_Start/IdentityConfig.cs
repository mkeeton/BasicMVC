using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Authentication.BasicMVC.Models;
using Authentication.BasicMVC.Domain.Models;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using Authentication.BasicMVC.Infrastructure.Repositories;
using System.Security.Claims;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

  public class ApplicationUserManager : UserManager<User,System.Guid>
    {
        private bool _disposed;

        public ApplicationUserManager(IUserStore<User,System.Guid> store)
            : base(store)
        {
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
        {
          // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
          var userIdentity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
          // Add custom user claims here
          return userIdentity;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<User>(context.Get<IDbContext>()));
            // Configure validation logic for usernames
            Controllers.UserValidator UserVal  = new Controllers.UserValidator(manager);
            UserVal.AllowOnlyAlphanumericUserNames = false;
            UserVal.RequireUniqueEmail = false;
            manager.UserValidator = UserVal;
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User,System.Guid>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User,System.Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
              manager.UserTokenProvider = new DataProtectorTokenProvider<User, System.Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        // IUserLoginStore methods
        private Authentication.BasicMVC.Infrastructure.Repositories.UserStore<User> GetLoginStore()
        {
          var cast = Store as Authentication.BasicMVC.Infrastructure.Repositories.UserStore<User>;
          if (cast == null)
          {
            //throw new NotSupportedException(Resources.StoreNotIUserLoginStore);
          }
          return cast;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

}
