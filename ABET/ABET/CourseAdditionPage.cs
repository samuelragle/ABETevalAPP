using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class CourseAdditionPage : ContentPage
    {
        StackLayout courseLayout = new StackLayout();
        Button submitButton = new Button();
        Entry departmentEntry = new Entry { Placeholder = "Department Name" };
        Entry courseNumEntry = new Entry { Placeholder = "Course Number" };
        Entry courseTitleEntry = new Entry { Placeholder = "Course Title" };
        Entry sectionNumEntry = new Entry { Placeholder = "Section Number" };


        /**
         Call - await App.Current.MainPage.Navigation.PushAsync(new CourseAdditionPage());
           on add button click (in Historical Page) that will open CourseAddtionPage as a new page
        **/
        public CourseAdditionPage()
        {
            submitButton.Text = "Submit";

            courseLayout.Children.Add(departmentEntry);
            courseLayout.Children.Add(courseNumEntry);
            courseLayout.Children.Add(courseTitleEntry);
            courseLayout.Children.Add(sectionNumEntry);
            courseLayout.Children.Add(submitButton);

            submitButton.Clicked += onSubmitClicked;

            Content = courseLayout;

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