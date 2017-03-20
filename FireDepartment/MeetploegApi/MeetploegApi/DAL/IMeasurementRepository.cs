using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the measurement repository which handles all queries for the measurement controller.
    /// </summary>
    public interface IMeasurementRepository : IDisposable
    {
        /// <summary>
        /// Insert a new measurement into the database
        /// </summary>
        /// <param name="measurement">All relevant measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertMeasurement(Measurement measurement);

        /// <summary>
        /// This method inserts a new gas measurement according to the retrieved information
        /// </summary>
        /// <param name="gasMeasurement">All relevant gas measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertGasMeasurement(GasMeasurement gasMeasurement);

        /// <summary>
        /// This method inserts a new earthquake measurement according to the retrieved information
        /// </summary>
        /// <param name="earthquake">All relevant earthquake measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertEarthquakeMeasurement(Earthquake earthquake);

        /// <summary>
        /// This method retrieves all assignments from the incident.
        /// Lazy loading is used to retrieve every measurement from the assignments.
        /// </summary>
        /// <param name="incidentId">The ID of the active incident</param>
        /// <returns>A list with all assignments of the active incident</returns>
        IQueryable<Assignment> GetMeasurements(int incidentId);
    }
}
