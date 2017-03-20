namespace MeetploegApi.Models.ChatModels
{
    /// <summary>
    /// Holds all user data
    /// </summary>
    public class UserInformationModel
    {
        public UserInformationModel()
        {
        }

        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        /// <remarks>
        /// This is the primary key in the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Holds the user name of the user
        /// </summary>
        public string UserName { get; set; }
    }
}
