namespace MeetploegApi.Models
{
    /// <summary>
    /// The Measure Assignment Model holds extra information about the assignment. 
    /// </summary>
    /// <remarks>
    /// This model implements from the AssignmentModel.
    /// This only applies for assignments that have this extra information, 
    /// like measurement assignments.
    /// </remarks>
    public class MeasureAssignmentModel : AssignmentModel
    {
        /// <summary>
        /// Holds the gastube code information the team has to use for this assignment
        /// </summary>
        public int AssignmentGastubeCode { get; set; }

        /// <summary>
        /// Indicates whether the team has to do a Lel Measurement.
        /// </summary>
        public bool AssignmentLelMeasurement { get; set; }

        /// <summary>
        /// Indicates whether the team has to use the automess.
        /// </summary>
        public bool AssignmentAutomess { get; set; }

        /// <summary>
        /// Indicates whether the team has to use the automess probe.
        /// </summary>
        public bool AssignmentAutomessProbe { get; set; }

        /// <summary>
        /// Indicates whether the team has to use the dose measurer.
        /// </summary>
        public bool AssignmentDoseMeasurer { get; set; }

        /// <summary>
        /// Indicates whether the team has to use the breathable air.
        /// </summary>
        public bool AssignmentBreathableAir { get; set; }
    }
}
