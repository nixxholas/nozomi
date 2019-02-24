namespace Nozomi.Data.Models.Web.Logging
{
    public enum RequestLogType
    {
        Unknown = 0, // Lol, no idea wtf is going on
        Success = 1, // API works, move on..
        Failure = 2, // API is completely cut off, could be an Internet issue either on the requester or requested.
        Unavailable = 3 // API is currently undergoing some maintainence...
    }
}