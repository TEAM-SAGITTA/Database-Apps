using System.Collections.Generic;
using Newtonsoft.Json;

namespace Task5
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using SagittaDB.Models;

    public static class Utility
    {
        private static readonly SagittaDBEntities Db = new SagittaDBEntities();
        public static readonly DateTime CalendarStartDate = GetFirstReportDate();
        public static readonly DateTime CalendarEndDate = DateTime.Now;
        public static DateTime ReportStartDate = CalendarStartDate;
        public static DateTime ReportEndDate = DateTime.Now;
        //        private static readonly BsonArray Reports = new BsonArray();
        private static readonly List<Report> ReportList = new List<Report>();


        public static DateTime GetFirstReportDate()
        {
            return Db.SalesReports.Select(p => p.ReportDate).Min();
        }

        public static bool SaveReportsInOutputFolder()
        {
            try
            {
                if (!ReportList.Any())
                {
                    GetReportsData();
                }

                if (!Directory.Exists(Constants.ReportsFolder))
                {
                    Directory.CreateDirectory(Constants.ReportsFolder);
                }

                var dateFolder = string.Format("{0}\\{1}",
                    Constants.ReportsFolder,
                    DateTime.Now.ToShortDateString().Replace('-', '_'));
                var destinationFolder = string.Format("{0}\\{1}",
                        dateFolder,
                        DateTime.Now.ToLongTimeString().Replace(':', '_'));
                if (!Directory.Exists(dateFolder))
                {
                    Directory.CreateDirectory(dateFolder);
                }

                Directory.CreateDirectory(destinationFolder);

                foreach (var report in ReportList)
                {
                    string destination = string.Format("{0}\\{1}.json",
                        destinationFolder,
                        report.ProductId);
                    JsonConvert.SerializeObject(report, Formatting.Indented);
                    File.WriteAllText(@destination,
                        JsonConvert.SerializeObject(report, Formatting.Indented));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void GetReportsData()
        {
            //Check user input date. witch is bigger date
            var maxDate = FindMaxDate();
            var minDate = FindMinDate();

            foreach (var product in Db.Products.Select(p => new
            {
                p.ID,
                p.Product_Name,
                Vendor = p.Vendor.Vendor_Name
            }))
            {
                Console.WriteLine(product.Product_Name);
                decimal totalIncomes = 0;
                int totalQuantity = 0;
                var inReports = Db.SalesReports.Any(p => p.ProductId == product.ID);
                if (inReports)
                {

                    foreach (var sale in Db.SalesReports
                        .Where(p => p.ProductId == product.ID
                            && p.ReportDate >= minDate
                            && p.ReportDate <= maxDate))
                    {
                        totalIncomes += sale.Quantity * sale.ActualPrice;
                        totalQuantity += sale.Quantity;
                    }
                }
                var report = new Report
                {
                    ProductId = product.ID,
                    ProductName = product.Product_Name,
                    TotalIncomes = totalIncomes,
                    TotalQuantitySold = totalQuantity,
                    VendorName = product.Vendor
                };



                //                var report = new BsonDocument
                //                    { 
                //                        {"product-id", product.ID},
                //                        {"product-name", product.Product_Name},
                //                        {"vendor-name", product.Vendor},
                //                        {"total-quantity-sold", totalQuantity},
                //                        {"total-incomes", Convert.ToDouble(totalIncomes)}
                //                    };
                ReportList.Add(report);
                //                Reports.Add(report.ToBson());
            }
        }

        private static DateTime FindMinDate()
        {
            if (ReportStartDate >= ReportEndDate)
            {
                return ReportEndDate;
            }
            return ReportStartDate;
        }

        private static DateTime FindMaxDate()
        {
            if (ReportStartDate >= ReportEndDate)
            {
                return ReportStartDate;
            }
            return ReportEndDate;
        }

        public static Process StartMongoServer()
        {
            if (!Directory.Exists(Constants.Dbpath))
            {
                Directory.CreateDirectory(Constants.Dbpath);
            }

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "mongod.exe",
                Arguments = "--dbpath " + Constants.Dbpath,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            return Process.Start(startInfo);
        }

        public static bool SendReportToMongo(Process process)
        {
            try
            {
                var client = new MongoClient(Constants.ConnetionString);
                var server = client.GetServer();
                var db = server.GetDatabase(Constants.DbName);
                var collectionName = string.Format("{0}_{1}_{2}",
                    Constants.BaseColectionName,
                    FindMinDate().ToShortDateString().Replace('-', '_'),
                    FindMaxDate().ToShortDateString().Replace('-', '_'));
                var isCollectionExist = db.CollectionExists(collectionName);
                if (!isCollectionExist)
                {
                    db.CreateCollection(collectionName);
                }

                if (!ReportList.Any())
                {
                    GetReportsData();
                }

                foreach (var report in ReportList)
                {
                    //                    Query<Report>.Where(r=>r.ProductId == report.ProductId))
                    var collection = db.GetCollection<Report>(collectionName);
                    var currentReport = collection.FindOne(Query<Report>.Where(r => r.ProductId == report.ProductId));
                    if (currentReport == null)
                    {
                        collection.Insert(report);
                    }
                    else
                    {
                        currentReport.ProductName = report.ProductName;
                        currentReport.VendorName = report.VendorName;
                        currentReport.TotalIncomes = report.TotalIncomes;
                        currentReport.TotalQuantitySold = report.TotalQuantitySold;
                        // Set only new values without make new document
                        //                        currentReport["product-name"] = report["product-name"];
                        //                        currentReport["vendor-name"] = report["vendor-name"];
                        //                        currentReport["total-quantity-sold"] = report["total-quantity-sold"];
                        //                        currentReport["total-incomes"] = report["total-incomes"];
                        collection.Save(currentReport);
                    }
                }

                Thread.Sleep(5000);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // This kill mongod.exe procces
                process.Kill();
            }
        }
    }
}
