using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Request
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class RequestController : AreaBaseViewController<RequestController>
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IComponentEvent _componentEvent;
        private readonly IRequestService _requestService;

        public RequestController(ILogger<RequestController> logger, IRequestEvent requestEvent,
            IRequestService requestService, IComponentEvent componentEvent,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
            : base(logger, signInManager, userManager)
        {
            _requestEvent = requestEvent;
            _requestService = requestService;
            _componentEvent = componentEvent;
        }

        #region GET Requests

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(new RequestsViewModel
                {
                    Requests = _requestEvent.GetAllDTO(0),
                    RequestTypes = NozomiServiceConstants.requestTypes,
                    ResponseTypes = NozomiServiceConstants.responseTypes
                }
            );
        }

        #endregion

        #region GET Request by Id

        [HttpGet("{id}")]
        public async Task<IActionResult> Request([FromRoute] long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            return View(new RequestViewModel
            {
                Request = _requestEvent.GetActive(id, true).ToDTO(),
                RequestTypes = NozomiServiceConstants.requestTypes,
                ResponseTypes = NozomiServiceConstants.responseTypes,
                RequestComponentTypes = NozomiServiceConstants.requestComponentTypes
            });
        }

        #endregion

        #region POST Request

        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequest createRequest)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            var result = _requestService.Create(createRequest);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }

        #endregion

        #region PUT Request

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRequest(long id, UpdateRequest updateRequest)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            if (id != updateRequest.Id)
            {
                return BadRequest();
            }

            var result = _requestService.Update(updateRequest);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }

        #endregion

        #region DELETE Request

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            var result = _requestService.Delete(id);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }

        #endregion
    }
}