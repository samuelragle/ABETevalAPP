using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class ResultsPage : ContentPage
    { 
        public ResultsPage()
        {

            //Create the grid for the page that displays the list of classes and pages.
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Create the grid for the Buttons in the lower left panel
            Grid newGrid = new Grid();
            newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

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
            newGrid.Children.Add(rawButton, 0, 0);
            newGrid.Children.Add(averageButton, 1, 0);
            newGrid.Children.Add(sButton, 0, 1);
            newGrid.Children.Add(vButton, 1, 1);

            //placeholder picker for demo purposes
            Picker picker = new Picker();
            List<string> semesterList = new List<string>();
            semesterList.Add("Fall 2015");
            semesterList.Add("Spring 2016");
            semesterList.Add("Fall 2016");
            semesterList.Add("Spring 2017");
            semesterList.Add("Fall 2017");
            semesterList.Add("Spring 2018");
            semesterList.Add("Fall 2018");
            picker.ItemsSource = semesterList;
            picker.SelectedIndex = 0;

            //placeholder list of switches for demo purposes
            TableView section = new TableView
            {
                Root = new TableRoot {
                     new TableSection("Fall 2015") { 
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },
                        new SwitchCell () {Text = "Some Engineering Class" },

                     }
                },
                Intent = TableIntent.Settings
            };

            //placeholder list of data
            //this will most likely need to change to accomodate the layout of information in the 
            //the ABET classes.
            ListView listView = new ListView();
            listView.ItemsSource = new string[]
            {
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
            };

            //populates top left of results page
            grid.Children.Add(listView, 0, 0);
            Grid.SetRowSpan(listView, 2);

            //populates bottom left of the results page
            grid.Children.Add(newGrid, 0, 2);

            //populates top right of the result page
            grid.Children.Add(picker, 1, 0);

            //populates the bottom right
            grid.Children.Add(section, 1, 1);
            Grid.SetRowSpan(section, 2);

            Content = grid;
        }


        //here are all the button, picker, and switch functionalities.

        //Changes the list of engineering classes based on the semester chosen from the picker
        async void OnSemesterSelected (object sender, EventArgs e)
        {
            //populate the TableView section with a switchcell representing each 
            //engineering class in the semester selected
        }

        //Performs the Average Function on the selected classes
        async void OnAverageSelected (object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function 
            //performed on the classes (switchcells) selected in the bottom left panel
        }

        //Outputs the complete responses from a selected class
        async void OnRawDataSelected (object sender, EventArgs e)
        {
            //Populate the top left panel with the results of the function 
            //performed on the classes (switchcells) selected in the bottom left panel
        }
    }
}