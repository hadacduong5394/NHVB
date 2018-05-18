using System;

namespace cvmksite.Helper
{
    public class DayHelper
    {
        public static DateTime GetFirstDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime GetLastDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }
    }
}