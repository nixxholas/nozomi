namespace Nozomi.Data.AreaModels.v1.Source
{
    public class DeleteSource
    {
        public long Id { get; set; }

        public bool HardDelete { get; set; } = false;
    }
}