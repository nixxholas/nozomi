using System.Threading.Tasks;
using Nozomi.Base.Admin.Domain.AreaModels.Exchange;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface IExchangeService
    {
        Task<bool> Initialise(CreateExchange createExchange, long userId = 0);
    }
}