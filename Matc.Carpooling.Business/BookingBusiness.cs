using Matc.Carpooling.DataAccess;
using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;

namespace Matc.Carpooling.Business
{
    public class BookingBusiness
    {
        #region Initialise
        BookingDataAccess bookingDataAccess = new BookingDataAccess();
        RouteDataAccess routeDataAccess = new RouteDataAccess();
        UserDataAccess userDataAccess = new UserDataAccess();
        JourneyDataAccess journeyDataAccess = new JourneyDataAccess(); 
        #endregion

        #region Methods

        /// <summary>
        /// Method to add booking to database
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public bool BookJourney(string routeID, string userID)
        {
            string status = "active";
            Guid bookingGuid = Guid.NewGuid();
            Route bookingRoute = routeDataAccess.GetRoute(routeID);
            User bookingUser = userDataAccess.GetActiveUser(userID);

            Booking newBooking = new Booking(bookingGuid.ToString(), status, bookingRoute, bookingUser);
            int newNumberOfSeats = bookingRoute.Available_Seats - 1;
            journeyDataAccess.EditJourney(bookingRoute.Route_Journey.Journey_ID, "Available_Seats", (bookingRoute.Route_Journey.Available_Seats - 1).ToString()); ;

            //Creates booking and decreases the number of seats available for the route
            return bookingDataAccess.BookJourney(newBooking) && routeDataAccess.UpdateRouteSeats(routeID, newNumberOfSeats);
        }

        /// <summary>
        /// Method to get all bookings for a specific user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Booking> GetAllBookings(string userID)
        {
            return bookingDataAccess.GetAllBookings(userID);
        }

        public List<Booking> GetBookingsOfJourney(string journeyID)
        {
            return bookingDataAccess.GetBookingsOfJourney(journeyID);
        }

        /// <summary>
        /// Method to get a booking instance
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        public Booking GetBooking(string bookingID)
        {
            return bookingDataAccess.GetBooking(bookingID);
        }

