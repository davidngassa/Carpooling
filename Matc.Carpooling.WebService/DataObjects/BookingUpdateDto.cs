using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class BookingUpdateDto
    {
        #region Attributes
        public string BookingID { get; set; }
        public string Status { get; set; }
        #endregion
    }
}