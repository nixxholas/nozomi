using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, 
        IAnalysedComponentService
    {
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public long Create(AnalysedComponent analysedComponent, long userId = 0)
        {
            if (analysedComponent != null)
            {
                _unitOfWork.GetRepository<AnalysedComponent>().Add(analysedComponent);
                _unitOfWork.Commit(userId);

                return analysedComponent.Id;
            }
            
            return -1;
        }

        public bool UpdateValue(long analysedComponentId, string value, long userId = 0)
        {
            if (string.IsNullOrEmpty(value)) return false;
            
            var comp = _unitOfWork.GetRepository<AnalysedComponent>()
                .Get(ac => ac.Id.Equals(analysedComponentId) &&
                           ac.DeletedAt == null)
                .SingleOrDefault();

            if (comp != null)
            {
                comp.Value = value;
                
                _unitOfWork.GetRepository<AnalysedComponent>().Update(comp);
                _unitOfWork.Commit(userId);

                return true;
            }

            return false;
        }
    }
}