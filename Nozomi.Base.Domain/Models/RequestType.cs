using System.ComponentModel;

namespace Nozomi.Data.Models
{
    public enum RequestType
    {
        [Description("HttpGet")]
        HttpGet = 0,
        [Description("HttpPost")]
        HttpPost = 1,
        [Description("WebSocket")]
        WebSocket = 50
    }
}
