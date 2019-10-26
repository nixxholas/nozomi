using Nozomi.Data.Interfaces;
using Nozomi.Data.Models;
using Nozomi.Data.Repositories;

namespace Nozomi.Repo.BCL.Repository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(IDbContext context) : base(context)
        {
        }
    }
}