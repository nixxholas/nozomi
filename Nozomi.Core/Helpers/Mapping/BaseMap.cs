using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nozomi.Core.Helpers.Mapping
{
    public abstract class BaseMap<T> where T : class
    {
        public BaseMap(EntityTypeBuilder<T> entityTypeBuilder)
        {
        }
    }
}
