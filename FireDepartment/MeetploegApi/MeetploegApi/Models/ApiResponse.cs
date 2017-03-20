namespace MeetploegApi.Models
{
    /// <summary>
    /// This is the base class of all custom models.
    /// It holds a message which is always send with a API call.
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse()
        {
        }
        
        /// <summary>
        /// Holds the message that is send with the API call.
        /// This is used to make clear for the recipient what is done or what went wrong.
        /// </summary>
        public string Message { get; set; }
    }
}
