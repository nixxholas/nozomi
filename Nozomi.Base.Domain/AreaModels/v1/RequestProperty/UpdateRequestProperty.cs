namespace Nozomi.Data.AreaModels.v1.RequestProperty
{
    public class UpdateRequestProperty : CreateRequestProperty
    {
        public long Id { get; set; }
        
        public long RequestId { get; set; }

        public bool ToBeDeleted()
        {
            return RequestId.Equals(0);
        }
    }
}