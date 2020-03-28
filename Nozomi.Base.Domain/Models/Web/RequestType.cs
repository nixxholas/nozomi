using System.ComponentModel;

namespace Nozomi.Data.Models.Web
{
    public enum RequestType
    {
        [Description("HttpGet")]
        HttpGet = 0,
        [Description("HttpPost")]
        HttpPost = 1,
        [Description("HttpPut")]
        HttpPut = 2,
        [Description("HttpPatch")]
        HttpPatch = 3,
        [Description("HttpDelete")]
        HttpDelete = 4, // Not sure why a user would poll a Delete endpoint, but sure
        [Description("WebSocket")]
        WebSocket = 50
    }
}
