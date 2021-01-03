using Matc.Carpooling.Entities;
using Matc.Carpooling.DataAccess;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Matc.Carpooling.Business
{
    public class UserBusiness
    {
        #region Intialization
        UserDataAccess userDataAccess = new UserDataAccess();
        Validations validations = new Validations();
        #endregion

        #region Methods to Transfer to DataAccess

        /// <summary>
        /// Sends user info to DataAccess for user registration
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(string identityNumber, string firstName, string lastName, string email, string phone, string password)
        {
            if (RegistrationValidationMessage(identityNumber, firstName, lastName, email, phone, password) == "Good")
            {
                //Generates a unique identifier
                Guid userGuid = Guid.NewGuid();

                User newUser = new User(userGuid.ToString(), identityNumber, firstName, lastName, email, phone, HashPassword(password));
                return userDataAccess.AddUser(newUser);
            }

            return false;
        }

        /// <summary>
        /// Deactivates the account with the correspoding user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeactivateUserAccount(string userId)
        {
            return userDataAccess.DeactivateUserAccount(userId);
        }

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(string userId, string parameter, string value)
        {
            bool updateSuccesful = false;
            string newValue = value;

            if (CheckUpdateInput(userId, parameter, value) == "Good")
            {
                if(parameter == "Password")
                {
                    newValue = HashPassword(value);
                }
                updateSuccesful = userDataAccess.UpdateUserInfo(userId, parameter, newValue);
            }

            return updateSuccesful;
        }

        #endregion

        #region Methods to Retrieve from DataAccess

        /// <summary>
        /// Gets the list of all active users
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllActiveUsers()
        {
            return userDataAccess.GetAllActiveUsers();
        }

        /// <summary>
        /// Gets user instance with corresponding user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetActiveUser(string userId)
        {
            return userDataAccess.GetActiveUser(userId);
        }

        /// <summary>
        /// Gets user instance with corresponding email-password combination
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetActiveUser(string email, string password)
        {
            string hashPassword = HashPassword(password);

            return userDataAccess.GetActiveUser(email, hashPassword);
        }

        #endregion

        #region Methods for input validation

        public bool IsValidEmail(string email)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isEmail = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            return isEmail;
        }

        public bool IsValidPhone(string phone)
        {
            var regex = @"^\d{8}$";
            bool isPhone = Regex.IsMatch(phone, regex);
            return isPhone && phone[0] == '5';
        }

        public bool IsValidPassword(string password)
        {
            return (password.Length >= 8 && password.Length <= 50);
        }

        public bool IsEmailAvailable(string email)
        {
            //Get list of all users
            List<User> userList = GetAllActiveUsers();

            //Stores all emails from users present in the database
            List<string> userEmails = new List<string>();

            //Iterates through each user and adds the user's email to the list of userEmails
            foreach (User user in userList)
            {
                userEmails.Add(user.Email);
            }

            //Returns false if email is already taken
            return !userEmails.Contains(email);
        }

        public bool IsValidParameter(string columnName)
        {
            string[] columnNames = { "Identity_Number", "First_Name", "Last_Name", "Email", "Phone", "Password" };
            return columnNames.Contains(columnName);
        }        

        #endregion

        #region Utilities

        /// <summary>
        /// Checks all inputs for validation and returns a message
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string RegistrationValidationMessage(string identityNumber, string firstName, string lastName, string email, string phone, string password)
        {
            string message = "Good";

            if (!validations.IsVarChar50(firstName))
            {
                message = "First name password cannot be null or greater than 50 characters";
            }
            else if (!validations.IsVarChar50(lastName))
            {
                message = "Last name password cannot be null or greater than 50 characters";
            }
            else if (!IsValidPhone(phone))
            {
                message = "Please enter a valid phone number (e.g 57202456)";
            }
            else if (!IsValidEmail(email))
            {
                message = "Invalid Email Format";
            }
            else if (!IsEmailAvailable(email))
            {
                message = "Sorry this email has already been used. Please try another one...";
            }                     
            else if (!IsValidPassword(password))
            {
                message = "Invalid Password (password cannot be less than 8 or greater than 50 characters)";
            }

            return message;
        }

        /// <summary>
        /// Method to hash password
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string HashPassword(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            // Compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            // Get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Checks if info entered at login is valid
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string CheckLoginInput(string email, string password)
        {
            string validationMessage = "No user corresponding to the entered email-password combination";

            if (!IsValidEmail(email))
            {
                validationMessage = "Invalid Email Format";
            }
            else if (!IsValidPassword(password))
            {
                validationMessage = "Invalid Password (password cannot be less than 8 or greater than 50 characters)";
            }

            return validationMessage;
        }

        /// <summary>
        /// Check if info entered for update is valid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckUpdateInput(string userId, string parameter, string value)
        {
            string answer = "Good";

            if (GetActiveUser(userId) != null)
            {
                if (IsValidParameter(parameter))
                {
                    if ((parameter == "Email"))
                    {
                        if (!IsValidEmail(value))
                        {
                            answer = "Invalid Email Format";
                        }
                        else if (!IsEmailAvailable(value))
                        {
                            answer = "Sorry this email has already been used. Please try another one...";
                        }
                    }
                    else if ((parameter == "First_Name") && (!validations.IsVarChar50(value)))
                    {
                        answer = "First name cannot be null or greater than 50 characters";
                    }
                    else if ((parameter == "Last_Name") && (!validations.IsVarChar50(value)))
                    {
                        answer = "Last name cannot be null or greater than 50 characters";
                    }
                    else if ((parameter == "Phone") && (!IsValidPhone(value)))
                    {
                        answer = "Please enter a valid phone number (e.g 57202456)";
                    }
                    else if ((parameter == "Password") && (!IsValidPassword(value)))
                    {
                        answer = "Invalid Password (password should be between 8 and 50 characters)";
                    }
                }
                else
                {
                    answer = "Please enter a valid parameter name (e.g First_Name, Last_Name, Email, Phone, Password)";
                }
            }
            else
            {
                answer = "User not found!";
            }

            return answer;
        }

        #endregion
    }
}
