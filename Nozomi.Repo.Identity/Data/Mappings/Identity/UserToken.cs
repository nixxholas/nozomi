using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Core.Helpers.Mapping;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserTokenMap : BaseMap<UserToken>
    {
        public UserTokenMap(EntityTypeBuilder<UserToken> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("UserTokens");
        }
    }
}