using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;

namespace Matc.Carpooling.WebService.Services
{
    public class UserService
    {
        UserBusiness userBusiness = new UserBusiness();

        /// <summary>
        /// Method to validate a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User ValidateUser(string email, string password)
        {
            User user = userBusiness.GetActiveUser(email, password);

            if (user != null)
            {                
                user.Password = null;
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}