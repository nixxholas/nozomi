using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Core;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Identity.Models.Subscription;
using Nozomi.Data;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.Identity.Data.Mappings;
using Nozomi.Repo.Identity.Data.Mappings.Identity;

namespace Nozomi.Repo.Identity.Data
{
    public class NozomiAuthContext : 
        IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IDbContext
    {
        public DbSet<ApiToken> ApiTokens { get; set; }
        public DbSet<DevKey> DevKeys { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        
        public NozomiAuthContext(DbContextOptions options) : base(options)
        { }
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            var roleClaimMap = new RoleClaimMap(builder.Entity<RoleClaim>());
            var roleMap = new RoleMap(builder.Entity<Role>());
            var userClaimMap = new UserClaimMap(builder.Entity<UserClaim>());
            var userLoginMap = new UserLoginMap(builder.Entity<UserLogin>());
            var userMap = new UserMap(builder.Entity<User>());
            var userRoleMap = new UserRoleMap(builder.Entity<UserRole>());
            var userTokenMap = new UserTokenMap(builder.Entity<UserToken>());

            var apiTokenMap = new ApiTokenMap(builder.Entity<ApiToken>());
            var devKeyMap = new DevKeyMap(builder.Entity<DevKey>());
            var userSubscriptionMap = new UserSubscriptionMap(builder.Entity<UserSubscription>());
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