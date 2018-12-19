using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Core.Helpers.Mapping;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserMap : BaseMap<User>
    {
        public UserMap(EntityTypeBuilder<User> entityTypeBuilder) : base(entityTypeBuilder)
        {
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