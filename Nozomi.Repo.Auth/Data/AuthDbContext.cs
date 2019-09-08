﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Repo.Auth.Data.Mappings;

namespace Nozomi.Repo.Auth.Data
{
    public class AuthDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<Address> Addresses { get; set; }
        
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

            builder
                .HasPostgresExtension("uuid-ossp")
                .Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            
            var addressMap = new AddressMap(builder.Entity<Address>());
            var roleClaimMap = new RoleClaimMap(builder.Entity<RoleClaim>());
            var roleMap = new RoleMap(builder.Entity<Role>());
            var userClaimMap = new UserClaimMap(builder.Entity<UserClaim>());
            var userLoginMap = new UserLoginMap(builder.Entity<UserLogin>());
            var userMap = new UserMap(builder.Entity<User>());
            var userRoleMap = new UserRoleMap(builder.Entity<UserRole>());
            var userTokenMap = new UserTokenMap(builder.Entity<UserToken>());
        }
    }
}