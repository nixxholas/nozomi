using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class RoleClaimMap : BaseMap<RoleClaim>
    {
        public RoleClaimMap(EntityTypeBuilder<RoleClaim> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(rc => rc.Role).WithMany(r => r.RoleClaims)
                .HasForeignKey(rc => rc.RoleId);
            
            entityTypeBuilder.ToTable("RoleClaims");
        }
    }
}