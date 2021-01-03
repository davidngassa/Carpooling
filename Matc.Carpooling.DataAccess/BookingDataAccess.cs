using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class BookingDataAccess
    {
        #region Initialise
        RouteDataAccess routeDataAccess = new RouteDataAccess();
        UserDataAccess userDataAccess = new UserDataAccess();
        string connectionString = Properties.Settings.Default.connString;
        #endregion

        #region Methods to put value in database

        /// <summary>
        /// Method to add booking to database
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public bool BookJourney(Booking newBooking)
        {
            bool commandExecutionStatus = false;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO BOOKINGS (BOOKING_ID, STATUS, ROUTE_ID_FK, USER_ID_FK) " +
                    $"VALUES('{newBooking.Booking_ID}','{newBooking.Status}','{newBooking.Booking_Route.Route_ID}','{newBooking.Booking_User.User_ID}'); ";
                int numberOfRowsAffected = command.ExecuteNonQuery();                

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }
            }

            return commandExecutionStatus;
        }
        #endregion

        #region Methods to get value in database

        /// <summary>
        /// Method to get all bookings of a specific user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Booking> GetAllBookings(string userID)
        {
            List<Booking> bookedJourneys = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT * FROM Bookings WHERE User_ID_FK = '{userID}'; "
                };
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string bookingId = reader["Booking_ID"].ToString();
                        string status = reader["Status"].ToString();
                        string routeId = reader["Route_ID_FK"].ToString();
                        string userId = reader["User_ID_FK"].ToString();

                        Booking booking = new Booking(bookingId, status, routeDataAccess.GetRoute(routeId), userDataAccess.GetActiveUser(userId));
                        bookedJourneys.Add(booking);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
                command.Dispose();
            }

            return bookedJourneys;
        }

        /// <summary>
        /// Get the booking that the user booked in a journey if there's one
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public Booking GetBookingsOfUserInJourney(string userID, string journeyID)
        {
            Booking booking = new Booking();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT * FROM BOOKINGS INNER JOIN ROUTES ON BOOKINGS.Route_ID_FK = ROUTES.Route_ID WHERE User_ID_FK = '{userID}' AND Journey_ID_FK = '{journeyID}' AND Status = 'Active'; "
                };
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        booking.Booking_ID = reader["Booking_ID"].ToString();
                        booking.Status = reader["Status"].ToString();
                        booking.Booking_Route = routeDataAccess.GetRoute(reader["Route_ID_FK"].ToString());
                        booking.Booking_User = userDataAccess.GetActiveUser(reader["Booking_ID"].ToString());
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
                command.Dispose();
            }

            return booking;
        }

        /// <summary>
        /// Get all the booking in a journey
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public List<Booking> GetBookingsOfJourney(string journeyID)
        {
            List<Booking> bookedJourneys = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT * FROM BOOKINGS INNER JOIN ROUTES ON BOOKINGS.Route_ID_FK = ROUTES.Route_ID WHERE Journey_ID_FK = '{journeyID}' AND Status = 'Active'; "
                };
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        bookedJourneys.Add(new Booking(reader.GetString(0), reader.GetString(1), routeDataAccess.GetRoute(reader.GetString(2)), userDataAccess.GetActiveUser(reader.GetString(3))));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
                command.Dispose();
            }

            return bookedJourneys;
        }

        /// <summary>
        /// Get all the bookings of a specified route
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public List<Booking> GetAllBookingsByRouteID(string routeID)
        {
            List<Booking> bookedJourneys = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT * FROM Bookings WHERE Route_ID_FK = '{routeID}'; "
                };
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        bookedJourneys.Add(new Booking(reader.GetString(0), reader.GetString(1), routeDataAccess.GetRoute(reader.GetString(2)), userDataAccess.GetActiveUser(reader.GetString(3))));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
                command.Dispose();
            }

            return bookedJourneys;
        }

        /// <summary>
        /// Method to get a booking instance
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        public Booking GetBooking(string bookingID)
        {
            Booking booking = new Booking();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {


                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT * FROM Bookings WHERE BOOKING_ID = '{bookingID}'; "
                };
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        booking.Booking_ID = reader["Booking_ID"].ToString();
                        booking.Status = reader["Status"].ToString();
                        booking.Booking_Route = routeDataAccess.GetRoute(reader["Route_ID_FK"].ToString());
                        booking.Booking_User = userDataAccess.GetActiveUser(reader["User_ID_FK"].ToString());                                          
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
                command.Dispose();
            }

            return booking;
        }

        /// <summary>
        /// Method to check if Booking exists in database
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool BookingExists(string routeID, string userID)
        {
            bool bookingExists = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"SELECT * FROM BOOKINGS WHERE ROUTE_ID_FK='{routeID}' AND USER_ID_FK='{userID}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        bookingExists = true;
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return bookingExists;
        }

        #endregion

        #region Methods to update value from database

        /// <summary>
        /// Method to update a booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public bool UpdateBooking(string bookingID, string value)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE BOOKINGS SET Status = '{value}' WHERE BOOKING_ID='{bookingID}'; "
                };
                int numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected > 0)
                {
                    Booking booking = GetBooking(bookingID);                    
                    connection.Close();
                    if (numberOfRowsAffected > 0)
                    {
                        commandExecutionStatus = true;
                    }
                }
            }

            return commandExecutionStatus;
        }

        /// <summary>
        /// Method to delete all bookings from the database for a specified route
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public bool DeleteBookingsOfRoute(string routeID)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM BOOKINGS WHERE ROUTE_ID_FK = '{routeID}';"
                };
                int numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected > 0)
                {
                    connection.Close();
                    if (numberOfRowsAffected > 0)
                    {
                        commandExecutionStatus = true;
                    }
                }
            }

            return commandExecutionStatus;
        }

        /// <summary>
        /// Deletes a booking from database
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        public bool DeleteBooking(string bookingID)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM BOOKINGS WHERE Booking_ID = '{bookingID}';"
                };

                int numberOfRowsAffected = command.ExecuteNonQuery();

                if (numberOfRowsAffected > 0)
                {
                    connection.Close();
                    if (numberOfRowsAffected > 0)
                    {
                        commandExecutionStatus = true;
                    }
                }
            }

            return commandExecutionStatus;
        }


        #endregion
    }
}
