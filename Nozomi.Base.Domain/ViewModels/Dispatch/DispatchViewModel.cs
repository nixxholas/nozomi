using System.Net.Http;
using System.Text.Json;

namespace Nozomi.Data.ViewModels.Dispatch
{
    public class DispatchViewModel
    {
        /// <summary>
        /// The granular details of the dispatch.
        /// </summary>
        public HttpResponseMessage Response { get; set; }
        
        /// <summary>
        /// The raw payload of the object, in JSON.
        /// </summary>
        public string Payload { get; set; }
    }
}