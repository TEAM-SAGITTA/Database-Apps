using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace Task5
{
    public partial class MainWindow : Window
    {
        private static SagittaDBEntities db = new SagittaDBEntities();
        private static readonly DateTime MinDate = db.SalesReports.Select(r => r.ReportDate).Min();
        private DateTime reportStarDate = MinDate;
        private DateTime reportEndDate = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            MakeCalendars();
        }

        private void MakeCalendars()
        {
            // TODO: Set dates wit first and last date form db
            var endCalendar = this.FindName("ReportEndDate") as Calendar;
            var startCalendar = this.FindName("ReportStartDate") as Calendar;

            // TODO: Check for null
            endCalendar.DisplayDateStart = MinDate;
            startCalendar.DisplayDateStart = MinDate;
            endCalendar.DisplayDateEnd = DateTime.Now;
            startCalendar.DisplayDateEnd = DateTime.Now;
        }

        public DateTime StarDate
        {
            get
            {
                return this.reportStarDate;
            }
            set
            {
                this.reportStarDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.reportEndDate;
            }
            set
            {
                this.reportEndDate = value;
            }
        }

        private void ReportStartDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            //TODO: Make check for null
            if (calendar.SelectedDate.HasValue)
            {
                // MessageBox.Show(calendar.SelectedDate.Value.ToShortDateString());
                this.StarDate = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("StartDateBox") as TextBox;

                //TODO: Make check for null
                box.Text = this.StarDate.ToShortDateString();
            }
        }

        private void ReportEndDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            //TODO: Make check for null
            if (calendar.SelectedDate.HasValue)
            {
                this.EndDate = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("EndDateBox") as TextBox;

                //TODO: Make check for null
                box.Text = this.EndDate.ToShortDateString();
            }
        }

        private void MakeReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Constants.ReportsFolder))
            {
                Directory.CreateDirectory(Constants.ReportsFolder);
            }

            string dateFolder = string.Format("{0}\\{1}",
                Constants.ReportsFolder,
                DateTime.Now.ToShortDateString().Replace('-', '_'));
            string destinationFolder = string.Format("{0}\\{1}",
                    dateFolder,
                    DateTime.Now.ToLongTimeString().Replace(':', '_'));
            if (!Directory.Exists(dateFolder))
            {
                Directory.CreateDirectory(dateFolder);
            }

            Directory.CreateDirectory(destinationFolder);

            foreach (var product in db.Products.Select(p=>new
            {
                p.ID,
                p.Product_Name,
                Vendor = p.Vendor.Vendor_Name
            }))
            {
                
                var inReports = db.SalesReports.Any(p => p.ProductId == product.ID);
                if (inReports)
                {
                    decimal totalIncomes = 0;
                    foreach (var sale in db.SalesReports.Where(p=>p.ProductId == product.ID))
                    {
                        totalIncomes += sale.Quantity*sale.ActualPrice;
                    }

                    var report = new BsonDocument
                    { 
                        {"product-id", product.ID},
                        {"product-name", product.Product_Name},
                        {"vendor-name", product.Vendor},
                        {"total-quantity-sold", db.SalesReports
                            .Where(r=>r.ProductId == product.ID)
                            .Sum(p=>p.Quantity)},
                        {"total-incomes", Convert.ToDouble(totalIncomes)}
                    };

                    string destination = string.Format("{0}\\{1}.json", 
                        destinationFolder, 
                        report["product-id"]);
                    File.WriteAllText(@destination, report.ToJson(
                        new JsonWriterSettings
                        {
                            OutputMode = JsonOutputMode.Strict
                        }));
                }
            }

            MessageBox.Show("Reports are done!");
        }

        private Process StartMongoServer()
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

        private void SendReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var process = StartMongoServer();
            SendReportToMongo(process);
        }

        private void SendReportToMongo(Process process)
        {
            try
            {
                var client = new MongoClient(Constants.ConnetionString);
                var server = client.GetServer();
                var db = server.GetDatabase(Constants.DbName);
                var collectionName = string.Format("{0}_{1}_{2}",
                    Constants.BaseColectionName,
                    this.reportStarDate.ToShortDateString().Replace('-', '_'),
                    this.reportEndDate.ToShortDateString().Replace('-', '_'));
                var isCollectionExist = db.CollectionExists(collectionName);
                if (!isCollectionExist)
                {
                    db.CreateCollection(collectionName);
                }

                var report = new BsonDocument
                {
                    {"product-id", 3},
                    {"product-name", "Beer “Zagorka”"},
                    {"vendor-name", "Zagorka Corp."},
                    {"total-quantity-sold", 673},
                    {"total-incomes", 609.24}
                };

                var reports = db.GetCollection(collectionName);
                reports.Insert(report);
                // If document exist change field values else create document
                //            var rep = reports.FindOne(Query.EQ("product-id", ));
                //            if (rep == null)
                //            {
                //                reports.Insert(report);
                //            }
                //            else
                //            {
                //                //TODO: Set only new values without make new document
                //                //                rep["total-incomes"] = 10;
                //                //                reports.Save(rep);
                //            }


                Thread.Sleep(5000);
                MessageBox.Show("Your data are on the server now!");
            }
            catch (Exception)
            {
                MessageBox.Show("We have problem with connection! Please try again later!");
            }
            finally
            {
                // This kill mongod.exe procces
                process.Kill();
            }

        }
    }
}
