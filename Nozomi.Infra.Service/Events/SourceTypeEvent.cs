using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Data.ResponseModels.SourceType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class SourceTypeEvent : BaseEvent<SourceTypeEvent, NozomiDbContext>, ISourceTypeEvent
    {
        public SourceTypeEvent(ILogger<SourceTypeEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public IEnumerable<SourceTypeViewModel> GetAll(bool track = false)
        {
            return _unitOfWork.GetRepository<SourceType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(st => st.DeletedAt == null && st.IsEnabled)
                .Select(st => new SourceTypeViewModel()
                {
                    Guid = st.Guid,
                    Name = st.Name,
                    Abbreviation = st.Abbreviation,
                    Sources = st.Sources.Select(s => new SourceViewModel
                    {
                        Id = s.Id,
                        Abbreviation = s.Abbreviation,
                        Name = s.Name,
                        ApiDocsUrl = s.APIDocsURL
                    })
                });
        }
    }
}