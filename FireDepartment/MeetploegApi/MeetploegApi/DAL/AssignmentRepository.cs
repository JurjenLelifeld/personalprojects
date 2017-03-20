using System;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The assignment repository handles all queries for the assignment controller.
    /// </summary>
    public class AssignmentRepository : IAssignmentRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public AssignmentRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insert a new assignment into the database
        /// </summary>
        /// <param name="assignment">All relevant assignment information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertAssignment(Assignment assignment)
        {
            bool success;

            try
            {
                context.AssignmentSet.Add(assignment);
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
        /// Insert a new measure assignment into the database
        /// </summary>
        /// <param name="measureAssignment">All relevant measure assignment information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertMeasureAssignment(MeasureAssignment measureAssignment)
        {
            bool success;

            try
            {
                context.AssignmentSet.Add(measureAssignment);
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
