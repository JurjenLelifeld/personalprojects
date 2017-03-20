namespace MeetploegApi.Models.MeasurementModels
{
    /// <summary>
    /// The Earthquake Measurement Model holds extra measurement data if the measurement is a gas measurement.
    /// </summary>
    /// <remarks>
    /// This model implements from the MeasurementModel.
    /// </remarks>
    public class EarthquakeMeasurementModel : MeasurementModel
    {
        /// <summary>
        /// Holds the data about the state of the victims.
        /// </summary>
        /// <remarks>
        /// This is a scale from 1 - 3.
        /// </remarks>
        public byte VictimsState { get; set; }

        /// <summary>
        /// Holds the data about the state of the buildings.
        /// </summary>
        /// <remarks>
        /// This is a scale from 1 - 3.
        /// </remarks>
        public byte BuildingsState { get; set; }

        /// <summary>
        /// Holds the data about the state of the infrastructure.
        /// </summary>
        /// <remarks>
        /// This is a scale from 1 - 3.
        /// </remarks>
        public byte InfrastructureState { get; set; }

        /// <summary>
        /// Holds extra details about the victims.
        /// </summary>
        public string VictimDetails { get; set; }

        /// <summary>
        /// Holds extra details about the buildings.
        /// </summary>
        public string BuildingDetails { get; set; }

        /// <summary>
        /// Holds extra details about the infrastructure.
        /// </summary>
        public string InfrastructureDetails { get; set; }
    }
}
