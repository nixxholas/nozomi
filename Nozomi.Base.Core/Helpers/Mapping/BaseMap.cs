using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nozomi.Base.Core.Helpers.Mapping
{
    public abstract class BaseMap<T> where T : class
    {
        public BaseMap(EntityTypeBuilder<T> entityTypeBuilder)
        {
        }
    }
}
