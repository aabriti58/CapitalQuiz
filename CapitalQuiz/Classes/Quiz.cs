/*
 * Program Author: Aabriti Shrestha
 * USM ID: w10171497
 * Assignment: Capital Quiz (Part 3)
 * 
 * Description: This file manages the logic behind the quiz including managing quiz questions, 
 * generating new questions, tracking missed states, generating options, and fetching next question.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CapitalQuiz.Classes
{
    public class State
    {
        public string StateName { get; set; } = "";
        public string CapitalName { get; set; } = "";
        public object Capital { get; internal set; } = "";
    }

    public class QuizQuestion
    {
        private State? _correct = null;
        private List<State> _distractors = new List<State>();

        public QuizQuestion(State? correct = null)
        {
            _correct = correct;

            if (correct != null)
                SetDistractors();
        }

        public State? Correct
        {
            get { return _correct; }
            set
            {
                _correct = value;
                SetDistractors();
            }
        }

        public List<State> GenerateOptions()
        {
            List<State> options = new List<State>();

            // Check if _correct is null
            if (_correct != null)
            {
                options.Add(_correct);
                options.AddRange(_distractors);
                Shuffle(options, 10);
            }

            return options;
        }


        public bool IsOptionCorrect(string option)
        {
            if (_correct != null && _correct.CapitalName != null && option != null)
            {
                return _correct.CapitalName.Equals(option);
            }
            return false;
        }




        private void Shuffle(List<State> options, int passes)
        {
            // Shuffle the List.
            Random random = new Random();
            for (int i = 0; i < passes; i++)
            {
                int x = random.Next(options.Count);
                int y = random.Next(options.Count);
                State temp = options[x];
                options[x] = options[y];
                options[y] = temp;
            }
        }

        private void SetDistractors()
        {
            if (_correct == null)
                return;

            // Called when the correct state is changed.
            List<State> copy = new List<State>(App.states);
            copy.Remove(_correct);
            List<State> final = new List<State>();
            Random r = new Random();

            for (int i = 0; i < 3; i++)
            {
                int chosen = r.Next(0, copy.Count);
                final.Add(copy[chosen]);
                copy.RemoveAt(chosen);
            }

            _distractors = final;
        }
    }

    public class Quiz
    {
        public Queue<QuizQuestion> questionQueue = new Queue<QuizQuestion>();
        int _count;

        public Quiz(int size = 20)
        {
            _count = size;
            GenerateNewQuiz(size);
        }

        public void GenerateNewQuiz(int size)
        {
            questionQueue.Clear();
            List<State> copy = new(App.states);
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                int index = r.Next(0, copy.Count);
                State state = copy[index];
                copy.RemoveAt(index);

                QuizQuestion question = new QuizQuestion { Correct = state };
                questionQueue.Enqueue(question);

            }
        }

        public bool IsQuizCompleted()
        {
            return questionQueue.Count == 0;
        }

        public List<State> GetMissedStates()
        {
            List<State> missedStates = new List<State>();
            foreach (var question in questionQueue)
            {
                // Check if the question was answered incorrectly
                if (!question.IsOptionCorrect(question.Correct?.CapitalName))
                {
                    missedStates.Add(question.Correct!);
                }
            }
            return missedStates;
        }



        public QuizQuestion? FetchNext()
        {
            if (questionQueue.Count == 0)
                return null;
            else
                return questionQueue.Dequeue();
        }

        public int Count { get { return _count; } }
    }
}
