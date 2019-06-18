using System;
using System.Collections.Generic;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Base.Admin.Domain.AreaModels.Request
{
    public class RequestDto : RequestDTO
    {
        public new ICollection<AnalysedComponentDto> AnalysedComponents { get; set; }
        
        public long DeletedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
    }
}