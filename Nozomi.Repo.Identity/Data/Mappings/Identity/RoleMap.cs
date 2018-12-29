using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Core.Helpers.Mapping;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
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