using Matc.Carpooling.DataAccess;
using Matc.Carpooling.Entities;
using System.Collections.Generic;

namespace Matc.Carpooling.Business
{
    public class RouteBusiness
    {
        #region Initialization
        readonly RouteDataAccess routeDataAccess = new RouteDataAccess();
        readonly JourneyBusiness journeyBusiness = new JourneyBusiness();
        readonly BookingDataAccess bookingDataAccess = new BookingDataAccess();
        #endregion

        #region Methods

        /// <summary>
        /// function to add routes to the database
        /// </summary>
        public bool AddRoutes(string journeyID)
        {

            if (bookingDataAccess.GetBookingsOfJourney(journeyID).Count != 0)
            {
                return false;
            }
            routeDataAccess.DeleteAllRoute(journeyID);
            Journey journey = journeyBusiness.GetJourney(journeyID);
            List<Route> routeList = new List<Route>();

            string departureId = "";
            float priceA = 0;
            float priceB = 0;

            // Add all routes [Departure to checkpoints/destination] in list
            foreach (Checkpoint c in journey.Journey_Checkpoints)
            {
                if (c.Type.Equals("departure"))
                {
                    string departure = c.Checkpoint_ID;
                    departureId = c.Checkpoint_ID;

                    foreach (Checkpoint d in journey.Journey_Checkpoints)
                    {
                        if (d.Type.Equals("stopover") || d.Type.Equals("destination"))
                        {
                            routeList.Add(new Route("", departure, d.Checkpoint_ID, d.Price, journey.Available_Seats, journey));
                        }
                    }
                }
            }

            // Add all routes [checkpoint to checkpoints/destination] in list
            foreach (Checkpoint c in journey.Journey_Checkpoints)
            {
                if (c.Type.Equals("stopover"))
                {
                    string departure = c.Checkpoint_ID;

                    foreach (Checkpoint d in journey.Journey_Checkpoints)
                    {
                        if (d.Type.Equals("stopover") || d.Type.Equals("destination"))
                        {
                            if (!c.Equals(d))
                            {
                                foreach (Route route in routeList)
                                {
                                    if (route.Departure.Equals(departureId) && route.Destination.Equals(d.Checkpoint_ID))
                                    {
                                        priceA = route.Price;
                                    }

                                    if (route.Departure.Equals(departureId) && route.Destination.Equals(c.Checkpoint_ID))
                                    {
                                        priceB = route.Price;
                                    }
                                }

                                float newPrice = priceA - priceB;

                                if (newPrice > 0)
                                {
                                    routeList.Add(new Route("", departure, d.Checkpoint_ID, newPrice, journey.Available_Seats, journey));
                                }
                            }
                        }
                    }
                }
            }
            return routeDataAccess.AddRoutes(routeList);
        }

        /// <summary>
        /// Method to get all route of a journey
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public List<Route> GetJourneyRoutes(string journeyID)
        {
            return routeDataAccess.GetJourneyRoutes(journeyID);
        }

        /// <summary>
        /// Method to get the instance of a route using the routeID
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public Route GetRoute(string routeId)
        {
            Route route = routeDataAccess.GetRoute(routeId);
            //List<Checkpoint> checkpointList = checkpointBusiness.GetJourneyCheckpoints(route.Route_Journey.Journey_ID);
            //route.Route_Journey.Journey_Checkpoints = checkpointList;
            return route;
        }

        /// <summary>
        /// Method to decrease the available seats of a route
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public bool DecreaseRouteSeats(string routeId)
        {
            Route route = GetRoute(routeId);
            int newNumberOfSeats = route.Available_Seats - 1;

            return routeDataAccess.UpdateRouteSeats(routeId, newNumberOfSeats);
        }

        /// <summary>
        /// Method to increase the available seats of a route
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public bool IncreaseRouteSeats(string routeId)
        {
            Route route = GetRoute(routeId);
            int newNumberOfSeats = route.Available_Seats + 1;

            return routeDataAccess.UpdateRouteSeats(routeId, newNumberOfSeats);
        }

        /// <summary>
        /// Method to update the price of a route
        /// </summary>
        /// <param name="routeID"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool UpdateRoutePrice(string routeID, float price)
        {
            return routeDataAccess.UpdateRoutePrice(routeID, price);
        }

        /// <summary>
        /// Method to delete a route
        /// </summary>
        /// <param name="checkpointID"></param>
        /// <returns></returns>
        public bool DeleteRoute(string routeId)
        {            
            bool isBookingsDeleted = false;

            List<Booking> routeBookings = bookingDataAccess.GetAllBookingsByRouteID(routeId);

            if(routeBookings.Count == 0)
            {
                isBookingsDeleted = true;
            }
            
            foreach(Booking booking in routeBookings)
            {
                isBookingsDeleted = bookingDataAccess.DeleteBooking(booking.Booking_ID);
            }

            return isBookingsDeleted && routeDataAccess.DeleteRoute(routeId);
        }

        #region Validation Methods

        /// <summary>
        /// Checks if the journeyID is valid
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public bool IsJourneyIDValid(string journeyID)
        {
            Journey journey = journeyBusiness.GetJourney(journeyID);
            return (journey != null);
        }

        /// <summary>
        /// Checks if the routeID is valid
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public bool IsRouteIDValid(string routeID)
        {
            Route route = routeDataAccess.GetRoute(routeID);
            return (route != null);
        }

        /// <summary>
        /// Checks if the price is valid
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool IsPriceValid(float price)
        {
            if (price < 1) return false;
            return true;
        }

        #endregion
        #endregion
    }
}
