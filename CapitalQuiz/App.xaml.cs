/*
 * Program Author: Aabriti Shrestha
 * USM ID: w10171497
 * Assignment: Capital Quiz (Part 3)
 * 
 * Description: It serves as the main entry point to handle the application. It sets initial page,
 * initializes essential components and mangages the navigation between different pages.
 */

using System.Diagnostics;

namespace CapitalQuiz
{
    public partial class App : Application
    {
        public static List<Classes.State> states = new List<Classes.State>();
        public App()
        {
            InitializeComponent();
            LoadStates();

            MainPage = new Pages.QuizView();

            //TestResult();
        }

        private void TestResult()
        {


            int score = 0;
            int overall = 20;
            List<Classes.State> Missed = new List<Classes.State>();

            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                int chosen = r.Next(0, App.states.Count);
                Missed.Add(App.states[chosen]);
            }

           //MainPage = new Pages.ResultsView(score, overall, Missed); //new AppShell();
        }


        private static void LoadStates()
        {
            try
            {
                //If this does not work, you may change the file location here
                //to where your file is located on your machine.
                var stream = FileSystem.OpenAppPackageFileAsync("StateCapitals.txt");
                var result = stream.Result;
                var file = new StreamReader(result);

                string? line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(" ");
                    data[0] = data[0].Replace("-", " ");
                    data[1] = data[1].Replace("-", " ");
                    Classes.State state = new Classes.State { StateName = data[0], CapitalName = data[1] };
                    states.Add(state);
                }

                file.Close();
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Could not load file.");
            }
        }
    }

}
