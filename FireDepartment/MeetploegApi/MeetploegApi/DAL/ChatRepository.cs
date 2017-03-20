using System;
using System.Linq;
using MeetploegApi.Models;

namespace MeetploegApi.DAL
{
    /// <summary>
    /// The chat repository handles all queries for the chat controller.
    /// </summary>
    public class ChatRepository : IChatRepository, IDisposable
    {
        private VGRDataModelContainer context;
        private bool disposed = false;

        public ChatRepository(VGRDataModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insert a new chat message into the database
        /// </summary>
        /// <param name="chatMessage">All relevant chat information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertNewChatMessage(Message chatMessage)
        {
            bool success;

            try
            {
                context.MessageSet.Add(chatMessage);
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
        /// Insert a new delivered data about a chat message 
        /// (set delivered to true, keep read false)
        /// </summary>
        /// <param name="messageDelivered">All relevant chat delivered information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool InsertMessageDeliveredInfomration(MessageToUser messageDelivered)
        {
            bool success;

            try
            {
                context.MessageToUserSet.Add(messageDelivered);
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
        /// Update the delivered information about a chat, 
        /// since the chat message is now read (set to true)
        /// </summary>
        /// <param name="messageRead">All relevant chat delivered information</param>
        /// <returns>True if the insert succeeded, false if it failed</returns>
        public bool UpdateMessageReadInformation(MessageToUser messageRead)
        {
            bool success;

            try
            {
                context.MessageToUserSet.Attach(messageRead);
                var entry = context.Entry(messageRead);
                entry.Property(e => e.Read).IsModified = true;
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
        /// Retrieves a list of active chat users from a incident
        /// </summary>
        /// <param name="incidentId">The incident ID that is active</param>
        /// <returns>A list of assignment that the teams have at the incident</returns>
        public IQueryable<Assignment> GetActiveChatUserInformation(IncidentIdModel incidentId)
        {
            var listOfAssignments = context.AssignmentSet.Where(i => i.IncidentId == incidentId.IncidentId);

            return listOfAssignments;
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
