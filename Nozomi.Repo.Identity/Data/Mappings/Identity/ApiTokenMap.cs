using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class ApiTokenMap : BaseMap<ApiToken>
    {
        public ApiTokenMap(EntityTypeBuilder<ApiToken> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(at => at.Guid).HasName("ApiToken_PK_Guid");
            entityTypeBuilder.Property(at => at.Guid).HasDefaultValue(Guid.NewGuid());

            entityTypeBuilder.Property(at => at.Label).IsRequired(false);
            entityTypeBuilder.Property(at => at.LastAccessed).HasDefaultValue(DateTime.Now);
            entityTypeBuilder.Property(at => at.Secret).IsRequired();
            entityTypeBuilder.Property(at => at.Key).IsRequired();

            entityTypeBuilder.HasOne(at => at.User).WithMany(u => u.ApiTokens)
                .HasForeignKey(at => at.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}