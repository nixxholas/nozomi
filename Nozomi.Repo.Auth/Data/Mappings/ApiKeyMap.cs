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

            entityTypeBuilder.Property(e => e.Label).IsRequired(false);
            entityTypeBuilder.Property(e => e.CreatedAt).ValueGeneratedOnAdd()
                .HasDefaultValueSql("getdate()");;

            entityTypeBuilder.HasOne(e => e.User)
                .WithMany(u => u.ApiKeys)
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}