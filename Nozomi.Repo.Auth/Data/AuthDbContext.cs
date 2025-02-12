﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Repo.Auth.Data.Mappings;

namespace Nozomi.Repo.Auth.Data
{
    public class AuthDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, 
        UserToken>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            var addressMap = new AddressMap(builder.Entity<Address>());
            var apiKeyMap = new ApiKeyMap(builder.Entity<ApiKey>());
            var roleClaimMap = new RoleClaimMap(builder.Entity<RoleClaim>());
            var roleMap = new RoleMap(builder.Entity<Role>());
            var userClaimMap = new UserClaimMap(builder.Entity<UserClaim>());
            var userLoginMap = new UserLoginMap(builder.Entity<UserLogin>());
            var userMap = new UserMap(builder.Entity<User>());
            var userRoleMap = new UserRoleMap(builder.Entity<UserRole>());
            var userTokenMap = new UserTokenMap(builder.Entity<UserToken>());
        }

        public int SaveChanges(string userId)
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddTimestamps(string userId)
        {
            // throw new NotImplementedException();
        }
    }
}