using System;

namespace Nozomi.Base.BCL
{
    /// <summary>
    /// KeyValuePairObject
    ///
    /// A simple and easy replacement to IDictionary of string, string in Javascript
    /// 
    /// Let's try to deprecate this as soon as possible.
    /// </summary>
    [Obsolete]
    public class KeyValuePairObject
    {
        public string Key { get; set; }
        
        public string Value { get; set; }
    }
}