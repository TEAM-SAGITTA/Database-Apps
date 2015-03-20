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
            if (ReportEndDate.SelectedDate.HasValue)
            {
                var date = ReportEndDate.SelectedDate.GetValueOrDefault(DateTime.Now);
                EndDateBox.Text = date.ToShortDateString();
                Utility.ReportEndDate = date;
            }
        }

        private void ReportStartDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportStartDate.SelectedDate.HasValue)
            {
                var date = ReportStartDate.SelectedDate.GetValueOrDefault(DateTime.Now);
                StartDateBox.Text = date.ToShortDateString();
                Utility.ReportStartDate = date;
            }
        }

        private void MakeReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isSaved = Utility.SaveReportsInOutputFolder();
            MessageBox.Show(isSaved
                ? "Reports are done!"
                : "We have probel with connections, plese try again later!");
        }

        private void SendReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var process = Utility.StartMongoServer();
            var isOnTheServer = Utility.SendReportToMongo(process);
            MessageBox.Show(isOnTheServer
                ? "Your data are on the server now!"
                : "We have problem with connection! Please try again later!");
        }

        private void MakeCalendars()
        {
            ReportEndDate.DisplayDateStart = Utility.CalendarStartDate;
            ReportStartDate.DisplayDateStart = Utility.CalendarStartDate;
            ReportEndDate.DisplayDateEnd = Utility.CalendarEndDate;
            ReportStartDate.DisplayDateEnd = Utility.CalendarEndDate;
        }
    }
}