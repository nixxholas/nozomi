using System.Collections.Generic;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ITickerEvent
    {
        /// <summary>
        /// Memory-driven DistinctiveTickerResponse API
        /// </summary>
        /// <returns></returns>
        IDictionary<KeyValuePair<string, string>, DistinctiveTickerResponse> GetAll();
        
        DataTableResult<UniqueTickerResponse> GetAllForDatatable(int index = 0);
    }
}