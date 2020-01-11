using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.SourceType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceTypeEvent
    {
        bool Exists(string abbreviation);

        bool Exists(Guid guid);
        
        SourceType Find(string sourceTypeGuid);

        SourceType Get(Guid guid, bool track = false);
        
        IEnumerable<SourceTypeViewModel> GetAll(bool track = false);
    }
}