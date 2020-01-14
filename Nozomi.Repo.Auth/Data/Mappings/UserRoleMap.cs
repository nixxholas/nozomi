using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Mapping;

namespace Nozomi.Repo.Auth.Data.Mappings
{
    public class UserRoleMap : BaseMap<UserRole>
    {
        public UserRoleMap(EntityTypeBuilder<UserRole> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            entityTypeBuilder.HasOne(ur => ur.User).WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            
            entityTypeBuilder.ToTable("UserRoles");
        }
    }
}