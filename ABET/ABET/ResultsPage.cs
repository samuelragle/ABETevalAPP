using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class ResultsPage : ContentPage
    {
        Grid classGrid = new Grid();
        Grid buttonGrid = new Grid();
        TableView classTableView;
        ListView infoListView;
        TableSection switchCellTable;
        List<string> semesterList;
        List<Cell> temporarylist;
        Picker picker;


        //  General string array for temp use
        string[] terms = new string[]
        {
            "Spring 2015",
            "Fall 2015",
            "Spring 2016",
            "Fall 2016",
            "Spring 2017",
            "Fall 2017",
            "Spring 2018",
            "Fall 2018",
        };
        string[] classes = new string[]
{
            "Some engineering",
            "Some engineering",
            "Some engineering",
            "Some engineering",
};

        public ResultsPage()
        {

            //Create the grid for the page that displays the list of classes and pages.
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.2, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            classGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            classGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Create the grid for the Buttons in the lower left panel
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            buttonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Create the buttons to perform certain function son the information
            //along with some extra placeholder functions while we figure out what
            //other functions to include to the application
            Button rawButton = new Button();
            rawButton.Text = "Raw Data";
            Button averageButton = new Button();
            averageButton.Text = "Average";
            Button sButton = new Button();
            sButton.Text = "Function";
            Button vButton = new Button();
            vButton.Text = "Function";
            buttonGrid.Children.Add(rawButton, 0, 0);
            buttonGrid.Children.Add(averageButton, 1, 0);
            buttonGrid.Children.Add(sButton, 0, 1);
            buttonGrid.Children.Add(vButton, 1, 1);

            //Events handlers when button is clicked
            //will call corresponding function
            rawButton.Clicked += OnRawDataClicked;
            averageButton.Clicked += OnAverageSelected;


            picker = new Picker();
            semesterList = new List<string>();
            infoListView = new ListView();



            //TODO
            //really need these prepared statements
            //  get the semesters to choose from
            //      SELECT semester, year FROM semesters
            //  get the courses for each semester
            //      SELECT courseTitle, year FROM course, semesters WHERE semester = '___' and year = 
            //  get the reponses for the courses
            //      SELECT goal, reponse FROM surveys 


            //depending on what sql select statements might have to change this
            picker.ItemsSource = terms;
            picker.SelectedIndex = 0;
            picker.SelectedIndexChanged += this.PickerSemester;


            //Controls the class picker selection
            classTableView = new TableView
            {
                Root = new TableRoot { },
                Intent = TableIntent.Settings
            };

            switchCellTable = new TableSection(picker.SelectedItem.ToString()) { };
            foreach (var element in classes)
            {
                switchCellTable.Add(new SwitchCell() { Text = element });
            }
            classTableView.Root.Add(switchCellTable);


            //populates top left of results page
            classGrid.Children.Add(infoListView, 0, 0);
            Grid.SetRowSpan(infoListView, 2);

            //populates bottom left of the results page
            classGrid.Children.Add(buttonGrid, 0, 2);

            //populates top right of the result page
            classGrid.Children.Add(picker, 1, 0);

            //populates the bottom right
            classGrid.Children.Add(classTableView, 1, 1);
            Grid.SetRowSpan(classTableView, 2);

            Content = classGrid;
        }


        //here are all the button, picker, and switch functionalities.

        //Changes the list of engineering classes based on the semester chosen from the picker
        public void PickerSemester(object sender, EventArgs e)
        {
            //populate the TableView section with a switchcell representing each 
            //engineering class in the semester selected
            if (picker.SelectedItem.ToString() == classTableView.Root.ToString())
            { }
            else
            {
                classGrid.Children.RemoveAt(3);
                classTableView = new TableView
                {
                    Root = new TableRoot { },
                    Intent = TableIntent.Settings
                };

                switchCellTable = new TableSection(picker.SelectedItem.ToString()) { };
                foreach (var element in classes)
                {
                    switchCellTable.Add(new SwitchCell() { Text = element + " " + picker.SelectedItem.ToString() });
                }
                classTableView.Root.Add(switchCellTable);
                classGrid.Children.Add(classTableView, 1, 1);

            }
        }

        //Performs the Average Function on the selected classes
        public void OnAverageSelected(object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function 
            //performed on the classes (switchcells) selected in the bottom left panel
            infoListView.ItemsSource = new string[]
            {
                "average",
            };

        }

        //Outputs the complete responses from a selected class
        public void OnRawDataClicked(object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function 
            //performed on the classes (switchcells) selected in the bottom left panel
            temporarylist = switchCellTable.ToList();
            List<string> temporary = new List<string>();
            foreach (SwitchCell element in temporarylist)
            {
                if (element.On)
                {
                    temporary.Add(element.Text.ToString());
                }
            }
            if (temporary.Count == 0)
            {
                temporary.Add("empty");
            }
            infoListView.ItemsSource = temporary;
        }
    }
}
