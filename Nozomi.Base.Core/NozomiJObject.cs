using Newtonsoft.Json.Linq;

namespace Nozomi.Base.Core
{
    public class NozomiJObject : JObject
    {
        public NozomiJObject() {}

        public NozomiJObject(bool success)
        {
            Success = success;
        }
        
        public NozomiJObject(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        
        public string Message { get; set; }
    }
}