using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace _04.XML_SalesReport
{
    class XMLHelper
    {
        public void exportToXML()
        {
            
            XmlDocument doc = new XmlDocument();
            var sale = doc.CreateElement("sale");
            var summary = doc.CreateElement("summary");
            summary.InnerText = "date=\"20-Jul-2014\" total-sum=\"54.75\"";
            summary.InnerText = "date=\"21-Jul-2014\" total-sum=\"40.35\"";
            summary.InnerText = "date=\"21-Jul-2014\" total-sum=\"40.35\"";
            sale.AppendChild(summary);
            doc.DocumentElement.AppendChild(sale);
            doc.Save("SalersReport.xml");
        }
        
    }
}
