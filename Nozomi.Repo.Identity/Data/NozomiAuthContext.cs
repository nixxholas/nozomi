using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Identity.Models;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Identity.Data.Mappings.Identity;

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
            var roleClaimMap = new RoleClaimMap(builder.Entity<RoleClaim>());
            var roleMap = new RoleMap(builder.Entity<Role>());
            var userClaimMap = new UserClaimMap(builder.Entity<UserClaim>());
            var userLoginMap = new UserLoginMap(builder.Entity<UserLogin>());
            var userMap = new UserMap(builder.Entity<User>());
            var userRoleMap = new UserRoleMap(builder.Entity<UserRole>());
            var userTokenMap = new UserTokenMap(builder.Entity<UserToken>());
            
            builder.Entity<ClientEntity>().HasKey(m => m.ClientId);
            builder.Entity<ApiResourceEntity>().HasKey(m => m.ApiResourceName);
            builder.Entity<IdentityResourceEntity>().HasKey(m => m.IdentityResourceName);
            base.OnModelCreating(builder);
        }
    }
}