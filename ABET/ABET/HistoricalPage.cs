using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Xamarin.Forms;
using Syncfusion.XlsIO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using ABET.Data;

namespace ABET
{
    public class HistoricalPage : ContentPage
    {
        Grid buttonGrid = new Grid();
        public static Picker semesterPicker;
        public static Picker coursePicker;

        public HistoricalPage()
        {
            //Create grid and add buttons
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            Button semAddButton = new Button();
            semAddButton.Text = "Add";
            Button couAddButton = new Button();
            couAddButton.Text = "Add";
            Button studentsButton = new Button();
            studentsButton.Text = "Num Students";
            Button button = new Button();
            button.Text = "Load .xlsx";


            //Placeholder lists for data
            var list1 = new List<string>();
            list1.Add("Something1");
            var list2 = new List<string>();
            list2.Add("Something2");

            semesterPicker = new Picker();
            semesterPicker.ItemsSource = list1;
            semesterPicker.SelectedIndex = 0;
            

            
            coursePicker = new Picker();
            coursePicker.ItemsSource = list2;
            coursePicker.SelectedIndex = 0;
            

            //Add buttons and pickers to grid
            buttonGrid.Children.Add(semesterPicker, 0, 0);
            buttonGrid.Children.Add(coursePicker, 0, 1);
            buttonGrid.Children.Add(semAddButton, 1, 0);
            buttonGrid.Children.Add(couAddButton, 1, 1);
            buttonGrid.Children.Add(studentsButton, 0, 2);
            buttonGrid.Children.Add(button, 0, 3);
            
            

            Content = buttonGrid;




            
            Label label = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
            
            button.Clicked += OnButtonClicked;

            /*Content = new StackLayout
            {
                Children =
            {
                button,
                label
            }
            
            };*/

            async void OnButtonClicked(object sender, EventArgs args)
            {
                try
                {
                    FileData filedata = await CrossFilePicker.Current.PickFile();
                    button.Text = filedata.FileName;

                    // Get file path
                    string filePath = filedata.FilePath;
                    ExcelEngine excelEngine = new ExcelEngine();

                    // Create stream object for reading excel
                    using (var inputStream = filedata.GetStream())
                    {
                        // Loads or open an existing workbook through Open method of IWorkbooks
                        IWorkbook workbook = excelEngine.Excel.Workbooks.Open(inputStream);
                        IWorksheet worksheet = workbook.Worksheets[0];

                        // Read loop
                        // Start at cell A1 (Teacher)
                        int row = 1;
                        string currentCell = string.Empty;
                        string currentValue = string.Empty;

                        do
                        {
                            // Increase to cell A2 (student name)
                            // Put the value into data structure in the future
                            // Just adding to label for now
                            row++;
                            currentCell = "A" + row.ToString();
                            currentValue = worksheet.Range[currentCell].Text;
                            label.Text = label.Text + currentValue;
                        } while (currentValue != "Sum");

                        // Print label
                        label.Text = worksheet.Range[currentCell].Text;

                        // Close workbook
                        workbook.Close();
                    }

                }
                catch (Exception ex)
                {
                    // Print exception message to label
                    label.Text = ex.Message;
                }
            }
        }
    }
}
