using System;
using System.Collections.Generic;

namespace Matc.Carpooling.Entities
{
    public class Journey
    {
        #region Attributes
        public string Journey_ID { get; set; }
        public DateTime Departure_Time { get; set; }
        public int Available_Seats { get; set; }
        public string Status { get; set; }
        public Car Journey_Car { get; set; }
        public List<Checkpoint> Journey_Checkpoints { get; set; }
        #endregion

        #region Constructors
        //Default constructor
        public Journey()
        {

        }

        public Journey(string journeyId, DateTime departureTime, int availableSeats, string status, Car journeyCar, List<Checkpoint> journeyCheckpoints)
        {
            this.Journey_ID = journeyId;
            this.Departure_Time = departureTime;
            this.Available_Seats = availableSeats;
            this.Status = status;
            this.Journey_Car = journeyCar;
            this.Journey_Checkpoints = journeyCheckpoints;
        }

        /// <summary>
        /// Contructor to add journey a new journey in the database
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="departureTime"></param>
        /// <param name="availableSeats"></param>
        /// <param name="status"></param>
        /// <param name="journeyCar"></param>
        /// <param name="journeyCheckpoints"></param>
        public Journey(string journeyId, DateTime departureTime, int availableSeats, string status, Car journeyCar)
        {
            this.Journey_ID = journeyId;
            this.Departure_Time = departureTime;
            this.Available_Seats = availableSeats;
            this.Status = status;
            this.Journey_Car = journeyCar;
        }
        #endregion
    }
}
