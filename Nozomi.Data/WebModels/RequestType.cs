using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public enum RequestType
    {
        [Description("HttpGet")]
        HttpGet = 0,
        [Description("HttpPost")]
        HttpPost = 1,
        [Description("WebSocket")]
        WebSocket = 50
    }
}
