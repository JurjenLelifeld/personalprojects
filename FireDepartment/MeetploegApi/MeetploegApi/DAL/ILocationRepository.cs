using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the location repository which handles all queries for the location controller.
    /// </summary>
    public interface ILocationRepository : IDisposable
    {
        /// <summary>
        /// Insert a new GPS location into the database
        /// </summary>
        /// <param name="gpsLocation">All relevant location data</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertNewGpsLocation(GpsLocation gpsLocation);

        /// <summary>
        /// Retrieves a list of the team's complete location history during that incident
        /// </summary>
        /// <param name="measuringTeamId">The ID of the measuring team</param>
        /// <returns>A list of all locations of the team</returns>
        IQueryable<GpsLocation> GetCompleteTeamLocationHistory(MeasuringTeamIdModel measuringTeamId);

        /// <summary>
        /// Retrieves the team's latest location data of that incident
        /// </summary>
        /// <param name="measuringTeamId">The ID of the measuring team</param>
        /// <returns>The latest GPS location data of the team</returns>
        GpsLocation GetLatestTeamLocation(MeasuringTeamIdModel measuringTeamId);

        /// <summary>
        /// Retrieves a list of all the latest teams locations
        /// </summary>
        /// <param name="incidentId">The ID of the measuring team</param>
        /// <returns>A list of the latest locations of all teams</returns>
        IQueryable<GpsLocation> GetLatestLocationOfAllTeams(int incidentId);

        /// <summary>
        /// Retrieves a list of the team's complete location history during that incident
        /// </summary>
        /// <param name="incidentId">The ID of the measuring team</param>
        /// <returns>A list of all locations of all teams during that incident</returns>
        IQueryable<GpsLocation> GetCompleteLocationListOfAllTeams(int incidentId);
    }
}
