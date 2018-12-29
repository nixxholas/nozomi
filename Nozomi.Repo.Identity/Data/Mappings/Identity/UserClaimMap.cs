using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Core.Helpers.Mapping;

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