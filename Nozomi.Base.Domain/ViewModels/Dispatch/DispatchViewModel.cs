using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Nozomi.Data.ViewModels.Dispatch
{
    public class DispatchViewModel
    {
        /// <summary>
        /// The granular details of the dispatch.
        /// </summary>
        public HttpResponse Response { get; set; }
        
        /// <summary>
        /// The raw payload of the object, in JSON.
        /// </summary>
        public JsonDocument Payload { get; set; }
    }
}