using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserClaimMap : BaseMap<UserClaim>
    {
        public UserClaimMap(EntityTypeBuilder<UserClaim> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(uc => uc.User).WithMany(u => u.UserClaims);
            
            entityTypeBuilder.ToTable("UserClaims");
        }
    }
}