namespace TestingCSharpCode
{
    using System.IO;

    class TestXMLLoad
    {
        public static void Main()
        {
            string currentDir = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(currentDir, @"..\..\expensesByVendorMonth.xml");
  
            string fileText = System.IO.File.ReadAllText(fileName);
            System.Console.WriteLine(fileName);

        }

    }
}
