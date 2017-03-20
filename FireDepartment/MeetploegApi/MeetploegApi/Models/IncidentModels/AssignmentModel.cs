using System;

namespace MeetploegApi.Models
{
    /// <summary>
    /// The Assignment Model holds all the assignment information
    /// </summary>
    public class AssignmentModel 
    {
        /// <summary>
        /// The unique identifier of the assignment.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int AssignmentId { get; set; }

        /// <summary>
        /// The unique identifier of the incident it is related to. 
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int IncidentId { get; set; }

        /// <summary>
        /// The assignment type indicates the type of assignment.
        /// </summary>
        /// <remarks>
        /// For example: this can be a "measurement" or "drive" assignment.
        /// </remarks>
        public string AssignmentType { get; set; }

        /// <summary>
        /// Holds the latitude information of the assignment.
        /// </summary>
        public double AssignmentLatitude { get; set; }

        /// <summary>
        /// Holds the longitude information of the assignment.
        /// </summary>
        public double AssignmentLongitude { get; set; }

        /// <summary>
        /// Holds the time the assignment is created by the admin.
        /// </summary>
        public DateTime AssignmenttTime { get; set; }

        /// <summary>
        /// The measuring team ID is the ID of the team this assignment is assigned to.
        /// </summary>
        /// <remarks>
        /// This is a foreign key in the database.
        /// </remarks>
        public int MeasuringTeamId { get; set; }

        /// <summary>
        /// Holds extra information about the assignment.
        /// </summary>
        public string AssignmentDetails { get; set; }

        /// <summary>
        /// Indicates whether the assignment is still active.
        /// </summary>
        public bool AssignmentActive { get; set; }

        /// <summary>
        /// Indicates wheter the team has arrived on the scene.
        /// </summary>
        public bool AssignmentArrived { get; set; }
    }
}