using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Web2.Controllers.v1.Trello
{
    public interface ITrelloController
    {
        Task<IActionResult> Lists(string boardId);

        Task<IActionResult> Cards(string listId);

        Task<IActionResult> CheckLists(string cardId);
    }
}
