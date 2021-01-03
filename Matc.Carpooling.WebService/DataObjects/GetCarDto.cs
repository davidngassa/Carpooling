using Matc.Carpooling.Entities;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class GetCarDto
    {
        #region Attributes
        public string Car_ID { get; set; }
        public string Number_Plate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Number_Of_Seats { get; set; }
        public string Owner_ID { get; set; }
        #endregion

        #region Constructor
        public GetCarDto(Car car)
        {
            this.Car_ID = car.Car_ID;
            this.Number_Plate = car.Number_Plate;
            this.Make = car.Make;
            this.Model = car.Model;
            this.Year = car.Year;
            this.Number_Of_Seats = car.Number_Of_Seats;
            this.Owner_ID = car.CarOwner.User_ID;
        }
        #endregion
    }
}