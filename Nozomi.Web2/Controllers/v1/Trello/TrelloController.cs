using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.Enums.Trello;
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
            try
            {
                List<ListViewModel> lists = await _trelloEvent.GetPublicList(BoardType.PublicBoards , boardId);
                return Ok(lists);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("{boardId}/List/{listId}")]
        public async Task<IActionResult> Cards(string boardId, string listId)
        {
            try
            {
                List<CardViewModel> cards = await _trelloEvent.GetPublicCard(BoardType.PublicBoards, boardId, listId);
                return Ok(cards);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("{boardId}/List/{listId}/Card/{cardId}")]
        public async Task<IActionResult> CheckLists(string boardId, string listId, string cardId)
        {
            try
            {
                List<CheckListViewModel> checklists = await _trelloEvent.GetPublicChecklist(BoardType.PublicBoards, boardId, listId, cardId);
                return Ok(checklists);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
