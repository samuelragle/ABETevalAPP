using ABET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class SemesterAdditionPage : ContentPage
    {

        Button submitButton = new Button();
        Entry yearEntry = new Entry { Placeholder = "Year" };
        Session session;
        public static Picker seasonPicker = new Picker();

        /**
         Call - await App.Current.MainPage.Navigation.PushAsync(new SemesterAdditionPage());
           on add button click (in Historical Page) that will open semesterAddtionPage as a new page
        **/
        public SemesterAdditionPage()
        {
            session = App.GetSession();
            NavigationPage.SetHasNavigationBar(this, false);

            seasonPicker = new Picker();
            seasonPicker.Items.Add("T Spring");
            seasonPicker.Items.Add("T Summer");
            seasonPicker.Items.Add("T Fall");
            seasonPicker.Items.Add("T Winter");
            submitButton.Text = "Submit";

            submitButton.WidthRequest = 300;
            yearEntry.WidthRequest = 300;
            seasonPicker.WidthRequest = 300;
            
            seasonPicker.SelectedIndex = 0;
            

            submitButton.Clicked += onSubmitClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,

                Children = {
                    seasonPicker,
                    yearEntry,
                    submitButton
                }
            };

        }
        public async void onCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        public async void onSubmitClicked(object sender, EventArgs e)
        {
            /**
             pops the current page 
             App.Current.MainPage.Navigation.PopAsync();

             pull information and populate
             Make sure year Entry box is 
            **/
            try
            {
                if (session.InsertSemester(seasonPicker.SelectedItem.ToString(), Convert.ToInt32(yearEntry.Text)))
                {
                    await Navigation.PopAsync();
                    HistoricalPage.semesterPicker.ItemsSource = null;
                    ResultsPage.semesterPicker.ItemsSource = null;

                    HistoricalPage.semesterPicker.ItemsSource = session.Semesters;
                    HistoricalPage.semesterPicker.SelectedIndex = 0;
                    ResultsPage.semesterPicker.ItemsSource = session.Semesters;
                    ResultsPage.semesterPicker.SelectedIndex = 0;
                }
                else
                {
                    // Tell user to try again
                }
            }
            catch(Exception ex)
            {
                // Likely couldn't convert string 
            }

            //update session object after updating database
        }
    }
}