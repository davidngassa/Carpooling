using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;

namespace Matc.Carpooling.WebService.Controllers
{
    [Authorize]
    [Route("api/Users")]
    public class UsersController : ApiController
    {
        #region Initialization
        UserBusiness userBusiness = new UserBusiness();
        Validations validations = new Validations();
        #endregion

        /// <summary>
        /// Check if user has permissions
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool isUserIdEqualToLoggedInUser(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                throw new System.ArgumentException("message", nameof(userID));
            }

            string loggedUserId = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            if (loggedUserId.Equals(userID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Gets a list of all active users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Users/GetAllActiveUsers")]
        public HttpResponseMessage GetAllActiveUsers()
        {
            List<User> activeUsers = userBusiness.GetAllActiveUsers();

            if (activeUsers.Count == 0)
            {
                var message = string.Format("No user entry in the database");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, activeUsers);
            }
        }

        /// <summary>
        /// Gets an active user with the corresponding ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Users/GetUser")]
        public HttpResponseMessage GetActiveUser(string userId)
        {
            if(userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Please enter a user ID"));
            }

            User user = userBusiness.GetActiveUser(userId);

            if (user == null)
            {
                var message = string.Format("User not found");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
        }

        /// <summary>
        /// Method for user login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Users/LoginUser")]
        public HttpResponseMessage LoginUser(UserLoginInputDto userLoginInputDto)
        {
            //Checks if object has any null values
            if(validations.IsNullOrEmpty(userLoginInputDto) == true)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }

            User user = userBusiness.GetActiveUser(userLoginInputDto.email, userLoginInputDto.password);

            if (!isUserIdEqualToLoggedInUser(user.User_ID))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {               
                if (user == null)
                {
                    var message = string.Format(userBusiness.CheckLoginInput(userLoginInputDto.email, userLoginInputDto.password));
                    HttpError error = new HttpError(message);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, error);
                }
                else
                {
                    UserLoginDto userLoginDto = new UserLoginDto(user);
                    return Request.CreateResponse(HttpStatusCode.OK, userLoginDto);
                }
            }
                
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        /// <param name="userUpdateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Users/UpdateUser")]
        public HttpResponseMessage UpdateUserInfo(UserUpdateDto userUpdateDto)
        {
            if(validations.IsNullOrEmpty(userUpdateDto))
            {
                var message = string.Format("Some values are missing");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            string userId = userUpdateDto.User_ID;
            string parameter = userUpdateDto.Parameter;
            string value = userUpdateDto.Value;

            if (!isUserIdEqualToLoggedInUser(userId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }

            if (userBusiness.UpdateUserInfo(userId, parameter, value) == false)
            {
                var message = string.Format(userBusiness.CheckUpdateInput(userId, parameter, value));
                HttpError error = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
            else
            {
                var message = string.Format($"{parameter} updated successfully");
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
                        
        }

        /// <summary>
        /// Deactivates user account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Users/DeactivateAccount")]
        public HttpResponseMessage DeactivateUserAccount(string userId)
        {
            if(userId == null)
            {
                Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Please enter a user ID"));
            }
            if (!isUserIdEqualToLoggedInUser(userId))
            {
                var message = string.Format("Permission denied");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }
            else
            {
                if (userBusiness.DeactivateUserAccount(userId) == false)
                {
                    var message = string.Format("User not found");
                    HttpError error = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, error);
                }
                else
                {
                    var message = string.Format("Account was successfully deactivated");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }            
        }
    }
}