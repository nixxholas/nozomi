using System;

namespace Nozomi.Data.ResponseModels
{
    public class DateValuePair<T>
    {
        public DateTime Time { get; set; }
        
        public T Value { get; set; }
    }
}