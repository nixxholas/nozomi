using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Auth.Data.Mappings
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