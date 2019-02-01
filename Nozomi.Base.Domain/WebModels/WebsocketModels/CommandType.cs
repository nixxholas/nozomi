using System.ComponentModel;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public enum CommandType
    {
        [Description("Plaintext")]
        PlainText = 0,
        [Description("Json")]
        Json = 1
    }
}