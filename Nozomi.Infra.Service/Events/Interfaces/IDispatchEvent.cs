using System.Threading.Tasks;
using Nozomi.Data.ViewModels.Dispatch;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IDispatchEvent
    {
        Task<DispatchViewModel> Dispatch(DispatchInputModel dispatchInputModel);
    }
}