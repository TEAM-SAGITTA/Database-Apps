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
            MessageBox.Show("Zip Import Magic !!!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PDF Report Magic !!!\nDeveloper - Doncho Donchev");
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
    }
}
