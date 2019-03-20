using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Services
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
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId) &&
                                       ac.DeletedAt == null);

            if (comp != null)
            {
                if (!string.IsNullOrEmpty(comp.Value))
                {
                    _unitOfWork.GetRepository<AnalysedHistoricItem>()
                        .Add(new AnalysedHistoricItem
                        {
                            AnalysedComponentId = comp.Id,
                            HistoricDateTime = comp.CreatedAt,
                            Value = comp.Value
                        });

                    _unitOfWork.Commit();
                }
                
                comp.Value = value;
                
                _unitOfWork.GetRepository<AnalysedComponent>().Update(comp);
                _unitOfWork.Commit(userId);

                return true;
            }

            return false;
        }
    }
}