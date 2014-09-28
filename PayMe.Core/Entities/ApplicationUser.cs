using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PayMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace servus.core.Entities
{
    public class CustomRole : IdentityRole<Guid, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserRole : IdentityUserRole<Guid> { }
    public class CustomUserClaim : IdentityUserClaim<Guid> { }
    public class CustomUserLogin : IdentityUserLogin<Guid> { }

    public class ApplicationUser : IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string GetName()
        {
            return FirstName + " " + LastName;
        }

        public ICollection<UserToInstanceMapping> Instances { get; set; }
        public ICollection<Abscense> Abscenses { get; set; }
        //public ICollection<Expense> Expenses { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
