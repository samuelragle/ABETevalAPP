using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class QuizPage : ContentPage
    {
        public QuizPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Quiz Control" }
                }
            };
        }
    }
}