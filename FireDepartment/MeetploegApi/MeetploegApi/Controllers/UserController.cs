using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The User Controller handles the retrieval and storage of user data.
    /// It is able to register new users and to deliver user information.
    /// </summary>
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository(new VGRDataModelContainer());
        }

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Receives a new app user and stores it in the relevant table
        /// </summary>
        /// <param name="appUserInformation">All relevant chat information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/user/registernewappuser")]
        public HttpResponseMessage RegisterNewAppUser(User appUserInformation)
        {
            bool success = _userRepository.InsertNewUser(appUserInformation);

            var response = CreateResponseMessage(success);

            return response;
        }

        private HttpResponseMessage CreateResponseMessage(bool success)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage response;

            if (success)
            {
                apiResponse.Message = "User data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "User data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }
    }
}
