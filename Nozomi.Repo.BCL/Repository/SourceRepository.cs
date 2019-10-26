using Nozomi.Data.Interfaces;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Repositories;

namespace Nozomi.Repo.BCL.Repository
{
    public class SourceRepository : Repository<Source>, ISourceRepository
    {
        public SourceRepository(IDbContext context) : base(context)
        {
        }
    }
}