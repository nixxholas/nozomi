﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public enum RequestPropertyType
    {
        Invalid = -1,
        HttpHeader = 0,
        HttpHeader_Accept = 1,
        HttpHeader_AcceptCharset = 2,
        HttpHeader_AcceptEncoding = 3,
        HttpHeader_AcceptLanguage = 4,
        HttpHeader_Authorization = 5,
        HttpHeader_CacheControl = 6,
        HttpHeader_Connection = 7,
        HttpHeader_ConnectionClose = 8,
        HttpHeader_Date = 9,
        HttpHeader_Expect = 10,
        HttpHeader_ExpectContinue = 11,
        HttpHeader_From = 12,
        HttpHeader_Host = 13,
        HttpHeader_IfMatch = 14,
        HttpHeader_IfModifiedSince = 15,
        HttpHeader_IfNoneMatch = 16,
        HttpHeader_IfRange = 17,
        HttpHeader_IfUnmodifiedSince = 18,
        HttpHeader_MaxForwards = 19,
        HttpHeader_Pragma = 20,
        HttpHeader_ProxyAuthorization = 21,
        HttpHeader_Range = 22,
        HttpHeader_Referrer = 23,
        HttpHeader_TE = 24,
        HttpHeader_Trailer = 25,
        HttpHeader_TransferEncoding = 26,
        HttpHeader_TransferEncodingChunked = 27,
        HttpHeader_Upgrade = 28,
        HttpHeader_UserAgent = 29,
        HttpHeader_Via = 30,
        HttpHeader_Warning = 31,
        HttpHeader_Custom = 32,
        HttpBody = 200,

        HttpQuery = 300,
        // more to come
        SocketSubscription = 500,
        SocketBody = 600,
    }
}
