using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;
using MeetploegApi.Models.ChatModels;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Chat Controller handles the retrieval and storage of chat data.
    /// It is able to handle incoming chat messages, deliver them to the right devices
    /// and store the deliverd and read information. 
    /// </summary>
    public class ChatController : ApiController
    {
        private readonly IChatRepository _chatRepository;

        public ChatController()
        {
            _chatRepository = new ChatRepository(new VGRDataModelContainer());
        }

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        /// <summary>
        /// Receives new chat messages and stores it in the relevant table
        /// </summary>
        /// <param name="chatMessage">All relevant chat information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/chat/setnewchatmessage")]
        public HttpResponseMessage SetNewChatMessage(Message chatMessage)
        {
            bool success = _chatRepository.InsertNewChatMessage(chatMessage);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives chat delivered information of a user and stores it in the relevant table
        /// </summary>
        /// <param name="messageDelivered">The user ID and the message ID in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/chat/setmessagedelivered")]
        public HttpResponseMessage SetMessageDelivered(MessageToUser messageDelivered)
        {
            messageDelivered.Delivered = true;

            bool success = _chatRepository.InsertMessageDeliveredInfomration(messageDelivered);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives chat read information of a user and updates the data in the relevant table
        /// </summary>
        /// <param name="messageRead">The user ID and the message ID in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/chat/updatemessageread")]
        public HttpResponseMessage UpdateMessageRead(MessageToUser messageRead)
        {
            messageRead.Read = true;

            bool success = _chatRepository.UpdateMessageReadInformation(messageRead);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives incident information and sends a active list of the active users on that incident
        /// </summary>
        /// <param name="incidentId">The incident ID in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/chat/getactivechatusers")]
        public HttpResponseMessage GetActiveChatUsers(IncidentIdModel incidentId)
        {
            HttpResponseMessage response;

            var userListModel = new UserListModel() { UserInformationList = new List<UserInformationModel>() };

            var assignmentList = _chatRepository.GetActiveChatUserInformation(incidentId);

            if (assignmentList == null || !assignmentList.Any())
            {
                userListModel.Message = "No active user chat data available.";
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, userListModel.Message);
            }
            else
            {
                response = GetActiveUsersFromAssignmentList(assignmentList, userListModel);
            }

            return response;
        }

        private HttpResponseMessage CreateResponseMessage(bool success)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage response;

            if (success)
            {
                apiResponse.Message = "Chat data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "Chat data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }

        private HttpResponseMessage GetActiveUsersFromAssignmentList(IQueryable<Assignment> assignmentList, UserListModel userListModel)
        {
            var alreadyAddedTeams = new List<int>();

            foreach (var assignment in assignmentList)
            {
                if (alreadyAddedTeams.Contains(assignment.Team.Id)) continue;

                var userList = assignment.Team.Users;

                foreach (var newUser in userList.Select(user => new UserInformationModel(){Id = user.Id, UserName = user.Username}))
                {
                    userListModel.UserInformationList.Add(newUser);
                }

                alreadyAddedTeams.Add(assignment.Team.Id);
            }

            userListModel.Message = "Lookup success. All active user data is available";
            var response = Request.CreateResponse(HttpStatusCode.OK, userListModel);

            return response;
        }
    }
}
