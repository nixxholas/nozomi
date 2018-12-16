using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Identity.Models;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Repo.Identity.Data
{
    public class NozomiAuthContext : IdentityDbContext<User, Role, long>
    {
        public NozomiAuthContext(DbContextOptions options) : base(options)
        { }
 
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ApiResourceEntity> ApiResources { get; set; }
        public DbSet<IdentityResourceEntity> IdentityResources { get; set; }
         
 
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            
            
            builder.Entity<ClientEntity>().HasKey(m => m.ClientId);
            builder.Entity<ApiResourceEntity>().HasKey(m => m.ApiResourceName);
            builder.Entity<IdentityResourceEntity>().HasKey(m => m.IdentityResourceName);
            base.OnModelCreating(builder);
        }
    }
}