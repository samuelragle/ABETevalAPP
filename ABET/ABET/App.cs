using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ABET.Data;
using System.Data.SqlClient;

namespace ABET
{
    public class App : Application
    {

        public static bool LoggedIn { get; set; }
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
            }

        }
        internal static Session GetSession()
        {
            if (Session == null) // Create the Session
            {
                string[] args = Environment.GetCommandLineArgs(); // Doesn't work, find some other way
                SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
                connBuilder.DataSource = "oudb.database.windows.net,1433";
                connBuilder.UserID = "hawk5461";
                connBuilder.Password = "CapSQLDB2019";
                connBuilder.InitialCatalog = "capDB";
                Session = new Session(connBuilder.ConnectionString, "Fall 2015");
            }
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
