using System;
using System.Data.Entity.Spatial;

namespace MeetploegApi.Models.MeasurementModels
{
    /// <summary>
    /// The Measurement Model holds the measurement data that is send by from the app by the user.
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiResponse.
    /// </remarks>
    public class MeasurementModel : ApiResponse
    {
        /// <summary>
        /// The unique identifier of the measurement data.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the assignment it is related to. 
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int AssignmentId { get; set; }

        /// <summary>
        /// Shows the type of measurement.
        /// </summary>
        /// <remarks>
        /// For example: this can be a "gas" or "earthquake" or "drive" type.
        /// </remarks>
        public string MeasurementType { get; set; }

        /// <summary>
        /// Holds the latitude information of the measurement.
        /// </summary>
        public double MeasurementLatitude { get; set; }

        /// <summary>
        /// Holds the longitude information of the measurement.
        /// </summary>
        public double MeasurementLongitude { get; set; }

        /// <summary>
        /// Holds extra observations done by the team.
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Holds the time the measurment was done and send by the user.
        /// </summary>
        public DateTime MeasurementTime { get; set; }
    }
}
