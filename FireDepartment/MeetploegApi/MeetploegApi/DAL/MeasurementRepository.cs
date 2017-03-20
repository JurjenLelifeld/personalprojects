using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The measurement repository handles all queries for the measurement controller.
    /// </summary>
    public class MeasurementRepository : IMeasurementRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public MeasurementRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insert a new measurement into the database
        /// </summary>
        /// <param name="measurement">All relevant measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertMeasurement(Measurement measurement)
        {
            bool success;

            try
            {
                context.MeasurementSet.Add(measurement);
                context.SaveChanges();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// This method inserts a new gas measurement according to the retrieved information
        /// </summary>
        /// <param name="gasMeasurement">All relevant gas measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertGasMeasurement(GasMeasurement gasMeasurement)
        {
            bool success = true;
            try
            {
                context.MeasurementSet.Add(gasMeasurement);

                context.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// This method inserts a new earthquake measurement according to the retrieved information
        /// </summary>
        /// <param name="earthquake">All relevant earthquake measurement information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertEarthquakeMeasurement(Earthquake earthquake)
        {
            bool success = true;
            try
            {
                context.MeasurementSet.Add(earthquake);

                context.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// This method retrieves all assignments from the incident.
        /// Lazy loading is used to retrieve every measurement from the assignments.
        /// </summary>
        /// <param name="incidentId">The ID of the active incident</param>
        /// <returns>A list with all assignments of the active incident</returns>
        public IQueryable<Assignment> GetMeasurements(int incidentId)
        {
            var assignmentList = context.AssignmentSet.Where(i => i.IncidentId == incidentId);

            return assignmentList;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
