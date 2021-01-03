using System;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class JourneyRegistrationDto
    {
        #region Attributes
        public DateTime Departure_Time { get; set; }
        public int Available_Seats { get; set; }
        public string Car_ID { get; set; }
        public string Driver_ID { get; set; }
        #endregion
    }
}