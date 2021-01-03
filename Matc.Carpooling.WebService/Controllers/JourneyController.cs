using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Matc.Carpooling.WebService.Controllers
{
    [Authorize]
    public class JourneyController : ApiController
    {
        #region Initialization
        readonly JourneyBusiness journeyBusiness = new JourneyBusiness();
        readonly UserBusiness userBusiness = new UserBusiness();
        readonly RouteBusiness routeBusiness = new RouteBusiness();
        readonly Validations validations = new Validations();
        #endregion

        /// <summary>
        /// Function to verify if actual user is same as user logged in
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsUserIdEqualToLoggedInUser(string userID)
        {
            string loggedUserId = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            if (loggedUserId.Equals(userID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }        

        /// <summary>
        /// Function to add a new journey
        /// </summary>
        /// <param name="newJourney"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Journeys/AddJourney/")]
        public HttpResponseMessage AddJourney(JourneyRegistrationDto newJourney)
        {
            if(validations.IsNullOrEmpty(newJourney))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }

            DateTime departureTime = newJourney.Departure_Time;
            int availableSeats = newJourney.Available_Seats;
            string carId = newJourney.Car_ID;
            string driverId = newJourney.Driver_ID;
            var message = string.Empty;

            if(!IsUserIdEqualToLoggedInUser(driverId))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, string.Format("Permission denied"));
            }

            string requestSuccessful = journeyBusiness.AddJourney(departureTime, availableSeats, carId, driverId);

            if (requestSuccessful == string.Empty)
            {
                message = string.Format(journeyBusiness.CheckJourneyValidationMessage(departureTime, availableSeats, carId, driverId));
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                message = string.Format("Journey created successfully");

                return Request.CreateResponse(HttpStatusCode.OK, requestSuccessful);
            }
        }

        /// <summary>
        /// Compute and creates routes for the journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Journeys/ComputeJourneyRoutes")]
        public HttpResponseMessage ComputeJourneyRoutes(string journeyId)
        {
            
            Journey journey = journeyBusiness.GetJourney(journeyId);
            var message = string.Empty;

            if (journey == null)
            {
                message = string.Format("Journey not found");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

            bool routesAdded = routeBusiness.AddRoutes(journeyId);

            if(routesAdded == false)
            {
                message = string.Format("An error occured when computing routes");
                HttpError err = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, err);
            }            

            message = string.Format("Routes computed successfully");
            return Request.CreateResponse(HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Gets a journey corresponding to id entered
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Journeys/GetJourney")]
        public HttpResponseMessage GetJourney(string journeyId)
        {
            Journey journey = journeyBusiness.GetJourney(journeyId);

            if (journey == null)
            {
                var message = string.Format("Journey not found");
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, journey);
            }
        }

        /// <summary>
        /// Return list of all active journeys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Journeys/GetActiveJourneys/")]
        public HttpResponseMessage GetActiveJourneys()
        {
            List<Journey> journeys = journeyBusiness.GetActiveJourneys();

            if (journeys.Count < 1)
            {
                var message = string.Format("No journey found");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, journeys);
            }
        }

        /// <summary>
        /// Returns a list of all journeys ever created
        /// </summary>
        /// <returns>A list of Journeys</returns>
        [HttpGet]
        [Route("api/Journeys/GetAllJourneys/")]
        public HttpResponseMessage GetAllJourneys()
        {
            List<Journey> journeys = journeyBusiness.GetAllJourneys();

            if (journeys.Count < 1)
            {
                var message = string.Format("No journey found");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, journeys);
            }
        }

        /// <summary>
        /// Function to get journey by user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Journeys/GetUserJourneys/")]
        public HttpResponseMessage GetUserJourneys(string userId)
        {
            List<Journey> journeys = journeyBusiness.GetUserJourneys(userId);
            User user = userBusiness.GetActiveUser(userId);
            HttpError error = new HttpError();

            if (user == null)
            {
                var message = string.Format("User not found");
                error.Message = message;

                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            else if (journeys.Count <= 0)
            {
                var message = string.Format("No journeys found for this user");
                error.Message = message;

                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, journeys);
            }
        }

        /// <summary>
        /// Function to search journey by destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Journeys/SearchJourney/")]
        public HttpResponseMessage SearchedJourneys(string destination)
        {
            List<Journey> searchResult = journeyBusiness.SearchJourney(destination);

            if (searchResult.Count == 0)
            {
                var message = string.Format("No journeys found");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, searchResult);
            }
        }

        /// <summary>
        /// Function to update info about a journey
        /// </summary>
        /// <param name="data"></param>
        [HttpPut]
        [Route("api/Journeys/UpdateJourney/")]
        public HttpResponseMessage UpdateJourney(UpdateJourneyDto updateJourneyDto)
        {
            if(validations.IsNullOrEmpty(updateJourneyDto))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }
            string journeyId = updateJourneyDto.Journey_ID;
            string parameter = updateJourneyDto.Parameter;
            string value = updateJourneyDto.Value.ToLower();

            if (journeyBusiness.EditJourney(journeyId, parameter, value) == false)
            {
                var message = string.Format(journeyBusiness.CheckUpdateInput(journeyId, parameter, value));
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                var message = string.Format($"{parameter} updated successfully");
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }       

    }
}
