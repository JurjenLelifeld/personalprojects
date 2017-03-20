using System;

namespace MeetploegApi.Models.MeasurementModels
{
    /// <summary>
    /// The Gas Measurement Model holds extra measurement data if the measurement is a gas measurement.
    /// </summary>
    /// <remarks>
    /// This model implements from the MeasurementModel.
    /// </remarks>
    public class GasMeasurementModel : MeasurementModel
    {
        /// <summary>
        /// Holds the LelResult data.
        /// </summary>
        public double LelResult { get; set; }

        /// <summary>
        /// Holds the amount of pump strokes done for this measurement.
        /// </summary>
        public int PumpStrokes { get; set; }

        /// <summary>
        /// Holds the time when the first pumpstroke was done.
        /// </summary>
        public DateTime FirstPumpStroke { get; set; }

        /// <summary>
        /// Holds the concentration measured.
        /// </summary>
        public double Concentration { get; set; }

        /// <summary>
        /// Holds the CO concentration measured.
        /// </summary>
        public double CoMeasurement { get; set; }

        /// <summary>
        /// Holds the code of the gastube that was used.
        /// </summary>
        public int GasTubeCode { get; set; }
    }
}
