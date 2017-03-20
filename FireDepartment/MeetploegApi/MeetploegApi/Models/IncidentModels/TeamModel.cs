namespace MeetploegApi.Models
{
    /// <summary>
    /// The Team Model holds the details of the team. 
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiRepsonse model.
    /// </remarks>
    public class TeamModel : ApiResponse
    {
        /// <summary>
        /// The unique identifier of the team.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int TeamId { get; set; }

        /// <summary>
        /// The team code that is given to a team by the fire department.
        /// </summary>
        public string TeamCode { get; set; }

        /// <summary>
        /// The team name that is given to a team by the fire department.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The latitue of the start position of the team.
        /// </summary>
        /// <remarks>
        /// This is a fixed starting position in their home base.
        /// </remarks>
        public double TeamLatitude { get; set; }

        /// <summary>
        /// The longitude of the start position of the team.
        /// </summary>
        /// <remarks>
        /// This is a fixed starting position in their home base.
        /// </remarks>
        public double TeamLongitude { get; set; }

        /// <summary>
        /// The address where the team is located.
        /// </summary>
        public string TeamAddress { get; set; }

        /// <summary>
        /// Indicates whether the team has departed for their assignment.
        /// </summary>
        public bool TeamDeparted { get; set; }
    }
}
