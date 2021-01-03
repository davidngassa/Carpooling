namespace Matc.Carpooling.WebService.DataObjects
{
    public class UpdateCheckpointDto
    {
        #region Attributes
        public string Checkpoint_ID { get; set; }
        public string Journey_ID { get; set; }
        public float Price { get; set; }
        #endregion
    }
}