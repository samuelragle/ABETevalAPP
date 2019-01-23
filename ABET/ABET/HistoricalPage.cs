using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
    public class HistoricalPage : ContentPage
    {
        public HistoricalPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Load some stuff!" }
                }
            };
        }
    }
}