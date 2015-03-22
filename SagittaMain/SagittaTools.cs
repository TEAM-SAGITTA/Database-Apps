namespace SagittaMain
{
    using System.IO;

    internal static class SagittaTools
    {

        internal static string FilePathPicker(string fileExtension)
        {
            Microsoft.Win32.OpenFileDialog filePicker = new Microsoft.Win32.OpenFileDialog();
            if (fileExtension != "all")
            {
                filePicker.DefaultExt = fileExtension.ToString();
                filePicker.Filter = string.Format("XML files (*{0})|*{0}", fileExtension);
            }

            filePicker.InitialDirectory = Directory.GetParent(
                Directory.GetParent((
                Directory.GetParent(
                Directory.GetCurrentDirectory())).ToString()).ToString()).ToString() + "\\InputFiles";
            bool? isFilePiced = filePicker.ShowDialog();
            string filePath = filePicker.FileName;

            return filePath;
        }

        internal static string FilePathPicker() 
        {
           return FilePathPicker("all");
        }

    }
}
