namespace Nozomi.Web.Controllers.APIs.v1.Request
{
    public class CreateRequestViewModel
    {
        /// <summary>
        /// Request Type. GET? PUT?
        /// </summary>
        public long Type { get; set; }

        /// <summary>
        /// JSON? XML?
        /// </summary>
        public long ResponseType { get; set; }

        /// <summary>
        /// URL to the endpoint
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// The delay between each poll.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// The delay after a failed poll attempt
        /// </summary>
        public int FailureDelay { get; set; }
    }
}
