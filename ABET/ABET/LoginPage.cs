using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using ABET.Data;

namespace ABET
{
    public class LoginPage : ContentPage
    {

        public LoginPage()
        {
            
            NavigationPage.SetHasNavigationBar(this, false);

            Entry usernameEntry;
            Entry passwordEntry;
            Button loginButton;

            double loginWidth = 300; // Change to be a function of the application width
            usernameEntry = new Entry();
            usernameEntry.Placeholder = "Username";
            usernameEntry.WidthRequest = loginWidth; 

            passwordEntry = new Entry();
            passwordEntry.IsPassword = true;
            passwordEntry.Placeholder = "Password";
            passwordEntry.IsPassword = true;
            passwordEntry.WidthRequest = loginWidth;

            loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Clicked += OnLoginButtonClicked;
            

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    usernameEntry,
                    passwordEntry,
                    loginButton
                }
            };

            
            async void OnLoginButtonClicked(object sender, EventArgs e)
            {
                //verify credentials

                App.LoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                
            }
            
        }
    }
}