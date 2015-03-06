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
            MessageBox.Show("Report Import Magic !!!");
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
            string fileDirectoryName = @"\Files\expensesByVendorMonth.xml";
            string currentDir = Directory.GetCurrentDirectory();
            string binDir = System.IO.Directory.GetParent(currentDir).FullName;
            string filePath = System.IO.Directory.GetParent(binDir).FullName + fileDirectoryName;
            string fileText = System.IO.File.ReadAllText(filePath);
            MessageBox.Show(fileText);
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
