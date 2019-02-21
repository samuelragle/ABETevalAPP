using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ABET.Data;

namespace ABET
{
    public class App : Application
    {

        public static bool LoggedIn { get; set; }
        private static string connString; // produce from some sort of args
        private static Session Session;

        public App()
        {

            
            if (!LoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new MainPage();
                Session = new Session(connString,"Fall 2015");
            }
            

        }
        public Session GetSession()
        {
            return Session;
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
