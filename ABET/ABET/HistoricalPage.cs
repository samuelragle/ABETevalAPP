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

                        // Get the number of worksheets
                        int numWorksheets = workbook.Worksheets.Count;

                        // Numworksheets > 1 means historical data
                        if (numWorksheets > 1)
                        {
                            // Loop through worksheets. Data that is not avaiable from just excel sheet is covered by the UI Text boxes
                            for (int s = 0; s < numWorksheets; s++)
                            {
                                IWorksheet worksheet = workbook.Worksheets[s];

                                /* 
                                 * ABET Goal info. Has goal, description, id
                                 * No description
                                 * The goals will be stored in the outcomeList, add each to an ABET object
                                 */
                                char goalAscii = (char)66;
                                string outcome = worksheet.Range[goalAscii + "1"].Text;
                                List<string> outcomeList = new List<string>();

                                while (outcome != null) // Loop through column headers
                                {
                                    outcomeList.Add(outcome);
                                    goalAscii++;
                                    outcome = worksheet.Range[goalAscii + "1"].Text;
                                }

                                // ABETGoal abetGoal = new ABETGoal(null, null, 0);                                

                                /* 
                                 * Course info. Has department, courseNum, course title, id
                                 * No course title in file
                                 */
                                string courseRawText = worksheet.Range["A1"].Text.Split(':')[0]; // CS 2314 for example
                                string department = courseRawText.Split(' ')[0];
                                int courseNum = Int32.Parse(courseRawText.Split(' ')[1]);

                                Course course = new Course(department, courseNum, null, 0); // Last two values will change

                                /* 
                                 * Semester info. Has semester, year, id
                                 * No semester listed
                                 * No year listed
                                 */

                                Semester semester = new Semester(null, 0, 0);  // I do not have semester, year or id so they are null for now

                                /* 
                                 * Section info. Has course obj, semester obj, sectionNum, student count, id
                                 * The course is a course object found from course info above
                                 * Student count in this case is how many responses were recieved, but don't know 
                                 * total number in class
                                 */

                                // Get student count to later check if correct amount of students answered. 
                                // This was just how many students answered, not the amount in the class.
                                string students = string.Empty;
                                int numStudents = 0;
                                int studentRow = 2;

                                while (students != "Sum")
                                {
                                    students = worksheet.Range["A" + studentRow].Text; // Move to next cell

                                    if (students != "Sum")
                                    {
                                        numStudents++; // Increment students
                                    }

                                    studentRow++; // Next row
                                }

                                Section section = new Section(course, semester, 0, 0, 0);   // I do not have sectionNum, student count or ID so they are 0 for now

                                /* 
                                 * Survey class info.
                                 * Has Section object, found above and id
                                 */

                                SurveyClass surveyClass = new SurveyClass(section, 0);

                                /*
                                 * Survey info. Has survey class obj, abet goal obj, response
                                 * Goals are stored in surveyGoals
                                 * These goals will be stored into Survey Objects which will be loaded into lists for Class.cs
                                 */
                                // Begin gathering goals
                                char surveyAscii = (char)66;
                                string goal = worksheet.Range[surveyAscii + "1"].Text;
                                List<string> surveyGoals = new List<string>();

                                while (goal != null) // Loop through column headers
                                {
                                    surveyGoals.Add(goal);
                                    surveyAscii++;
                                    goal = worksheet.Range[surveyAscii + "1"].Text;
                                }
                                // Get the responses, into a survey object
                                int numColumns = surveyGoals.Count;
                                int numRows = numStudents;
                                string response = string.Empty;
                                List<Survey> surveyResponses = new List<Survey>();

                                for (int i = 2; i <= numColumns + 1; i++)
                                {
                                    for (int j = 2; j <= numRows + 1; j++)
                                    {
                                        goal = surveyGoals[i - 2];
                                        response = worksheet.GetValueRowCol(j, i).ToString();
                                        // Survey surveyObj = new Survey(surveyClass, goal, response); 
                                        // Add survey obj to survey responses list
                                    }
                                }

                                /* 
                                 * Class info. Has section, list of survey objs
                                 * No section listed on excel files.
                                 * The list of surveys comes from the Survey objects parsed below
                                 */

                                // Data.Class class = new Data.Class(section, surveyResponses(from above));                               
                            }
                        }

                        // This will be a google form sheet. Class name can maybe be found in file path?
                        else
                        {
                            IWorksheet worksheet = workbook.Worksheets[0];

                            // Gather goals from column headers
                            char columnHeader = (char)66;   // Starts at B, skip timestamp column
                            string goal = worksheet.Range[columnHeader + "1"].Text;
                            List<string> goals = new List<string>();

                            while (goal != null && goal != "") // Loop through column headers
                            {
                                goals.Add(goal);
                                columnHeader++;
                                goal = worksheet.Range[columnHeader + "1"].Text;
                            }

                            // ABETGoal goal = new ABETGoal();

                            // Most of these I do not have the data for
                            List<Survey> surveyResponses = new List<Survey>();
                            Course course = new Course(null, 0, null, 0);
                            Semester semester = new Semester(null, 0, 0);
                            Section section = new Section(course, semester, 0, 0, 0);
                            SurveyClass surveyClass = new SurveyClass(section, 0);

                            // Gather responses for each goal
                            int numColumns = goals.Count;
                            int numRows = 0;
                            int startingRow = 2;
                            string response = string.Empty;
  
                            // First get the number of rows, starting after header
                            while (!worksheet.Range["B" + startingRow].IsBlank)
                            {
                                numRows++;
                                startingRow++;
                            }

                            // Next get responses
                            for (int i = 2; i <= numColumns + 1; i++)
                            {
                                for (int j = 2; j <= numRows + 1; j++)
                                {
                                    goal = goals[i - 2];
                                    response = worksheet.GetValueRowCol(j, i).ToString();
                                    // Survey survey = new Survey(surveyClass, goal, response); 
                                    // Add survey obj to survey responses list
                                }
                            }

                            // Data.Class class = new Data.Class(section, surveyResponses(from above));
                        }

                        // Print label
                        label.Text = "Input sucessfull.";

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
