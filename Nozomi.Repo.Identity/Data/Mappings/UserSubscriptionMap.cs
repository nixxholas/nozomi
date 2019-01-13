using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.ViewModels.Subscription;

namespace Nozomi.Repo.Identity.Data.Mappings
{
    public class UserSubscriptionMap : BaseMap<UserSubscription>
    {
        public UserSubscriptionMap(EntityTypeBuilder<UserSubscription> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(us => us.Id).HasName("UserSubscription_PK_Id");
            entityTypeBuilder.Property(us => us.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(us => new { us.UserId, us.DeletedAt }).IsUnique()
                .HasName("UserSubscription_Index_UserId_DeletedAt");
            entityTypeBuilder.HasIndex(us => new {us.UserId, us.SubscriptionId}).IsUnique()
                .HasName("UserSubscription_Index_UserId_SubscriptionId");

            entityTypeBuilder.Property(us => us.SubscriptionId).HasDefaultValue(null).IsRequired(false);
            entityTypeBuilder.Property(us => us.PlanType).IsRequired().HasDefaultValue(PlanType.Basic);

            entityTypeBuilder.HasOne(us => us.User).WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId);
            entityTypeBuilder.HasMany(us => us.DevKeys).WithOne(dk => dk.UserSubscription)
                .HasForeignKey(dk => dk.UserSubscriptionId);
        }
    }
}