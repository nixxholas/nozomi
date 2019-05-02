using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;

namespace Nozomi.Ticker.Controllers.APIs.v1.Source
 {
     public interface ISourceController
     {
         NozomiResult<ICollection<Data.Models.Currency.Source>> All();

         NozomiResult<JsonResult> Create([FromBody] CreateSource source);

         NozomiResult<JsonResult> Update([FromBody] UpdateSource source);

         NozomiResult<JsonResult> Delete([FromBody] DeleteSource source);
     }
 }