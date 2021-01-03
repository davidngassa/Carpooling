using System.Reflection;
using System.Collections.Generic;

namespace Matc.Carpooling.Business
{
    public class Validations
    {
        #region General

        /// <summary>
        /// Checks if string is between 1 and 50 characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsVarChar50(string text)
        {
            return (text.Length > 0 && text.Length <= 50);
        }

        /// <summary>
        /// Checks if any entries are null or empty
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public bool IsAnyNullOrEmpty(List<object> entries)
        {
            foreach (object entry in entries)
            {
                if (entry == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if object is null or has any empty values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsNullOrEmpty(object obj)
        {
            bool isNullOrEmpty = false;

            if (obj == null)
            {
                isNullOrEmpty = true;
            }
            else
            {
                var properties = obj.GetType().GetProperties();

                foreach (var p in properties)
                {
                    if (p.Name == null || p.GetValue(obj) == null)
                    {
                        isNullOrEmpty = true;
                    }
                }
            }

            return isNullOrEmpty;
        }

        #endregion       
    }
}
