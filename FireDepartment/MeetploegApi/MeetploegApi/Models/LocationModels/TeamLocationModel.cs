using System;

namespace MeetploegApi.Models.LocationModels
{
    /// <summary>
    /// Holds a the team location data. 
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiResponse.
    /// </remarks>
    public class TeamLocationModel : ApiResponse
    {
        /// <summary>
        /// The unique identifier of the team location.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the measuring team it is related to. 
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int MeasuringTeamId { get; set; }

        /// <summary>
        /// The unique identifier of the incident it is related to. 
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int IncidentId { get; set; }

        /// <summary>
        /// Holds the latitude information of the location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Holds the longitude information of the location.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Holds the time the location was send by the team.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Holds the name of the team
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Holds the code of the team
        /// </summary>
        public string TeamCode { get; set; }
    }
}
