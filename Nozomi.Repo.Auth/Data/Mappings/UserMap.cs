using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Core.Helpers.Mapping;

namespace Nozomi.Repo.Auth.Data.Mappings
{
    public class UserMap : BaseMap<User>
    {
        public UserMap(EntityTypeBuilder<User> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(u => u.Id).HasName("User_PK_Id");

            entityTypeBuilder.HasIndex(u => u.UserName).IsUnique().HasName("User_Index_UserName");
            entityTypeBuilder.HasIndex(u => u.NormalizedUserName).IsUnique()
                .HasName("User_Index_NormalizedUserName");
            entityTypeBuilder.HasIndex(u => u.Email).IsUnique().HasName("User_Index_Email");
            entityTypeBuilder.HasIndex(u => u.NormalizedEmail).IsUnique()
                .HasName("User_Index_NormalizedEmail");

            entityTypeBuilder.ForNpgsqlUseXminAsConcurrencyToken();

            entityTypeBuilder.HasMany(u => u.UserClaims).WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId);
            entityTypeBuilder.HasMany(u => u.UserLogins).WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId);
            entityTypeBuilder.HasMany(u => u.UserRoles).WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
            entityTypeBuilder.HasMany(u => u.UserTokens).WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId);
            entityTypeBuilder.HasMany(u => u.Addresses).WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
            
            entityTypeBuilder.ToTable("Users");
        }
    }
}