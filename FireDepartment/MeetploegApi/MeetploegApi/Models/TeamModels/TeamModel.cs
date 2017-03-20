namespace MeetploegApi.Models.TeamModels
{
    /// <summary>
    /// Holds data of a team.
    /// </summary>
    public class TeamModel
    {
        public TeamModel()
        {
        }

        /// <summary>
        /// The unique identifier of the team.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Holds the unique code of every team that the fire department uses.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Holds the name of the team the fire depeartment uses.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Holds the address where the team is located.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Shows whether the team is departed when it is busy with an incident.
        /// </summary>
        public bool Departed { get; set; }

        /// <summary>
        /// Holds the latitude information of the location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Holds the longitude information of the location.
        /// </summary>
        public double Longitude { get; set; }
    }
}
