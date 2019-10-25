using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Auth.Data.Mappings
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