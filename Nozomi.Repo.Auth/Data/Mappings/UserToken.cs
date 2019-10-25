using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Auth.Data.Mappings
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