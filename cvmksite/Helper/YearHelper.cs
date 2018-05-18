using System;
using System.Collections.Generic;

namespace cvmksite.Helper
{
    public class YearHelper
    {
        public static List<int> GetYears()
        {
            List<int> nams = new List<int>();
            int now = DateTime.Now.Year;
            for (int i = now; i >= now - 10; i--)
                nams.Add(i);
            return nams;
        }
    }
}