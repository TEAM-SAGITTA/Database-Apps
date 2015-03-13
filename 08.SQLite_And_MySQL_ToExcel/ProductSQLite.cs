using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SQLite_And_MySQL_ToExcel
{
    public class ProductSQLite
    {
        
        private int id;
        private string productName;
        private double productTax;

        public double ProductTax
        {
            get { return productTax; }
            set { productTax = value; }
        }
        

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        
    }
}
