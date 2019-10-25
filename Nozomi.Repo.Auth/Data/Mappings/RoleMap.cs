using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Auth.Data.Mappings
{
    public class RoleMap : BaseMap<Role>
    {
        public RoleMap(EntityTypeBuilder<Role> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasMany(r => r.RoleClaims).WithOne(rc => rc.Role)
                .HasForeignKey(rc => rc.RoleId);
            
            entityTypeBuilder.ToTable("Roles");
        }
    }
}