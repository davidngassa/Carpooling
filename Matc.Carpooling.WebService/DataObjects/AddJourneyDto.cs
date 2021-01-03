using Matc.Carpooling.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class AddJourneyDto
    {
        #region Attributes
        public DateTime Departure_Time { get; set; }
        public int Available_Seats { get; set; }
        public string Status { get; set; }
        public Car Journey_Driver { get; set; }
        public List<Checkpoint> Journey_Checkpoints { get; set; }
        #endregion
    }
}