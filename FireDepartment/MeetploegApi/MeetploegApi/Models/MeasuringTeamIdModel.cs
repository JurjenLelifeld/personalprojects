namespace MeetploegApi.Models
{
    /// <summary>
    /// Stores the incoming data for the GPS retrieval
    /// </summary>
    public class MeasuringTeamIdModel
    {
        /// <summary>
        /// The unique identifier of the team.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int MeasuringTeamId { get; set; }

        /// <summary>
        /// The unique identifier of the incident it is related to. 
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int IncidentId { get; set; }
    }
}
