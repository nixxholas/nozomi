using System;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyViewModel : CreateWebsocketCommandPropertyInputModel
    {
        public WebsocketCommandPropertyViewModel() {}

        public WebsocketCommandPropertyViewModel(Guid guid, CommandPropertyType type, string key, string value)
        {
            Guid = guid;
            Type = type;
            Key = key;
            Value = value;
        }

        public WebsocketCommandPropertyViewModel(long id, CommandPropertyType type, string key, string value)
        {
            Id = id;
            Type = type;
            Key = key;
            Value = value;
        }
        
        public long Id { get; set; }
        
        public Guid Guid { get; set; }
    }
}