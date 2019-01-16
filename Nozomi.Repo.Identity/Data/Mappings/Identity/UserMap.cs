using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserMap : BaseMap<User>
    {
        public UserMap(EntityTypeBuilder<User> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(u => u.Id).HasName("User_PK_Id");

            entityTypeBuilder.Property(u => u.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(u => u.UserName).IsUnique().HasName("User_Index_UserName");
            entityTypeBuilder.HasIndex(u => u.NormalizedUserName).IsUnique()
                .HasName("User_Index_NormalizedUserName");
            entityTypeBuilder.HasIndex(u => u.Email).IsUnique().HasName("User_Index_Email");
            entityTypeBuilder.HasIndex(u => u.NormalizedEmail).IsUnique()
                .HasName("User_Index_NormalizedEmail");

            entityTypeBuilder.ForNpgsqlUseXminAsConcurrencyToken();

            entityTypeBuilder.HasMany(u => u.ApiTokens).WithOne(at => at.User)
                .HasForeignKey(at => at.UserId);
            entityTypeBuilder.HasMany(u => u.UserSubscriptions).WithOne(us => us.User)
                .HasForeignKey(us => us.UserId);
            entityTypeBuilder.HasMany(u => u.UserClaims).WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId);
            entityTypeBuilder.HasMany(u => u.UserLogins).WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId);
            entityTypeBuilder.HasMany(u => u.UserRoles).WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
            entityTypeBuilder.HasMany(u => u.UserTokens).WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId);
            
            entityTypeBuilder.ToTable("Users");
        }
    }
}