namespace Task6
{
    using SagittaDB.Models;
    using System;
    using System.Linq;
    using System.Xml;
    public class XmlToSqlServerLoader
    {
        public static void PolulateSqlTables(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList xmlVendors = root.SelectNodes("/expenses-by-month/vendor");

            using (var context = new SagittaDBEntities())
            {
                var dbVendors = context.Vendors;
               
                foreach (XmlNode vendor in xmlVendors)
                {
                    var vendorName = vendor.Attributes["name"].Value;
                    XmlNodeList expenses = vendor.SelectNodes("expenses");
                    for (int i = 0; i < expenses.Count; i++)
                    {
                        var expense = expenses[i];
                        var expenceMonth = expense.Attributes["month"].Value;
                        var expenceDate = DateTime.Parse(expenceMonth);
                        var expenceSum = decimal.Parse(expenses[i].InnerText);
                        var vendorId = dbVendors
                            .Where(v => v.Vendor_Name == vendorName)
                            .Select(v => v.ID).FirstOrDefault();
                        var expenseEntity = new ExpensesByMonth()
                        {
                            ExpenseMonth = expenceDate,
                            Expenses = expenceSum,
                            VendorId = vendorId
                        };

                        context.ExpensesByMonths.Add(expenseEntity);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
