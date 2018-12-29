using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Core.Helpers.Mapping;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserTokenMap : BaseMap<UserToken>
    {
        public UserTokenMap(EntityTypeBuilder<UserToken> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(ut => ut.User).WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId);
            
            entityTypeBuilder.ToTable("UserTokens");
        }
    }
}