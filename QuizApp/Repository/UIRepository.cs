using QuizApp.Models;
using System.Text.Json;
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
                Console.WriteLine("Welcome! Choose 1 for SignUp, 2 for SignIn, or X to Exit:");
                sign = Console.ReadLine();

                if (sign == "1")
                {
                    User newUser = SignUpFunction();
                    if (newUser != null) { Actions(newUser); }

                }
                else if (sign == "2")
                {
                    User newUser = SignInFunction();
                    if (newUser != null) { Actions(newUser); }
                }
                else if (sign != "X")
                {
                    Console.WriteLine("Invalid option. Please enter R, S, or X.");
                }
            } while (sign != "X");
        }
        public delegate byte[] ShowQuizDelegate(Quiz quiz);
        public void Actions(User currentUser)
        {
            Console.WriteLine("Choose option:\n 1. Create Quiz \n 2. Edit your Quiz \n 3. Take a Quiz ");
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                newQuizInputs(currentUser);
            }
            else if (choice == "2")
            {
                EditQuiz(currentUser);
            }
            else if (choice == "3")
            {
                quizRep.GetQuizzesByAuthor(currentUser.UserName);
                Console.WriteLine("Choose Quiz and enter its Name:");
                string currentQuizName = Console.ReadLine();
                Quiz currentQuiz = quizRep.GetQuiz(currentQuizName);
                QuizScore(currentQuiz, TakeQuiz(currentQuiz), currentUser);
            }
        }
        public void EditQuiz(User currentUser)
        {
            Console.WriteLine("Enter the name of the quiz you want to edit:");
            string quizName = Console.ReadLine();

            Quiz quizToEdit = quizRep.GetQuiz(quizName);

            if (quizToEdit == null || quizToEdit.Author != currentUser.UserName)
            {
                Console.WriteLine("Quiz not found.");
                return;
            }

            Console.WriteLine($"Editing Quiz: {quizToEdit.Name}");
            string editChoice;
            do
            {
                Console.WriteLine("\n Choose option by index:");
                Console.WriteLine("1. Edit Question");
                Console.WriteLine("2. Edit Answer");
                Console.WriteLine("3. Change Correct Answer");
                Console.WriteLine("X. Exit Editing");

                editChoice = Console.ReadLine();

                if (editChoice == "1")
                {
                    EditQuestion(quizToEdit);
                }
                else if (editChoice == "2")
                {
                    EditAnswer(quizToEdit);
                }
                else if (editChoice == "3")
                {
                    EditCorrectAnswer(quizToEdit);
                }

            } while (editChoice != "X");
            quizRep.SaveData();
        }

        public void EditQuestion(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to edit (1-5):");
            int questionNumber = Convert.ToInt32(Console.ReadLine()) - 1;

            if (questionNumber >= 0 && questionNumber < quizToEdit.Questions.Count)
            {
                Console.WriteLine($"Current Question: {quizToEdit.Questions[questionNumber].Text}");
                Console.WriteLine("Enter new question text:");
                string newQuestionText = Console.ReadLine();
                quizToEdit.Questions[questionNumber].Text = newQuestionText;
            }
            else
            {
                Console.WriteLine("Enter valid Question number");
            }
        }

        public void EditAnswer(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to edit answers for (1-5):");
            int questionNumber = Convert.ToInt32(Console.ReadLine()) - 1;

            if (questionNumber >= 0 && questionNumber < quizToEdit.Questions.Count)
            {
                var question = quizToEdit.Questions[questionNumber];
                Console.WriteLine("Current Answers:");
                for (int i = 0; i < question.Answers.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i]}");
                }

                Console.WriteLine("Enter the answer number (1-4) you want to change:");
                int answerNumber = Convert.ToInt32(Console.ReadLine()) - 1;

                if (answerNumber >= 0 && answerNumber < question.Answers.Length)
                {
                    Console.WriteLine($"Current Answer: {question.Answers[answerNumber]}");
                    Console.WriteLine("Enter new answer:");
                    string newAnswer = Console.ReadLine();
                    question.Answers[answerNumber] = newAnswer;
                }
                else
                {
                    Console.WriteLine("Enter valid index of Answer.");
                }
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }
        }

        public void EditCorrectAnswer(Quiz quizToEdit)
        {
            Console.WriteLine("Enter the question number you want to change the correct answer for (1-5):");
            int questionNumber = Convert.ToInt32(Console.ReadLine()) - 1;

            if (questionNumber >= 0 && questionNumber < quizToEdit.Questions.Count)
            {
                var question = quizToEdit.Questions[questionNumber];
                Console.WriteLine($"Current Correct Answer: {question.Answers[question.CorrectAnswer - 1]}");
                Console.WriteLine("Enter the number of the correct answer (1-4):");
                byte correctAnswerIndex;
                while (!byte.TryParse(Console.ReadLine(), out correctAnswerIndex) || correctAnswerIndex < 1 || correctAnswerIndex > 4)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                }

                question.CorrectAnswer = correctAnswerIndex;
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }
        }

        public void newQuizInputs(User currentUser)
        {
            Console.WriteLine("Enter Quiz name");
            string quizName = Console.ReadLine();
            List<Quiz.Question> questions = new List<Quiz.Question>();
            for (int i = 0; i < 5; i++)
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


                questions.Add(new Quiz.Question(questionText, answers, correctAnswerIndex));
            }
            Quiz newQuiz = new Quiz(currentUser.UserName, quizName, questions);
            quizRep.CreateNewQuiz(newQuiz);
        }

        public User SignInFunction()
        {
            Console.WriteLine("Enter your Username:");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                return null;
            }

            if (userRep.UserExists(userName))
            {
                Console.WriteLine("Enter password:");
                string password = Console.ReadLine();

                if (userRep.SignIn(userName, password))
                {
                    User currentUser = userRep.GetUser(userName);
                    return currentUser;
                }
                else
                {
                    Console.WriteLine("Incorrect password.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("User not found.");
                return null;
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
        public int QuizScore(Quiz currentQuiz, byte[] userAnswers, User currentUser)
        {
            int score = 0;
            for (int i = 0; i < 5; i++)
            {
                if (currentQuiz.Questions[i].CorrectAnswer == userAnswers[i])
                {
                    score += 20;
                }
                else
                {
                    score -= 20;
                }
            }
            currentUser.Score += score;
            userRep.SaveData();
            Console.WriteLine($"Your score is {currentUser.Score}");
            return score;
        }

        public byte[] TakeQuiz(Quiz quiz)
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
            Console.WriteLine("Finished");
            return answerArr.ToArray();

        }


    }
}
