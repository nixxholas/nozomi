using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ResponseModels.Request
{
    public class RequestViewModel
    {
        public RequestViewModel() {}

        public RequestViewModel(string guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled)
        {
            Guid = Guid.Parse(guid);
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            IsEnabled = isEnabled;
        }

        public RequestViewModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled)
        {
            Guid = guid;
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            IsEnabled = isEnabled;
        }
        
        public Guid Guid { get; set; }
        
        public RequestType RequestType { get; set; }
        
        public ResponseType ResponseType { get; set; }
        
        public string DataPath { get; set; }
        
        public int Delay { get; set; }
        
        public long FailureDelay { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}