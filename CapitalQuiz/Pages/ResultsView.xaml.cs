/*
 * Program Author: Aabriti Shrestha
 * USM ID: w10171497
 * Assignment: Capital Quiz (Part 3)
 * 
 * Description: This is the code-behinf file for results view, and provides logic for displaying quiz results, handling
 * button clicks, and managing list of missed states. Also, it interacts with quiz model and updates UI based on quiz results
 */

using CapitalQuiz.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace CapitalQuiz.Pages
{
    public partial class ResultsView : ContentPage
    {
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        private ObservableCollection<MissedStateView> MissedStateViews { get; set; }
        private List<State> MissedStates { get; set; }

        public class MissedStateView
        {
            public string Text { get; set; }
        }

        public ResultsView(int score, int totalQuestions, List<State> missedStates)
        {
            InitializeComponent();
            Score = score;
            TotalQuestions = totalQuestions;
            MissedStateViews = new ObservableCollection<MissedStateView>();

            BindingContext = this;

            if (score == totalQuestions)
            {
                // All answers are correct, display the image
                winnerImage.IsVisible = true;
                resultMessage.Text = "You got all questions correct!";
            }
            else
            {
                // Not all answers are correct, hide the image
                resultMessage.Text = "You missed the following states:";
                winnerImage.IsVisible = false;

                ListMissedStates(missedStates);
            }
        }

        private void ListMissedStates(List<State> missedStates)
        {
            foreach (State state in missedStates)
            {
                string output = $"{state.CapitalName}, {state.StateName}";
                MissedStateViews.Add(new MissedStateView { Text = output });
            }

            collMissedStates.ItemsSource = MissedStateViews;
        }

        private void NewGameButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new QuizView();
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            App.Current.Quit();
        }
    }
}
