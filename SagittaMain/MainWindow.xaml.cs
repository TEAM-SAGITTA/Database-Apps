namespace SagittaMain
{
    using System;
    using System.Collections.Generic;
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
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            // Should be made a class that holds the button methods!
            string fileDirectoryName = @"\Files\expensesByVendorMonth.xml";
            string currentDir = Directory.GetCurrentDirectory();
            string binDir = System.IO.Directory.GetParent(currentDir).FullName;
            string filePath = System.IO.Directory.GetParent(binDir).FullName + fileDirectoryName;
                        
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList vendors = root.SelectNodes("/expenses-by-month/vendor");
            string vendorsAtribute = "name";
            string expensesAtribute = "month";

            // Just for test that did I extract the XML data correct!! 
            foreach (XmlNode vendor in vendors)
            {
                var vendorName = vendor.Attributes[vendorsAtribute].Value;
                MessageBox.Show(vendorName);

                XmlNodeList expenses = vendor.SelectNodes("expenses");
                foreach (XmlNode expense in expenses)
                {
                    var month = expense.Attributes[expensesAtribute].Value;
                    var monthExpence = expense.InnerText;
                    MessageBox.Show(string.Format("{0} expenses: {1} $", month, monthExpence));
                }
            }

            MessageBox.Show("Data loaded from XML file!");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MS SQL to XML Magic !!!");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("JSON to MongDB Magic !!!");
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
            MessageBox.Show("SQLite to Excel Magic !!!");
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MS SQL to JSON Magic !!!");
        }
    }
}
