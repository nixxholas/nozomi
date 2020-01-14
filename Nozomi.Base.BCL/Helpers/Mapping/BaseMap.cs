using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nozomi.Base.BCL.Helpers.Mapping
{
    public abstract class BaseMap<T> where T : class
    {
        public BaseMap(EntityTypeBuilder<T> entityTypeBuilder)
        {
        }
    }
}
