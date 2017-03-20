using System;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The user repository handles all queries for the user controller.
    /// </summary>
    public class UserRepository : IUserRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public UserRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insert a new user into the database
        /// </summary>
        /// <param name="appUserInformation">All relevant user information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertNewUser(User appUserInformation)
        {
            bool success;

            try
            {
                context.UserSet.Add(appUserInformation);
                context.SaveChanges();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
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
