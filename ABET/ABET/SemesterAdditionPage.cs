using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class SemesterAdditionPage : ContentPage
    {
        StackLayout semesterLayout = new StackLayout();
        Button submitButton = new Button();
        Entry yearEntry = new Entry { Placeholder = "Year" };
        public static Picker seasonPicker;

        /**
         Call - await App.Current.MainPage.Navigation.PushAsync(new SemesterAdditionPage());
           on add button click (in Historical Page) that will open semesterAddtionPage as a new page
        **/
        public SemesterAdditionPage()
        {
            submitButton.Text = "Submit";

            seasonPicker = new Picker();
            seasonPicker.Items.Add("Spring");
            seasonPicker.Items.Add("Summer");
            seasonPicker.Items.Add("Fall");
            seasonPicker.Items.Add("Winter");
            seasonPicker.SelectedIndex = 0;

            semesterLayout.Children.Add(seasonPicker);
            semesterLayout.Children.Add(yearEntry);
            semesterLayout.Children.Add(submitButton);

            submitButton.Clicked += onSubmitClicked;

            Content = semesterLayout;

        }

        public void onSubmitClicked(object sender, EventArgs e)
        {
            /**
             pops the current page 
             App.Current.MainPage.Navigation.PopAsync();

             pull information and populate
             Make sure year Entry box is 
            **/
        }
    }
}