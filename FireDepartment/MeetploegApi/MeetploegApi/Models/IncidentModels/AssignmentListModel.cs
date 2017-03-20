using System.Collections.Generic;

namespace MeetploegApi.Models
{
    /// <summary>
    /// The Assignment List Model holds the datalist of assignments. 
    /// </summary>
    /// <remarks>
    /// This model implements from the IncidentModel.
    /// </remarks>
    public class AssignmentListModel : IncidentModel
    {
        /// <summary>
        /// AssignmentList contains a list of assignments (of type AssignmentModel)
        /// </summary>
        public List<AssignmentModel> AssignmentList { get; set; } 
    }
}