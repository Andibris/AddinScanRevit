using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ParameterScannerRevit
{
    public class RevitElementManipulator
    {
        private Document Doc;

        public RevitElementManipulator(Document doc)
        {
            Doc = doc;
        }

        public List<Element> GetAllElements()
        {
            FilteredElementCollector collector = new FilteredElementCollector(Doc)
                .WhereElementIsNotElementType();

            return collector.ToList();
        }

        public void HandleButtonClick(string paramName, string paramValue, bool isolateMode)
        {
            try
            {
                if (string.IsNullOrEmpty(paramName))
                {
                    TaskDialog.Show("Error", "Please enter a Parameter Name before performing the operation.");
                    return;
                }

                if (!IsValidViewType())
                {
                    TaskDialog.Show("Error", "This functionality is restricted to Floor Plans, Reflected Ceiling Plans, and 3D Views.");
                    return;
                }

                IList<Element> elements = GetAllElements();
                IList<Element> filteredElements = new List<Element>();

                foreach (Element element in elements)
                {
                    //Checks if the parameter with the specific name exists in elements
                    Parameter parameter = GetParameterByName(element, paramName);

                    //Checks if the parameter value matches the specified value (if provided)
                    if (parameter != null)
                    {
                        string parameterValue = GetParameterValueAsString(parameter);

                        if (IsParameterMatching(parameterValue, paramValue))
                        {
                            filteredElements.Add(element);
                        }
                    }
                    // If it is a family instance, check the family type
                    else if (element is FamilyInstance familyInstance)
                    {
                        ElementId typeId = familyInstance.Symbol?.Id;
                        Element typeElement = Doc.GetElement(typeId);
                        if (typeElement != null)
                        {
                            parameter = GetParameterByName(typeElement, paramName);
                            if (parameter != null)
                            {
                                string parameterValue = GetParameterValueAsString(parameter);
                                if (IsParameterMatching(parameterValue, paramValue))
                                {
                                    filteredElements.Add(element);
                                }
                            }
                        }
                    }
                }

                // Executes the action based on the mode (isolateMode or not)
                if (isolateMode)
                {
                    IsolateElementsInView(filteredElements);
                    TaskDialog.Show("Result", $"Found {filteredElements.Count} elements and isolated them.");
                }
                else
                {
                    if (filteredElements.Any())
                    {
                        ICollection<ElementId> elementIds = filteredElements.Select(elem => elem.Id).ToList();
                        UIDocument uidoc = new UIDocument(Doc);
                        uidoc.Selection.SetElementIds(elementIds);

                        TaskDialog.Show("Action", $"Selected {filteredElements.Count} elements.");
                    }
                    else
                    {
                        TaskDialog.Show("Result", "No elements found matching the specified criteria.");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //validates if view is type floor plan, ceiling plan or 3D
        private bool IsValidViewType()
        {
            View activeView = Doc.ActiveView;
            ViewType viewType = activeView.ViewType;

            return viewType == ViewType.FloorPlan 
                || viewType == ViewType.CeilingPlan 
                || viewType == ViewType.ThreeD;
        }

        private void IsolateElementsInView(IEnumerable<Element> elements)
        {
            View activeView = Doc.ActiveView;

            ICollection<ElementId> elementIds = elements.Select(elem => elem.Id).ToList();
            activeView.IsolateElementsTemporary(elementIds);
        }

        private Parameter GetParameterByName(Element element, string paramName)
        {
            foreach (Parameter parameter in element.Parameters)
            {
                string parameterName = GetVisibleParameterName(parameter);
                if (parameterName != null && parameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                {
                    return parameter;
                }
            }
            return null;
        }

        private string GetVisibleParameterName(Parameter parameter)
        {
            if (parameter?.Definition != null)
            {
                return parameter.Definition.Name;
            }
            else
            {
                return "Parameter definition not found.";
            }
        }

        private string GetParameterValueAsString(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                case StorageType.Integer:
                    return parameter.AsInteger().ToString();
                case StorageType.ElementId:
                    return parameter.AsElementId().IntegerValue.ToString();
                default:
                    return "Unexposed parameter.";
            }
        }

        private bool IsParameterMatching(string actualValue, string expectedValue)
        {
            try
            {
                if (string.IsNullOrEmpty(expectedValue))
                {
                    return true;
                }

                if (string.IsNullOrEmpty(actualValue))
                {
                    return false;
                }
                return actualValue.IndexOf(expectedValue, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}