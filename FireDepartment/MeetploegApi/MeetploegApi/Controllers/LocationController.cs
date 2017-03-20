using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;
using MeetploegApi.Models.LocationModels;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Location Controller handles the retrieval and storage of location (GPS) data.
    /// It is able to handle incoming location information and deliver them to the right devices.
    /// </summary>
    public class LocationController : ApiController
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController()
        {
            _locationRepository = new LocationRepository(new VGRDataModelContainer());
        }

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// Receives new location information from a device and stores it in the relevant table
        /// </summary>
        /// <param name="gpsLocation">All relevant chat information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/location/setnewgpslocation")]
        public HttpResponseMessage SetNewGpsLocation(GpsLocation gpsLocation)
        {
            bool success = _locationRepository.InsertNewGpsLocation(gpsLocation);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Sends the latest team location data.
        /// </summary>
        /// <param name="measuringTeamId">All relevant chat information in JSON</param>
        /// <returns>
        /// HttpResponseMessage with a bad request if no data was send, 
        /// a not found message if there was no location data and 
        /// otherwise a OK message with the team location data
        /// </returns>
        [HttpPost]
        [Route("api/location/getlatestteamlocation")]
        public HttpResponseMessage GetLatestTeamLocation(MeasuringTeamIdModel measuringTeamId)
        {
            HttpResponseMessage response = null;

            var teamLocationModel = new TeamLocationModel();

            if (measuringTeamId.MeasuringTeamId == 0)
            {
                teamLocationModel.Message = "No data received";
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, teamLocationModel.Message);
            }
            else
            {
                var latestTeamLocation = _locationRepository.GetLatestTeamLocation(measuringTeamId);

                if (latestTeamLocation == null) // No GPS data available (yet)
                {
                    teamLocationModel.Message = "No location data available for this team at this incident";
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, teamLocationModel.Message);
                }
                else
                {
                    teamLocationModel.Id = latestTeamLocation.Id;
                    teamLocationModel.MeasuringTeamId = latestTeamLocation.MeasuringTeamId;
                    teamLocationModel.IncidentId = latestTeamLocation.IncidentId;
                    teamLocationModel.Latitude = latestTeamLocation.Latitude;
                    teamLocationModel.Longitude = latestTeamLocation.Longitude;
                    teamLocationModel.Time = latestTeamLocation.Time;
                    teamLocationModel.TeamName = latestTeamLocation.MeasuringTeam.Name;
                    teamLocationModel.TeamCode = latestTeamLocation.MeasuringTeam.Code;

                    teamLocationModel.Message = "Lookup success. All relevant data is available";
                    response = Request.CreateResponse(HttpStatusCode.OK, teamLocationModel);
                }
            }
            
            return response;
        }

        /// <summary>
        /// Sends all the team location data of a team during a incident.
        /// </summary>
        /// <param name="measuringTeamId">All relevant chat information in JSON</param>
        /// <returns>
        /// HttpResponseMessage with a bad request if no data was send, 
        /// a not found message if there was no location data and 
        /// otherwise a OK message with the list of team location data
        /// </returns>
        [HttpPost]
        [Route("api/location/getcompleteteamlocationhistory")]
        public HttpResponseMessage GetCompleteTeamLocationHistory(MeasuringTeamIdModel measuringTeamId)
        {
            HttpResponseMessage response = null;

            var teamLocationListModel = new TeamLocationListModel { TeamLocationList = new List<TeamLocationModel>() };

            if (measuringTeamId.MeasuringTeamId == 0)
            {
                teamLocationListModel.Message = "No data received";
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, teamLocationListModel.Message);
            }
            else
            {
                var listOfTeamLocations = _locationRepository.GetCompleteTeamLocationHistory(measuringTeamId);

                if (listOfTeamLocations == null) // No GPS data available (yet)
                {
                    teamLocationListModel.Message = "No location data available for this team at this incident";
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, teamLocationListModel.Message);
                }
                else
                {
                    response = CreateLocationListResponse(listOfTeamLocations, teamLocationListModel, response);
                }
            }

            return response;
        }
        
        /// <summary>
        /// Sends the latest team location data of a all active teams during a incident.
        /// </summary>
        /// <param name="incidentId">The incident ID in JSON</param>
        /// <returns>
        /// HttpResponseMessage with a bad request if no data was send, 
        /// a not found message if there was no location data and 
        /// otherwise a OK message with the list of the latest location data of all teams
        /// </returns>
        [HttpPost]
        [Route("api/location/getlatestlocationofallteams")]
        public HttpResponseMessage GetLatestLocationOfAllTeams(MeasuringTeamIdModel incidentId)
        {
            HttpResponseMessage response = null;

            var teamLocationListModel = new TeamLocationListModel { TeamLocationList = new List<TeamLocationModel>() };

            if (incidentId.IncidentId == 0)
            {
                teamLocationListModel.Message = "No data received";
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, teamLocationListModel.Message);
            }
            else
            {
                var listOfTeamLocations = _locationRepository.GetLatestLocationOfAllTeams(incidentId.IncidentId);

                if (listOfTeamLocations == null) // No GPS data available (yet)
                {
                    teamLocationListModel.Message = "No location data available for any team at this incident";
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, teamLocationListModel.Message);
                }
                else
                {
                    response = CreateLocationListResponseOfAllTeamLatestLocations(listOfTeamLocations, teamLocationListModel, response);
                }
            }

            return response;
        }
        
        /// <summary>
        /// Sends the team location data of a all active teams during a incident.
        /// </summary>
        /// <param name="incidentId">The incident ID in JSON</param>
        /// <returns>
        /// HttpResponseMessage with a bad request if no data was send, 
        /// a not found message if there was no location data and 
        /// otherwise a OK message with the list of the location data of all teams
        /// </returns>
        [HttpPost]
        [Route("api/location/getcompletelocationlistofallteams")]
        public HttpResponseMessage GetCompleteLocationListOfAllTeams(MeasuringTeamIdModel incidentId)
        {
            HttpResponseMessage response = null;

            var teamLocationListModel = new TeamLocationListModel { TeamLocationList = new List<TeamLocationModel>() };

            if (incidentId.IncidentId == 0)
            {
                teamLocationListModel.Message = "No data received";
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, teamLocationListModel.Message);
            }
            else
            {
                var listOfTeamLocations = _locationRepository.GetCompleteLocationListOfAllTeams(incidentId.IncidentId);

                if (listOfTeamLocations == null) // No GPS data available (yet)
                {
                    teamLocationListModel.Message = "No location data available for any team at this incident";
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, teamLocationListModel.Message);
                }
                else
                {
                    response = CreateLocationListResponse(listOfTeamLocations, teamLocationListModel, response);
                }
            }

            return response;
        }

        private HttpResponseMessage CreateResponseMessage(bool success)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage response;

            if (success)
            {
                apiResponse.Message = "GPS data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "GPS data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }

        private HttpResponseMessage CreateLocationListResponse(IQueryable<GpsLocation> listOfTeamLocations,
            TeamLocationListModel teamLocationListModel, HttpResponseMessage response)
        {
            foreach (var location in listOfTeamLocations)
            {
                var newLocation = new TeamLocationModel
                {
                    Id = location.Id,
                    MeasuringTeamId = location.MeasuringTeamId,
                    IncidentId = location.IncidentId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Time = location.Time,
                    TeamName = location.MeasuringTeam.Name,
                    TeamCode = location.MeasuringTeam.Code
                };

                teamLocationListModel.TeamLocationList.Add(newLocation);
            }

            teamLocationListModel.Message = "Lookup success. All relevant data is available";
            response = Request.CreateResponse(HttpStatusCode.OK, teamLocationListModel);

            return response;
        }

        private HttpResponseMessage CreateLocationListResponseOfAllTeamLatestLocations(IQueryable<GpsLocation> listOfTeamLocations,
            TeamLocationListModel teamLocationListModel, HttpResponseMessage response)
        {
            int latestIteratedTeamId = 0;

            foreach (var location in listOfTeamLocations)
            {
                if (latestIteratedTeamId == location.MeasuringTeamId) continue;
                var newLocation = new TeamLocationModel
                {
                    Id = location.Id,
                    MeasuringTeamId = location.MeasuringTeamId,
                    IncidentId = location.IncidentId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Time = location.Time,
                    TeamName = location.MeasuringTeam.Name,
                    TeamCode = location.MeasuringTeam.Code
            };

                teamLocationListModel.TeamLocationList.Add(newLocation);
                latestIteratedTeamId = location.MeasuringTeamId;
            }

            teamLocationListModel.Message = "Lookup success. All relevant data is available";
            response = Request.CreateResponse(HttpStatusCode.OK, teamLocationListModel);
            return response;
        }
    }
}
