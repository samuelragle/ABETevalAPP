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
using System.Diagnostics;

namespace ABET
{
    public class HistoricalPage : ContentPage
    {
        Grid semesterGrid = new Grid();
        Grid courseGrid = new Grid();
        Grid studentGrid = new Grid();
        Grid sectionGrid = new Grid();

        public static Picker semesterPicker;
        public static Picker coursePicker = new Picker();
        Entry numEntry = new Entry { Placeholder = "Number of students" };
        Entry sectionNumEntry = new Entry { Placeholder = "Section Number" };
        Button semAddButton = new Button();
        Button couAddButton = new Button();
        Button button = new Button();

        Session session = App.GetSession();

        public HistoricalPage()
        {

            semesterPicker = new Picker();
            NavigationPage.SetHasNavigationBar(this, false);

            Label label = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            semAddButton.Text = "Add";
            couAddButton.Text = "Add";
            button.Text = "Load .xlsx";


            semAddButton.Clicked += OnAddSemesterClicked;
            couAddButton.Clicked += OnAddCourseButton;
            button.Clicked += OnButtonClicked;




            semesterPicker.ItemsSource = session.Semesters;
            semesterPicker.SelectedIndex = 0;



            /**
             * 
             * NEED A LIST OF ALL COURSES IN THE DATABASE IN THE SESSION OBJECT
             * AND A TOSTRING() FOR COURSE OBJECTS SO THAT THE USERS CAN SELECT 
             * FROM THE LIST OF ALL POSSIBLE COURSES WHEN ADDING DATA
             * 
             * */
            //coursePicker.ItemsSource = session.Courses;
            
            session.PullCourses();
            coursePicker.ItemsSource = session.Courses;
            coursePicker.SelectedIndex = 0;


            //Create grid and add buttons

            semesterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            semesterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            semesterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            courseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            courseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            courseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            studentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            studentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            studentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            sectionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            sectionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            sectionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });






            //Add buttons and pickers to grid
            semesterGrid.Children.Add(semesterPicker, 0, 0);
            semesterGrid.Children.Add(semAddButton, 1, 0);
            courseGrid.Children.Add(coursePicker, 0, 0);
            courseGrid.Children.Add(couAddButton, 1, 0);
            studentGrid.Children.Add(numEntry, 0, 0);
            sectionGrid.Children.Add(sectionNumEntry, 0, 0);



            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,

