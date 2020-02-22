using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data.ViewModels.Trello;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class TrelloEvent : BaseEvent<TrelloEvent, NozomiDbContext>, ITrelloEvent
    {
        private readonly HttpClient _httpClient;
        private readonly string _publicApiKey;
        private readonly string _authToken;

        public TrelloEvent(ILogger<TrelloEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork, IConfiguration configuration)
            : base(logger, unitOfWork)
        {
            // Requires keys to access Trello API
            _publicApiKey = configuration["TrelloToken:ApiKey"];
            _authToken = configuration["TrelloToken:AuthToken"];

            // Prepare HttpClient to reuse in this class
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.trello.com");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient = client;
        }


        public async Task<List<ListViewModel>> GetListsAsync(string boardId)
        {
            if (string.IsNullOrWhiteSpace(boardId))
                throw new ArgumentNullException("Parameter 'boardId' is supposesd to contain a valid string.");

            HttpResponseMessage response = await _httpClient.GetAsync($"/1/boards/{boardId}/lists?key={_publicApiKey}&token={_authToken}");
            response.EnsureSuccessStatusCode();

            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ListViewModel>>(resultString);
        }


        public async Task<List<CardViewModel>> GetCardsAsync(string listId)
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


        public async Task<List<CheckListViewModel>> GetCheckListsAsync(string cardId)
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