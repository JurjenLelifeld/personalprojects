using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MeetploegApi.DAL;
using MeetploegApi.Models;

namespace MeetploegApi.ChatService
{
    public class Chat : Hub
    {
        private readonly IChatRepository _chatRepository;
        private readonly IIncidentRepository _incidentRepository;

        public Chat()
        {
            _chatRepository = new ChatRepository(new VGRDataModelContainer());
            _incidentRepository = new IncidentRepository(new VGRDataModelContainer());
        }

        public ApiServices api { get; set; }

        public void Send(string deviceID, string message, int incidentId)
        {
            var deviceId = new DeviceIDModel { deviceID = deviceID};

            var user = _incidentRepository.GetUserInformationByDeviceId(deviceId);

            string messageToSend = $"{user.Username}: {message}";

            Clients.All.messageReceived(deviceID, messageToSend);

            var messageData = new Message
            {
                Content = message,
                Time = DateTime.Now,
                SenderUserId = user.Id,
                IncidentId = incidentId
            };

            _chatRepository.InsertNewChatMessage(messageData);
        }
    }
}