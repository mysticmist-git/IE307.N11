using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime firstDayOfMonth(this DateTime dt)
        {
            var firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);

            return firstDayOfMonth;
        }

        public static DateTime lastDayOfMonth(this DateTime dt)
        {
            var firstDay = firstDayOfMonth(dt);
            var lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);

            return lastDayOfMonth;
        }
    }
}
