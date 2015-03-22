using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _04.XML_SalesReport
{
    public class Summary
    {
        public Summary(DateTime initDate, decimal initTotalSum)
        {
            this.dateOfSale = initDate;
            this.TotalSum = initTotalSum;
        }
        private DateTime dateOfSale;
        private decimal totalSum;

        public decimal TotalSum
        {
            get { return totalSum; }
            set { totalSum = value; }
        }
        
        public DateTime DateOfSale
        {
            get { return dateOfSale; }
            set { dateOfSale = value; }
        }
        
    }
}
