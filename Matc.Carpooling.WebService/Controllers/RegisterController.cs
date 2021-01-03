using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Matc.Carpooling.WebService.Controllers
{
    public class RegisterController : ApiController
    {
        UserBusiness userBusiness = new UserBusiness();
        Validations validations = new Validations();

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>        
        [HttpPost]
        [Route("api/Register")]
        public HttpResponseMessage AddUser(UserRegistrationDto newUser)
        {
            if(validations.IsNullOrEmpty(newUser))
            {
                var message = string.Format("Some values are missing");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            string identityNumber = newUser.Identity_Number;
            string firstName = newUser.First_Name;
            string lastName = newUser.Last_Name;
            string email = newUser.Email;
            string phone = newUser.Phone;
            string password = newUser.Password;

            bool requestSuccessful = userBusiness.AddUser(identityNumber, firstName, lastName, email, phone, password);

            if (requestSuccessful == false)
            {
                var message = userBusiness.RegistrationValidationMessage(identityNumber, firstName, lastName, email, phone, password);
                HttpError error = new HttpError(string.Format(message));
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
            else
            {
                var success = string.Format("User was registered successfully");
                return Request.CreateResponse(HttpStatusCode.OK, success);
            }
        }

    }
}
