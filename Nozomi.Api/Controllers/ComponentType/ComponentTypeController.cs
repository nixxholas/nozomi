using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.ComponentType.Examples;
using Nozomi.Data.ViewModels.ComponentType;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Preprocessing.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.ComponentType
{
    /// <summary>
    /// Component Type APIs
    /// </summary>
    public class ComponentTypeController : BaseApiController<ComponentTypeController>, IComponentTypeController
    {
        /// <summary>
        /// Default Constructor..
        /// </summary>
        /// <param name="logger">Logger DI</param>
        public ComponentTypeController(ILogger<ComponentTypeController> logger) : base(logger)
        {
        }

        /// <summary>
        /// Obtain all of the component types that are publicly available or the ones that you own.
        /// </summary>
        /// <param name="index">the 'page' of the list of results in 50s.</param>
        /// <returns>Collection of component types.</returns>
        [Authorize]
        [TokenBucket(Name = "ComponentType/All", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ComponentTypeViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public Task<IActionResult> All(int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}