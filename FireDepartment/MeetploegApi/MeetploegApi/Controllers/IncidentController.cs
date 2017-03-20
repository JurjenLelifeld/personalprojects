using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Incident Controller handles the initial information phase of the application.
    /// It sends assignment and incident information to the application at startup and it
    /// allows users to set their departure state.
    /// </summary>
    public class IncidentController : ApiController
    {
        private readonly IIncidentRepository _incidentRepository;

        public IncidentController()
        {
            _incidentRepository = new IncidentRepository(new VGRDataModelContainer());
        }

        public IncidentController(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        /// <summary>
        /// Method retrieves and returns all incident and assignment information, as well as the departed information
        /// </summary>
        /// <param name="deviceID">The ID of the users' mobile device</param>
        /// <returns>HttpResponseMessage with a bad request if the lookup fails or with missing data, otherwise a OK message with the data</returns>
        [HttpPost]
        [Route("api/incident/getincidentinfo")]
        public HttpResponseMessage GetIncidentInfo(DeviceIDModel deviceID)
        {
            HttpResponseMessage responseMessage = null;

            var assignmentListModel = new AssignmentListModel { AssignmentList = new List<AssignmentModel>() };

            if (deviceID == null) // DeviceID needs to be provided
            {
                assignmentListModel.Message = "No data received";
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.BadRequest, assignmentListModel.Message);
            }
            else
            {
                responseMessage = GetUserInformation(deviceID, assignmentListModel, responseMessage);
            }

            return responseMessage;
        }

        /// <summary>
        /// Method returns the latest active incident
        /// </summary>
        /// <returns>HttpResponseMessage with Not Found if the lookup fails or with missing data, otherwise a OK message with the data</returns>
        [HttpPost]
        [Route("api/incident/getlatestactiveincident")]
        public HttpResponseMessage GetLatestActiveIncident()
        {
            HttpResponseMessage response;

            var latestIncident = _incidentRepository.GetLatestIncident();

            if (latestIncident == null)
            {
                var incident = new IncidentModel { Message = "No active incidents at this moment" };
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, incident.Message);
            }
            else
            {
                var incident = new IncidentModel()
                {
                    IncidentId = latestIncident.Id,
                    IncidentAddress = latestIncident.Address,
                    IncidentGasMoldCode = latestIncident.GasMoldCode,
                    IncidentGasMoldColor = latestIncident.GasMoldColor,
                    IncidentWindDirection = latestIncident.WindDirection,
                    IncidentTime = latestIncident.Time,
                    IncidentDetails = latestIncident.Details,
                    IncidentType = latestIncident.Type,
                    IncidentLatitude = latestIncident.Latitude,
                    IncidentLongitude = latestIncident.Longitude
                };

                incident.Message = "Lookup success. Latest active incident is available";
                response = Request.CreateResponse(HttpStatusCode.OK, incident);
            }

            return response;
        }

        /// <summary>
        /// Make a call to the database to set the value of depart from the team to true
        /// </summary>
        /// <param name="deviceID">The id of the phone device</param>
        /// <returns>The api will return an OK response if the changes were made, otherwise a no-content message</returns>
        [HttpPost]
        [Route("api/incident/setreadytodepart")]
        public HttpResponseMessage SetReadyToDepart(DeviceIDModel deviceID)
        {
            HttpResponseMessage responseMessage;

            var response = new ApiResponse();

            if (_incidentRepository.UpdateDeparted(deviceID.deviceID, true))
            {
                response.Message = "Departed succesfully set to true";

                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                response.Message = "Setting departure state failed, please try again";

                responseMessage = Request.CreateErrorResponse(HttpStatusCode.Conflict, response.Message);
            }

            return responseMessage;
        }

        /// <summary>
        /// Make a call to the database to create a new incident.
        /// </summary>
        /// <param name="incident">The information of the new incident</param>
        /// <returns>The api will return an OK response if the changes were made, otherwise a no-content message</returns>
        [HttpPost]
        [Route("api/incident/setnewincident")]
        public HttpResponseMessage SetNewIncident(Incident incident)
        {
            bool success = _incidentRepository.InsertIncident(incident);

            var response = CreateResponseMessage(success);

            return response;
        }

        private HttpResponseMessage GetUserInformation(DeviceIDModel deviceID, AssignmentListModel assignmentListModel,
            HttpResponseMessage responseMessage)
        {
            var user = _incidentRepository.GetUserInformationByDeviceId(deviceID);

            if (user == null) // User needs to be registered in the database
            {
                assignmentListModel.Message = "No data available for this device";
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, assignmentListModel.Message);
            }
            else
            {
                responseMessage = GetIncidentInformation(user, assignmentListModel, responseMessage);
            }
            return responseMessage;
        }

        private HttpResponseMessage GetIncidentInformation(User user, AssignmentListModel assignmentListModel,
            HttpResponseMessage responseMessage)
        {
            if (user.MeasuringTeamId != null) // User needs to be part of a team
            {
                var teamInformation = GetTeamInformation(user, assignmentListModel);

                if (teamInformation.Assignments.Any()) // Check if there are any assignments available
                {
                    var incident = GetActiveAssignmentInformation(user, assignmentListModel);                    

                    if (incident != null) // Check if there is an incident
                    {
                        responseMessage = GetIncidentInformation(assignmentListModel, incident);
                    }
                    else
                    {
                        assignmentListModel.Message = "There is no incident";
                        responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, assignmentListModel.Message);
                    }
                }
                else
                {
                    assignmentListModel.Message = "No assignments available for team";
                    responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, assignmentListModel.Message);
                }
            }
            else
            {
                assignmentListModel.Message = "User not a member of a team";
                responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, assignmentListModel.Message);
            }
            return responseMessage;
        }

        private MeasuringTeam GetTeamInformation(User user, AssignmentListModel assignmentListModel)
        {
            MeasuringTeam teamInformation = user.Team;

            assignmentListModel.TeamId = teamInformation.Id;
            assignmentListModel.TeamCode = teamInformation.Code;
            assignmentListModel.TeamName = teamInformation.Name;
            assignmentListModel.TeamLatitude = teamInformation.Latitude;
            assignmentListModel.TeamLongitude = teamInformation.Longitude;
            assignmentListModel.TeamAddress = teamInformation.Address;
            assignmentListModel.TeamDeparted = teamInformation.Departed;

            return teamInformation;
        }

        private Incident GetActiveAssignmentInformation(User user, AssignmentListModel assignmentListModel)
        {
            var measureAssignmentList = user.Team.Assignments.OfType<MeasureAssignment>();
            var measureIdList = new List<int>();
            Incident incident = null;

            foreach (var assignment in measureAssignmentList)
            {
                var newAssignment = new MeasureAssignmentModel
                {
                    AssignmentId = assignment.Id,
                    AssignmentActive = assignment.Active,
                    AssignmentDetails = assignment.Details,
                    AssignmentLatitude = assignment.Latitude,
                    AssignmentLongitude = assignment.Longitude,
                    AssignmentType = assignment.Type,
                    AssignmenttTime = assignment.Time,
                    AssignmentArrived =  assignment.Arrived,

                    //add measureassignments to database
                    AssignmentGastubeCode = assignment.GastubeCode,
                    AssignmentLelMeasurement = assignment.LelMeasurement,
                    AssignmentAutomess = assignment.Automess,
                    AssignmentAutomessProbe = assignment.AutomessProbe,
                    AssignmentDoseMeasurer = assignment.DoseMeasurer,
                    AssignmentBreathableAir = assignment.BreathableAir
                };

                assignmentListModel.AssignmentList.Add(newAssignment);
                measureIdList.Add(assignment.Id);

                incident = incident ?? assignment.Incident;
            }

            incident = getOtherAssignmentInformation(user, assignmentListModel, measureIdList, incident);

            return incident;
        }

        private Incident getOtherAssignmentInformation(User user, AssignmentListModel assignmentListModel, List<int> measureIdList, Incident incident)
        {
            var assignmentList = user.Team.Assignments;

            foreach (var assignment in assignmentList)
            {
                if (!measureIdList.Contains(assignment.Id))
                {
                    var newAssignment = new AssignmentModel
                    {
                        AssignmentId = assignment.Id,
                        AssignmentActive = assignment.Active,
                        AssignmentDetails = assignment.Details,
                        AssignmentLatitude = assignment.Latitude,
                        AssignmentLongitude = assignment.Longitude,
                        AssignmentType = assignment.Type,
                        AssignmenttTime = assignment.Time,
                        AssignmentArrived = assignment.Arrived
                    };

                    assignmentListModel.AssignmentList.Add(newAssignment);

                    incident = incident ?? assignment.Incident;
                }
            }

            return incident;
        }

        private HttpResponseMessage GetIncidentInformation(AssignmentListModel assignmentListModel, Incident incidentInformation)
        {
            assignmentListModel.IncidentId = incidentInformation.Id;
            assignmentListModel.IncidentLatitude = incidentInformation.Latitude;
            assignmentListModel.IncidentLongitude = incidentInformation.Longitude;
            assignmentListModel.IncidentAddress = incidentInformation.Address;
            assignmentListModel.IncidentGasMoldCode = incidentInformation.GasMoldCode;
            assignmentListModel.IncidentGasMoldColor = incidentInformation.GasMoldColor;
            assignmentListModel.IncidentWindDirection = incidentInformation.WindDirection;
            assignmentListModel.IncidentTime = incidentInformation.Time;
            assignmentListModel.IncidentType = incidentInformation.Type;
            assignmentListModel.IncidentDetails = incidentInformation.Details;

            assignmentListModel.Message = "Lookup success. All relevant data is available";
            var responseMessage = Request.CreateResponse(HttpStatusCode.OK, assignmentListModel);

            return responseMessage;
        }

        private HttpResponseMessage CreateResponseMessage(bool success)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage response;

            if (success)
            {
                apiResponse.Message = "Incident data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "Incident data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }
    }
}
