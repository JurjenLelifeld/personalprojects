using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MeetploegApi.DAL;
using MeetploegApi.Models;
using MeetploegApi.Models.MeasurementModels;

namespace MeetploegApi.Controllers
{
    /// <summary>
    /// The Measurement Controller handles the retrieval and storage of measurement data.
    /// It is able to handle measurementinformation, drive assignment information,
    /// earthquake information and gasmeasurements. 
    /// </summary>
    public class MeasurementController : ApiController
    {
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementController()
        {
            _measurementRepository = new MeasurementRepository(new VGRDataModelContainer());
        }

        public MeasurementController(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }

        /// <summary>
        /// Receives measurement information (drive assignment or own observation) and stores it in the relevant table
        /// </summary>
        /// <param name="measurement">All relevant measurement information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/measurement/setmeasurementinformation")]
        public HttpResponseMessage SetMeasurementInformation(Measurement measurement)
        {
            bool success = _measurementRepository.InsertMeasurement(measurement);
            
            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives gas measurement information and stores it in the relevant table
        /// </summary>
        /// <param name="gasMeasurement">All relevant gas measurement information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/measurement/setgasmeasurementinformation")]
        public HttpResponseMessage SetGasMeasurementInformation(GasMeasurement gasMeasurement)
        {
            bool success = _measurementRepository.InsertGasMeasurement(gasMeasurement);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Receives earthquake measurement information and stores it in the relevant table
        /// </summary>
        /// <param name="earthquakeMeasurement">All relevant earthquake measurement information in JSON</param>
        /// <returns>HTTPStatusCode.OK code if save succceeded and HTTPStatusCode.Conflict if the save failed</returns>
        [HttpPost]
        [Route("api/measurement/setearthquakemeasurementinformation")]
        public HttpResponseMessage SetEarthquakeMeasurementInformation(Earthquake earthquakeMeasurement)
        {
            bool success = _measurementRepository.InsertEarthquakeMeasurement(earthquakeMeasurement);

            var response = CreateResponseMessage(success);

            return response;
        }

        /// <summary>
        /// Sends all measurements of the incident
        /// </summary>
        /// <param name="input">The incident ID and an indicator if the user wants to get the latest value</param>
        /// <returns>
        /// HttpResponseMessage with a bad request if no data was send, 
        /// a not found message if there was no measurement data and 
        /// otherwise a OK message with the measurement data
        /// </returns>
        [HttpPost]
        [Route("api/measurement/getincidentmeasurements")]
        public HttpResponseMessage GetIncidentMeasurements(MeasurementInputModel input)
        {
            HttpResponseMessage response;

            if (input.IncidentId == 0)
            {
                string message = "No data received";
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            else
            {
                var assignmentList = _measurementRepository.GetMeasurements(input.IncidentId);

                if (assignmentList == null || !assignmentList.Any()) // No assignment data (and thus no measurement data) available (yet)
                {
                    string message = "No measurement data available for this incident";
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    var listOfAllMeasurements = new List<MeasurementModel>();
                    var listOfAddedIds = new List<int>();

                    foreach (var assignment in assignmentList)
                    {
                        AddGasMeasurements(assignment, listOfAllMeasurements, listOfAddedIds);

                        AddEarthquakeMeasurements(assignment, listOfAllMeasurements, listOfAddedIds);

                        AddMeasurements(assignment, listOfAddedIds, listOfAllMeasurements);
                    }

                    response = input.GetLatest ? 
                                    Request.CreateResponse(HttpStatusCode.OK, listOfAllMeasurements.OrderByDescending(t => t.MeasurementTime).First()) 
                                    : Request.CreateResponse(HttpStatusCode.OK, listOfAllMeasurements);
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
                apiResponse.Message = "Measurement data succesfully inserted into the database";
                response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
            {
                apiResponse.Message = "Measurement data insertion failed. Please try again!";
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, apiResponse.Message);
            }

            return response;
        }

        private static void AddMeasurements(Assignment assignment, List<int> listOfAddedIds, List<MeasurementModel> listOfAllMeasurements)
        {
            var measurements = assignment.Measurements;

            foreach (var measurement in measurements)
            {
                if (listOfAddedIds.Contains(measurement.Id)) continue;
                var newMeasurement = new MeasurementModel()
                {
                    Id = measurement.Id,
                    AssignmentId = measurement.AssignmentId,
                    MeasurementType = measurement.MeasurementType,
                    Observations = measurement.Observations,
                    MeasurementTime = measurement.MeasurementTime,
                    MeasurementLatitude = measurement.Latitude,
                    MeasurementLongitude = measurement.Longitude
                };
                listOfAllMeasurements.Add(newMeasurement);
                listOfAddedIds.Add(newMeasurement.Id);
            }
        }

        private static void AddEarthquakeMeasurements(Assignment assignment, List<MeasurementModel> listOfAllMeasurements, List<int> listOfAddedIds)
        {
            var earthquakeMeasurements = assignment.Measurements.OfType<Earthquake>();

            foreach (var earthquakeMeasurement in earthquakeMeasurements)
            {
                var newEarthquakeMeasurement = new EarthquakeMeasurementModel()
                {
                    Id = earthquakeMeasurement.Id,
                    AssignmentId = earthquakeMeasurement.AssignmentId,
                    MeasurementType = earthquakeMeasurement.MeasurementType,
                    Observations = earthquakeMeasurement.Observations,
                    MeasurementTime = earthquakeMeasurement.MeasurementTime,
                    MeasurementLatitude = earthquakeMeasurement.Latitude,
                    MeasurementLongitude = earthquakeMeasurement.Longitude,
                    VictimsState = earthquakeMeasurement.VictimsState,
                    BuildingsState = earthquakeMeasurement.BuildingsState,
                    InfrastructureState = earthquakeMeasurement.InfrastructureState,
                    VictimDetails = earthquakeMeasurement.VictimDetails,
                    BuildingDetails = earthquakeMeasurement.BuildingDetails,
                    InfrastructureDetails = earthquakeMeasurement.InfrastructureDetails
                };

                listOfAllMeasurements.Add(newEarthquakeMeasurement);
                listOfAddedIds.Add(newEarthquakeMeasurement.Id);
            }
        }

        private static void AddGasMeasurements(Assignment assignment, List<MeasurementModel> listOfAllMeasurements, List<int> listOfAddedIds)
        {
            var gasMeasurements = assignment.Measurements.OfType<GasMeasurement>();

            foreach (var gasMeasurement in gasMeasurements)
            {
                var newGasMeasurement = new GasMeasurementModel()
                {
                    Id = gasMeasurement.Id,
                    AssignmentId = gasMeasurement.AssignmentId,
                    MeasurementType = gasMeasurement.MeasurementType,
                    Observations = gasMeasurement.Observations,
                    MeasurementTime = gasMeasurement.MeasurementTime,
                    MeasurementLatitude = gasMeasurement.Latitude,
                    MeasurementLongitude = gasMeasurement.Longitude,
                    LelResult = gasMeasurement.LelResult,
                    PumpStrokes = gasMeasurement.PumpStrokes,
                    FirstPumpStroke = gasMeasurement.FirstPumpStroke,
                    Concentration = gasMeasurement.Concentration,
                    CoMeasurement = gasMeasurement.CoMeasurement,
                    GasTubeCode = gasMeasurement.GasTubeCode
                };

                listOfAllMeasurements.Add(newGasMeasurement);
                listOfAddedIds.Add(gasMeasurement.Id);
            }
        }
    }
}