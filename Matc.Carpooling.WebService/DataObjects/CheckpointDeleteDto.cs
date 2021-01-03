using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class CheckpointDeleteDto
    {
        #region Attributes

        public string checkpointID { get; set; }
        public string journeyID { get; set; }
        public string location { get; set; }
        #endregion
    }
}