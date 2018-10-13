namespace Nozomi.Ticker.Controllers
{
    public static class GlobalApiVariables
    {
        // Versioning for v1.x
        public static long V1_MAJOR_VERSION_REVISION = 1;
        public static string V1_MAJOR_VERSION = "v1.0";

        // This is the latest revision of the APIs.
        public static long CURRENT_API_REVISION = V1_MAJOR_VERSION_REVISION;
        
        // This is the latest iteration of the APIs.
        public static string CURRENT_API_VERSION = V1_MAJOR_VERSION;
    }
}