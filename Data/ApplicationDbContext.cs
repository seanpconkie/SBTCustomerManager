using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models;

namespace SBTCustomerManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CompanyDetail> CompanyDetails { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RoleTypes> RoleTypes { get; set; }
        public DbSet<RoleType> RoleType { get; set; }
        public DbSet<RoleDescription> RoleDescriptions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
