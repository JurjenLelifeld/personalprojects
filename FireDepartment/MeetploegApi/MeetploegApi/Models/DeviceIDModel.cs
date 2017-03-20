namespace MeetploegApi.Models
{
    /// <summary>
    /// Holds the device ID of a users' phone.
    /// </summary>
    public class DeviceIDModel
    {
        /// <summary>
        /// Holds the device ID of the users' phone, 
        /// which is used to lookup the corresponding user information.
        /// </summary>
        /// <remarks>
        /// The device ID is a unique identifier (unique for every phone).
        /// </remarks>
        public string deviceID { get; set; }
    }
}
