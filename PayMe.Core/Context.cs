using Microsoft.AspNet.Identity.EntityFramework;
using PayMe.Core.Entities;
using servus.core.Entities;
using System;
using System.Data.Entity;

namespace PayMe.Core
{
    public class Context : IdentityDbContext<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public Context(string cstring) : base(cstring)
        {

        }

        public Context()
            : base("DefaultConnection")
        {
        }

        public static Context Create()
        {
            var db = new Context();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            return db;
        }

        public DbSet<Instance> Instances { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<UserToInstanceMapping> UserToInstanceMappings { get; set; }
        public DbSet<StoredEvent> StoredEvents { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Expense> Expenses { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Abscense> Abscenses { get; set; }

        protected override void OnModelCreating(DbModelBuilder m)
        {
            base.OnModelCreating(m);

            m.Entity<UserToInstanceMapping>().HasKey(x => new { x.InstanceId, x.UserId });

            m.Entity<Instance>().Property(x => x.Name).IsRequired();
            m.Entity<Instance>().Property(x => x.JoinCode).IsRequired();

            m.Entity<Abscense>().HasKey(x => new { x.UserId, x.InstanceId, x.From });
        }
    }
}
