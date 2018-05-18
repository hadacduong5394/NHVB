using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models
{
    public class ChangeProductQuantityOrderModelDetail
    {
        public int OrderId { get; set; }
        public int DetailId { get; set; }
        public int Quantity { get; set; }
    }
}