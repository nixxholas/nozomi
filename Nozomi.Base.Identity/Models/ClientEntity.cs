using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityServer4.Models;
using Newtonsoft.Json;

namespace Nozomi.Base.Identity.Models
{
    public class ClientEntity
    {
        public string ClientData { get; set; }
 
        [Key]
        public string ClientId { get; set; }
 
        [NotMapped]
        public Client Client { get; set; }
 
        public void AddDataToEntity()
        {
            ClientData = JsonConvert.SerializeObject(Client);
            ClientId = Client.ClientId;
        }
 
        public void MapDataFromEntity()
        {
            Client = JsonConvert.DeserializeObject<Client>(ClientData);
            ClientId = Client.ClientId;
        }
    }
}