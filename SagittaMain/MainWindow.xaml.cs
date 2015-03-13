using System.Diagnostics;
using System.Reflection;

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
            MessageBox.Show("Zip file to MS SQL Server Magic !!!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MS SQL to PDF Magic !!!\nDeveloper -> ");
        }

        private void LoadExpenseDataFromXML(object sender, RoutedEventArgs e)
        {
            //TODO change the connection string, Test DB is on my pc
            string DbConnectionString = "Server=.; Integrated security=SSPI; database=Test";
            string firstTableName = "Test100"; // can be direct parameter in the method
            string secondTableName = "Test200"; // can be direct parameter in the method

            CreateSqlServerTables(DbConnectionString, firstTableName, secondTableName);

            string fileDirectoryName = @"\Files\expensesByVendorMonth.xml";
            string currentDir = Directory.GetCurrentDirectory();
            string binDir = System.IO.Directory.GetParent(currentDir).FullName;
            string filePath = System.IO.Directory.GetParent(binDir).FullName + fileDirectoryName;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement root = xmlDoc.DocumentElement;
            string RootPath = "/expenses-by-month/vendor";
            string RootAtribute = "name";
            string childName = "expenses";
            string childAtribute = "month";
            XmlNodeList rootNodes = root.SelectNodes(RootPath);


            // Just for test that did I extract the XML data correct!!


            //foreach (XmlNode vendor in rootNodes)
            //{
            //    var RootAtributeValue = vendor.Attributes[RootAtribute].Value;
            //    MessageBox.Show(RootAtributeValue); // remove this

            //    XmlNodeList childNodes = vendor.SelectNodes(childName);
            //    foreach (XmlNode expense in childNodes)
            //    {
            //        var childNameValue = expense.Attributes[childAtribute].Value;
            //        var monthExpence = expense.InnerText;
            //        // change with Insert INTO DB
            //        MessageBox.Show(string.Format("{0} {1} : {2} $", childNameValue, childName, monthExpence)); // remove this
            //    }
            //}

            MessageBox.Show("Data loaded from XML file!");
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MS SQL to XML Magic !!!");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("JSON to MongDB Magic !!!");
            //DataGeterForJSONReport.
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
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
            const string connectionString = @"DataSource = ..\..\..\08.SQLite_And_MySQL_ToExcel\Data\Product.db; Version = 3";
            using (SQLiteConnection con = new SQLiteConnection(connectionString) )
            {
                try
                {
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        MessageBox.Show("SQLite is connected");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                FileName = "Task5.exe" //TODO remove comment I got an exeption
            };

            Process.Start(startInfo);
        }

        private static void CreateSqlServerTables(string DbConnectionString, string firstTableName, string secondTableName)
        {
            string dropConstraintStr = string.Format("IF OBJECT_ID('{0}') IS NOT NULL ALTER TABLE {0} DROP CONSTRAINT FK_CompanyNameId;", secondTableName);

            string createFirstTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(CompanyNameId  int IDENTITY(1,1) PRIMARY KEY, CompanyName varchar(max))",
                firstTableName);

            string createSecondTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(Id  int IDENTITY(1,1) PRIMARY KEY, ExpenceDate varchar(max)," +
                "CompanyNameId int, Expenses FLOAT," +
                "CONSTRAINT FK_CompanyNameId FOREIGN KEY (CompanyNameId) REFERENCES {1}(CompanyNameId) ON DELETE SET NULL)", secondTableName, firstTableName);

            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();
                SqlCommand dropConstraint = new SqlCommand(dropConstraintStr, connection);
                dropConstraint.ExecuteNonQuery();

                SqlCommand createFirstTable = new SqlCommand(createFirstTableStr, connection);
                createFirstTable.ExecuteNonQuery();

                SqlCommand createSecondTable = new SqlCommand(createSecondTableStr, connection);
                createSecondTable.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
