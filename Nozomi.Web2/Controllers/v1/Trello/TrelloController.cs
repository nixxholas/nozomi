using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Trello;
using Nozomi.Service.Events.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nozomi.Web2.Controllers.v1.Trello
{
    public class TrelloController : BaseApiController<TrelloController>, ITrelloController
    {
        private readonly ITrelloEvent _trelloEvent;

        public TrelloController(ILogger<TrelloController> logger,
            ITrelloEvent trelloEvent) : base(logger)
        {
            _trelloEvent = trelloEvent;
        }

        [AllowAnonymous]
        [HttpGet("{boardId}")]
        public async Task<IActionResult> Lists(string boardId)
        {
            // Hard coded board as current trello auth token has
            // access to all the different boards.
            // Making sure that private boards are not being exploited
            // through this controller
            boardId = "5e2c209ad3384a49871082bd";

            List<ListViewModel> lists = await _trelloEvent.GetListsAsync(boardId);
            return Ok(lists);
        }

        [AllowAnonymous]
        [HttpGet("{listId}")]
        public async Task<IActionResult> Cards(string listId)
        {
            List<CardViewModel> cards = await _trelloEvent.GetCardsAsync(listId);
            return Ok(cards);
        }

        [AllowAnonymous]
        [HttpGet("{cardId}")]
        public async Task<IActionResult> CheckLists(string cardId)
        {
            try
            {
                List<CheckListViewModel> checklists = await _trelloEvent.GetCheckListsAsync(cardId);
                return Ok(checklists);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
