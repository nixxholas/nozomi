using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public enum RequestPropertyType
    {
        HttpHeader = 0,
        HttpBody = 1,

        // more to come
        SocketBody = 50,
        SocketSubscription = 51
    }
}
