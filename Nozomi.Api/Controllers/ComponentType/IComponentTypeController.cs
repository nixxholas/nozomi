using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.ComponentType
{
    /// <summary>
    /// Component Type API Interface
    /// </summary>
    public interface IComponentTypeController
    {
        /// <summary>
        /// Obtain all of the component types that are publicly available or the ones that you own.
        /// </summary>
        /// <param name="index">the 'page' of the list of results in 50s.</param>
        /// <returns>Collection of component types.</returns>
        Task<IActionResult> All(int index = 0);
    }
}