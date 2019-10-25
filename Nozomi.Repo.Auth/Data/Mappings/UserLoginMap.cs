using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Auth.Data.Mappings
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