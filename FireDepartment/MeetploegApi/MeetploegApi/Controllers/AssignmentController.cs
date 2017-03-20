using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Assignment Controller handles the retrieval and storage of assignment data.
    /// It is able to handle new assignment information.
    /// </summary>
    public class AssignmentController : ApiController
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentController()
        {
            _assignmentRepository = new AssignmentRepository(new VGRDataModelContainer());
        }

        public AssignmentController(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        /// <summary>
        /// Receives assignment information and stores it in the relevant table
        /// </summary>
        /// <param name="assignment">All relevant assignment information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/assignment/setnewassignment")]
        public HttpResponseMessage SetNewAssignment(Assignment assignment)
        {
            bool success = _assignmentRepository.InsertAssignment(assignment);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives measure assignment information and stores it in the relevant table
        /// </summary>
        /// <param name="measureAssignment">All relevant measure assignment information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/assignment/setnewmeasureassignment")]
        public HttpResponseMessage SetNewMeasureAssignment(MeasureAssignment measureAssignment)
        {
            bool success = _assignmentRepository.InsertMeasureAssignment(measureAssignment);

            var response = CreateResponseMessage(success);

            return response;
        }

        private HttpResponseMessage CreateResponseMessage(bool success)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage response;

            if (success)
            {
                apiResponse.Message = "Assignment data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "Assignment data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }
    }
}
