using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Source;

namespace Nozomi.Web.Controllers.APIs.v1.Source
 {
     public interface ISourceController
     {
         IActionResult Create(CreateSourceViewModel vm);

         IActionResult All();

         NozomiResult<ICollection<Data.Models.Currency.Source>> GetCurrencySources(string slug, int page = 0);
     }
 }
