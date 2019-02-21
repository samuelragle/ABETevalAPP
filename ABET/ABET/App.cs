using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ABET.Data;

namespace ABET
{
    public class App : Application
    {

        public static bool LoggedIn { get; set; }
        public static Session Session { get; set; }

        public App()
        {

            
            if (!LoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new MainPage();
                Session = new Session("","Fall 2015");
            }
            

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            // Load data in


        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            //Save app state
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
