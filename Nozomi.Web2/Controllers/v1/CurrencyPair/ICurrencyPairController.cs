using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.CurrencyPair;
using Nozomi.Data.ViewModels.CurrencyPair;

namespace Nozomi.Web2.Controllers.v1.CurrencyPair
{
    public interface ICurrencyPairController
    {
        IActionResult All(int page = 0, int itemsPerPage = 50, 
            string sourceGuid = null, bool orderAscending = true, string orderingParam = "TickerPair");
        
        IActionResult Create(CreateCurrencyPairViewModel vm);
        
        /// <summary>
        /// An explicitly-defined variation of the GET Ticker API, allows the user to explicitly call a single ticker
        /// when the primary key is known to the caller.
        /// </summary>
        /// <param name="id">Unique identifier </param>
        /// <returns></returns>
        Task Get(long id);

        ICollection<DistinctCurrencyPairResponse> ListAll();

        NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>> Ticker(string abbrv);
    }
}
