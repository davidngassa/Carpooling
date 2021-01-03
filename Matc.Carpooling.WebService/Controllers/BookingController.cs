using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Matc.Carpooling.WebService.Controllers
{
    public class BookingController : ApiController
    {
        #region Class Instantiation
        BookingBusiness bookingBusiness = new BookingBusiness();
        #endregion Instanciate Classes

        #region Methods

        /// <summary>
        /// Method to get all the bookings done by a user using userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of Bookings done by a specific user</returns>
        [HttpGet]
        [Route("api/Bookings/GetUserBookings")]
        public HttpResponseMessage GetUserBookings(string userID)
        {
            if (!bookingBusiness.IsUserIDValid(userID))
            {
                HttpError err = new HttpError("The userID you entered is not valid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            List<Booking> userBookings = bookingBusiness.GetAllBookings(userID);
            return Request.CreateResponse(HttpStatusCode.OK, userBookings);
        }

        /// <summary>
        /// Update Info about Booking
        /// </summary>
        /// <param name="data"></param>
        [HttpPut]
        [Route("api/Bookings/UpdateBooking/")]
        public HttpResponseMessage UpdateBooking(BookingUpdateDto bookingDto)
        {
            if (!bookingBusiness.IsBookingIDValid(bookingDto.BookingID))
            {
                HttpError err = new HttpError("The bookingID you entered is not valid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else if (!bookingBusiness.IsStatusValid(bookingDto.Status))
            {
                HttpError err = new HttpError("The Status must be either Active, Completed or Cancelled.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            bookingBusiness.UpdateBooking(bookingDto.BookingID, bookingDto.Status);
            return Request.CreateResponse(HttpStatusCode.OK, "Booking updated");
        }

        /// <summary>
        /// Method to Create a Booking
        /// </summary>
        /// <param name="data">Contains the booking data(user and journey)</param>
        [HttpPost]
        [Route("api/Bookings/BookJourneyRoute")]
        public HttpResponseMessage BookJourney(BookingCreateDto bookingDto)
        {
            if (bookingDto == null)
            {
                HttpError err = new HttpError("The bookingDto cannot be null");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else if (bookingBusiness.IsAttributeNull(bookingDto))
            {
                HttpError err = new HttpError("There is at least one value missing");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                string routeID = bookingDto.routeID;
                string userID = bookingDto.userID;

                if (!bookingBusiness.IsUserIDValid(userID))
                {
                    HttpError err = new HttpError("The userID you entered is not valid.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else if (bookingBusiness.SameDepartureTime(routeID, userID))
                {
                    HttpError err = new HttpError("Have Already Booked A Journey With the Same Departure Time");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else if (bookingBusiness.UpperNearDepartureTime(routeID, userID))
                {
                    HttpError err = new HttpError("A Journey which has a departure time within 3 hours of new booking was already booked ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else if (bookingBusiness.BookingExists(routeID, userID))
                {
                    HttpError err = new HttpError("User Already Booked This Journey");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else if (!bookingBusiness.HasAlreadyBookedThisJourney(routeID, userID))
                {
                    HttpError err = new HttpError("You have already have a booking in this journey.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else if (bookingBusiness.IsSeatsAvailable(routeID))
                {
                    HttpError err = new HttpError("No Seats Available");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                bookingBusiness.BookJourney(routeID, userID);
                return Request.CreateResponse(HttpStatusCode.OK, "Journey Booked");
            }
            
        }

        /// <summary>
        /// Method to Delete a Booking
        /// </summary>
        /// <param name="data">Contains the booking data(user and journey)param>
        [HttpDelete]
        [Route("api/Bookings/CancelBooking")]
        public HttpResponseMessage CancelBooking(string bookingID)
        {
            bool isCanceled = bookingBusiness.DeleteBooking(bookingID);

            //if (bookingBusiness.LowerNearDepartureTime(bookingID))
            //{
            //    HttpError error = new HttpError("Journey starts in 2 hours. Cannot Cancel Booking");
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            //}
            if (isCanceled == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Booking canceled successfully");
            }
            HttpError err = new HttpError("Could not cancel the booking");
            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        }

        #endregion
    }
}
