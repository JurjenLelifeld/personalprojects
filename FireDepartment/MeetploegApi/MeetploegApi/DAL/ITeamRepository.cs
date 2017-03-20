using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the team repository which handles all queries for the team controller.
    /// </summary>
    public interface ITeamRepository : IDisposable
    {
        /// <summary>
        /// Retrieves a list of all the teams.
        /// </summary>
        /// <returns>A queryable list of all teams</returns>
        IQueryable<MeasuringTeam> GetAllTeams();
    }
}
