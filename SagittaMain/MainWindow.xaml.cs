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
            //TODO change the connection string, Test DB is on my pc
            string DbConnectionString = "Server=.; Integrated security=SSPI; database=Test";
            string firstTableName = "CompanyNames";
            string secondTableName = "ExpencesByMonth";

            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DropCreateSqlServerTables(connection, firstTableName, secondTableName, transaction);

                        PolulateSqlTables(connection, firstTableName, secondTableName, transaction);

                        transaction.Commit();
                        MessageBox.Show("Data loaded from XML file!");
                    }
                    catch (Exception exeption)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Data loading falied!\n" + exeption.Message);
                    }
                }

                connection.Close();
            }
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

        private static void DropCreateSqlServerTables(
      SqlConnection connection,
      string firstTableName,
      string secondTableName,
      SqlTransaction transaction)
        {
            string dropConstraintStr = string.Format("IF OBJECT_ID('{0}') IS NOT NULL ALTER TABLE {0} DROP CONSTRAINT FK_CompanyNameId;"
                , secondTableName);

            string createFirstTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(CompanyNameId  int IDENTITY(1,1) PRIMARY KEY, CompanyName nvarchar(max) NOT NULL)",
                firstTableName);

            string createSecondTableStr = string.Format(
                "IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {0} CREATE TABLE {0}" +
                "(Id  int IDENTITY(1,1) PRIMARY KEY, ExpenseMonth nvarchar(50) NOT NULL," +
                "CompanyNameId int, Expenses nvarchar(20) NOT NULL," + //TODO make it money if you can
                "CONSTRAINT FK_CompanyNameId FOREIGN KEY (CompanyNameId) REFERENCES {1}(CompanyNameId) ON DELETE SET NULL)",
                secondTableName, firstTableName);

            SqlCommand dropConstraint = new SqlCommand(dropConstraintStr, connection, transaction);
            dropConstraint.ExecuteNonQuery();

            SqlCommand createFirstTable = new SqlCommand(createFirstTableStr, connection, transaction);
            createFirstTable.ExecuteNonQuery();

            SqlCommand createSecondTable = new SqlCommand(createSecondTableStr, connection, transaction);
            createSecondTable.ExecuteNonQuery();
        }

        private static void PolulateSqlTables(
            SqlConnection connection,
            string firstTableName,
            string secondTableName,
            SqlTransaction transaction)
        {
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
            var nodeId = 1;

            foreach (XmlNode vendor in rootNodes)
            {
                var RootAtributeValue = vendor.Attributes[RootAtribute].Value;
                var insertStringCmd = string.Format("INSERT INTO {0} VALUES('{1}')", firstTableName, RootAtributeValue);
                SqlCommand insertVendor = new SqlCommand(insertStringCmd, connection, transaction);
                insertVendor.ExecuteNonQuery();
                XmlNodeList childNodes = vendor.SelectNodes(childName);

                for (int i = 0; i < childNodes.Count; i++)
                {
                    var expense = childNodes[i];
                    var expenceMonth = expense.Attributes[childAtribute].Value;
                    var expenceValue = float.Parse(expense.InnerText);

                    string insertChildsStr = string.Format("INSERT INTO {0} VALUES({1},{2},'{3}')",
                        secondTableName, expenceValue, (nodeId), expenceMonth);

                    SqlCommand inserChilds = new SqlCommand(insertChildsStr, connection, transaction);
                    inserChilds.ExecuteNonQuery();
                }

                nodeId++;
            }
        }
    }
}