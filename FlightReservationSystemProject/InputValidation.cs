using System.Globalization;
using System.Text.RegularExpressions;

namespace FlightReservationSystemProject
{
    public static class InputValidation
    {
        // Regex patterns
        /// <summary>
        ///  Name should only consist of characters 
        /// </summary>
        private static readonly string NamePattern = @"^[a-zA-Z\s]+$";

        /// <summary>
        /// phone can only hold numbers of 9 digits 
        /// Eg. (333)333-3333
        ///      3334445555
        ///      333-444-5555
        /// </summary>
        private static readonly string PhonePattern = @"^(\(\d{3}\)|\d{3})[-\s]?\d{3}[-\s]?\d{4}$";

        
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, NamePattern);
        }
       
        public static bool IsValidPhoneNumber(string phone)
        {
            return !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, PhonePattern);
        }

        public static string CapitalizeEachWord(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

    }
}
