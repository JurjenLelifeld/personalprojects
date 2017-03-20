namespace MeetploegApi.Models
{
    /// <summary>
    /// Stores the incoming data for the retrieval of active chat users
    /// </summary>
    public class IncidentIdModel
    {
        public IncidentIdModel()
        {
        }

        /// <summary>
        /// The unique identifier of the incident.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int IncidentId { get; set; }
    }
}
