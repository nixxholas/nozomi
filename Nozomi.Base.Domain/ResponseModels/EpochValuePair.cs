namespace Nozomi.Data.ResponseModels
{
    public class EpochValuePair<T> : DateValuePair<T> where T : class
    {
        public new double Time { get; set; }
    }
}