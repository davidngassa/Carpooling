using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class JourneyDataAccess
    {
        #region Initialization        
        CarDataAccess carDataAccess = new CarDataAccess();
        string connectionString = Properties.Settings.Default.connString;
        #endregion

        #region Methods to put value in database

        /// <summary>
        /// Method to insert jourey information into database
        /// </summary>
        /// <param name="journey"></param>
        /// <returns></returns>
        public bool AddJourney(Journey journey)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"INSERT INTO JOURNEYS (Journey_ID, Departure_Time, Available_Seats, Status, Car_ID_FK)" +
                    $" VALUES ('{journey.Journey_ID}', '{journey.Departure_Time}', '{journey.Available_Seats}', '{journey.Status}', '{journey.Journey_Car.Car_ID}');";

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

        #region Methods to get value from database        

        /// <summary>
        /// Method to get list of all journey instances
        /// </summary>
        /// <returns></returns>
        public List<Journey> GetAllJourneys()
        {
            List<Journey> journeys = new List<Journey>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"SELECT * FROM JOURNEYS; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string journeyId = reader["Journey_Id"].ToString();
                        DateTime departureTime = Convert.ToDateTime(reader["Departure_Time"]);
                        int AvailableSeats = Convert.ToInt32(reader["Available_Seats"]);
                        string Status = reader["Status"].ToString();
                        Car journeyCar = carDataAccess.GetCarInstance(reader["Car_ID_FK"].ToString());

                        journeys.Add(new Journey(journeyId, departureTime, AvailableSeats, Status, journeyCar));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return journeys;
        }

        /// <summary>
        /// Returns a list of all journeys that are still active
        /// </summary>
        /// <returns></returns>
        public List<Journey> GetActiveJourneys()
        {
            List<Journey> journeys = new List<Journey>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"SELECT * FROM JOURNEYS WHERE Status = 'active'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string journeyId = reader["Journey_Id"].ToString();
                        DateTime departureTime = Convert.ToDateTime(reader["Departure_Time"]);
                        int AvailableSeats = Convert.ToInt32(reader["Available_Seats"]);
                        string Status = reader["Status"].ToString();
                        Car journeyCar = carDataAccess.GetCarInstance(reader["Car_ID_FK"].ToString());

                        journeys.Add(new Journey(journeyId, departureTime, AvailableSeats, Status, journeyCar));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return journeys;
        }

        /// <summary>
        /// Method to get list of journeys for specific user using User ID
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public List<Journey> GetUserJourneys(string _userId)
        {
            List<Journey> listOfJourneys = new List<Journey>();
            List<Car> listOfCars = carDataAccess.GetListOfCarInstances(_userId);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                foreach (Car car in listOfCars)
                {
                    command.CommandText = $"SELECT * FROM JOURNEYS WHERE CAR_ID_FK ='{car.Car_ID}'; ";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string journeyId = reader["Journey_ID"].ToString();
                            DateTime departureTime = Convert.ToDateTime(reader["Departure_Time"]);
                            int availableSeats = Convert.ToInt32(reader["Available_Seats"]);
                            string status = reader["Status"].ToString();
                            string carId = reader["Car_ID_FK"].ToString();

                            Car journeyCar = carDataAccess.GetCarInstance(carId);
                            Journey journey = new Journey(journeyId, departureTime, availableSeats, status, journeyCar);

                            listOfJourneys.Add(journey);
                        }
                    }
                    finally
                    {
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            return listOfJourneys;
        }

        /// <summary>
        /// Method to get list of journeys for specific user using User ID
        /// </summary>
        /// <param name="_journeyId"></param>
        /// <returns></returns>
        public Journey GetJourney(string journeyId)
        {
            Journey journey = new Journey();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM JOURNEYS WHERE Journey_ID='{journeyId}'; ";
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        DateTime departureTime = Convert.ToDateTime(reader["Departure_Time"]);
                        int availableSeats = Convert.ToInt32(reader["Available_Seats"]);
                        string status = reader["Status"].ToString();
                        string carId = reader["Car_ID_FK"].ToString();

                        Car journeyCar = carDataAccess.GetCarInstance(carId);

                        journey = new Journey(journeyId, departureTime, availableSeats, status, journeyCar);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }

            }
            if (journey.Journey_ID == null)
            {
                return null;
            }

            return journey;
        }

        #endregion

        /// <summary>
        /// Method to check if Journey Id exists in database
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool JourneyIdExists(string journeyId)
        {
            bool journeyIdExists = false;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"SELECT * FROM JOURNEYS WHERE JOURNEY_ID='{journeyId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        journeyIdExists = true;
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return journeyIdExists;
        }        

        /// <summary>
        /// Edit and update journey information
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EditJourney(string journeyId, string parameter, string value)
        {
            bool commandExecutionStatus = false;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (parameter == "Departure_Time")
                {
                    command.CommandText = $"UPDATE JOURNEYS SET {parameter} = '{Convert.ToDateTime(value)}' WHERE JOURNEY_ID = '{journeyId}';";
                }
                else
                {
                    command.CommandText = $"UPDATE JOURNEYS SET {parameter} = '{value}' WHERE JOURNEY_ID = '{journeyId}';";
                }
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
        /// Method to delete journey from database using Journey Id
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool DeleteJourney(int journeyId)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"DELETE FROM CHECKPOINTS WHERE JOURNEY_ID_FK ='{journeyId}'; ";
                command.ExecuteNonQuery();

                command.CommandText = $"DELETE FROM JOURNEYS WHERE JOURNEY_ID='{journeyId}'; ";
                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }

            }

            return commandExecutionStatus;
        }        

    }
}
