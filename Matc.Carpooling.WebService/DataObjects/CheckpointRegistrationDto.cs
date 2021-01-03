namespace Matc.Carpooling.WebService.Controllers
{
    public class CheckpointRegistrationDto
    {
        #region Attributes        
        public string Location { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public string JourneyId { get; set; }
        #endregion
    }
}