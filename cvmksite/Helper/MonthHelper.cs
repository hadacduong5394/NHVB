using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Helper
{

    public class MonthHelper
    {
        public static IList<int> GetMonths()
        {
            var rs = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                rs.Add(i);
            }
            return rs;
        }

        public static IList<int> GetMonthLimit(int limitMonth)
        {
            if (limitMonth > 12 || limitMonth < 1)
            {
                limitMonth = 12;
            }
            var rs = new List<int>();
            for (int i = 1; i <= limitMonth; i++)
            {
                rs.Add(i);
            }
            return rs;
        }
    }
}