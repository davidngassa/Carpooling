using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Matc.Carpooling.WebService.Controllers
{
    [Authorize]
    [Route("api/Cars")]
    public class CarController : ApiController
    {
        #region Initialization
        CarBusiness carBusiness = new CarBusiness();
        UserBusiness userBusiness = new UserBusiness();
        #endregion


        /// <summary>
        /// Check if user has permissions
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool isUserIdEqualToLoggedInUser(string userID)
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
        /// Register a car in the app
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Cars/AddCar")]
        public HttpResponseMessage AddCar(CarRegistrationDto carRegistrationDto)
        {
            string numberPlate = carRegistrationDto.Number_Plate;
            string make = carRegistrationDto.Make;
            string model = carRegistrationDto.Model;
            int year = carRegistrationDto.Year;
            int numberOfSeats = carRegistrationDto.Number_Of_Seats;
            string ownerId = carRegistrationDto.Owner_ID;            
            
            if(!isUserIdEqualToLoggedInUser(ownerId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {
                bool success = carBusiness.AddCar(numberPlate, make, model, year, numberOfSeats, ownerId);

                if (!success)
                {
                    var message = string.Format(carBusiness.CarRegistrationValidationMessage(numberPlate, make, model, year, numberOfSeats, ownerId));
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else
                {
                    var message = string.Format("Car registered successfully!");
                    return Request.CreateResponse(HttpStatusCode.OK, string.Format(message));
                }
            }            
        }

        /// <summary>
        /// Gets car corresponding to ID
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Cars/GetCar")]
        public HttpResponseMessage GetCar(string carId)
        {
            Car car = carBusiness.GetCar(carId);

            if (car == null)
            {
                var message = string.Format("Car not found");
                HttpError error = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
            else
            {
                GetCarDto getCarDto = new GetCarDto(car);
                return Request.CreateResponse(HttpStatusCode.OK, getCarDto);
            }
        }

        /// <summary>
        /// Get car details of a relevant user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>  
        [HttpGet]
        [Route("api/Cars/GetUserCars/")]
        public HttpResponseMessage GetUserCars(string userId)
        {
            if(!isUserIdEqualToLoggedInUser(userId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {
                if (userBusiness.GetActiveUser(userId) == null)
                {
                    var response = string.Format("User not found");
                    HttpError err = new HttpError(response);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                else
                {
                    List<GetCarDto> usercars = new List<GetCarDto>();

                    foreach (Car car in carBusiness.GetAllUserCars(userId))
                    {
                        GetCarDto getCarDto = new GetCarDto(car);
                        usercars.Add(getCarDto);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, usercars);
                }
            }            
        }

        /// <summary>
        /// Update information about car
        /// </summary>
        /// <param name="carUpdateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Cars/UpdateCar")]
        public HttpResponseMessage UpdateCarinfo(CarUpdateDto carUpdateDto)
        {
            string carId = carUpdateDto.Car_ID;
            string parameter = carUpdateDto.Parameter;
            string value = carUpdateDto.Value;

            string ownerId = carBusiness.GetCar(carId).CarOwner.User_ID;

            if (!isUserIdEqualToLoggedInUser(ownerId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {
                if (carBusiness.UpdateCarInfo(carId, parameter, value) == false)
                {
                    var message = string.Format(carBusiness.CheckUpdateInput(carId, parameter, value));
                    HttpError error = new HttpError(message);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, error);
                }
                else
                {
                    var message = string.Format($"{parameter} updated successfully");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }            
        }

        /// <summary>
        /// unregister a car from the app
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Cars/DeleteCar")]
        public HttpResponseMessage Delete(string carId)
        {
            var response = "";
            string ownerId = carBusiness.GetCar(carId).CarOwner.User_ID;

            if (!isUserIdEqualToLoggedInUser(ownerId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {
                if (carBusiness.CarIdExists(carId))
                {
                    bool carDeleted = carBusiness.DeleteCar(carId);
                    if (carDeleted)
                    {
                        response = string.Format("Car unregistered successfully!");
                        return Request.CreateResponse(HttpStatusCode.OK, string.Format(response));
                    }
                    else
                    {
                        response = string.Format("Car not found");
                        HttpError err = new HttpError(response);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    response = string.Format("Car not found");
                    HttpError err = new HttpError(response);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }          
        }
    }
}
