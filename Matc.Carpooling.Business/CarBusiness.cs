using System;
using System.Collections.Generic;
using System.Linq;
using Matc.Carpooling.Entities;
using Matc.Carpooling.DataAccess;

namespace Matc.Carpooling.Business
{
    public class CarBusiness
    {
        #region Initialization
        CarDataAccess carDataAccess = new CarDataAccess();
        UserBusiness userBusiness = new UserBusiness();
        #endregion

        #region Methods

        /// <summary>
        /// Method to add Car to Database from user input
        /// </summary>
        /// <param name="newCar"></param>
        /// <returns></returns>
        public bool AddCar(string numberPlate, string make, string model, int year, int numberOfSeats, string ownerId)
        {
            if (CarRegistrationValidationMessage(numberPlate, make, model, year, numberOfSeats, ownerId).Equals("valid"))
            {
                Guid carGuid = Guid.NewGuid();
                User carOwner = userBusiness.GetActiveUser(ownerId);

                Car newCar = new Car(carGuid.ToString(), numberPlate, make, model, year, numberOfSeats, carOwner);

                return carDataAccess.SetCar(newCar);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to get list of car instances for a user using User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Car> GetAllUserCars(string userId)
        {
            if (userBusiness.GetActiveUser(userId) != null)
            {
                return carDataAccess.GetListOfCarInstances(userId);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Get list of All registered cars 
        /// </summary>
        /// <returns></returns>
        public List<Car> GetAllCars()
        {
            return carDataAccess.GetAllCars();
        }

        /// <summary>
        /// Get car corresponding to ID
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public Car GetCar(string carId)
        {
            return carDataAccess.GetCarInstance(carId);
        }

        /// <summary>
        /// Update information about car
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateCarInfo(string carId, string parameter, string value)
        {
            bool updateSuccesful = false;

            if (CheckUpdateInput(carId, parameter, value).Equals("valid"))
            {
                updateSuccesful = carDataAccess.UpdateCarInfo(carId, parameter, value);
            }

            return updateSuccesful;
        }

        /// <summary>
        /// Method to delete a user's car using Car Id
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool DeleteCar(string carId)
        {
            return carDataAccess.DeleteCar(carId);
        }

        /// <summary>
        /// Method to verify car ID of user
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool CarIdExists(string carId)
        {
            return carDataAccess.CarIdExists(carId);
        }

        #endregion

        #region Validations

        /// <summary>
        /// Method to verify car year of user's car
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsCarYear(int year)
        {
            if (year <= DateTime.Now.Year && year >= 1900)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verify number plate validity
        /// </summary>
        /// <param name="numberPlate"></param>
        /// <returns></returns>
        public bool IsValidNumberPlate(string numberPlate)
        {          
            return (numberPlate.Length > 0 && numberPlate.Length < 11);
        }

        /// <summary>
        /// Checks number of seats
        /// </summary>
        /// <param name="numberOfSeats"></param>
        /// <returns></returns>
        public bool IsValidNumberOfSeats(int numberOfSeats)
        {
            return (numberOfSeats > 0 && numberOfSeats <= 15);
        }

        public bool IsValidParameter(string columnName)
        {
            string[] columnNames = { "Number_Plate", "Make", "Model", "Year", "Number_Of_Seats" };
            return columnNames.Contains(columnName);
        }

        public bool IsVarChar50(string text)
        {
            return (text.Length > 0 && text.Length <= 50);
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Tests information entered during car registration
        /// </summary>
        /// <param name="numberPlate"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="year"></param>
        /// <param name="numberOfSeats"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string CarRegistrationValidationMessage(string numberPlate, string make, string model, int year, int numberOfSeats, string ownerId)
        {
            string message = "valid";

            if (!IsValidNumberPlate(numberPlate))
            {
                message = "Please enter a valid number plate";
            }
            else if (NumplateAlreadyExists(numberPlate))
            {
                message = "Number plate must be unique";
            }
            else if (!IsVarChar50(make))
            {
                message = "Invalid input for make (50char max)";
            }
            else if (!IsVarChar50(model))
            {
                message = "Invalid input for model (50char max)";
            }
            else if (!IsCarYear(year))
            {
                message = "Please enter a valid year car";
            }
            else if (!IsValidNumberOfSeats(numberOfSeats))
            {
                message = "Number of seats must be in range (15char max)";
            }
            else if (userBusiness.GetActiveUser(ownerId) == null)
            {
                message = "User not found";
            }

            return message;
        }

        /// <summary>
        /// Check if info entered for update is valid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckUpdateInput(string carId, string parameter, string value)
        {
            int parseOut = 0;
            string answer = "valid";

            if (CarIdExists(carId))
            {
                if (IsValidParameter(parameter))
                {
                    if ((parameter == "Number_Plate") && !IsValidNumberPlate(value))
                    {
                        answer = "Please enter a valid number plate";
                    }
                    else if ((parameter == "Number_Plate") && NumplateAlreadyExists(value))
                    {
                        answer = "Number plate must be unique";
                    }
                    else if ((parameter == "Make") && (!IsVarChar50(value)))
                    {
                        answer = "Invalid input for Make (50char max)";
                    }
                    else if ((parameter == "Model") && (!IsVarChar50(value)))
                    {
                        answer = "Invalid input for Model (50char max)";
                    }
                    else if ((parameter == "Year"))
                    {
                        if (int.TryParse(value, out parseOut) == false)
                        {
                            answer = "Invalid input for Year";
                        }
                        else if (!IsCarYear(int.Parse(value)))
                        {
                            answer = "Invalid input for Year";
                        }
                    }
                    else if ((parameter == "Number_Of_Seats"))
                    {
                        if (int.TryParse(value, out parseOut) == false)
                        {
                            answer = "Invalid input for number of seats";
                        }
                        else if (!IsValidNumberOfSeats(int.Parse(value)))
                        {
                            answer = "Invalid input for number of seats";
                        }
                    }
                }
                else
                {
                    answer = "Please enter a valid parameter name (e.g Number_Plate, Make, Model, Year, Number_Of_Seats)";
                }
            }
            else
            {
                answer = "Car not found!";
            }

            return answer;
        }

        /// <summary>
        /// Checks if the number plate exists already in the database
        /// </summary>
        /// <param name="numberPlate"></param>
        /// <returns></returns>
        public bool NumplateAlreadyExists(string newNumberPlate)
        {
            List<String> numberPlates = new List<string>();
            List<Car> listOfCars = GetAllCars();

            foreach (Car car in listOfCars)
            {
                numberPlates.Add(car.Number_Plate.ToUpper());
            }

            return numberPlates.Contains(newNumberPlate.ToUpper());
        }

        #endregion

    }
}
