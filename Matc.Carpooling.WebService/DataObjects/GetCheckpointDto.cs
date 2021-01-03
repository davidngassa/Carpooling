using Matc.Carpooling.Entities;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class GetCheckpointDto
    {
        #region Attributes
        public string Checkpoint_ID { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public float Price { get; set; }
        public string Journey_ID { get; set; }
        #endregion

        #region Constructor
        public GetCheckpointDto(Checkpoint checkpoint)
        {
            this.Checkpoint_ID = checkpoint.Checkpoint_ID;
            this.Type = checkpoint.Type;
            this.Location = checkpoint.Location;
            this.Price = checkpoint.Price;
            this.Journey_ID = checkpoint.Checkpoint_Journey.Journey_ID;
        }
        #endregion
    }
}