                Children = {
                    semesterGrid,
                    courseGrid,
                    sectionGrid,
                    studentGrid,
                    button
                }
            };

            

            async void OnButtonClicked(object sender, EventArgs args)
            {


                FileData filedata;
                ExcelEngine excelEngine;
                IWorkbook workbook;
                IWorksheet worksheet;
                char surveyAscii;
                char goalAscii;
                string outcome;
                string courseRawText;
                string department;
                string goal;
                string students;
                string response;
                string filePath;
                int startingRow = 0;
                int sectionNum = 0;
                int studentNum = 0;
                int courseNum = 0;
                int numStudents = 0;
                int studentRow = 0;
                int numColumns = 0;
                int numRows = 0;
                int numWorksheets = 0;
                List<string> outcomeList;
                Course course = (Course)coursePicker.SelectedItem;
                Semester semester = (Semester)semesterPicker.SelectedItem;


                //THESE NEEED TO GO INTO DB
                Section section;
                SurveyClass surveyClass;
                List<Survey> surveyResponses;
                List<ABETGoal> surveyGoals;



                try
                {
                    filedata = await CrossFilePicker.Current.PickFile();
                    button.Text = filedata.FileName;

                    // Get file path
                    filePath = filedata.FilePath;
                    excelEngine = new ExcelEngine();

                    // Create stream object for reading excel
                    using (var inputStream = filedata.GetStream())
                    {
                        // Loads or open an existing workbook through Open method of IWorkbooks
                        workbook = excelEngine.Excel.Workbooks.Open(inputStream);
                        numWorksheets = workbook.Worksheets.Count;

                        // Numworksheets > 1 means historical data
                        if (numWorksheets > 1)
                        {
                            // Loop through worksheets. Data that is not avaiable from just excel sheet is covered by the UI Text boxes
                            for (int s = 0; s < numWorksheets; s++)
                            {
                                worksheet = workbook.Worksheets[s];

                                /* 
                                 * ABET Goal info. Has goal, description, id
                                 * No description
                                 * The goals will be stored in the outcomeList, add each to an ABET object
                                 */
                                goalAscii = (char)66;
                                outcome = worksheet.Range[goalAscii + "1"].Text;
                                outcomeList = new List<string>();

                                while (outcome != null) // Loop through column headers
                                {
                                    outcomeList.Add(outcome);
                                    goalAscii++;
                                    outcome = worksheet.Range[goalAscii + "1"].Text;
                                }                             

                                /* 
                                 * Course info. Has department, courseNum, course title, id
                                 * No course title in file
                                 */
                                courseRawText = worksheet.Range["A1"].Text.Split(':')[0]; // CS 2314 for example
                                department = courseRawText.Split(' ')[0];
                                courseNum = Int32.Parse(courseRawText.Split(' ')[1]);
                                //Debug.Print(courseRawText);
                                //Debug.Print("\n");
                                
                                

                                // Get student count to later check if correct amount of students answered. 
                                // This was just how many students answered, not the amount in the class.
                                students = string.Empty;
                                numStudents = 0;
                                studentRow = 2;

                                while (students != "Sum")
                                {
                                    students = worksheet.Range["A" + studentRow].Text; // Move to next cell

                                    if (students != "Sum")
                                    {
                                        numStudents++; // Increment students
                                    }

                                    studentRow++; // Next row
                                }

                                //NEED TO FIGURE OUT ID
                                section = new Section(course, semester, 0, 0, 0);   // I do not have sectionNum, student count or ID so they are 0 for now

                                /* 
                                 * Survey class info.
                                 * Has Section object, found above and id
                                 */

                                //NEED TO FIGURE OUT ID
                                surveyClass = new SurveyClass(section, 0);

                                /*
                                 * Survey info. Has survey class obj, abet goal obj, response
                                 * Goals are stored in surveyGoals
                                 * These goals will be stored into Survey Objects which will be loaded into lists for Class.cs
                                 */
                                // Begin gathering goals
                                surveyAscii = (char)66;
                                goal = worksheet.Range[surveyAscii + "1"].Text;
                                surveyGoals = new List<ABETGoal>();

                                while (goal != null) // Loop through column headers
                                {
                                    //NEED TO FIGURE OUT ID
                                    surveyGoals.Add(new ABETGoal(goal, goal, 0));
                                    surveyAscii++;
                                    goal = worksheet.Range[surveyAscii + "1"].Text;
                                }
                                // Get the responses, into a survey object
                                numColumns = surveyGoals.Count;
                                numRows = numStudents;
                                response = string.Empty;
                                surveyResponses = new List<Survey>();

                                for (int i = 2; i <= numColumns + 1; i++)
                                {
                                    for (int j = 2; j <= numRows + 1; j++)
                                    {

                                        try
                                        {
                                            response = worksheet.GetValueRowCol(j, i).ToString();
                                            surveyResponses.Add(new Survey(surveyClass, surveyGoals[i - 2], Int32.Parse(response)));
                                        }
                                        catch (FormatException)
                                        {

                                        }
                                        // Add survey obj to survey responses list
                                    }
                                }

                                /* 
                                 * Class info. Has section, list of survey objs
                                 * No section listed on excel files.
                                 * The list of surveys comes from the Survey objects parsed below
                                 **/
                                 

                                Debug.Print(section.ToString());
                                Debug.Print("\n");
                                Debug.Print("\n");
                                Debug.Print(surveyClass.ToString());
                                Debug.Print("\n");
                                Debug.Print("\n");
                                for (int pennie = 0; pennie < surveyGoals.Count; ++pennie)
                                {
                                    Debug.Print(surveyGoals[pennie].ToString());
                                    Debug.Print("\n");
                                }
                                Debug.Print("\n");
                                for (int pennie = 0; pennie < surveyResponses.Count; ++pennie)
                                {
                                    Debug.Print(surveyResponses[pennie].ToString());
                                    Debug.Print("\n");
                                }
                                Debug.Print("\n");

                            }
                        }

                        // This will be a google form sheet. Class name can maybe be found in file path?
                        else
                        {
                            worksheet = workbook.Worksheets[0];

                            // Gather goals from column headers
                            surveyAscii = (char)66;   // Starts at B, skip timestamp column
                            goal = worksheet.Range[surveyAscii + "1"].Text;
                            surveyGoals = new List<ABETGoal>();

                            while (goal != null && goal != "Score" && goal != "") // Loop through column headers
                            {

                                //NEED TO FIGURE OUT ID
                                surveyGoals.Add(new ABETGoal(goal, goal, 0));
                                surveyAscii++;
                                goal = worksheet.Range[surveyAscii + "1"].Text;
                            }

                            // ABETGoal goal = new ABETGoal();

                            // Most of these I do not have the data for
                            surveyResponses = new List<Survey>();

                            try
                            {
                                try { studentNum = Convert.ToInt32(numEntry.Text); }
                                catch { studentNum = 0; }

                                sectionNum = Convert.ToInt32(sectionNumEntry.Text);

                                section = new Section(course, semester, sectionNum, studentNum, 0);
                            }
                            catch (FormatException)
                            {
                                sectionNum = 0;
                            }

                            //NEED TO FIGURE OUT ID
                            section = new Section(course, semester, sectionNum, studentNum, 0);
                            //NEED TO FIGURE OUT ID
                            surveyClass = new SurveyClass(section, 0);

                            // Gather responses for each goal
                            numColumns = surveyGoals.Count;
                            numRows = 0;
                            startingRow = 2;
                            response = string.Empty;
  
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
                                    try
                                    {
                                        response = worksheet.GetValueRowCol(j, i).ToString();
                                        surveyResponses.Add(new Survey(surveyClass, surveyGoals[i - 2], Int32.Parse(response)));
                                    }
                                    catch (FormatException)
                                    {
                                        
                                    }
                                    // Survey survey = new Survey(surveyClass, goal, response); 
                                    // Add survey obj to survey responses list
                                }
                            }
                            
                            Debug.Print(section.ToString());
                            Debug.Print("\n");
                            Debug.Print("\n");
                            Debug.Print(surveyClass.ToString());
                            Debug.Print("\n");
                            Debug.Print("\n");
                            for (int pennie = 0; pennie < surveyGoals.Count; ++pennie)
                            {
                                Debug.Print(surveyGoals[pennie].ToString());
                                Debug.Print("\n");
                            }
                            Debug.Print("\n");
                            for (int pennie = 0; pennie < surveyResponses.Count; ++pennie)
                            {
                                Debug.Print(surveyResponses[pennie].ToString());
                                Debug.Print("\n");
                            }
                            Debug.Print("\n");


                        }


                        // Print label
                        label.Text = "Input sucessful.";
                        
                        

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

            async void OnAddSemesterClicked(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new SemesterAdditionPage());
                semesterPicker.ItemsSource = session.Semesters;

            }

            async void OnAddCourseButton(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new CourseAdditionPage());
                //coursePicker.ItemsSource = session.Courses;

            }

        }

        
    }
}
