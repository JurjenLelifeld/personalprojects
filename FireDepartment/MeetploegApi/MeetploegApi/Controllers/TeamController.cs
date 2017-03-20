using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;
using MeetploegApi.Models.TeamModels;
using TeamModel = MeetploegApi.Models.TeamModels.TeamModel;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Team Controller handles the retrieval of team data.
    /// It is able to send the data of the available teams. 
    /// </summary>
    public class TeamController : ApiController
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController()
        {
            _teamRepository = new TeamRepository(new VGRDataModelContainer());
        }

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Retrieves and sends a list with all team information.
        /// </summary>
        /// <returns>
        /// A not found message if there was no team data and 
        /// otherwise a OK message with the list of the latest location data of all teams
        /// </returns>
        [HttpPost]
        [Route("api/team/getallteams")]
        public HttpResponseMessage GetAllTeams()
        {
            HttpResponseMessage response;
            var teamList = new TeamListModel() { TeamList = new List<TeamModel>() };

            var retrievedTeamList = _teamRepository.GetAllTeams();

            if (retrievedTeamList == null || !retrievedTeamList.Any())
            {
                teamList.Message = "No team data available.";
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, teamList.Message);
            }
            else
            {
                foreach (var team in retrievedTeamList)
                {
                    var newTeam = new TeamModel()
                    {
                        Id = team.Id,
                        Code = team.Code,
                        Name = team.Name,
                        Address = team.Address,
                        Departed = team.Departed,
                        Latitude = team.Latitude,
                        Longitude = team.Longitude
                    };

                    teamList.TeamList.Add(newTeam);
                }

                teamList.Message = "Lookup success. Team data is available";
                response = Request.CreateResponse(HttpStatusCode.OK, teamList);
            }

            return response;
        }
    }
}
