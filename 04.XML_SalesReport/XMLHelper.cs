using SagittaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace _04.XML_SalesReport
{
    public class XMLHelper
    {
        public void exportToXML(Dictionary<string, List<Summary>> salesByVendor, string outputFileName)
        {
            var doc = new XDocument();
            var sales = new XElement("sales");
            foreach (var sale in salesByVendor)
            {
                var summaries = sale.Value;
                var xSale = new XElement("sale");

                foreach (var summary in summaries)
                {
                    var xSummary = new XElement("summary");
                    xSummary.Add(new XAttribute("date", summary.DateOfSale.ToString("dd-MMM-yyyy")));
                    xSummary.Add(new XAttribute("total-sum", string.Format("{0:F2}", summary.TotalSum)));
                    xSale.Add(xSummary);
                }

                xSale.Add(new XAttribute("vendor", sale.Key));
                sales.Add(xSale);
            }
            doc.Add(sales);
            doc.Save(outputFileName);
        }

        public Dictionary<string, List<Summary>> ReadData(DateTime startDate, DateTime endDate)
        {
            //string outputData = string.Empty;

            using (SagittaDBEntities msSqlDB = new SagittaDBEntities())
            {
                //var dbVendors = msSqlDB.SalesReports;
                var salesResult = new Dictionary<string, List<Summary>>();
                var sales = msSqlDB.SalesReports.Select(s => new
                {
                    s.Product,
                    s.Product.Vendor,
                    s.ReportDate
                });


                foreach (var sale in sales)
                {
                    if (startDate <= sale.ReportDate && sale.ReportDate <= endDate)
                    {
                        var vendorName = sale.Product.Vendor.Vendor_Name;
                        if (!salesResult.ContainsKey(vendorName))
                        {
                            salesResult[vendorName] = new List<Summary>();
                        }
                        var summary = new Summary(sale.ReportDate, sale.Product.Price);
                        var summariesWithEqualDates = salesResult[vendorName].Where(x => x.DateOfSale == summary.DateOfSale);

                        if (summariesWithEqualDates.Count() == 0)
                        {
                            salesResult[vendorName].Add(summary);
                        }
                        else
                        {
                            summariesWithEqualDates.FirstOrDefault().TotalSum += summary.TotalSum;
                        }
                    }
                }

                return salesResult;

            }

        }
    }
}
