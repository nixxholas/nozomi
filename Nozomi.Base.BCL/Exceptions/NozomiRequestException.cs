namespace Nozomi.Base.BCL.Exceptions
{
    public class NozomiRequestException : System.Exception
    {
        public int Code { get; }
        public string Description { get; }

        public NozomiRequestException(string message, string description, int code) : base(message)
        {
            Code = code;
            Description = description;
        }
    }
}