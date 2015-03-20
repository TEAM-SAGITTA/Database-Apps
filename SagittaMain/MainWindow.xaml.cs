using Task6;

namespace SagittaMain
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Xml;
    using System.Xml.Linq;
    using System.Data.SQLite;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Reflection;
    using _02.ZipReports_To_MSSQL;
    using _04.XML_SalesReport;
    using _07.MSSQL__to_MySQL;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Product Data to Oracle Magic !!!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataMigrator dataMigrator = new DataMigrator();
            dataMigrator.MigrateDataFromExcelFiles();
            MessageBox.Show("Zip file to MS SQL Server Magic !!!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MS SQL to PDF Magic !!!\nDeveloper -> ");
        }

        private void LoadExpenseDataFromXML(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathPicker(".xml");

            if (filePath != string.Empty)
            {
                MessageBoxResult confirmation = MessageBox.Show(string.Format("Do you want to load the data from: {0} ?", filePath),
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirmation == MessageBoxResult.Yes)
                {
                    try
                    {
                        XmlToSqlServerLoader.PolulateSqlTables(filePath);
                        MessageBox.Show("Data loaded successful!");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Data loading falied!\n" + exception.Message);
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("MS SQL to XML Magic !!!");
            XMLHelper xmlFile = new XMLHelper();
            xmlFile.exportToXML();
            MessageBox.Show("MS SQL Export to XML file has been finished!");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("JSON to MongDB Magic !!!");
            //DataGeterForJSONReport.
        }

        private void LoadDataFromSQLServerToMySQL(object sender, RoutedEventArgs e)
        {
            MySqlDataMigrator dataMigrator = new MySqlDataMigrator();
            dataMigrator.MigrateDataSqlServerToFromMySql();
            MessageBox.Show("MS SQL to MySQL Magic !!!");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MySQL to Excel Magic !!!");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Oralce to MS SQL Server Magic !!!");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            using (var db = new TaxEntities())
            {
                var a = db.Products.ToList();
                MessageBox.Show(a[0].ProductName);
            }
            //            const string connectionString = @"DataSource = ..\..\..\08.SQLite_And_MySQL_ToExcel\Data\Product.db; Version = 3";
            //            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            //            {
            //                try
            //                {
            //                    con.Open();
            //                    if (con.State == System.Data.ConnectionState.Open)
            //                    {
            //                        MessageBox.Show("SQLite is connected");
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(ex.Message);
            //                }
            //            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var window = new Task5Window();
            window.Show();
        }

        private static string FilePathPicker(string fileExtension)
        {
            Microsoft.Win32.OpenFileDialog filePicker = new Microsoft.Win32.OpenFileDialog();
            if (fileExtension != "all")
            {
                filePicker.DefaultExt = fileExtension.ToString();
                filePicker.Filter = string.Format("XML files (*{0})|*{0}", fileExtension);
            }

            // TODO think better way 
            filePicker.InitialDirectory = Directory.GetParent(
                Directory.GetParent((
                Directory.GetParent(
                Directory.GetCurrentDirectory())).ToString()).ToString()).ToString() + "\\InputFiles";
            bool? isFilePiced = filePicker.ShowDialog();
            string filePath = filePicker.FileName;

            return filePath;
        }

        private static string FilePathPicker()
        {
            return FilePathPicker("all");
        }
    }
}