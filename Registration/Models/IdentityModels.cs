using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Registration.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Address { get; set; }
        public string Name { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Registration.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Registration.Models.Route> Routes { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Route>()
                        .HasRequired(m => m.FromCity)
                        .WithMany(t => t.FromRouts)
                        .HasForeignKey(m => m.FromCityId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                        .HasRequired(m => m.TOCity)
                        .WithMany(t => t.TORouts)
                        .HasForeignKey(m => m.TOCityId)
                        .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<Registration.Models.Trip> Trips { get; set; }

        public System.Data.Entity.DbSet<Registration.Models.Reserve> Reserves { get; set; }

    }
}