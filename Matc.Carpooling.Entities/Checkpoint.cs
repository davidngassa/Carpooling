using System.Collections.Generic;

namespace Matc.Carpooling.Entities
{
    public class Checkpoint
    {
        #region Attributes
        public string Checkpoint_ID { get; set; }
        public float Price { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public Journey Checkpoint_Journey { get; set; }
        #endregion

        #region Constructors
        //Default Constructor
        public Checkpoint()
        {
        }

        public Checkpoint(string checkpoint_ID, float price, string location, string type, Journey checkpointJourney)
        {
            this.Checkpoint_ID = checkpoint_ID;
            this.Location = location;
            this.Type = type;
            this.Checkpoint_Journey = checkpointJourney;

            //Set price to zero if checkpoint is of type departure
            if (type.ToLower() == "departure")
            {
                this.Price = 0;
            }
            else
            {
                this.Price = price;
            }                            
        }



        #endregion

        #region compare methods

        public override bool Equals(object obj)
        {
            return obj is Checkpoint checkpoint &&
                   Checkpoint_ID == checkpoint.Checkpoint_ID &&
                   Price == checkpoint.Price &&
                   Location == checkpoint.Location &&
                   Type == checkpoint.Type &&
                   Checkpoint_Journey == checkpoint.Checkpoint_Journey;
        }

        public override int GetHashCode()
        {
            var hashCode = -316351474;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Checkpoint_ID);
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Checkpoint_Journey.Journey_ID);
            return hashCode;
        }

        #endregion
    }
}
