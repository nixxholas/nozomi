using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Core.Helpers.Mapping;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserRoleMap : BaseMap<UserRole>
    {
        public UserRoleMap(EntityTypeBuilder<UserRole> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("UserRoles");
        }
    }
}