using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;

namespace Nozomi.Ticker.Areas.v1.CurrencySource
 {
     public interface ICurrencySourceController
     {
         NozomiResult<ICollection<JsonResult>> All();

         NozomiResult<JsonResult> Create([FromBody] CreateSource source);

         NozomiResult<JsonResult> Update([FromBody] UpdateSource source);

         NozomiResult<JsonResult> Delete([FromBody] DeleteSource source);
     }
 }