using ABET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ABET.Data;
using System.Collections;

namespace ABET
{
    public class ResultsPage : ContentPage
    {
        
        Grid classGrid = new Grid();
        Grid buttonGrid = new Grid();
        Grid ABETquestions = new Grid();
        Grid surveyResults = new Grid();
        StackLayout resultStack = new StackLayout();
        ScrollView resultScroll = new ScrollView();
        Hashtable outcomesTable = new Hashtable(); //TODO: Use a SortedDictionary to replace this later
        ListView classView;
        public static Picker semesterPicker;
        Session session;

        
        

        public ResultsPage()
        {
            session = App.GetSession();
            //Create the grid for the page that displays the list of classes and pages.
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.2, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Create the grid for the Buttons in the lower left panel
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Create the buttons to perform certain function son the information
            //along with some extra placeholder functions while we figure out what
            //other functions to include to the application
            Button rawButton = new Button();
            rawButton.Text = "Raw Data";
            Button averageButton = new Button();
            averageButton.Text = "Average";
            buttonGrid.Children.Add(rawButton, 0, 0);
            buttonGrid.Children.Add(averageButton, 1, 0);

            //Events handlers when button is clicked
            //will call corresponding function
            rawButton.Clicked += OnRawDataClicked;
            averageButton.Clicked += OnAverageSelected;



            /** Create example results **/

            //Create grid for output. Do this for each question being displayed
            ABETquestions.ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition(){Width = GridLength.Star},
                new ColumnDefinition(){Width = GridLength.Star},
                new ColumnDefinition(){Width = GridLength.Star},
            };


            //depending on what sql select statements might have to change this
            session.PullSemesters();
            semesterPicker = new Picker();
            semesterPicker.ItemsSource = session.Semesters;
            semesterPicker.SelectedIndex = 0;
            semesterPicker.SelectedIndexChanged += PickerSemester;
            
            ABETquestions.Children.Add(new Label() { Text = "Outcome 1" }, 0, 0);
            ABETquestions.Children.Add(new Label() { Text = "Outcome 2" }, 1, 0);
            ABETquestions.Children.Add(new Label() { Text = "Outcome 3" }, 2, 0);


            surveyResults.ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition(){Width = ABETquestions.ColumnDefinitions[0].Width},
                new ColumnDefinition(){Width = ABETquestions.ColumnDefinitions[1].Width},
                new ColumnDefinition(){Width = ABETquestions.ColumnDefinitions[2].Width},
            };

            surveyResults.RowDefinitions = new RowDefinitionCollection();

            //create a new row for each object in datacollection
            for (int i = 0; i < 20; i++)
            {
                surveyResults.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                surveyResults.Children.Add(new Label() { Text = "1" }, 0, i);
                surveyResults.Children.Add(new Label() { Text = "3" }, 1, i);
                surveyResults.Children.Add(new Label() { Text = "4" }, 2, i);

            }
            


            resultScroll.Content = surveyResults;
            resultStack.Children.Add(ABETquestions); //header grid view in stackLayout
            resultStack.Children.Add(resultScroll); // scrollview in stackLayout


            
            //Controls the class selection
            classView = new ListView();
            session.PullSections((Semester)semesterPicker.SelectedItem);
            classView.ItemsSource = session.Classes;
            classView.ItemSelected += OnClassSelected;
            

            //populates top left of results page
            classGrid.Children.Add(resultStack, 0, 0);
            Grid.SetRowSpan(resultStack, 2);

            //populates bottom left of the results page
            classGrid.Children.Add(buttonGrid, 0, 2);

            //populates top right of the result page
            classGrid.Children.Add(semesterPicker, 1, 0);

            //populates the bottom right
            classGrid.Children.Add(classView, 1, 1);
            Grid.SetRowSpan(classView, 2);

            Content = classGrid;
        }

        
        public void OnClassSelected(object sender, EventArgs e)
        {
            outcomesTable.Clear();
            // Populate raw data here
            Class selectedClass = (Class)((ListView)sender).SelectedItem;
            foreach (Survey s in selectedClass.Surveys)
            {
                if (!outcomesTable.ContainsKey(s.surveyClass))
                {
                    outcomesTable.Add(s.surveyClass, new List<Survey>());
                }
                ((List<Survey>)outcomesTable[s.surveyClass]).Add(s);
            }
            
        }
        //Changes the list of engineering classes based on the semester chosen from the picker
        public void PickerSemester(object sender, EventArgs e)
        {

            if (semesterPicker.SelectedItem != null)
            {
                classView.ItemsSource = null; // Because it won't update without being set to a list of a different pointer
                session.PullSections((Semester)semesterPicker.SelectedItem);
                classView.ItemsSource = session.Classes;
            }
            //remove all the current classes in the classView object
            //populate classView with all classes in the semester chosen by the picker            
        }

        //Performs the Average Function on the selected classes
        public void OnAverageSelected(object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function 
            //performed on the classes (switchcells) selected in the bottom left panel
            

        }

        //Outputs the complete responses from a selected class
        public void OnRawDataClicked(object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function similar to the example above

            /** Update the ABET Questions at the top of the left panel **/
        }
        public static void UpdateSemesters(List<Semester> newSemesters)
        {
            semesterPicker.ItemsSource = newSemesters;
            semesterPicker.SelectedIndex = 0;
        }
    }
}
