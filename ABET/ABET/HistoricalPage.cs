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

namespace ABET
{
    public class HistoricalPage : ContentPage
    {
        public HistoricalPage()
        {
            Label label = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
            Button button = new Button
            {
                Text = "Load Historical Data",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += OnButtonClicked;

            Content = new StackLayout
            {
                Children =
            {
                button,
                label
            }
            };

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
