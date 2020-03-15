using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Request;

namespace Nozomi.Api.Controllers.Request
{
    public interface IRequestController
    {
        /// <summary>
        /// Generic listing API for Requests
        /// </summary>
        /// <param name="index">Pagination identifier</param>
        /// <returns>List of requests along with an encapsulated HTTP request.</returns>
        IActionResult All(int index = 0);

        IActionResult Get(string guid);
    }
}