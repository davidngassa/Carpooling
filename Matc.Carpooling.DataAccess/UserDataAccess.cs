using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class UserDataAccess
    {
        #region Initialization        
        string connectionString = Properties.Settings.Default.connString;
        #endregion

        #region Methods to set values in database

        /// <summary>
        /// Adds a normal user to the database 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            bool querySuccessful = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO USERS (User_ID, Identity_Number, First_Name, Last_Name, Email, Phone, Password, Is_Active, Is_Admin, Rating, Number_Of_Ratings)" +
                    $"VALUES ('{user.User_ID}','{user.Identity_Number}', '{user.First_Name}', '{user.Last_Name}', '{user.Email}', '{user.Phone}','{user.Password}', '1', '0', '0', '0') ";

                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    querySuccessful = true;
                }
            }

            return querySuccessful;
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns>commandExexutionStatus</returns>
        public bool UpdateUserInfo(string userId, string parameter, string value)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE USERS SET {parameter} = '{value}' WHERE USER_ID = '{userId}'; ";
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
        /// Sets IsActive value of user with corresponding ID to false
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeactivateUserAccount(string userId)
        {
            bool querySuccessful = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE USERS SET Is_Active = '0' WHERE User_ID = '{userId}'";

                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRowsAffected > 0)
                {
                    querySuccessful = true;
                }
            }

            return querySuccessful;

        }
        #endregion

        #region Methods to retrieve values from database

        /// <summary>
        /// Gets an instance of user with the corresponding userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetActiveUser(string userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM USERS WHERE USER_ID='{userId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string identityNumber = reader["Identity_Number"].ToString();
                        string firstName = reader["First_Name"].ToString();
                        string lastName = reader["Last_Name"].ToString();
                        string email = reader["Email"].ToString();
                        string phone = reader["Phone"].ToString();
                        bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                        bool isAdmin = Convert.ToBoolean(reader["Is_Admin"]);
                        float rating = Convert.ToInt32(reader["Rating"]);
                        int numberOfRatings = Convert.ToInt32(reader["Number_Of_Ratings"]);

                        user = new User(userId, identityNumber, firstName, lastName, email, phone, isActive, isAdmin, rating, numberOfRatings);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (user.User_ID == null)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Gets instance of user with corresponding email-password combination
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetActiveUser(string email, string password)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM USERS WHERE Email='{email}' AND Password = '{password}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string userId = reader["User_ID"].ToString();
                        string identityNumber = reader["Identity_Number"].ToString();
                        string firstName = reader["First_Name"].ToString();
                        string lastName = reader["Last_Name"].ToString();
                        string phone = reader["Phone"].ToString();
                        bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                        bool isAdmin = Convert.ToBoolean(reader["Is_Admin"]);
                        float rating = Convert.ToInt32(reader["Rating"]);
                        int numberOfRatings = Convert.ToInt32(reader["Number_Of_Ratings"]);

                        user = new User(userId, identityNumber, firstName, lastName, email, phone, isActive, isAdmin, rating, numberOfRatings);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (user.User_ID == null)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Returns a list of all active users present in the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllActiveUsers()
        {
            List<User> activeUsers = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM users WHERE Is_Active = 'True'; ";
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        string userId = reader["User_ID"].ToString();
                        string identityNumber = reader["Identity_Number"].ToString();
                        string firstName = reader["First_Name"].ToString();
                        string lastName = reader["Last_Name"].ToString();
                        string email = reader["Email"].ToString();
                        string phone = reader["Phone"].ToString();
                        bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                        bool isAdmin = Convert.ToBoolean(reader["Is_Admin"]);
                        float rating = Convert.ToInt32(reader["Rating"]);
                        int numberOfRatings = Convert.ToInt32(reader["Number_Of_Ratings"]);

                        User activeUser = new User(userId, identityNumber, firstName, lastName, email, phone, isActive, isAdmin, rating, numberOfRatings);
                        activeUsers.Add(activeUser);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            if (activeUsers == null)
            {
                return null;
            }
            else
            {
                return activeUsers;
            }
        }

        /// <summary>
        /// Returns a list of all users stored in the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USERS";
                SqlDataReader reader = command.ExecuteReader();

                try{
                    while(reader.Read())
                    {
                        string userId = reader["User_ID"].ToString();
                        string identityNumber = reader["Identity_Number"].ToString();
                        string firstName = reader["First_Name"].ToString();
                        string lastName = reader["Last_Name"].ToString();
                        string email = reader["Email"].ToString();
                        string phone = reader["Phone"].ToString();
                        bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                        bool isAdmin = Convert.ToBoolean(reader["Is_Admin"]);
                        float rating = Convert.ToInt32(reader["Rating"]);
                        int numberOfRatings = Convert.ToInt32(reader["Number_Of_Ratings"]);

                        User user = new User(userId, identityNumber, firstName, lastName, email, phone, isActive, isAdmin, rating, numberOfRatings);
                        users.Add(user);
                    }
                }
                finally{
                    reader.Close();
                    connection.Close();
                }
            }

            return users;
        }
        #endregion

    }
}
