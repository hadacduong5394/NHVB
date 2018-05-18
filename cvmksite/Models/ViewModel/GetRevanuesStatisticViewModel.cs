using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class GetRevanuesStatisticViewModel
    {
        public DateTime Date { get; set; }
        public decimal Revanues { get; set; }
        public decimal Benefit { get; set; }
    }
}