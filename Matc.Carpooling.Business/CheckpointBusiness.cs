using Matc.Carpooling.DataAccess;
using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;

namespace Matc.Carpooling.Business
{
    public class CheckpointBusiness
    {
        #region Initialization
        CheckpointDataAccess checkpointDataAccess = new CheckpointDataAccess();
        JourneyDataAccess journeyDataAccess = new JourneyDataAccess();
        RouteDataAccess routeDataAccess = new RouteDataAccess();
        BookingDataAccess bookingDataAccess = new BookingDataAccess();
        #endregion

        #region Methods

        /// <summary>
        /// Method to get list of checkpoint instances for a user using journey
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public List<Checkpoint> GetJourneyCheckpoints(string journeyID)
        {
            List<Checkpoint> journeyCheckpoints = checkpointDataAccess.GetJourneyCheckpoints(journeyID);

            if(journeyCheckpoints == null)
            {
                return null;
            }
            else
            {
                foreach(Checkpoint checkpoint in journeyCheckpoints)
                {
                    Journey checkpointJourney = journeyDataAccess.GetJourney(journeyID);
                    checkpoint.Checkpoint_Journey = checkpointJourney;                    
                }

                return journeyCheckpoints;
            }
        }

        /// <summary>
        /// Method to get checkpoint instance using checkpoint Id
        /// </summary>
        /// <param name="checkpointID"></param>
        /// <returns></returns>
        public Checkpoint GetCheckpoint(string journeyID, string checkpointID)
        {
            Checkpoint checkpoint = checkpointDataAccess.GetCheckpoint(checkpointID);
            Journey journey = journeyDataAccess.GetJourney(journeyID);

            foreach(Checkpoint journeyCheckpoint in GetJourneyCheckpoints(journeyID))
            {
                if(journeyCheckpoint.Checkpoint_ID == checkpointID)
                {
                    checkpoint.Checkpoint_Journey = journey;

                    return checkpoint;
                }
            }

            return null;
        }        

        /// <summary>
        /// Method to edit and update checkpoint information
        /// </summary>
        /// <param name="checkpoint"></param>
        /// <returns></returns>
        public bool EditCheckpoint(string journeyID, string checkpointID, float newPrice)
        {
            bool commandSuccesful = false;

            if(IsCheckpointEditable(journeyID, checkpointID).Equals("valid"))
            {
                commandSuccesful = checkpointDataAccess.EditCheckpoint(checkpointID, newPrice);
            }

            return commandSuccesful;
        }

        /// <summary>
        /// Method to delete checkpoint
        /// </summary>
        /// <param name="checkpointID"></param>
        /// <returns></returns>
        public bool DeleteCheckpoint(string journeyID, string checkpointID)
        {
            if(DeleteCheckpointMessage(journeyID, checkpointID) != "valid")
            {
                return false;
            }

            //Deletes every booking and routes linked to this checkpoint
            foreach(Route route in routeDataAccess.GetCheckpointRoutes(journeyID, checkpointID))
            {
                bookingDataAccess.DeleteBookingsOfRoute(route.Route_ID);
                routeDataAccess.DeleteRoute(route.Route_ID);
            }

            return checkpointDataAccess.DeleteCheckpoint(checkpointID);
        }

        #endregion

        #region Utilities       

        public string IsCheckpointEditable(string journeyID, string checkpointID)
        {           
            var message = "valid";
            Checkpoint checkpoint = GetCheckpoint(journeyID, checkpointID);

            if(checkpoint == null)
            {
                message = "Checkpoint not found";
            }
            else
            {                
                Journey journey = checkpoint.Checkpoint_Journey;

                if(DateTime.Now > journey.Departure_Time.AddHours(-2))
                {
                    message = "A checkpoint cannot be edited within two hours of the journey departure time";
                }
            }
           
            return message;
        }

        public string DeleteCheckpointMessage (string journeyID, string checkpointID)
        {
            string message = "valid";

            if(journeyDataAccess.GetJourney(journeyID) == null)
            {
                message = "Journey not found";
            }
            else if(GetCheckpoint(journeyID, checkpointID) == null)
            {
                message = "Checkpoint not found";
            }
            else
            {
                if (DateTime.Now > journeyDataAccess.GetJourney(journeyID).Departure_Time.AddHours(-2))
                {
                    message = "Checkpoint cannot be deleted within two hours of the journey departure time"; ;
                }
            }

            return message;
        }

        #endregion

    }
}
