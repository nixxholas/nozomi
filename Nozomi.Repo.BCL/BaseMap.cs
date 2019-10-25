using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Models;

namespace Nozomi.Repo.BCL
{
    public abstract class BaseMap<T> where T : class
    {
        public BaseMap(EntityTypeBuilder<T> entityTypeBuilder)
        {
        }
    }
}
