using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Matc.Carpooling.DataAccess
{
    public class CheckpointDataAccess
    {
        #region  Initialization
        string connectionString = Properties.Settings.Default.connString;        
        #endregion

        #region Methods to put value in database

        /// <summary>
        /// Add new Checkpoint in database
        /// </summary>
        /// <param name="checkpoint"></param>
        /// <returns></returns>
        public bool AddCheckpoint(Checkpoint checkpoint)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO CHECKPOINTS (Checkpoint_ID, Price, Location, Type, Journey_ID_FK) " +
                    $"VALUES ('{checkpoint.Checkpoint_ID}', '{checkpoint.Price}', '{checkpoint.Location}', '{checkpoint.Type}', '{checkpoint.Checkpoint_Journey.Journey_ID}')";

                int numberOfRowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if(numberOfRowsAffected > 0)
                {
                    commandExecutionStatus = true;
                }

                return commandExecutionStatus;
            }
        }

        #endregion

        #region Methods to get value from database

        /// <summary>
        /// Get All checkpoints registered for a journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public List<Checkpoint> GetJourneyCheckpoints(string journeyId)
        {
            List<Checkpoint> journeyCheckpoints = new List<Checkpoint>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM CHECKPOINTS WHERE JOURNEY_ID_FK='{journeyId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Checkpoint checkpoint = GetCheckpoint(reader["Checkpoint_ID"].ToString());
                        journeyCheckpoints.Add(checkpoint);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }

            return journeyCheckpoints;
        }

        /// <summary>
        /// Method to get checkpoint instance using checkpoint Id
        /// </summary>
        /// <param name="checkpointId"></param>
        /// <returns></returns>
        public Checkpoint GetCheckpoint(string checkpointId)
        {
            Checkpoint checkpoint = new Checkpoint();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM CHECKPOINTS WHERE CHECKPOINT_ID='{checkpointId}'; ";
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        checkpoint.Checkpoint_ID = reader["Checkpoint_ID"].ToString();
                        checkpoint.Price = Convert.ToSingle(reader["Price"]);
                        checkpoint.Location = reader["Location"].ToString();
                        checkpoint.Type = reader["Type"].ToString();
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }

            return checkpoint;
        }

        #endregion

        #region Methods to update value in database

        /// <summary>
        /// Method to update checkpoint
        /// </summary>
        /// <param name="checkpoint"></param>
        /// <returns></returns>
        public bool EditCheckpoint(string checkpointId, float newPrice)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE CHECKPOINTS SET Price = '{newPrice}' WHERE CHECKPOINT_ID = '{checkpointId}';";
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
        /// Method to delete checkpoint
        /// </summary>
        /// <param name="checkpointId"></param>
        /// <returns></returns>
        public bool DeleteCheckpoint(string checkpointId)
        {
            bool commandExecutionStatus = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = $"DELETE FROM CHECKPOINTS WHERE CHECKPOINT_ID='{checkpointId}'; ";
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