        /// <summary>
        /// Method to cancel a booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public bool DeleteBooking(string bookingID)
        {
            Booking booking = GetBooking(bookingID);
            DateTime localDate = DateTime.Now;
            TimeSpan timeDifference = localDate - booking.Booking_Route.Route_Journey.Departure_Time;

            
            if (true)
            {
                Route bookingRoute = booking.Booking_Route;
                int newNumberOfSeats = bookingRoute.Available_Seats + 1;

                //Deletes the booking and increments the number of seats available for the route
                return bookingDataAccess.DeleteBooking(booking.Booking_ID) && routeDataAccess.UpdateRouteSeats(bookingRoute.Route_ID, newNumberOfSeats);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if there are seats available in a route of a journey
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsSeatsAvailable(string routeID)
        {
            Route route = routeDataAccess.GetRoute(routeID);
            return (route.Available_Seats < 1);
        }

        /// <summary>
        /// Get all the Departure times of the bookings done by the user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<DateTime> GetListOfBookingDepartureTimes(string userID)
        {
            List<Booking> bookings = GetAllBookings(userID);
            List<DateTime> bookingDepartureTimes = new List<DateTime>();
            foreach (Booking booking in bookings)
            {
                bookingDepartureTimes.Add(booking.Booking_Route.Route_Journey.Departure_Time);
            }
            return bookingDepartureTimes;
        }

        /// <summary>
        /// Checks if there is already a booking for the same time
        /// </summary>
        /// <param name="selectedBooking"></param>
        /// <returns></returns>
        public bool SameDepartureTime(string routeID, string userID)
        {
            bool timeExists = false;
            List<DateTime> existingBookingDepartureTimes = GetListOfBookingDepartureTimes(userID);
            Journey journey = routeDataAccess.GetRoute(routeID).Route_Journey;

            if (existingBookingDepartureTimes.Contains(journey.Departure_Time))
            {
                timeExists = true;
            }
            else if (existingBookingDepartureTimes == null)
            {
                timeExists = false;
            }
            return timeExists;
        }

        /// <summary>
        /// Checks if there is already a booking for the 3 hours after another booking
        /// </summary>
        /// <param name="selectedBooking"></param>
        /// <returns></returns>
        public bool UpperNearDepartureTime(string routeID, string userID)
        {
            bool nearTimeExists = false;

            List<DateTime> existingBookingDepartureTimes = GetListOfBookingDepartureTimes(userID);
            Journey journey = routeDataAccess.GetRoute(routeID).Route_Journey;

            DateTime LimitTime = journey.Departure_Time;
            DateTime upperLimitTime = LimitTime.AddHours(3);

            foreach (DateTime time in existingBookingDepartureTimes)
            {
                int value1 = DateTime.Compare(time, LimitTime);
                int value2 = DateTime.Compare(time, upperLimitTime);

                if ((value1 > 0) && (value2 < 0))
                {
                    nearTimeExists = true;
                }
            }

            return nearTimeExists;
        }

        /// <summary>
        /// Checks if there is already a booking 2 hours before another booking
        /// </summary>
        /// <param name="selectedBooking"></param>
        /// <returns></returns>
        public bool LowerNearDepartureTime(string bookingID)
        {
            Booking selectedBooking = GetBooking(bookingID);
            bool nearTimeExists = false;

            List<DateTime> existingBookingDepartureTimes = GetListOfBookingDepartureTimes(selectedBooking.Booking_User.User_ID);
            DateTime LimitTime = selectedBooking.Booking_Route.Route_Journey.Departure_Time;
            DateTime lowerLimitTime = LimitTime.AddHours(-2);

            foreach (DateTime time in existingBookingDepartureTimes)
            {
                int value1 = DateTime.Compare(time, LimitTime);
                int value2 = DateTime.Compare(time, lowerLimitTime);

                if ((value2 > 0) && (value1 < 0))
                {
                    nearTimeExists = true;
                }
            }
            return nearTimeExists;
        }

        /// <summary>
        /// Checks if the user already booked a journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool BookingExists(string routeID, string userID)
        {
            return (bookingDataAccess.BookingExists(routeID, userID));
        }

        /// <summary>
        /// Method to update a booking. Because the Status is the only changeable variable, we only pass the bookingID and the new value
        /// </summary>
        /// <param name="bookingID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateBooking(string bookingID, string value)
        {
            return (bookingDataAccess.UpdateBooking(bookingID, value));
        }

        #region Validation Methods

        /// <summary>
        /// Checks if the user has already a booking in the given route
        /// </summary>
        /// <param name="journeyID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool HasAlreadyBookedThisJourney(string routeID, string userID)
        {
            Journey journey = routeDataAccess.GetRoute(routeID).Route_Journey;

            Booking userBooking = bookingDataAccess.GetBookingsOfUserInJourney(userID, journey.Journey_ID);

            return (userBooking.Booking_Route == null);
        }

        /// <summary>
        /// Checks if the journeyID exists
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsJourneyIDValid(string journeyID)
        {
            Journey journey = journeyDataAccess.GetJourney(journeyID);
            return (journey != null);
        }

        /// <summary>
        /// Checks if the journeyID exists
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsUserIDValid(string userID)
        {
            User user = userDataAccess.GetActiveUser(userID);
            return (user != null);
        }

        /// <summary>
        /// Checks if any of the Attribute is missing
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsAttributeNull(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.GetValue(obj, null) == null) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the new status is valid
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsStatusValid(string status)
        {
            return (status == "Active" || status == "Completed" || status == "Cancelled");
        }

        /// <summary>
        /// Checks if the bookingID exists
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsBookingIDValid(string bookingID)
        {
            Booking booking = bookingDataAccess.GetBooking(bookingID);
            return (booking.Booking_ID != null);
        }

        #endregion

        #endregion
    }
}
