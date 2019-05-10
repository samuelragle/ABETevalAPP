using ABET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Collections;
using Syncfusion.XlsIO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System.IO;
using Xamarin.Forms.PlatformConfiguration;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;


namespace ABET
{
    public class ResultsPage : ContentPage
    {
        
        Grid classGrid = new Grid();
        StackLayout buttonStack = new StackLayout();
        Grid ABETquestions = new Grid();
        Grid surveyResults = new Grid();
        StackLayout resultStack = new StackLayout();
        ScrollView resultScroll = new ScrollView();
        Hashtable outcomesTable = new Hashtable(); //TODO: Use a SortedDictionary to replace this later
        ListView classView;
        public static Picker semesterPicker;
        Button outputButton = new Button();
        Session session;
        double averages = 0;

        
        

        public ResultsPage()
        {
            session = App.GetSession();
            //Create the grid for the page that displays the list of classes and pages.
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.2, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            

            //Create the buttons to perform certain function son the information
            //along with some extra placeholder functions while we figure out what
            //other functions to include to the application
            
            outputButton.WidthRequest = 200;
            outputButton.Text = "Delete Selected Data";

            //set layout for buttons in lower left
            buttonStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,

                Children = {
                    outputButton
                }
            };

            //Events handlers when button is clicked
            //will call corresponding function
            outputButton.Clicked += OnOutputClicked;



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
            
            surveyResults.ColumnDefinitions = new ColumnDefinitionCollection();

            surveyResults.RowDefinitions = new RowDefinitionCollection();
            


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
            classGrid.Children.Add(buttonStack, 0, 2);

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
            ABETquestions.Children.Clear();
            ABETquestions.ColumnDefinitions.Clear();
            surveyResults.ColumnDefinitions.Clear();
            surveyResults.Children.Clear();
           // Populate raw data here
           Class selectedClass = (Class)((ListView)sender).SelectedItem;
            foreach (Survey s in selectedClass.Surveys)
            {
                if (!outcomesTable.Contains(s.goal))
                {
                    outcomesTable.Add(s.goal, new List<Survey>());
                    
                }
                ((List<Survey>)outcomesTable[s.goal]).Add(s);
            }
            int index = 0;
            foreach (ABETGoal a in outcomesTable.Keys)
            {
                averages = 0;
                ABETquestions.Children.Add(new Label() { Text = a.ToString() }, index, 0);
                ABETquestions.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                surveyResults.ColumnDefinitions.Add(new ColumnDefinition() { Width = ABETquestions.ColumnDefinitions[index].Width });
                int vindex = 0;
                foreach(Survey s in (List<Survey>)outcomesTable[a])
                {
                    surveyResults.Children.Add(new Label() { Text = s.response.ToString() }, index, vindex);
                    averages += s.response;
                    vindex++;
                }

                surveyResults.Children.Add(new Label() { Text = (averages/ vindex).ToString() }, index, ++vindex);
                index++;
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
        public async void OnOutputClicked(object sender, EventArgs e)
        {
            
            


        }

        public static void UpdateSemesters(List<Semester> newSemesters)
        {
            semesterPicker.ItemsSource = newSemesters;
            semesterPicker.SelectedIndex = 0;
        }
    }
}
