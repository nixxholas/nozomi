using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Counter.SDK.SharedModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Identity.Models;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.Identity.Data.Mappings.Identity;

namespace Nozomi.Repo.Identity.Data
{
    public class NozomiAuthContext : IdentityDbContext<User, Role, long>, IDbContext
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

        public int SaveChanges(long userId = 0)
        {
            AddTimestamps(userId);
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(long userId = 0
            , CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps(userId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddTimestamps(long userId = 0)
        {
            var entities = ChangeTracker.Entries().Where(x =>
                x.Entity is BaseEntityModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        ((BaseEntityModel) entity.Entity).CreatedAt = DateTime.UtcNow;
                        ((BaseEntityModel) entity.Entity).CreatedBy = userId;
                        break;
                    case EntityState.Deleted:
                        ((BaseEntityModel) entity.Entity).DeletedAt = DateTime.UtcNow;
                        ((BaseEntityModel) entity.Entity).DeletedBy = userId;
                        break;
                }

                ((BaseEntityModel) entity.Entity).ModifiedAt = DateTime.UtcNow;
                ((BaseEntityModel) entity.Entity).ModifiedBy = userId;
            }
        }
    }
}