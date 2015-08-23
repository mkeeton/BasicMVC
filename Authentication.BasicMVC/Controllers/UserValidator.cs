using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Authentication.BasicMVC.Domain.Models;

namespace Authentication.BasicMVC.Controllers
{

  /// <summary>
  ///     Validates users before they are saved to an IUserStore
  /// </summary>
  /// <typeparam name="TUser"></typeparam>
  /// <typeparam name="TKey"></typeparam>
  public class UserValidator : IIdentityValidator<User>
  {
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="manager"></param>
    public UserValidator(ApplicationUserManager manager)
    {
      if (manager == null)
      {
        throw new ArgumentNullException("manager");
      }
      AllowOnlyAlphanumericUserNames = true;
      Manager = manager;
    }

    /// <summary>
    ///     Only allow [A-Za-z0-9@_] in UserNames
    /// </summary>
    public bool AllowOnlyAlphanumericUserNames { get; set; }

    /// <summary>
    ///     If set, enforces that emails are non empty, valid, and unique
    /// </summary>
    public bool RequireUniqueEmail { get; set; }

    /// <summary>
    ///     If set, enforces that usernames are non empty, valid, and unique
    /// </summary>
    public bool RequireUniqueUserName { get; set;}

    private ApplicationUserManager Manager { get; set; }

    /// <summary>
    ///     Validates a user before saving
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResult> ValidateAsync(User item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      var errors = new List<string>();
      if (RequireUniqueUserName)
      {
        await ValidateUserName(item, errors);
      }
      if (RequireUniqueEmail)
      {
        await ValidateEmail(item, errors);
      }
      if (errors.Count > 0)
      {
        return IdentityResult.Failed(errors.ToArray());
      }
      return IdentityResult.Success;
    }

