using ABET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ABET
{
	public class MainPage : TabbedPage
	{

        public MainPage()
        {
            Session session = App.GetSession();

            NavigationPage.SetHasNavigationBar(this, false);

            
            ResultsPage resultsPage = new ResultsPage();
            resultsPage.Title = "Results";
            HistoricalPage historicalPage = new HistoricalPage();
            historicalPage.Title = "Load Data";
            Children.Add(resultsPage);
            Children.Add(historicalPage);

           
            
        }
	}
}