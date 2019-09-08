using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Base.Core.Helpers.Mapping;

namespace Nozomi.Repo.Auth.Data.Mappings
{
    public class AddressMap : BaseMap<Address>
    {
        public AddressMap(EntityTypeBuilder<Address> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(addr => new { addr.Hash, addr.Type }).HasName("Address_CK_Hash_Type");
            
            entityTypeBuilder.HasOne(addr => addr.User).WithMany(u => u.Addresses)
                .HasForeignKey(addr => addr.UserId);
        }
    }
}