    private async Task ValidateUserName(User user, List<string> errors)
    {
      if (string.IsNullOrWhiteSpace(user.UserName))
      {
        errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Name"));
      }
      else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
      {
        // If any characters are not letters or digits, its an illegal user name
        errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.InvalidUserName, user.UserName));
      }
      else
      {
        var owner = await Manager.FindByNameAsync(user.UserName);
        if (owner != null && (owner.Id != user.Id))
        {
          errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.DuplicateName, user.UserName));
        }
      }
    }

    // make sure email is not empty, valid, and unique
    private async Task ValidateEmail(User user, List<string> errors)
    {
      var email = user.Email;
      if (string.IsNullOrWhiteSpace(email))
      {
        errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Email"));
        return;
      }
      try
      {
        var m = new MailAddress(email);
      }
      catch (FormatException)
      {
        errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.InvalidEmail, email));
        return;
      }
      var owner = await Manager.FindByEmailAsync(email);
      if (owner != null && (owner.Id != user.Id))
      {
        errors.Add(String.Format(CultureInfo.CurrentCulture, Resources.DuplicateEmail, email));
      }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources
    {

      private static global::System.Resources.ResourceManager resourceMan;

      private static global::System.Globalization.CultureInfo resourceCulture;

      [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
      internal Resources()
      {
      }

      /// <summary>
      ///   Returns the cached ResourceManager instance used by this class.
      /// </summary>
      [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
      internal static global::System.Resources.ResourceManager ResourceManager
      {
        get
        {
          if (object.ReferenceEquals(resourceMan, null))
          {
            global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.AspNet.Identity.Resources", typeof(Resources).Assembly);
            resourceMan = temp;
          }
          return resourceMan;
        }
      }

      /// <summary>
      ///   Overrides the current thread's CurrentUICulture property for all
      ///   resource lookups using this strongly typed resource class.
      /// </summary>
      [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
      internal static global::System.Globalization.CultureInfo Culture
      {
        get
        {
          return resourceCulture;
        }
        set
        {
          resourceCulture = value;
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to An unknown failure has occured..
      /// </summary>
      internal static string DefaultError
      {
        get
        {
          return ResourceManager.GetString("DefaultError", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Email &apos;{0}&apos; is already taken..
      /// </summary>
      internal static string DuplicateEmail
      {
        get
        {
          return ResourceManager.GetString("DuplicateEmail", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Name {0} is already taken..
      /// </summary>
      internal static string DuplicateName
      {
        get
        {
          return ResourceManager.GetString("DuplicateName", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to A user with that external login already exists..
      /// </summary>
      internal static string ExternalLoginExists
      {
        get
        {
          return ResourceManager.GetString("ExternalLoginExists", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Email &apos;{0}&apos; is invalid..
      /// </summary>
      internal static string InvalidEmail
      {
        get
        {
          return ResourceManager.GetString("InvalidEmail", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Invalid token..
      /// </summary>
      internal static string InvalidToken
      {
        get
        {
          return ResourceManager.GetString("InvalidToken", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to User name {0} is invalid, can only contain letters or digits..
      /// </summary>
      internal static string InvalidUserName
      {
        get
        {
          return ResourceManager.GetString("InvalidUserName", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to No IUserTokenProvider is registered..
      /// </summary>
      internal static string NoTokenProvider
      {
        get
        {
          return ResourceManager.GetString("NoTokenProvider", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to No IUserTwoFactorProvider for &apos;{0}&apos; is registered..
      /// </summary>
      internal static string NoTwoFactorProvider
      {
        get
        {
          return ResourceManager.GetString("NoTwoFactorProvider", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Incorrect password..
      /// </summary>
      internal static string PasswordMismatch
      {
        get
        {
          return ResourceManager.GetString("PasswordMismatch", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Passwords must have at least one digit (&apos;0&apos;-&apos;9&apos;)..
      /// </summary>
      internal static string PasswordRequireDigit
      {
        get
        {
          return ResourceManager.GetString("PasswordRequireDigit", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Passwords must have at least one lowercase (&apos;a&apos;-&apos;z&apos;)..
      /// </summary>
      internal static string PasswordRequireLower
      {
        get
        {
          return ResourceManager.GetString("PasswordRequireLower", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Passwords must have at least one non letter or digit character..
      /// </summary>
      internal static string PasswordRequireNonLetterOrDigit
      {
        get
        {
          return ResourceManager.GetString("PasswordRequireNonLetterOrDigit", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Passwords must have at least one uppercase (&apos;A&apos;-&apos;Z&apos;)..
      /// </summary>
      internal static string PasswordRequireUpper
      {
        get
        {
          return ResourceManager.GetString("PasswordRequireUpper", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Passwords must be at least {0} characters..
      /// </summary>
      internal static string PasswordTooShort
      {
        get
        {
          return ResourceManager.GetString("PasswordTooShort", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to {0} cannot be null or empty..
      /// </summary>
      internal static string PropertyTooShort
      {
        get
        {
          return ResourceManager.GetString("PropertyTooShort", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Role {0} does not exist..
      /// </summary>
      internal static string RoleNotFound
      {
        get
        {
          return ResourceManager.GetString("RoleNotFound", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IQueryableRoleStore&lt;TRole&gt;..
      /// </summary>
      internal static string StoreNotIQueryableRoleStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIQueryableRoleStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IQueryableUserStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIQueryableUserStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIQueryableUserStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserClaimStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserClaimStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserClaimStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserConfirmationStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserConfirmationStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserConfirmationStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserEmailStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserEmailStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserEmailStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserLoginStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserLoginStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserLoginStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserPasswordStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserPasswordStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserPasswordStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserPhoneNumberStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserPhoneNumberStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserPhoneNumberStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserRoleStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserRoleStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserRoleStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserSecurityStampStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserSecurityStampStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserSecurityStampStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to Store does not implement IUserTwoFactorStore&lt;TUser&gt;..
      /// </summary>
      internal static string StoreNotIUserTwoFactorStore
      {
        get
        {
          return ResourceManager.GetString("StoreNotIUserTwoFactorStore", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to User already has a password set..
      /// </summary>
      internal static string UserAlreadyHasPassword
      {
        get
        {
          return ResourceManager.GetString("UserAlreadyHasPassword", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to User already in role..
      /// </summary>
      internal static string UserAlreadyInRole
      {
        get
        {
          return ResourceManager.GetString("UserAlreadyInRole", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to UserId not found..
      /// </summary>
      internal static string UserIdNotFound
      {
        get
        {
          return ResourceManager.GetString("UserIdNotFound", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to User {0} does not exist..
      /// </summary>
      internal static string UserNameNotFound
      {
        get
        {
          return ResourceManager.GetString("UserNameNotFound", resourceCulture);
        }
      }

      /// <summary>
      ///   Looks up a localized string similar to User is not in role..
      /// </summary>
      internal static string UserNotInRole
      {
        get
        {
          return ResourceManager.GetString("UserNotInRole", resourceCulture);
        }
      }
    }
  }
}