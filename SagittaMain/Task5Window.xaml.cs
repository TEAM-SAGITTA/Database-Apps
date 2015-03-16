using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB.Bson.IO;
using Task5;

namespace SagittaMain
{
    /// <summary>
    /// Interaction logic for Task5Window.xaml
    /// </summary>
    public partial class Task5Window : Window
    {
        public Task5Window()
        {
            InitializeComponent();
            this.MakeCalendars();
        }

        private void ReportEndDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            //TODO: Make check for null
            if (calendar.SelectedDate.HasValue)
            {
                var date = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("EndDateBox") as TextBox;

                //TODO: Make check for null
                box.Text = date.ToShortDateString();
                Utility.ReportEndDate = date;
            }
        }

        private void ReportStartDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            //TODO: Make check for null
            if (calendar.SelectedDate.HasValue)
            {
                var date = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("StartDateBox") as TextBox;

                //TODO: Make check for null
                box.Text = date.ToShortDateString();
                Utility.ReportStartDate = date;
            }
        }

        private void MakeReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isSaved = Utility.SaveReportsInOutputFolder();
            if (isSaved)
            {
                MessageBox.Show("Reports are done!");
            }
            else
            {
                MessageBox.Show("We have probel with connections, plese try again later!");
            }
        }

        private void SendReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var process = Utility.StartMongoServer();
            var isOnTheServer = Utility.SendReportToMongo(process);
            if (isOnTheServer)
            {
                MessageBox.Show("Your data are on the server now!");
            }
            else
            {
                MessageBox.Show("We have problem with connection! Please try again later!");
            }
        }

        private void MakeCalendars()
        {
            // TODO: Set dates wit first and last date form db
            var endCalendar = this.FindName("ReportEndDate") as Calendar;
            var startCalendar = this.FindName("ReportStartDate") as Calendar;

            // TODO: Check for null
            endCalendar.DisplayDateStart = Utility.CalendarStartDate;
            startCalendar.DisplayDateStart = Utility.CalendarStartDate;
            endCalendar.DisplayDateEnd = Utility.CalendarEndDate;
            startCalendar.DisplayDateEnd = Utility.CalendarEndDate;
        }
    }
}
