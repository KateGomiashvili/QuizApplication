using QuizApp.Models;
using Timer = System.Timers.Timer;
using System.Text.Json;
using System.Transactions;
using System.Timers;
using System.Security.AccessControl;

namespace QuizApp.Repository
{
    public class UIRepository
    {
        UserJsonRepository userRep = new UserJsonRepository(@"../../../Repository/Data/User.json");
        QuizJsonRepository quizRep = new QuizJsonRepository(@"../../../Repository/Data/Quiz.json");
        public delegate User GetcurrentUser();
        public static Timer _timer;
        public static bool _isWorking = true;
        public static User _currentUser = new();
        public void Autorization() 
        {
            
            while (true)
            {
                Console.WriteLine("1. Register\n2. Sign In\nX. Exit");
                var choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "1")
                {
                    User newUser = SignUpFunction();
                }
                else if (choice == "2")
                {
                    User newUser = SignInFunction();
                }
                else if (choice == "X") break;
                else Console.WriteLine("Invalid option. Try again.");
            }
        }
        public void Actions(User currentUser)
        {
            Console.WriteLine("Choose option:\n 1. Create Quiz \n 2. Edit your Quiz \n 3. Take a Quiz \n 4. Delete Quiz");
            string choice = Console.ReadLine();


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
                TakeQuiz(currentUser);
            }
            else if (choice == "4")
            {
                DeleteQuiz(currentUser);
            }
        }
        public void EditQuiz(User currentUser)
        {
            var userQuizzes = quizRep.GetQuizzesByAuthor(currentUser.UserName);

            if (userQuizzes.Count == 0)
            {
                Console.WriteLine("You have no quizzes to edit.");
                return;
            }

            Console.WriteLine("Your quizzes:");
            for (int i = 0; i < userQuizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizzes[i].Name}");
            }

            Console.WriteLine("Enter the number of the quiz you want to edit or 'X' to return to the menu:");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToUpper() == "X")
                {
                    Console.WriteLine("Returning to the menu...");
                    Actions(currentUser);
                    return;
                }

                if (int.TryParse(input, out int quizIndex) && quizIndex >= 1 && quizIndex <= userQuizzes.Count)
                {
                    Quiz quizToEdit = userQuizzes[quizIndex - 1];
                    Console.WriteLine($"Editing Quiz: {quizToEdit.Name}");

                    string editChoice;
                    do
                    {
                        Console.WriteLine("\nChoose an option:");
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
                        else if (editChoice.ToUpper() != "X")
                        {
                            Console.WriteLine("Invalid option. Please try again.");
                        }

                    } while (editChoice.ToUpper() != "X");

                    quizRep.SaveData();
                    Console.WriteLine("Quiz changes saved. Returning to the menu...");
                    Actions(currentUser);
                    return; 
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid quiz number or 'X' to return to the menu.");
                }
            }
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
                Console.WriteLine($"Enter {i + 1}th Question:");
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

            Console.WriteLine("Quiz created successfully");
            Quiz newQuiz = new Quiz(currentUser.UserName, quizName, questions);
            quizRep.CreateNewQuiz(newQuiz);
            Actions(currentUser);
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
                    _currentUser = currentUser;
                    Actions(_currentUser);
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
                _currentUser = newUser;
                Console.WriteLine("Registration successful.");
                Actions(_currentUser);
                return newUser;

            }
            else
            {
                Console.WriteLine("Username or password cannot be empty.");
                return null;
            }
        }

        public void DeleteQuiz(User currentUser)
        {
            var userQuizzes = quizRep.GetQuizzesByAuthor(currentUser.UserName);

            if (userQuizzes.Count == 0)
            {
                Console.WriteLine("You have no quizzes to delete.");
                return;
            }

            Console.WriteLine("Your quizzes:");
            for (int i = 0; i < userQuizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizzes[i].Name}");
            }

            Console.WriteLine("Enter the number of the quiz you want to delete or 'X' to return to the menu:");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToUpper() == "X")
                {
                    Console.WriteLine("Returning to the menu...");
                    Actions(currentUser);
                    return; 
                }

                if (int.TryParse(input, out int quizIndex) && quizIndex >= 1 && quizIndex <= userQuizzes.Count)
                {
                    Quiz quizToDelete = userQuizzes[quizIndex - 1];

                    Console.WriteLine($"Are you sure you want to delete the quiz '{quizToDelete.Name}'? (Y/N)");
                    string confirmation = Console.ReadLine();

                    if (confirmation.ToUpper() == "Y")
                    {
                        quizRep.DeleteQuiz(quizToDelete.Name); 
                        Console.WriteLine($"Quiz '{quizToDelete.Name}' has been deleted.");
                        Actions(currentUser);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Deletion canceled. Returning to the menu...");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid quiz number or 'X' to return to the menu.");
                }
            }
        }





        public void TakeQuiz(User currentUser)
        {
            Console.WriteLine("Available Quizzes:");
            var quizzes = quizRep.GetQuizzes().Where(q => q.Author != currentUser.UserName).ToList();

            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available.");
                return;
            }

            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quizzes[i].Name}");
            }

            Console.WriteLine("Select Quiz Number:");
            var EnteredquizIndex = Console.ReadLine();
            if (int.TryParse(EnteredquizIndex, out int quizIndex))
            {
                quizIndex -= 1;
                if (quizIndex >= 0 && quizIndex < quizzes.Count)
                {
                    Console.WriteLine($"You selected quiz number {quizIndex + 1}.");
                    var quiz = quizzes[quizIndex];
                    Console.WriteLine($"Starting Quiz: {quiz.Name}");
                    StartQuiz(currentUser, quiz);
                }
                else
                {
                    Console.WriteLine("Invalid quiz number. Please choose a valid number.");
                    Actions(currentUser);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Actions(currentUser);
            }
        }
        private void StartQuiz(User user, Quiz quiz)
        {
            var timer = new System.Timers.Timer(12000); //time
            var questions = quiz.Questions;
            int score = 0;
            bool timeUp = false;

            timer.Elapsed += (sender, e) =>
            {
                timeUp = true;
                timer.Stop();
                Console.WriteLine("\nTime is up! Press some key to continue");
            };

            timer.Start();

            foreach (var question in questions)
            {
                if (timeUp) break; 

                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Answers.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i]}");
                }

                Console.WriteLine("Enter your answer (1-4):");

                if (timeUp) break;
                string input = Console.ReadLine();
                if (timeUp) break;
                
                if (int.TryParse(input, out int answer) && answer >= 1 && answer <= 4)
                {
                    if (answer == question.CorrectAnswer) score += 20;
                }
                else
                {
                    Console.WriteLine("Invalid input. Skipping question.");
                }
            }

            timer.Stop();
            user.Score += score;
            Console.WriteLine($"Quiz finished. Current Score: {score}, Your total score: {_currentUser.Score}");
            Actions(_currentUser);
        }
    }
}
