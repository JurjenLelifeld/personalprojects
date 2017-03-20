using System;
using System.Collections.Generic;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the incident repository which handles all queries for the incident controller.
    /// </summary>
    public interface IIncidentRepository : IDisposable
    {
        /// <summary>
        /// Retrieves the user and assignment information from the database based on the device ID.
        /// </summary>
        /// <param name="deviceId">The device ID of the mobile phone of the user</param>
        /// <returns>Userinformation</returns>
        User GetUserInformationByDeviceId(DeviceIDModel deviceId);

        /// <summary>
        /// Change the departed value of a measurementTeam to true or false
        /// This method should be called if a measurement team is ready to 
        /// go to the place of the incident
        /// </summary>
        /// <param name="phoneId">The device ID of the mobile phone of the user</param>
        /// <param name="departed">The status of the departure to set</param>
        /// <returns>true - The value has correctly been changed
        /// false - The value hasn't been changed</returns>
        bool UpdateDeparted(string phoneId, bool departed);

        /// <summary>
        /// Insert a new incident into the database
        /// </summary>
        /// <param name="incident">All relevant incident information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertIncident(Incident incident);

        /// <summary>
        /// Retrieves the latest active incident
        /// </summary>
        /// <returns>The latest active incident</returns>
        Incident GetLatestIncident();
    }
}
