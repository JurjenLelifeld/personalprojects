using System.Collections.Generic;

namespace MeetploegApi.Models.ChatModels
{
    /// <summary>
    /// The User List Model holds a list of all users that are active during an incident. 
    /// </summary>
    /// <remarks>
    /// This model implements from the ApiResponse.
    /// </remarks>
    public class UserListModel : ApiResponse
    {
        public UserListModel()
        {
        }

        /// <summary>
        /// Contains a list of all team locations during the incident (of type TeamLocationModel)
        /// </summary>
        public List<UserInformationModel> UserInformationList { get; set; }
    }
}
