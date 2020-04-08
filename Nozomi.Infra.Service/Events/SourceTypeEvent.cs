using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Data.ViewModels.SourceType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class SourceTypeEvent : BaseEvent<SourceTypeEvent, NozomiDbContext>, ISourceTypeEvent
    {
        public SourceTypeEvent(ILogger<SourceTypeEvent> logger, NozomiDbContext unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public bool Exists(string abbreviation)
        {
            return _context.SourceTypes.AsNoTracking()
                .Any(st => st.Abbreviation.Equals(abbreviation));
        }

        public bool Exists(Guid guid)
        {
            return _context.SourceTypes.AsNoTracking()
                .Any(st => st.Guid.Equals(guid));
        }

        public SourceType Find(string sourceTypeGuid)
        {
            if (string.IsNullOrWhiteSpace(sourceTypeGuid))
                throw new ArgumentNullException("Invalid source type guid.");
            
            return _context.SourceTypes.AsNoTracking()
                .SingleOrDefault(st => st.Guid.Equals(Guid.Parse(sourceTypeGuid)));
        }

        public SourceType Get(Guid guid, bool track = false)
        {
            if (!track)
                return _context.SourceTypes.AsNoTracking()
                    .SingleOrDefault(st => st.Guid.Equals(guid));
            
            return _context.SourceTypes.AsTracking()
                .SingleOrDefault(st => st.Guid.Equals(guid));
        }

        public IEnumerable<SourceTypeViewModel> GetAll(bool track = false)
        {
            if (!track)
                return _context.SourceTypes.AsNoTracking()
                    .Where(st => st.DeletedAt == null && st.IsEnabled)
                    .Select(st => new SourceTypeViewModel()
                    {
                        Guid = st.Guid,
                        Name = st.Name,
                        Abbreviation = st.Abbreviation
                    });
            
            return _context.SourceTypes.AsNoTracking()
                .Where(st => st.DeletedAt == null && st.IsEnabled)
                .Include(st => st.Sources)
                .Select(st => new SourceTypeViewModel()
                {
                    Guid = st.Guid,
                    Name = st.Name,
                    Abbreviation = st.Abbreviation,
                    Sources = st.Sources
                        .Where(s => s.DeletedAt == null && s.IsEnabled)
                        .Select(s => new SourceViewModel
                        {
                            Guid = s.Guid,
                            Abbreviation = s.Abbreviation,
                            Name = s.Name,
                            ApiDocsUrl = s.APIDocsURL,
                            SourceTypeGuid = st.Guid
                        })
                });
        }
    }
}