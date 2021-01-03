using System.Collections.Generic;

namespace Matc.Carpooling.Entities
{
    public class User
    {
        #region Attributes
        public string User_ID { get; set; }
        public string Identity_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public float Rating { get; set; }
        public int Number_Of_Ratings { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Admin { get; set; }
        public List<Car> User_Cars { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public User()
        {

        }
        /// <summary>
        /// Constructor for user registration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="rating"></param>
        /// <param name="numberOfRatings"></param>
        public User(string userId, string identityNumber, string firstName, string lastName, string email, string phone, string password)
        {
            this.User_ID = userId;
            this.Identity_Number = identityNumber;
            this.First_Name = firstName;
            this.Last_Name = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Password = password;
        }

        /// <summary>
        /// Constructor to get a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name=""></param>
        public User(string userId, string identityNumber, string firstName, string lastName, string email, string phone, bool isActive, bool isAdmin, float rating, int numberOfRatings)
        {
            this.User_ID = userId;
            this.Identity_Number = identityNumber;
            this.First_Name = firstName;
            this.Last_Name = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Is_Active = isActive;
            this.Is_Admin = isAdmin;
            this.Rating = rating;
            this.Number_Of_Ratings = numberOfRatings;
        }

        #endregion
    }
}
