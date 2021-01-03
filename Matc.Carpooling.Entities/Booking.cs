namespace Matc.Carpooling.Entities
{
    public class Booking
    {
        #region Attributes
        public string Booking_ID { get; set; }
        public string Status { get; set; }
        public Route Booking_Route { get; set; }
        public User Booking_User { get; set; }
        #endregion

        #region Constructors
        //Default constructor
        public Booking()
        {

        }

        public Booking(string bookingId, string status, Route bookingRoute, User bookingUser)
        {
            this.Booking_ID = bookingId;
            this.Status = status;
            this.Booking_Route = bookingRoute;
            this.Booking_User = bookingUser;
        }
        #endregion
    }
}
