using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class RouteDataAccess
    {
        #region Initialization
        JourneyDataAccess journeyDataAccess = new JourneyDataAccess();
        CheckpointDataAccess checkpointDataAccess = new CheckpointDataAccess();
        string connectionString = Properties.Settings.Default.connString;
        #endregion

        #region Methods to put value in database

           
        /// <summary>
        /// Method to add routes to the database
        /// </summary>
        /// <param name="journey"></param>
        /// <returns></returns>
        public bool AddRoutes(List<Route> routeList)
        {
            // Add list of routes to database
            bool commandExecutionStatus = false;
            int numberOfRowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                foreach (Route route in routeList)
                {
                    Guid routeId = Guid.NewGuid();
                    route.Route_ID = routeId.ToString();
                    command.CommandText = $"INSERT INTO ROUTES (ROUTE_ID, DEPARTURE, DESTINATION, PRICE, AVAILABLE_SEATS, JOURNEY_ID_FK) VALUES('{route.Route_ID}','{route.Departure}','{route.Destination}','{route.Price}','{route.Available_Seats}','{route.Route_Journey.Journey_ID}'); ";
                    numberOfRowsAffected = command.ExecuteNonQuery();
                }

                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }
            }

            return commandExecutionStatus;
        }

        #endregion

        #region Methods to get value from database

        /// <summary>
        /// Method to get a list of all route in a journey from database
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public List<Route> GetJourneyRoutes(string journeyID)
        {
            List<Route> userRoutes = new List<Route>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM ROUTES WHERE JOURNEY_ID_FK='{journeyID}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string routeId = reader["Route_ID"].ToString();
                        userRoutes.Add(GetRoute(routeId));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }

            return userRoutes;
        }

        /// <summary>
        /// Method to get all routes linked to a checkpoint
        /// </summary>
        /// <param name="checkpointID"></param>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public List<Route> GetCheckpointRoutes(string journeyID, string checkpointID)
        {
            List<Route> checkpointRoutes = new List<Route>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM ROUTES WHERE Journey_ID_FK='{journeyID}' AND (Departure = '{checkpointID}' OR Destination = '{checkpointID}'); ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string routeId = reader["Route_ID"].ToString();
                        checkpointRoutes.Add(GetRoute(routeId));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }

            return checkpointRoutes;
        }

        /// <summary>
        /// Method to get the instance of a route using the routeID
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public Route GetRoute(string routeId)
        {
            Route thisRoute = new Route();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM ROUTES WHERE ROUTE_ID='{routeId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        thisRoute.Route_ID = routeId;
                        thisRoute.Departure = reader["Departure"].ToString();
                        thisRoute.Destination = reader["Destination"].ToString();
                        thisRoute.Price = Convert.ToSingle(reader["Price"]);
                        thisRoute.Available_Seats = Convert.ToInt32(reader["Available_Seats"]);
                        thisRoute.Route_Journey = journeyDataAccess.GetJourney(reader["Journey_ID_FK"].ToString());
                    }
                    List<Checkpoint> checkpointList = checkpointDataAccess.GetJourneyCheckpoints(thisRoute.Route_Journey.Journey_ID);
                    thisRoute.Route_Journey.Journey_Checkpoints = checkpointList;
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (thisRoute.Route_ID == null)
            {
                return null;
            }

            return thisRoute;
        }

        #endregion

        #region Methods to update value in database

        /// <summary>
        /// Update number of seats available for a route
        /// </summary>
        /// <param name="routeID"></param>
        /// <param name="availableSeats"></param>
        /// <returns></returns>
        public bool UpdateRouteSeats(string routeID, int availableSeats)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE ROUTES SET AVAILABLE_SEATS = {availableSeats} WHERE ROUTE_ID='{routeID}'; ";
                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }
            }

            return commandExecutionStatus;
        }

        /// <summary>
        /// Method to update the price of a route
        /// </summary>
        /// <param name="routeID"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool UpdateRoutePrice(string routeID, float price)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE ROUTES SET PRICE = {price} WHERE ROUTE_ID='{routeID}'; ";
                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }
            }

            return commandExecutionStatus;
        }

        #endregion

        #region Methods to delete value from database

        /// <summary>
        /// Method to delete a route
        /// </summary>
        /// <param name="checkpointID"></param>
        /// <returns></returns>
        public bool DeleteRoute(string routeID)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"DELETE FROM ROUTES WHERE Route_ID='{routeID}'; ";
                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }

            }

            return commandExecutionStatus;
        }

        public bool DeleteAllRoute(string journeyID)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"DELETE FROM ROUTES WHERE Journey_ID_FK='{journeyID}'; ";
                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }

            }

            return commandExecutionStatus;
        }



        #endregion
    }
}
