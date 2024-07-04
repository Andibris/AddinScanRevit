using Autodesk.Revit.DB;
using System.Windows;

namespace ParameterScannerRevit
{
    public partial class UI : Window
    {
        private RevitElementManipulator elementManipulator;

        public UI(Document doc)
        {
            InitializeComponent();
            elementManipulator = new RevitElementManipulator(doc);
        }

        private void BtnIsolate_Click(object sender, RoutedEventArgs e)
        {
            string paramName = TxtName.Text.Trim();
            string paramValue = TxtValue.Text.Trim();
            elementManipulator.HandleButtonClick(paramName, paramValue, true);
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            string paramName = TxtName.Text.Trim();
            string paramValue = TxtValue.Text.Trim();
            elementManipulator.HandleButtonClick(paramName, paramValue, false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}