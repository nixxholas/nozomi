using System.ComponentModel;

namespace Nozomi.Data.Models.Web.Websocket
{
    public enum CommandType
    {
        [Description("Plaintext")]
        PlainText = 0,
        [Description("Json")]
        Json = 1
    }
}