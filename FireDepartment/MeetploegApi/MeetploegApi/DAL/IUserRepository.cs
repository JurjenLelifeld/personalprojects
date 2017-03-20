using System;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the user repository which handles all queries for the user controller.
    /// </summary>
    public interface IUserRepository : IDisposable
    {
        /// <summary>
        /// Insert a new user into the database
        /// </summary>
        /// <param name="appUserInformation">All relevant user information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertNewUser(User appUserInformation);
    }
}
