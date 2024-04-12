/*
 * Program Author: Aabriti Shrestha
 * USM ID: w10171497
 * Assignment: Capital Quiz (Part 3)
 * 
 * Description: This page serves as the logic for quiz view xmal file. It handles behind-the-code
 * scene for loading questions, submitting answers, updating scores, and navigating with result page.
 */

using CapitalQuiz.Classes;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CapitalQuiz.Pages
{
    public partial class QuizView : ContentPage
    {
        private Quiz quiz;
        private QuizQuestion currentQuestion;
        private int score = 0;
        private int questionCount = 0;

        private List<State> missedStates = new List<State>();

        public QuizView()
        {
            InitializeComponent();
            quiz = new Quiz();
            score = 0;
            UpdateScoreLabel();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            currentQuestion = quiz.FetchNext();
            if (currentQuestion != null)
            {
                lblQuestion.Text = $"What is the capital of {currentQuestion.Correct?.StateName ?? ""}?";

                var options = currentQuestion.GenerateOptions();
                if (options != null && options.Count >= 4)
                {
                    rbOption1.Content = options[0]?.CapitalName ?? "";
                    rbOption2.Content = options[1]?.CapitalName ?? "";
                    rbOption3.Content = options[2]?.CapitalName ?? "";
                    rbOption4.Content = options[3]?.CapitalName ?? "";
                }
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            imgCorrect.IsVisible = false; // Show the correct icon
            imgIncorrect.IsVisible = false; // Hide the incorrect icon
            questionCount++;
            if (questionCount >= 20)
            {
                App.Current.MainPage = new ResultsView(score, 20, missedStates);
            }
            else
            {
                LoadNextQuestion();
                ResetUI();
            }
                
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {

            if (!rbOption1.IsChecked && !rbOption2.IsChecked && !rbOption3.IsChecked && !rbOption4.IsChecked)
            {
                await DisplayAlert("Error", "Please select one option!!!.", "OK");
                return;
            }

            string selectedOption = GetSelectedOption();
            if (currentQuestion != null)
            {
                if (currentQuestion.IsOptionCorrect(selectedOption))
                {
                    score++;
                    lblQuestionResult.Text = "Correct!";
                    imgCorrect.IsVisible = true; // Show the correct icon
                    imgIncorrect.IsVisible = false; // Hide the incorrect icon
                }
                else
                {
                    lblQuestionResult.Text = "Incorrect... ";
                    imgCorrect.IsVisible = false; // Hide the correct icon
                    imgIncorrect.IsVisible = true; // Show the incorrect icon
                    missedStates.Add(currentQuestion.Correct!);
                }

                UpdateScoreLabel();
            }
            else
            {
                lblQuestionResult.Text = "Error: No question found";
            }

            lblQuestionResult.IsVisible = true;
            submitButton.IsVisible = false;

            // Check if it's the 20th question
            if (questionCount >= 20)
            {
                // If it's the 20th question, navigate to ResultsView
                NextButton_Clicked(sender, e);
            }
            else
            {
                // If it's not the 20th question, show the next button
                nextButton.IsVisible = true;
            }

            DisableRadioButtons();
        }


        private string GetSelectedOption()
        {
            if (rbOption1.IsChecked)
                return rbOption1.Content?.ToString();
            else if (rbOption2.IsChecked)
                return rbOption2.Content?.ToString();
            else if (rbOption3.IsChecked)
                return rbOption3.Content?.ToString();
            else if (rbOption4.IsChecked)
                return rbOption4.Content?.ToString();
            else
                return null;
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"{score} / 20";
        }

        private void DisableRadioButtons()
        {
            rbOption1.IsEnabled = false;
            rbOption2.IsEnabled = false;
            rbOption3.IsEnabled = false;
            rbOption4.IsEnabled = false;
        }

        private void ResetUI()
        {
            lblQuestionResult.IsVisible = false;
            submitButton.IsVisible = true;
            nextButton.IsVisible = false;
            rbOption1.IsChecked = false;
            rbOption2.IsChecked = false;
            rbOption3.IsChecked = false;
            rbOption4.IsChecked = false;
            EnableRadioButtons();
        }

        private void EnableRadioButtons()
        {
            rbOption1.IsEnabled = true;
            rbOption2.IsEnabled = true;
            rbOption3.IsEnabled = true;
            rbOption4.IsEnabled = true;
        }

        private void QuitButton_Clicked(object sender, EventArgs e)
        {
            App.Current.Quit();
        }
    }
}
