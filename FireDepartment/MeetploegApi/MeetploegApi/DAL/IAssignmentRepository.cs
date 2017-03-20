using System;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the assignment repository which handles all queries for the assignment controller.
    /// </summary>
    public interface IAssignmentRepository : IDisposable
    {
        /// <summary>
        /// Insert a new assignment into the database
        /// </summary>
        /// <param name="assignment">All relevant assignment information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertAssignment(Assignment assignment);

        /// <summary>
        /// Insert a new measure assignment into the database
        /// </summary>
        /// <param name="measureAssignment">All relevant measure assignment information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertMeasureAssignment(MeasureAssignment measureAssignment);
    }
}
