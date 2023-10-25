using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Hardy.Classes
{
    internal class TimestampUtility
    {
        public static int toTimestamp(DateTime value)
        {
            if (value == null)
                return 0;

            return (int)ToDateTimeOffset(value).ToUnixTimeSeconds();
        }
        private static DateTimeOffset ToDateTimeOffset(DateTime dateTime)
        {
            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                       ? DateTimeOffset.MinValue
                       : new DateTimeOffset(dateTime);
        }

        public static DateTime toDateTime(int unixTimeStamp)
        {
            if (unixTimeStamp == 0)
                return new DateTime();

            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).LocalDateTime;
            return dateTime;
        }
    }
}
