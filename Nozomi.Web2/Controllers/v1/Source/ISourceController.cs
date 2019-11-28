using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Source;

namespace Nozomi.Web2.Controllers.v1.Source
 {
     public interface ISourceController
     {
         IActionResult Create(CreateSourceViewModel vm);

         IActionResult All();

         NozomiResult<ICollection<Data.Models.Currency.Source>> GetCurrencySources(string slug, int page = 0);

         IActionResult ListByCurrency(string slug, int page = 0, int itemsPerPage = 50);
     }
 }
