using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ABET
{
    public class App : Application
    {

        public static bool LoggedIn { get; set; }

        public App()
        {

            
            if (!LoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new MainPage();
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
