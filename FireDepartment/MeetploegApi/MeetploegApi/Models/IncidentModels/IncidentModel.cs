using System;

namespace MeetploegApi.Models
{
    /// <summary>
    /// The Incident Model holds the datalist of the incident. 
    /// </summary>
    /// <remarks>
    /// This model implements from the TeamModel.
    /// </remarks>
    public class IncidentModel : TeamModel
    {
        /// <summary>
        /// The unique identifier of the incident.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int IncidentId { get; set; }

        /// <summary>
        /// Holds the latitude information of the incident.
        /// </summary>
        public double IncidentLatitude { get; set; }

        /// <summary>
        /// Holds the longitude information of the incident.
        /// </summary>
        public double IncidentLongitude { get; set; }

        /// <summary>
        /// Holds the address informatoin for the incident.
        /// </summary>
        public string IncidentAddress { get; set; }

        /// <summary>
        /// Holds the gasmold code for this incident.
        /// </summary>
        public int IncidentGasMoldCode { get; set; }

        /// <summary>
        /// Holds the gasmold color for this incident.
        /// </summary>
        public string IncidentGasMoldColor { get; set; }

        /// <summary>
        /// Holds the wind direction for this incident.
        /// </summary>
        public int IncidentWindDirection { get; set; }

        /// <summary>
        /// Holds the time the assignment is created by the admin.
        /// </summary>
        public DateTime IncidentTime { get; set; }

        /// <summary>
        /// The incident type indicates the type of incident.
        /// </summary>
        /// <remarks>
        /// For example: this can be a "fire" or "gas" incident.
        /// </remarks>
        public string IncidentType { get; set; }

        /// <summary>
        /// Holds extra information about the incident.
        /// </summary>
        public string IncidentDetails { get; set; }
    }
}
