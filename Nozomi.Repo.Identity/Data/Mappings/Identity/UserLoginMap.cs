using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Repo.Identity.Data.Mappings.Identity
{
    public class UserLoginMap : BaseMap<UserLogin>
    {
        public UserLoginMap(EntityTypeBuilder<UserLogin> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(ul => ul.User).WithMany(u => u.UserLogins)
                .HasForeignKey(ul => ul.UserId);
            
            entityTypeBuilder.ToTable("UserLogins");
        }
    }
}