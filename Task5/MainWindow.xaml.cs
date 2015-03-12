using System;
using System.Collections.Generic;
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
        private DateTime starDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;

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
            endCalendar.DisplayDateStart = new DateTime(2015, 02, 12);
            startCalendar.DisplayDateStart = new DateTime(2015, 02, 12);
            endCalendar.DisplayDateEnd = DateTime.Now;
            startCalendar.DisplayDateEnd = DateTime.Now;
        }

        public DateTime StarDate
        {
            get
            {
                return this.starDate;
            }
            set
            {
                this.starDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
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
            // TODO: When db is redy
            // Get all data from sql server. With foreach meake report for all products in given period

            var report = new BsonDocument
            {
                {"product-id", 3},
                {"product-name", "Beer “Zagorka”"},
                {"vendor-name", "Zagorka Corp."},
                {"total-quantity-sold", 673},
                {"total-incomes", 700.24}
            };

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
            string destination = string.Format("{0}\\{1}.json",
                destinationFolder,
                report["product-id"]);
            File.WriteAllText(@destination, report.ToJson(
                new JsonWriterSettings
                {
                    OutputMode = JsonOutputMode.Strict
                }));
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
                    this.starDate.ToShortDateString().Replace('-', '_'),
                    this.endDate.ToShortDateString().Replace('-', '_'));
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

                // This kill mongod.exe procces
                Thread.Sleep(5000);
                //                process.Kill();
                MessageBox.Show("Your data are on the server now!");
            }
            catch (Exception)
            {
                MessageBox.Show("We have problem with connection! Please try again later!");
            }
        }
    }
}
