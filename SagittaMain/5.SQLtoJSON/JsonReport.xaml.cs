using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SagittaMain._5.SQLtoJSON
{
    /// <summary>
    /// Interaction logic for JsonReport.xaml
    /// </summary>
    public partial class JsonReport : Window
    {
        private DateTime starDate;
        private DateTime endDate;
        
        public JsonReport()
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
            MessageBox.Show("Report compleated!");
        }

        private void SendReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Data are on the serever now!");
        }
    }
}
