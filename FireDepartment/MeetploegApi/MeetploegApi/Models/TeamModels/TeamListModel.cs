using System.Collections.Generic;

namespace MeetploegApi.Models.TeamModels
{
    /// <summary>
    /// The Team List Model holds a list of all teams during.
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiResponse.
    /// </remarks>
    public class TeamListModel : ApiResponse
    {
        public TeamListModel()
        {
        }

        /// <summary>
        /// Contains a list of all teams (of type TeamModel)
        /// </summary>
        public List<TeamModel> TeamList { get; set; }
    }
}
