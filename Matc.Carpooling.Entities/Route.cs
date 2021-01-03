namespace Matc.Carpooling.Entities
{
    public class Route
    {
        #region Attributes
        public string Route_ID { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public float Price { get; set; }
        public int Available_Seats { get; set; }
        public Journey Route_Journey { get; set; }

        #endregion

        #region Constructors
        public Route(string route_ID, string departure, string destination, float price, int available_Seats, Journey journey)
        {
            this.Route_ID = route_ID;
            this.Departure = departure;
            this.Destination = destination;
            this.Price = price;
            this.Available_Seats = available_Seats;
            this.Route_Journey = journey;
        }
        public Route()
        {
        }
        #endregion
    }
}
