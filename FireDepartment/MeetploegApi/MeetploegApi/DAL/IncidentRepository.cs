using System;
using System.Linq;
using MeetploegApi.Models;
using System.Data.Entity.Infrastructure;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The incident repository handles all queries for the incident controller.
    /// </summary>
    public class IncidentRepository : IIncidentRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public IncidentRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves the user and assignment information from the database based on the device ID.
        /// </summary>
        /// <param name="deviceId">The device ID of the mobile phone of the user</param>
        /// <returns>Userinformation</returns>
        public User GetUserInformationByDeviceId(DeviceIDModel deviceId)
        {
            var user = context.UserSet.FirstOrDefault(device => device.PhoneId == deviceId.deviceID);

            return user;
        }

        /// <summary>
        /// Change the departed value of a measurementTeam to true or false
        /// This method should be called if a measurement team is ready to 
        /// go to the place of the incident
        /// </summary>
        /// <param name="phoneId">The device ID of the mobile phone of the user</param>
        /// <param name="departed">The status of the departure to set</param>
        /// <returns>true - The value has correctly been changed
        /// false - The value hasn't been changed</returns>
        public bool UpdateDeparted(string phoneId, bool departed)
        {
            bool success = true;
            try
            {
                var user = context.UserSet.SingleOrDefault(u => u.PhoneId.Equals(phoneId));
                var team = user.Team;

                team.Departed = departed;

                int amountOfChanges = context.SaveChanges();

                if (amountOfChanges != 1)
                {
                    throw new DbUpdateException(
                        "The Database couldn't save the changes, because the person with this phoneId is in zero or more than 1 teams");
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Insert a new incident into the database
        /// </summary>
        /// <param name="incident">All relevant incident information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertIncident(Incident incident)
        {
            bool success;

            try
            {
                context.IncidentSet.Add(incident);
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
        /// Retrieves the latest active incident
        /// </summary>
        /// <returns>The latest active incident</returns>
        public Incident GetLatestIncident()
        {
            return context.IncidentSet.Where(a => a.Active).OrderByDescending(t => t.Time).FirstOrDefault();
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
