using Matc.Carpooling.Entities;

namespace Matc.Carpooling.WebService.DataObjects
{
    public class UserLoginDto
    {
        #region Attributes
        public string User_ID { get; set; }
        public string Identity_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public float Rating { get; set; }
        public int Number_Of_Ratings { get; set; }
        #endregion

        #region Constructor
        public UserLoginDto(User user)
        {
            this.User_ID = user.User_ID;
            this.Identity_Number = user.Identity_Number;
            this.First_Name = user.First_Name;
            this.Last_Name = user.Last_Name;
            this.Email = user.Email;
            this.Phone = user.Phone;
            this.Rating = user.Rating;
            this.Number_Of_Ratings = user.Number_Of_Ratings;
        }
        #endregion
    }
}