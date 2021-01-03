namespace Matc.Carpooling.Entities
{
    public class Car
    {
        #region Attributes
        public string Car_ID { get; set; }
        public string Number_Plate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Number_Of_Seats { get; set; }
        public User CarOwner { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Car()
        {

        }

        public Car(string carId, string numberPlate, string make, string model, int year, int numberOfSeats, User carOwner)
        {
            this.Car_ID = carId;
            this.Number_Plate = numberPlate;
            this.Make = make;
            this.Model = model;
            this.Year = year;
            this.Number_Of_Seats = numberOfSeats;
            this.CarOwner = carOwner;
        }
        #endregion
    }
}
