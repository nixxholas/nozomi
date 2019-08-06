namespace Nozomi.Data.ResponseModels
{
    public class EpochValuePair<T> : DateValuePair<T>
    {
        public new double Time { get; set; }
    }
}