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
        public static Picker seasonPicker = new Picker();

        /**
         Call - await App.Current.MainPage.Navigation.PushAsync(new SemesterAdditionPage());
           on add button click (in Historical Page) that will open semesterAddtionPage as a new page
        **/
        public SemesterAdditionPage()
        {
            submitButton.Text = "Submit";

            submitButton.WidthRequest = 300;
            yearEntry.WidthRequest = 300;
            seasonPicker.WidthRequest = 300;
            
            seasonPicker.Items.Add("Spring");
            seasonPicker.Items.Add("Summer");
            seasonPicker.Items.Add("Fall");
            seasonPicker.Items.Add("Winter");
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

        public async void onSubmitClicked(object sender, EventArgs e)
        {
            /**
             pops the current page 
             App.Current.MainPage.Navigation.PopAsync();

             pull information and populate
             Make sure year Entry box is 
            **/

            await Navigation.PopAsync();

            //update session object after updating database
        }
    }
}