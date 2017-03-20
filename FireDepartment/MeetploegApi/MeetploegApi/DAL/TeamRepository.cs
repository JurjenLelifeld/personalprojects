using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The team repository handles all queries for the team controller.
    /// </summary>
    public class TeamRepository : ITeamRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public TeamRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        public IQueryable<MeasuringTeam> GetAllTeams()
        {
            return context.MeasuringTeamSet;
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
