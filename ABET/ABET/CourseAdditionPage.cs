using ABET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class CourseAdditionPage : ContentPage
    {
        Button submitButton = new Button();
        Button cancelButton = new Button();
        Entry departmentEntry = new Entry { Placeholder = "Department Name (Ex: CS)" };
        Entry courseNumEntry = new Entry { Placeholder = "Course Number (Ex: 4013)" };
        Entry courseTitleEntry = new Entry { Placeholder = "Course Title (Ex: Artificial Intelligence)" };
        Session session;
        


        /**
         Call - await App.Current.MainPage.Navigation.PushAsync(new CourseAdditionPage());
           on add button click (in Historical Page) that will open CourseAddtionPage as a new page
        **/
        public CourseAdditionPage()
        {
            session = App.GetSession();
            NavigationPage.SetHasNavigationBar(this, false);

            submitButton.WidthRequest = 300;
            departmentEntry.WidthRequest = 300;
            courseNumEntry.WidthRequest = 300;
            courseTitleEntry.WidthRequest = 300;
            

            submitButton.Text = "Submit";

            submitButton.Clicked += onSubmitClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,

                Children = {
                    departmentEntry,
                    courseNumEntry,
                    courseTitleEntry,
                    submitButton
                }
            };

        }

        public async void onSubmitClicked(object sender, EventArgs e)
        {
            /**
             pops the current page 
             

             pull information and populate
             Make sure the entries map the data tyupe they need to be stored to
            **/
            try
            {
                if (session.InsertCourse(departmentEntry.Text.ToUpper(), Convert.ToInt32(courseNumEntry.Text), courseTitleEntry.Text))
                {
                    await Navigation.PopAsync();
                    HistoricalPage.coursePicker.ItemsSource = null;

                    HistoricalPage.coursePicker.ItemsSource = session.Courses;
                    HistoricalPage.coursePicker.SelectedIndex = 0;
                }
                else
                {
                    // Tell user to try again
                }
            }
            catch (Exception ex)
            {
                // Likely couldn't convert string 
            }

            //update session object after updating database
        }
    }
}