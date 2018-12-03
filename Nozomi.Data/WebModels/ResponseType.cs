using System.ComponentModel;

namespace Nozomi.Data.WebModels
{
    public enum ResponseType
    {
        [Description("application/json")]
        Json = 1,
        [Description("application/xml")]
        XML = 2
    }
}