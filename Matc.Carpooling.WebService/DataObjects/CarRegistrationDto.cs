namespace Matc.Carpooling.WebService.DataObjects
{
    public class CarRegistrationDto
    {
        #region Attributes        
        public string Number_Plate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Number_Of_Seats { get; set; }
        public string Owner_ID { get; set; }
        #endregion
    }
}