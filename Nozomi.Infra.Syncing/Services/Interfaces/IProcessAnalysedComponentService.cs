using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Syncing.Services.Interfaces
{
    public interface IProcessAnalysedComponentService
    {
        long Create(AnalysedComponent analysedComponent, string userId = null);

        bool UpdateValue(long analysedComponentId, string value, string userId = null);

        bool Checked(long analysedComponentId, bool isFailing = false, string userId = null);
        
        bool Disable(long analysedComponentId, string userId = null);
    }
}