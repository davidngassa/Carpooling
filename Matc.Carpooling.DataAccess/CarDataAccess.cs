using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class CarDataAccess
    {
        #region Initialization
        UserDataAccess userDataAccess = new UserDataAccess();
        string connectionString = Properties.Settings.Default.connString;
        #endregion

        #region Methods to put value in database

        /// <summary>
        /// Method to insert car information into database
        /// </summary>
        /// <param name="thisCar"></param>
        /// <returns></returns>
        public bool SetCar(Car thisCar)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO CARS (CAR_ID, NUMBER_PLATE, MAKE, MODEL, YEAR, NUMBER_OF_SEATS, USER_ID_FK) VALUES('{thisCar.Car_ID}','{thisCar.Number_Plate}','{thisCar.Make}','{thisCar.Model}','{thisCar.Year}','{thisCar.Number_Of_Seats}','{thisCar.CarOwner.User_ID}'); ";
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
        /// Update Info about car
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateCarInfo(string carId, string parameter, string value)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE CARS SET {parameter} = '{value}' WHERE Car_ID = '{carId}'; ";
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
        /// Method to get car information in database using Car ID and return instance of Car
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public Car GetCarInstance(string carId)
        {
            Car thisCar = new Car();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM CARS WHERE CAR_ID='{carId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        thisCar.Car_ID = carId;
                        thisCar.Number_Plate = reader.GetString(1);
                        thisCar.Make = reader.GetString(2);
                        thisCar.Model = reader.GetString(3);
                        thisCar.Year = reader.GetInt32(4);
                        thisCar.Number_Of_Seats = reader.GetInt32(5);
                        thisCar.CarOwner = userDataAccess.GetActiveUser(reader.GetString(6));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (thisCar.Car_ID == null)
            {
                return null;
            }

            return thisCar;
        }

        /// <summary>
        /// Method to get list of instances of Car for specific user using User ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Car> GetListOfCarInstances(string userId)
        {
            List<Car> listOfCarInstancesForUser = new List<Car>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM CARS WHERE USER_ID_FK='{userId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string carId = reader.GetString(0);
                        listOfCarInstancesForUser.Add(GetCarInstance(carId));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }

            return listOfCarInstancesForUser;
        }

        /// <summary>
        /// Get List of all cars present in the database
        /// </summary>
        /// <returns></returns>
        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM CARS;";
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        string carId = reader["Car_ID"].ToString();
                        string numberPlate = reader["Number_Plate"].ToString();
                        string make = reader["Make"].ToString();
                        string model = reader["Model"].ToString();
                        int year = Convert.ToInt32(reader["Year"]);
                        int numberOfSeats = Convert.ToInt32(reader["Number_Of_Seats"]);
                        string userId = reader["User_ID_FK"].ToString();

                        User carOwner = userDataAccess.GetActiveUser(userId);
                        Car car = new Car(carId, numberPlate, make, model, year, numberOfSeats, carOwner);

                        cars.Add(car);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (cars == null)
            {
                return null;
            }
            else
            {
                return cars;
            }
        }

        #endregion

        /// <summary>
        /// Method to delete car information from database using car ID
        /// </summary>
        /// <param name="_carId"></param>
        /// <returns></returns>
        public bool DeleteCar(string _carId)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM JOURNEYS WHERE CAR_ID_FK ='{_carId}'; ";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM CARS WHERE CAR_ID='{_carId}'; ";
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
        /// Method to check if car exists by ID
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool CarIdExists(string carId)
        {
            bool carIdExists = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"SELECT * FROM CARS WHERE CAR_ID='{carId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        carIdExists = true;
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return carIdExists;
        }

    }
}
