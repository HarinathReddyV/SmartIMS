using System.Globalization;
using SmartIMS.ClientLib.Constants;

namespace SmartIMS.ClientLib.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullString(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullString(this string value)
        {
            if (IsNullString(value))
                return false;
            else
                return true;
        }

        public static decimal ConvertToDecimal(this string value)
        {
            if (IsNotNullString(value))
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            else
                return 0;
        }

        public static decimal ConvertTimein100(this string value)
        {
            string strVal = value.Substring(value.IndexOf(CharConstants.TimeData_delimited) + 1);
            return value.Substring(0, value.IndexOf(CharConstants.TimeData_delimited)).ConvertToDecimal() + (strVal.ConvertToDecimal() / 60);
        }


        public static bool IsValidTime(this string value)
        {
            if (value.IsNullString())
                return false;
            if (!value.Contains(CharConstants.TimeData_delimited.ToString()))
                return false;
            return true;
        }

        public static bool IsNotValidTime(this string value)
        {
            return !value.IsValidTime();
        }

    }
}
