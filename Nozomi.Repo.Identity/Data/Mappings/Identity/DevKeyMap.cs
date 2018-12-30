using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.Models.Subscription;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class DevKeyMap : BaseMap<DevKey>
    {
        public DevKeyMap(EntityTypeBuilder<DevKey> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(dk => dk.Id).HasName("DevKey_PK_Id");
            entityTypeBuilder.Property(dk => dk.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(dk => dk.CreatedAt).HasDefaultValue(DateTime.UtcNow);
            entityTypeBuilder.Property(dk => dk.ModifiedAt).HasDefaultValue(DateTime.UtcNow);
            entityTypeBuilder.Property(dk => dk.DeletedAt).HasDefaultValue(null);

            entityTypeBuilder.HasOne(dk => dk.User).WithMany(u => u.DevKeys)
                .HasForeignKey(dk => dk.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}