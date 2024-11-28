using QuizApp.Models;
using System.Transactions;

namespace QuizApp.Repository
{
    public class UIRepository
    {
        UserJsonRepository userRep = new UserJsonRepository(@"../../../Repository/Data/User.json");
        QuizJsonRepository quizRep = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
        public delegate User GetcurrentUser();
        public void Autorization()
        {
            string sign;
            User currentUser = null;

            do
            {
                Console.WriteLine("Welcome! Enter R to register a new user, S to Sign In, or X to Exit:");
                sign = Console.ReadLine()?.ToUpper();

                if (sign == "R")
                {
                    User newUser = SignUpFunction();
                    Actions(newUser);
                }
                else if (sign == "S")
                {
                   User newUser =  SignInFunction();
                    Actions(newUser);
                }
                else if (sign != "X")
                {
                    Console.WriteLine("Invalid option. Please enter R, S, or X.");
                }
            } while (sign != "X");
        }
        public void Actions(User currentUser)
        {
            Console.WriteLine("Choose option:\n 1. Create Quiz \n 2. Edit you Quiz \n 3. Take a Quiz ");
            var choice = Console.ReadLine();
            if (choice == "1") {
                Console.WriteLine("Enter Quiz name");
                string quizName = Console.ReadLine();
                List<Quiz.Question> questions = new List<Quiz.Question>();
                for (int i=0; i<5; i++)
                {
                    Console.WriteLine("Enter Question:");
                    string questionText = Console.ReadLine();
                    string[] answers = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        Console.WriteLine($"Enter Answer {j + 1}:");
                        answers[j] = Console.ReadLine();
                    }
                    Console.WriteLine("Enter the number of the correct answer (1-4):");
                    byte correctAnswerIndex;
                    while (!byte.TryParse(Console.ReadLine(), out correctAnswerIndex) || correctAnswerIndex < 1 || correctAnswerIndex > 4)
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                    }

                    // Subtract 1 because user input is 1-based, but array index is 0-based
                    questions.Add(new Quiz.Question(questionText, answers, correctAnswerIndex));
                }
                Quiz newQuiz = new Quiz(currentUser.UserName, quizName, questions);
                quizRep.CreateNewQuiz(newQuiz);
            }
        }
        
        public User SignInFunction()
        {
            Console.WriteLine("Enter your Username:");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                return null; // Explicitly return a null User object
            }

            if (userRep.UserExists(userName))
            {
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();

                if (userRep.SignIn(userName, password))
                {
                    User currentUser = userRep.GetUser(userName);
                    return currentUser; // Return the user if SignIn succeeds
                }
                else
                {
                    Console.WriteLine("Incorrect password.");
                    return null; // Return null if password is incorrect
                }
            }
            else
            {
                Console.WriteLine("User not found.");
                return null; // Return null if user doesn't exist
            }
        }

        public User SignUpFunction()
        {
            Console.WriteLine("Enter your Username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                User newUser = new(userName, password);
                userRep.RegisterNewUser(newUser);
                Console.WriteLine("Welcome!");
                return newUser;
                
            }
            else
            {
                Console.WriteLine("Username or password cannot be empty.");
                return null;
            }
        }
        //public int QuizScore(Quiz currentQuiz, byte[] userAnswers, User currentUser)
        //{
        //    int score = 0;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (currentQuiz.CorrectAnswers[i] == userAnswers[i])
        //        {
        //            score += 20;
        //        }
        //        else
        //        {
        //            score -= 20;
        //        }
        //    }
        //    currentUser.Score += score;
        //    return score;
        //}

        public byte[] ShowQuiz(Quiz quiz)
        {
            List<byte> answerArr = new List<byte>();
            foreach (var question in quiz.Questions)
            {
                Console.WriteLine($"{question.Text}");
                for (var i = 1; i < 5; i++)
                {
                    Console.WriteLine($"{i}) {question.Answers[i - 1]}");
                }
                Console.WriteLine("Enter Your Answer");
                string input = Console.ReadLine();

                if (byte.TryParse(input, out byte userAnswer) && userAnswer >= 0 && userAnswer <= 3)
                {
                    answerArr.Add(userAnswer);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 3.");
                }
            }
            return answerArr.ToArray();

        }


    }
}
