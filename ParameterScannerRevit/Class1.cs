using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace ParameterScannerRevit
{
    public class Class1 : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Parameters");

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData = new PushButtonData("cmdParameterScanner", "Parameter\rScanner", thisAssemblyPath, "ParameterScannerRevit.Scan");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            string imagePath = Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "images", "icon.png");
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                pushButton.LargeImage = bitmapImage;
                pushButton.ToolTip = "Specify a parameter name and value to search for within the model elements.";
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to load icon.png: {ex.Message}");
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Scan : IExternalCommand
    {
        private static UI window;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (window == null || !window.IsVisible)
                {
                    var uiApp = commandData.Application;
                    var uiDoc = uiApp.ActiveUIDocument;
                    var doc = uiDoc.Document;

                    window = new UI(doc);
                    window.Closed += Window_Closed;
                    window.Show();
                }
                else
                {
                    window.Focus();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to open UI: {ex.Message}");
                return Result.Failed;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            window = null;
        }
    }
}