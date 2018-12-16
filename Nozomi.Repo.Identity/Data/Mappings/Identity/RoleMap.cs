using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Data.Mappings;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class RoleMap : BaseMap<Role>
    {
        public RoleMap(EntityTypeBuilder<Role> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Roles");
        }
    }
}