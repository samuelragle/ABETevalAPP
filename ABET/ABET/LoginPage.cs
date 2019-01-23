using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

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

            usernameEntry = new Entry();
            usernameEntry.Placeholder = "Username";

            passwordEntry = new Entry();
            passwordEntry.IsPassword = true;
            passwordEntry.Placeholder = "Password";

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