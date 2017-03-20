using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The location repository handles all queries for the location controller.
    /// </summary>
    public class LocationRepository : ILocationRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public LocationRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insert a new GPS location into the database
        /// </summary>
        /// <param name="gpsLocation">All relevant location data</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertNewGpsLocation(GpsLocation gpsLocation)
        {
            bool success;

            try
            {
                context.GpsLocationSet.Add(gpsLocation);
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
        /// Retrieves a list of the team's complete location history during that incident
        /// </summary>
        /// <param name="measuringTeamId">The ID of the measuring team</param>
        /// <returns>A list of the team location history</returns>
        public IQueryable<GpsLocation> GetCompleteTeamLocationHistory(MeasuringTeamIdModel measuringTeamId)
        {
            var incidentLocationHistory = context.GpsLocationSet.Where(team => team.MeasuringTeamId == measuringTeamId.MeasuringTeamId && team.IncidentId == measuringTeamId.IncidentId);

            return incidentLocationHistory;
        }

        /// <summary>
        /// Retrieves the team's latest location data of that incident
        /// </summary>
        /// <param name="measuringTeamId">The ID of the measuring team</param>
        /// <returns>The Gps Location data, null if there is no data available</returns>
        public GpsLocation GetLatestTeamLocation(MeasuringTeamIdModel measuringTeamId)
        {
            var latestTeamLocation = context
                                        .GpsLocationSet
                                        .Where(
                                            team =>
                                                team.MeasuringTeamId == measuringTeamId.MeasuringTeamId &&
                                                team.IncidentId == measuringTeamId.IncidentId)
                                        .OrderByDescending(t => t.Time)
                                        .FirstOrDefault();

            return latestTeamLocation;
        }

        /// <summary>
        /// Retrieves a list of all the latest teams locations
        /// </summary>
        /// <param name="incidentId">The ID of the measuring team</param>
        /// <returns>A list of the latest locations of all teams</returns>
        public IQueryable<GpsLocation> GetLatestLocationOfAllTeams(int incidentId)
        {
            var incidentLocationHistory = context.GpsLocationSet
                                                 .Where(team => team.IncidentId == incidentId)
                                                 .OrderByDescending(x => x.MeasuringTeamId)
                                                 .ThenByDescending(t => t.Time);

            return incidentLocationHistory;
        }

        /// <summary>
        /// Retrieves a list of the team's complete location history during that incident
        /// </summary>
        /// <param name="incidentId">The ID of the measuring team</param>
        /// <returns>A list of all locations of all teams during that incident</returns>
        public IQueryable<GpsLocation> GetCompleteLocationListOfAllTeams(int incidentId)
        {
            var incidentLocationHistory = context.GpsLocationSet.Where(team => team.IncidentId == incidentId);

            return incidentLocationHistory;
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
