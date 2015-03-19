namespace _02.ZipReports_To_MSSQL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;
    using ICSharpCode.SharpZipLib.Core;
    using ICSharpCode.SharpZipLib.Zip;
   
    using _02.ZipReports_To_MSSQL.Properties;
    using SagittaDB.Models;

    public class DataMigrator
    {
        private const string TempFolderForExtract = @"../../Temp";
        private const string ReportsFilePath = "../../../InputFiles/Sample-Sales-Reports.zip";

        public void MigrateDataFromExcelFiles()
        {
            ExtractZipFile(ReportsFilePath);

            using (var supermarketContext = new SagittaDBEntities())
            {
                IList<SalesReport> allSales = new List<SalesReport>();

                GetSales(TempFolderForExtract, supermarketContext, allSales);

                foreach (var sale in allSales)
                {
                    supermarketContext.SalesReports.Add(sale);
                }

                supermarketContext.SaveChanges();
            }

            Directory.Delete(TempFolderForExtract, true);
        }

        private void ExtractZipFile(string filepath)
        {
            FileStream fileReadStream = File.OpenRead(filepath);
            ZipFile zipFile = new ZipFile(fileReadStream);
            using (zipFile)
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    if (zipEntry.IsFile)
                    {
                        String entryFileName = zipEntry.Name;

                        byte[] buffer = new byte[4096];
                        Stream zipStream = zipFile.GetInputStream(zipEntry);

                        string fullZipToPath = Path.Combine(TempFolderForExtract, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
            }
        }

        private void GetSales(string directory,
           SagittaDBEntities supermarketContext, IList<SalesReport> allSales)
        {
            foreach (var sale in GetSalesFromExcelFiles(directory, supermarketContext))
            {
                allSales.Add(sale);
            }

            string[] subDirectories = Directory.GetDirectories(directory);
            if (subDirectories.Length > 0)
            {
                for (int i = 0; i < subDirectories.Length; i++)
                {
                    GetSales(subDirectories[i], supermarketContext, allSales);
                }
            }
        }

        private ICollection<SalesReport> GetSalesFromExcelFiles(string directory,
            SagittaDBEntities supermarketContext)
        {
            IList<SalesReport> sales = new List<SalesReport>();

            string[] excelFilesPaths = Directory.GetFiles(directory, "*.xls");

            foreach (var excelFilePath in excelFilesPaths)
            {
                string excelConnectionString = string.Format(Settings.Default.ExcelReadConnectionString, excelFilePath);

                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();
                DataSet dataSet = new DataSet();

                using (excelConnection)
                {
                    string selectAllRowsCommandString = "SELECT * FROM [Sales$]";
                    OleDbCommand selectAllRowsCommand = new OleDbCommand(selectAllRowsCommandString, excelConnection);

                    OleDbDataAdapter excelAdapter = new OleDbDataAdapter(selectAllRowsCommand);
                    excelAdapter.Fill(dataSet, "SalesReports");
                }

                DataRowCollection excelRows = dataSet.Tables["SalesReports"].Rows;

                string supermarketName = excelRows[0][0].ToString();
                if (!supermarketContext.Supermarkets.Any(s => s.Name == supermarketName))
                {
                    supermarketContext.Supermarkets.Add(new Supermarket()
                    {
                        Name = supermarketName
                    });

                    supermarketContext.SaveChanges();
                }

                for (int i = 2; i < excelRows.Count - 1; i++)
                {

                    string productName = excelRows[i][0].ToString();

                    var product = supermarketContext.Products.FirstOrDefault(p => p.Product_Name == productName);

                    if (product != null)
                    {
                        int productID = product.ID;

                        int quantity = 0;
                        int.TryParse(excelRows[i][1].ToString(), out quantity);

                        decimal unitPrice = 0;
                        decimal.TryParse(excelRows[i][2].ToString(), out unitPrice);

                        int supermarketID =
                            supermarketContext.Supermarkets.First(s => s.Name == supermarketName).Id;
                        string saleDateString = Path.GetFileName(Path.GetDirectoryName(excelFilePath));

                        sales.Add(new SalesReport()
                        {
                            ReportDate = DateTime.Parse(saleDateString),
                            Product = supermarketContext.Products.Find(productID),
                            Quantity = quantity,
                            Supermarket = supermarketContext.Supermarkets.Find(supermarketID),
                            ActualPrice = unitPrice
                        });
                    }
                    else
                    {
                        //supermarketContext.Products.Add(new Product()
                        //{
                        //    Product_Name = productName,
                        //    VendorID = 1,
                        //    Measure = new Measure(),
                        //    Price = 1
                        //});
                    }
                }
            }

            return sales;
        }
    }
}