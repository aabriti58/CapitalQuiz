using CapitalQuiz.Pages;
using Microsoft.Maui.Controls;
using System;

namespace CapitalQuiz
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void StartNewGame_Clicked(object sender, EventArgs e)
        {
            // Navigate to the QuizView page to start a new game
            QuizView? quizView = new QuizView(); // Nullable annotation added
            if (quizView != null) // Added null check
            {
                App.Current.MainPage = quizView;
            }
        }

        private void Quit_Clicked(object sender, EventArgs e)
        {
            App.Current.Quit();
        }
    }
}
