using Matc.Carpooling.DataAccess;
using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matc.Carpooling.Business
{
    public class JourneyBusiness
    {
        #region Initialization        
        JourneyDataAccess journeyDataAccess = new JourneyDataAccess();
        CheckpointDataAccess checkpointDataAccess = new CheckpointDataAccess();
        CarBusiness carBusiness = new CarBusiness();
        Validations validations = new Validations();
        #endregion

        #region Journey Methods

        /// <summary>
        /// Create a new journey
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="availableSeats"></param>
        /// <param name="carId"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public string AddJourney(DateTime departureTime, int availableSeats, string carId, string driverId)
        {
            //bool commandSuccessful = false;
            string createdJourneyID = string.Empty;

            if (CheckJourneyValidationMessage(departureTime, availableSeats, carId, driverId).Equals("Valid"))
            {
                Guid journeyGUID = Guid.NewGuid();
                createdJourneyID = journeyGUID.ToString();
                string status = "active";
                Car car = carBusiness.GetCar(carId);

                Journey journey = new Journey(journeyGUID.ToString(), departureTime, availableSeats, status, car);
                //commandSuccessful = journeyDataAccess.AddJourney(journey);
                journeyDataAccess.AddJourney(journey);
            }

            return createdJourneyID;
        }

        /// <summary>
        /// Add a checkpoint to a journey 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool AddCheckpointToJourney(float price, string location, string type, string journeyId)
        {
            bool status = false;
            price = Math.Abs(price);

            if (CheckpointRegistrationValidation(price, location, type, journeyId).Equals("valid"))
            {
                //Generate guid
                Guid checkpointGuid = Guid.NewGuid();
                string checkpointId = checkpointGuid.ToString();

                Journey checkpointJourney = GetJourney(journeyId);
                Checkpoint checkpoint = new Checkpoint(checkpointId, price, location, type, checkpointJourney);

                status = checkpointDataAccess.AddCheckpoint(checkpoint);
            }

            return status;
        }

        /// <summary>
        /// Method to get list of journeys for specific user using User ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Journey> GetUserJourneys(string userId)
        {
            List<Journey> journeys = journeyDataAccess.GetUserJourneys(userId);

            if(journeys == null)
            {
                return null;
            }

            foreach(Journey journey in journeys)
            {                
                journey.Journey_Checkpoints = checkpointDataAccess.GetJourneyCheckpoints(journey.Journey_ID); 
            }

            return journeys;
        }

        /// <summary>
        /// Method to verify if journeyId exists in database
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool JourneyIdExists(string journeyId)
        {
            return journeyDataAccess.JourneyIdExists(journeyId);
        }

        /// <summary>
        /// Method to get list of journeys for specific user using User ID
        /// </summary>
        /// <param name="journeyID"></param>
        /// <returns></returns>
        public Journey GetJourney(string journeyID)
        {
            Journey journey = journeyDataAccess.GetJourney(journeyID);
            if(journey == null)
            {
                return null;
            }

            journey.Journey_Checkpoints = checkpointDataAccess.GetJourneyCheckpoints(journeyID);            
            return journey;
        }

        /// <summary>
        /// Method to delete journey using Journey Id
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool DeleteJourney(int journeyId)
        {
            return journeyDataAccess.DeleteJourney(journeyId);
        }

        /// <summary>
        /// Returns a list of all active journeys with their checkpoints
        /// </summary>
        /// <returns></returns>
        public List<Journey> GetActiveJourneys()
        {
            List<Journey> journeys = journeyDataAccess.GetActiveJourneys();

            if (journeys == null)
            {
                return null;
            }

            foreach (Journey journey in journeys)
            {
                journey.Journey_Checkpoints = checkpointDataAccess.GetJourneyCheckpoints(journey.Journey_ID);
            }

            return journeys;
        }

        /// <summary>
        /// Method to get list of all journey instances
        /// </summary>
        /// <returns></returns>
        public List<Journey> GetAllJourneys()
        {
            List<Journey> journeys = journeyDataAccess.GetAllJourneys();

            if(journeys == null)
            {
                return null;
            }

            foreach (Journey journey in journeys)
            {
                journey.Journey_Checkpoints = checkpointDataAccess.GetJourneyCheckpoints(journey.Journey_ID);
            }

            return journeys;
        }

        /// <summary>
        /// Method to edit journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EditJourney(string journeyId, string parameter, string value)
        {
            bool status = false;

            if (CheckUpdateInput(journeyId, parameter, value.ToLower()).Equals("valid"))
            {
                status = journeyDataAccess.EditJourney(journeyId, parameter, value);
            }

            return status;
        }

        /// <summary>
        /// Method to get list of journeys according to search
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<Journey> SearchJourney(string destination)
        {
            List<Journey> searchResult = new List<Journey>();
            List<Journey> listOfAllJourney = GetAllJourneys();

            foreach (Journey journey in listOfAllJourney)
            {
                foreach (Checkpoint checkpoint in journey.Journey_Checkpoints)
                {
                    if (checkpoint.Location.ToLower().Equals(destination.ToLower()) && (checkpoint.Type.Equals("destination") || checkpoint.Type.Equals("stopover")))
                    {
                        searchResult.Add(journey);
                    }
                }
            }

            return searchResult;
        }

        public bool IsValidParameter(string columnName)
        {
            string[] columnNames = { "departure_time", "available_seats", "status", "car_id_fk" };
            return columnNames.Contains(columnName.ToLower());
        }

        #region Validations

        /// <summary>
        /// Returns false if departure time is not at least 3 hours after of the current time, or if it's not either 3 hours after or 3 hours before any created journeys
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool IsValidDepartureTime(DateTime departureTime, string carId)
        {
            User driver = carBusiness.GetCar(carId).CarOwner;
            List<Journey> driverJourneys = GetUserJourneys(driver.User_ID);

            bool boolean = true;

            if (departureTime <= DateTime.Now.AddHours(3))
            {
                boolean = false;
            }
            else
            {
                foreach (Journey journey in driverJourneys)
                {
                    if ((departureTime >= journey.Departure_Time.AddHours(-3) && departureTime <= journey.Departure_Time.AddHours(3)))
                    {
                        boolean = false;
                    }
                }
            }

            return boolean;
        }

        /// <summary>
        /// Check if the number of seats entered is not greater than the number of seats available for the car
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="availableSeats"></param>
        /// <param name="status"></param>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool IsValidAvailableSeats(int availableSeats, string carId)
        {
            return ((availableSeats < carBusiness.GetCar(carId).Number_Of_Seats) && availableSeats > 0);
        }

        /// <summary>
        /// Checks if user owns car 
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsValidCar(string carId, string userId)
        {
            bool exists = false;

            List<Car> userCars = carBusiness.GetAllUserCars(userId);

            foreach (Car car in userCars)
            {
                if (car.Car_ID == carId)
                {
                    exists = true;
                }
            }

            return exists;
        }

        /// <summary>
        /// Check if the status is a valid option
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsValidStatus(string status)
        {
            string[] statusOptions = { "active", "completed", "canceled" };

            return statusOptions.Contains(status);
        }

        /// <summary>
        /// Checks if any checkpoint with the corresponding location exists already for the journey
        /// </summary>
        /// <param name="location"></param>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool LocationExistsInJourney(string location, string journeyId)
        {
            List<Checkpoint> journeyCheckpoints = checkpointDataAccess.GetJourneyCheckpoints(journeyId);
            List<string> existingLocations = new List<string>();

            foreach (Checkpoint checkpoint in journeyCheckpoints)
            {
                existingLocations.Add(checkpoint.Location);
            }

            return existingLocations.Contains(location);
        }

        /// <summary>
        /// Checks if a checkpoint of type departure has already been specified for a journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool DepartureExistsInJourney(string journeyId)
        {
            List<Checkpoint> journeyCheckpoints = checkpointDataAccess.GetJourneyCheckpoints(journeyId);
            List<string> checkpointTypes = new List<string>();

            foreach (Checkpoint checkpoint in journeyCheckpoints)
            {
                checkpointTypes.Add(checkpoint.Type.ToLower());
            }

            return checkpointTypes.Contains("departure");
        }

        /// <summary>
        /// Checks if any checkpoint of type destination has already been registered for this journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public bool DestinationExistsInJourney(string journeyId)
        {
            List<Checkpoint> journeyCheckpoints = checkpointDataAccess.GetJourneyCheckpoints(journeyId);
            List<string> checkpointTypes = new List<string>();

            foreach (Checkpoint checkpoint in journeyCheckpoints)
            {
                checkpointTypes.Add(checkpoint.Type.ToLower());
            }

            return checkpointTypes.Contains("destination");
        }

        /// <summary>
        /// Checks if type is either departure, stopover or destination
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsValidCheckpointType(string type)
        {
            string[] types = { "departure", "stopover", "destination" };

            return types.Contains(type.ToLower());
        }

        #endregion

        #region Utilities

        public string CheckJourneyValidationMessage(DateTime departureTime, int availableSeats, string carId, string driverId)
        {
            string message = "Valid";

            if (!IsValidDepartureTime(departureTime, carId))
            {
                message = "Invalid departure time";
            }
            else if (!IsValidAvailableSeats(availableSeats, carId))
            {
                message = "Available seats cannot be greater than car capacity";
            }
            else if (availableSeats <= 0)
            {
                message = "Available seats cannot be less than 1";
            }
            else if (!IsValidCar(carId, driverId))
            {
                message = "Car not found";
            }

            return message;
        }

        public string CheckUpdateInput(string journeyId, string parameter, string value)
        {
            string message = "valid";

            Journey journey = GetJourney(journeyId);
            DateTime departureTime = journey.Departure_Time;

            Car car = journey.Journey_Car;
            string driverId = car.CarOwner.User_ID;
            string carId = car.Car_ID;


            DateTime time = DateTime.Now;
            int result = 0;

            if (GetJourney(journeyId) != null)
            {
                if (IsValidParameter(parameter))
                {
                    if (parameter == "Departure_Time")
                    {
                        if (DateTime.TryParse(value, out time) == false)
                        {
                            message = "Invalid time format";
                        }
                        else if (!IsValidDepartureTime(DateTime.Parse(value), carId))
                        {
                            message = "Invalid departure time";
                        }
                    }
                    else if (parameter == "Available_Seats")
                    {
                        if (int.TryParse(value, out result) == false)
                        {
                            message = "Available seats must be an integer";
                        }
                        else if (!IsValidAvailableSeats(int.Parse(value), carId))
                        {
                            message = "Available seats cannot be greater than car capacity";
                        }
                    }
                    else if (parameter == "Status")
                    {
                        if (!IsValidStatus(value))
                        {
                            message = "Invalid status option";
                        }
                        else if ((value == "completed") && (DateTime.Now < departureTime))
                        {
                            message = "Journey cannot be completed before departure time";
                        }
                    }
                    else if ((parameter == "Car_ID_FK") && !IsValidCar(value, driverId))
                    {
                        message = "Car not found";
                    }
                }
                else
                {
                    message = "Please enter a valid parameter name (e.g Departure_Time, Available_Seats, Status, Car_ID_FK)";
                }
            }
            else
            {
                message = "Journey not found";
            }

            return message;
        }

        public string CheckpointRegistrationValidation(float price, string location, string type, string journeyId)
        {
            List<object> entries = new List<object>();
            entries.Add(price);
            entries.Add(location);
            entries.Add(type);
            entries.Add(journeyId);

            string message = "valid";

            if (validations.IsAnyNullOrEmpty(entries))
            {
                message = "Value missing, please check entries (all inputs are mandatory)";
            }
            else
            {
                if (GetJourney(journeyId) == null)
                {
                    message = "Journey not found";
                }
                else
                {
                    if (!validations.IsVarChar50(location))
                    {
                        message = "Location name should have a maximum of 50 characters";
                    }
                    else if (LocationExistsInJourney(location, journeyId))
                    {
                        message = "This location has already been added as checkpoint for this journey";
                    }
                    else if (!IsValidCheckpointType(type))
                    {
                        message = "Checkpoint type must be either 'departure', 'stopover' or 'destination'";
                    }
                    else if ((type.ToLower() == "departure") && DepartureExistsInJourney(journeyId))
                    {
                        message = "A checkpoint of type departure has already been specified for this journey";
                    }
                    else if ((type.ToLower() == "destination") && DestinationExistsInJourney(journeyId))
                    {
                        message = "A checkpoint of type destination has already been specified for this journey";
                    }
                }
            }

            return message;
        }

        #endregion
    }

    #endregion
}

