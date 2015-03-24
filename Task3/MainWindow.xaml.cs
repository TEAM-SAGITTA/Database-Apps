namespace Task3
{
    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.draw;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.UI;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

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
            var db = new SagittaDBEntities();
            var dbProductEntities = db.Products;
            //SqlCommand allProducts = new SqlCommand("SELECT * FROM Products", db);

            DataRow dr = GetData("SELECT * FROM Products where VendorId = 10").Rows[0];
            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, memoryStream);
               // System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                

                //var phrase;
                //var cell;
                //var table;
                //Color color = null;

                document.Open();
                document.NewPage();

                //Header table
                var table = new PdfPTable(2);
                table.TotalWidth = 50f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 0.3f, 0.7f });

                //Separater Line
                DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f);
                DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f);
                document.Add(table);

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                //Vendor Details
                var cell = PhraseCell(new Phrase("Vendor Record", FontFactory.GetFont("Arial", 12, Font.UNDERLINE)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);

                //Name
                var phrase = new Phrase();
                phrase.Add(new Chunk(dr["VendorName"] + "\n", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                phrase.Add(new Chunk("(" + dr["Title"].ToString() + ")", FontFactory.GetFont("Arial", 8, Font.BOLD)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cell);
                document.Add(table);

                DrawLine(writer, 160f, 80f, 160f, 690f);
                DrawLine(writer, 115f, document.Top - 200f, document.PageSize.Width - 100f, document.Top - 200f);

                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 340f;
                table.LockedWidth = true;
                table.SpacingBefore = 20f;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Pdf Report", "attachment; filename=Report.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
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

        private DataTable GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["SagittaDBEntities"].ConnectionString;

            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

    }
}
