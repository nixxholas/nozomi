using System.ComponentModel;

namespace Nozomi.Data.WebModels
{
    public enum ResponseType
    {
        [Description("application/json")]
        Json = 1,
        [Description("text/xml")]
        XML = 2
    }
}