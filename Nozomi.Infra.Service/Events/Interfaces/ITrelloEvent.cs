using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data.ViewModels.Trello;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ITrelloEvent
    {
        Task<List<ListViewModel>> GetListsAsync(string boardId = null);

        Task<List<CardViewModel>> GetCardsAsync(string listId = null);

        Task<List<CheckListViewModel>> GetCheckListsAsync(string cardId = null);
    }
}
