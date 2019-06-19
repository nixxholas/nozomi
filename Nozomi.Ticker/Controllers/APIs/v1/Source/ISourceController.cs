using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Source;

namespace Nozomi.Ticker.Controllers.APIs.v1.Source
 {
     public interface ISourceController
     {
         NozomiResult<ICollection<Data.Models.Currency.Source>> All();
     }
 }