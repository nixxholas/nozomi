using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Mapping;

namespace Nozomi.Repo.Auth.Data.Mappings
{
    public class ApiKeyMap : BaseMap<ApiKey>
    {
        public ApiKeyMap(EntityTypeBuilder<ApiKey> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid);
            entityTypeBuilder.HasAlternateKey(e => e.Value);

            entityTypeBuilder.Property(e => e.Revealed).HasDefaultValue(false);
            entityTypeBuilder.Property(e => e.Label).IsRequired(false);
            // now() - https://www.postgresql.org/docs/9.1/functions-datetime.html
            entityTypeBuilder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entityTypeBuilder.HasOne(e => e.User)
                .WithMany(u => u.ApiKeys)
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}