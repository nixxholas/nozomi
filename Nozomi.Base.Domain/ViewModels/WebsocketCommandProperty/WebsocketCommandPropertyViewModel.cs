using System;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyViewModel : CreateWebsocketCommandPropertyInputModel
    {
        public WebsocketCommandPropertyViewModel() {}

        public WebsocketCommandPropertyViewModel(string guid, CommandPropertyType type, string key, string value, 
            bool isEnabled, string commandGuid)
        {
            Guid = guid;
            Type = type;
            Key = key;
            Value = value;
            IsEnabled = isEnabled;
            CommandGuid = commandGuid;
        }
        
        public WebsocketCommandPropertyViewModel(string guid, CommandPropertyType type, string key, string value,
            bool isEnabled, long commandId)
        {
            Guid = guid;
            Type = type;
            Key = key;
            Value = value;
            IsEnabled = isEnabled;
            CommandId = commandId;
        }

        public WebsocketCommandPropertyViewModel(long id, CommandPropertyType type, string key, string value,
            bool isEnabled, string commandGuid)
        {
            Id = id;
            Type = type;
            Key = key;
            Value = value;
            IsEnabled = isEnabled;
            CommandGuid = commandGuid;
        }
        
        public WebsocketCommandPropertyViewModel(long id, CommandPropertyType type, string key, string value,
            bool isEnabled, long commandId)
        {
            Id = id;
            Type = type;
            Key = key;
            Value = value;
            IsEnabled = isEnabled;
            CommandId = commandId;
        }
        
        public long Id { get; set; }
        
        public string Guid { get; set; }
    }
}