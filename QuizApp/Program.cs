using QuizApp.Repository;
using QuizApp.Models;

namespace QuizApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QuizJsonRepository quizRep = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
            UserJsonRepository userRep = new UserJsonRepository(@"../../../Repository/Data/User.json");
            UIRepository Ui = new UIRepository();
            userRep.GetTopUsers();
            Ui.Autorization();
        }
    }
}
