using System.Collections.Generic;

namespace cvmksite.Helper
{
    public class TypeViewReport
    {
        public static IDictionary<int, string> TYPE_MONTH_WEEK = new Dictionary<int, string>
        {
            { (int)Months.THANG_1, "Tháng 1" },
            { (int)Months.THANG_2, "Tháng 2" },
            { (int)Months.THANG_3, "Tháng 3" },
            { (int)Months.THANG_4, "Tháng 4" },
            { (int)Months.THANG_5, "Tháng 5" },
            { (int)Months.THANG_6, "Tháng 6" },
            { (int)Months.THANG_7, "Tháng 7" },
            { (int)Months.THANG_8, "Tháng 8" },
            { (int)Months.THANG_9, "Tháng 9" },
            { (int)Months.THANG_10, "Tháng 10" },
            { (int)Months.THANG_11, "Tháng 11" },
            { (int)Months.THANG_12, "Tháng 12" },
        };
    }

    public enum Months
    {
        THANG_1 = 1,
        THANG_2 = 2,
        THANG_3 = 3,
        THANG_4 = 4,
        THANG_5 = 5,
        THANG_6 = 6,
        THANG_7 = 7,
        THANG_8 = 8,
        THANG_9 = 9,
        THANG_10 = 10,
        THANG_11 = 11,
        THANG_12 = 12
    }
}