﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.Enums.Trello;
using Nozomi.Data.ViewModels.Trello;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class TrelloEvent : BaseEvent<TrelloEvent, NozomiDbContext>, ITrelloEvent
    {
        private readonly HttpClient _httpClient;
        private readonly string _publicApiKey;
        private readonly string _authToken;

        public TrelloEvent(ILogger<TrelloEvent> logger, NozomiDbContext nozomiDbContext, IOptions<TrelloOptions> trelloOptions)
            : base(logger, nozomiDbContext)
        {
            // Requires keys to access Trello API
            _publicApiKey = trelloOptions.Value.ApiKey;
            _authToken = trelloOptions.Value.AuthToken;
            
            // Prepare HttpClient to reuse in this class
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.trello.com");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient = client;
        }

        public async Task<List<ListViewModel>> GetPublicList(BoardType boardType, string boardId)
        {
            if (string.IsNullOrWhiteSpace((boardId)))
                throw new ArgumentNullException("Parameter 'boardId' is supposed to contain a valid string.");
            
            // Enum description contains boardId and "," separated
            List<string> publicBoards = boardType.GetDescription().Split(",").ToList<string>();
            if (!publicBoards.Any(s => boardId.Contains((s))))
                throw new ArgumentException("Unauthorized access to board");

            return await GetListsAsync(boardId);
        }
        
        public async Task<List<CardViewModel>> GetPublicCard(BoardType boardType, string boardId, string listId)
        {
            List<ListViewModel> trelloList = await GetPublicList(boardType, boardId);

            if (!trelloList.Any(t => listId.Contains(t.ID)))
                throw new ArgumentException("Unauthorized access to list");

            return await GetCardsAsync(listId);
        }

        public async Task<List<CheckListViewModel>> GetPublicChecklist(BoardType boardType, string boardId,
            string listId, string cardId)
        {
            List<CardViewModel> cardList = await GetPublicCard(boardType, boardId, listId);

            if (!cardList.Any(c => cardId.Contains(c.ID)))
                throw new ArgumentException("Unauthorized access to card");
            
            return await GetCheckListsAsync(cardId);
        }

        private async Task<List<ListViewModel>> GetListsAsync(string boardId)
        {
            if (string.IsNullOrWhiteSpace(boardId))
                throw new ArgumentNullException("Parameter 'boardId' is supposesd to contain a valid string.");
        
            // Check out link for more information to limit result fields
            // https://developers.trello.com/reference#listsidcards
            string trelloCardFilter = "cards=all&card_fields=id";
            
            HttpResponseMessage response = await _httpClient
                .GetAsync($"/1/boards/{boardId}/lists?" +
                          $"key={_publicApiKey}&token={_authToken}&{trelloCardFilter}");
            response.EnsureSuccessStatusCode();
        
            string resultString = await response.Content.ReadAsStringAsync();
            List<ListViewModel> trelloLists = JsonConvert.DeserializeObject<List<ListViewModel>>(resultString);

            foreach (ListViewModel trelloList in trelloLists)
            {
                trelloList.CardCount = trelloList.Cards.Count();
                
                // Saving out-going bandwidth when Lists is loaded, main objective is to retrieve
                // card count from Trello board
                trelloList.Cards = null;
            }
            
            return trelloLists;
        }

        private async Task<List<CardViewModel>> GetCardsAsync(string listId)
        {
            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("Parameter 'listId' is supposed to contain a valid string");

            // Check out link for more information to limit result fields
            // https://developers.trello.com/reference#listsidcards
            string cardFields = "id,dateLastActivity,desc,name,badges,labels";

            HttpResponseMessage response = await _httpClient.GetAsync($"/1/lists/{listId}/cards?fields={cardFields}&key={_publicApiKey}&token={_authToken}");
            response.EnsureSuccessStatusCode();

            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CardViewModel>>(resultString);
        }

        private async Task<List<CheckListViewModel>> GetCheckListsAsync(string cardId)
        {
            if (string.IsNullOrWhiteSpace(cardId))
                throw new ArgumentNullException("Parameter 'cardId' is supposed to contain a valid string");

            HttpResponseMessage response = await _httpClient.GetAsync($"/1/cards/{cardId}/checklists?key={_publicApiKey}&token={_authToken}");
            response.EnsureSuccessStatusCode();

            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CheckListViewModel>>(resultString);
        }
    }
}