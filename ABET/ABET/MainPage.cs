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

            NavigationPage.SetHasNavigationBar(this, false);

            
            ResultsPage resultsPage = new ResultsPage();
            resultsPage.Title = "Results";
            QuizPage quizPage = new QuizPage();
            quizPage.Title = "Quizzes";
            HistoricalPage historicalPage = new HistoricalPage();
            historicalPage.Title = "Load Historical Data";

            Children.Add(resultsPage);
            Children.Add(quizPage);
            Children.Add(historicalPage);

           
            
        }
	}
}