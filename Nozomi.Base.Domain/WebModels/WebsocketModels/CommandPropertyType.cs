using System.ComponentModel;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public enum CommandPropertyType
    {
        [Description("String")]
        Default = 0,
        [Description("Long")]
        Integer = 1,
        [Description("Double")]
        Decimal = 2,
        [Description("Float")]
        Float = 3,
        [Description("DateTime")]
        DateAndTime = 4,
        [Description("Collection")]
        Collection = 5
    }
}