using QuizApp.Repository;
using QuizApp.Models;

namespace QuizApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //User levani = new("Levani", "asdasd");

            // Initialize the repository
            QuizJsonRepository quizRep = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
            UserJsonRepository userRep = new UserJsonRepository(@"../../../Repository/Data/User.json");
            UIRepository Ui = new UIRepository();
            Ui.Autorization();

            

           




           // List<Quiz.Question> questions = new List<Quiz.Question>
           //{
           //     new Quiz.Question(
           //         "What is the largest mammal on Earth?",
           //         new[] { "Elephant", "Blue Whale", "Giraffe", "Hippopotamus" }
           //        // 1 // Correct answer: Blue Whale
           //     ),
           //     new Quiz.Question(
           //         "What process do plants use to make food?",
           //         new[] { "Respiration", "Digestion", "Photosynthesis", "Fermentation" }
           //         //2 // Correct answer: Photosynthesis
           //     ),
           //     new Quiz.Question(
           //         "Which layer of Earth is made up of tectonic plates?",
           //         new[] { "Mantle", "Crust", "Core", "Outer Core" }
           //         //1 // Correct answer: Crust
           //     ),
           //     new Quiz.Question(
           //         "Which of these is not a type of cloud?",
           //         new[] { "Cumulus", "Stratus", "Cirrus", "Nimbus" }
           //         //3 // Correct answer: Nimbus
           //     ),
           //     new Quiz.Question(
           //         "What is the chemical symbol for water?",
           //         new[] { "H2O", "O2", "CO2", "H2" }
           //         //0 // Correct answer: H2O
           //     )
           //};

            //Quiz newQuiz = new(
            //    1, // Quiz ID
            //    "Keti", // Author
            //    "Nature", // Name
            //    questions,
            //    [1, 2, 1, 3, 0]
            //);

            //  Save the new quiz to the repository
            //  quizRep.CreateNewQuiz(newQuiz);


            //Console.WriteLine("Quiz created and saved successfully!");
            //userRep.QuizScore(newQuiz, [1, 2, 1, 3, 0], levani);
            //User keti = new(1, "keti", "asdasd");
            //userRep.RegisterNewUser(keti);
            //userRep.QuizScore(newQuiz, [1, 2, 1, 3, 0], keti);
            //Console.WriteLine(userRep.QuizScore(newQuiz, quizRep.ShowQuiz, levani));
        }
    }
}
