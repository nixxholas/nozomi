using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data.Enums.Trello;
using Nozomi.Data.ViewModels.Trello;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ITrelloEvent
    {
        Task<List<ListViewModel>> GetPublicList(BoardType boardType = BoardType.PublicBoards,string boardId = null);

        Task<List<CardViewModel>> GetPublicCard(BoardType boardType = BoardType.PublicBoards, string boardId = null, string listId = null);
        
        Task<List<CheckListViewModel>> GetPublicChecklist(BoardType boardType = BoardType.PublicBoards, 
            string boardId = null, string listId = null, string cardId = null);
    }
}
