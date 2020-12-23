using System;

namespace DV.FrenteLoja.Core.Util
{
    public static class ProtheusConversions
    {
        public static DateTime ProtheusDate2DotNetDate(string value)
        {
            return DateTime.ParseExact(value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        }
        public static DateTime? ProtheusDate2DotNetDateHour(string datevalue, string hourValue)
        {
            if (String.IsNullOrEmpty(datevalue))
            {
                return null;
            }
            else if (String.IsNullOrEmpty(hourValue))
            {
                return DateTime.ParseExact($"{datevalue} 00:00:00", "yyyyMMdd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            }
            else
            {
                return DateTime.ParseExact($"{datevalue} {hourValue}:00", "yyyyMMdd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            }
        }
    }
}