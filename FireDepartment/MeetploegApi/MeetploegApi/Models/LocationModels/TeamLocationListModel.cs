using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetploegApi.Models.LocationModels
{
    /// <summary>
    /// The Team Location List Model holds a list of all team locations during an incident. 
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiResponse.
    /// </remarks>
    public class TeamLocationListModel : ApiResponse
    {
        /// <summary>
        /// Contains a list of all team locations during the incident (of type TeamLocationModel)
        /// </summary>
        public List<TeamLocationModel> TeamLocationList { get; set; }
    }
}
