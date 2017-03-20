using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The interface for the chat repository which handles all queries for the chat controller.
    /// </summary>
    public interface IChatRepository : IDisposable
    {
        /// <summary>
        /// Insert a new chat message into the database
        /// </summary>
        /// <param name="chatMessage">All relevant chat information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertNewChatMessage(Message chatMessage);

        /// <summary>
        /// Insert a new delivered data about a chat message 
        /// (set delivered to true, keep read false)
        /// </summary>
        /// <param name="messageDelivered">All relevant chat delivered information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool InsertMessageDeliveredInfomration(MessageToUser messageDelivered);

        /// <summary>
        /// Update the delivered information about a chat, 
        /// since the chat message is now read (set to true)
        /// </summary>
        /// <param name="messageRead">All relevant chat delivered information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        bool UpdateMessageReadInformation(MessageToUser messageRead);

        /// <summary>
        /// Retrieves a list of active chat users from a incident
        /// </summary>
        /// <param name="incidentId">The incident ID that is active</param>
        /// <returns>A list of assignment that the teams have at the incident</returns>
        IQueryable<Assignment> GetActiveChatUserInformation(IncidentIdModel incidentId);
    }
}
