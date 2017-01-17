using System;

namespace DocumentsExchange.Common.Extensions
{
    public static class DateTimeExtensions
    {
        // 
        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// Converts a DateTime into a (JavaScript parsable) Int64.
        /// </summary>
        /// <param name="from">The DateTime to convert from</param>
        /// <returns>An integer value representing the number of seconds since 1 January 1970 00:00:00 UTC.</returns>
        public static long Convert(this DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalSeconds * 1000);
        }

        /// <summary>
        /// Converts a (JavaScript parsable) Int64 into a DateTime.
        /// </summary>
        /// <param name="from">An integer value representing the number of seconds since 1 January 1970 00:00:00 UTC.</param>
        /// <returns>The date as a DateTime</returns>
        public static DateTime Convert(this long from)
        {
            return _jan1st1970.AddSeconds(from / 1000);
        }
    }
}
