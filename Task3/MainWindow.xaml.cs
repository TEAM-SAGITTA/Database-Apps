namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data.Entity;
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.Data;
    using System.Diagnostics;
    using System.Configuration;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            
            var endCalendar = this.FindName("ReportEndDate") as Calendar;
            var startCalendar = this.FindName("ReportStartDate") as Calendar;

            
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

            
            if (calendar.SelectedDate.HasValue)
            {
                
                this.StarDate = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("StartDateBox") as TextBox;

                
                box.Text = this.StarDate.ToShortDateString();
            }
        }

        private void ReportEndDate_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            
            if (calendar.SelectedDate.HasValue)
            {
                this.EndDate = calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
                var box = this.FindName("EndDateBox") as TextBox;

                
                box.Text = this.EndDate.ToShortDateString();
            }
        }

        private void MakePdfReportButton_OnClick(object sender, EventArgs e)
        {
            DataRow dr = GetData("SELECT * FROM Employees where EmployeeId = " + ddlEmployees.SelectedItem.Value).Rows[0]; ;
            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, Color.BLACK);

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                Color color = null;

                document.Open();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Employee.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
            //using (TeamWorkEntities cn = new TeamWorkEntities())
            //{
               
            //    FileStream fStream = File.OpenRead("E:\\MyFiles\\TempReport.pdf");
            //    byte[] contents = new byte[fStream.Length];
            //    fStream.Read(contents, 0, (int)fStream.Length);
            //    fStream.Close();
            //    using (SqlCommand cmd = new SqlCommand("insert into SavePDFTable " + "(PDFFile)values(@data)"))
            //    {
            //        cmd.Parameters.Add("@data", contents);
            //        cmd.ExecuteNonQuery();
            //        Console.Write("Pdf File Save in Dab");
            //    }
            //}

          
        }

    }
}
