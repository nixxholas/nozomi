using Nozomi.Data.Models;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRcdHistoricItemService
    {
        bool Push(RequestComponent rc);
    }
}