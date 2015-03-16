using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _04.XML_SalesReport
{
    public class SalesReport
    {
        private string vendorName;
        private DateTime dateReported;
        private decimal totalSum;

        public decimal TotalSum
        {
            get { return totalSum; }
            set { totalSum = value; }
        }
        
        public DateTime DateReported
        {
            get { return dateReported; }
            set { dateReported = value; }
        }
        
        public string VendorName
        {
            get { return vendorName; }
            set { vendorName = value; }
        }
        
    }
}
