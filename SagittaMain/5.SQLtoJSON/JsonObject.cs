namespace SagittaMain._5.SQLtoJSON
{
    class JsonObject
    {
        private int product_id;
        private string product_name;
        private string vendor_name;
        private int total_quantity_sold;
        private decimal total_incomes;

        public JsonObject()
        {
        }

        public JsonObject(int productId, string productName, string vendorName, int totalQuantitySold, decimal totalIncomes)
        {
            this.product_id = productId;
            this.product_name = productName;
            this.vendor_name = vendorName;
            this.total_quantity_sold = totalQuantitySold;
            this.total_incomes = totalIncomes;
        }

        public int ProductId
        {
            get { return this.product_id; }
            set { this.product_id = value; }
        }

        public string ProductName
        {
            get { return this.product_name; }
            set { this.product_name = value; }
        }

        public string VendorName
        {
            get { return this.vendor_name; }
            set { this.vendor_name = value; }
        }

        public int TotalQuantitySold
        {
            get { return this.total_quantity_sold; }
            set { this.total_quantity_sold = value; }
        }

        public decimal TotalIncomes
        {
            get { return this.total_incomes; }
            set { this.total_incomes = value; }
        }
    }
}
