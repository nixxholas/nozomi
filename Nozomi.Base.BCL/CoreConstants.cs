using System;

namespace Nozomi.Base.BCL
{
    public static class CoreConstants
    {
        public const string Name = "Nozomi";
        
        public const string GenericCurrency = "USD";
        public const string GenericCounterCurrency = "USD";

        public static readonly DateTime BuildDateTime = DateTime.UtcNow;
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}