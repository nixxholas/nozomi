using System.Collections.Generic;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.Source
 {
     public interface ISourceController
     {
         NozomiResult<ICollection<Data.Models.Currency.Source>> All();

         NozomiResult<ICollection<Data.Models.Currency.Source>> GetCurrencySources(string slug, int page = 0);
     }
 }
