using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Syncing.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Syncing.Services
{
    public class ProcessAnalysedComponentService : BaseService<ProcessAnalysedComponentService, NozomiDbContext>, 
        IProcessAnalysedComponentService
    {
        private readonly IAnalysedHistoricItemService _analysedHistoricItemService;
        
        public ProcessAnalysedComponentService(ILogger<ProcessAnalysedComponentService> logger, 
            IAnalysedHistoricItemService analysedHistoricItemService, IUnitOfWork<NozomiDbContext> context) 
            : base(logger, context)
        {
            _analysedHistoricItemService = analysedHistoricItemService;
        }

        public long Create(AnalysedComponent analysedComponent, string userId = null)
        {
            if (analysedComponent != null)
            {
                _context.GetRepository<AnalysedComponent>().Add(analysedComponent);
                _context.Commit(userId);

                return analysedComponent.Id;
            }
            
            return -1;
        }

        public bool UpdateValue(long analysedComponentId, string value, string userId = null)
        {
            if (string.IsNullOrEmpty(value)) return false;
            
            var comp = _context.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .Include(ac => ac.AnalysedHistoricItems)
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId) &&
                                       ac.DeletedAt == null);

            if (comp != null)
            {
                if (comp.StoreHistoricals && !string.IsNullOrEmpty(comp.Value))
                    _analysedHistoricItemService.Push(comp.Id, comp.Value, comp.ModifiedAt);
                
                if (comp.IsFailing)
                    comp.IsFailing = false;
                    
                // Make sure we update the datetime as well.. 
                comp.ModifiedAt = DateTime.UtcNow;
                comp.Value = value;
                
                _context.Commit(userId);

                return true;
            }

            return false;
        }

        public bool Checked(long analysedComponentId, bool isFailing = false, string userId = null)
        {
            var comp = _context.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId) && ac.DeletedAt == null);

            if (comp != null)
            {
                comp.IsFailing = isFailing;
                
                _context.Commit(userId);

                return true;
            }
            
            return false;
        }

        public bool Disable(long analysedComponentId, string userId = null)
        {
            var comp = _context.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.DeletedAt == null
                                       && ac.IsEnabled
                                       && ac.Id.Equals(analysedComponentId));

            if (comp != null)
            {
                comp.IsEnabled = false;
                comp.ModifiedAt = DateTime.UtcNow;

                _context.Commit();
            }

            return false;
        }
    }
}