namespace Matc.Carpooling.WebService.DataObjects
{
    public class UserRegistrationDto
    {
        #region Attributes
        public string Identity_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        #endregion
    }
}