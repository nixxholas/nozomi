using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IAnalysedHistoricItemService
    {
        long Create(AnalysedHistoricItem item, long userId = 0);
    }
}