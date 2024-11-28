using System.Text.Json;
using System.Xml.Serialization;
using QuizApp.Models;

namespace QuizApp.Repository
{

    public class QuizJsonRepository
    {
        private readonly string _filePath;
        private readonly List<Quiz> _quizzes;

        public QuizJsonRepository(string filePath)
        {
            _filePath = filePath;
            _quizzes = LoadData();
        }
        public List<Quiz> GetQuizzes() => _quizzes;
        public Quiz GetQuiz(string quizName) => _quizzes.FirstOrDefault(acc => acc.Name == quizName);
        public void CreateNewQuiz(Quiz quiz)
        {
            if (!_quizzes.Any(x => x.Name == quiz.Name))
            {
                _quizzes.Add(quiz);
                SaveData();
            }
            else Console.WriteLine("Quiz with such Name already exists.");
        }
        public void SaveData()
        {

            var json = JsonSerializer.Serialize(_quizzes, new JsonSerializerOptions { WriteIndented = true });

            using (var writer = new StreamWriter(_filePath, false))
                writer.Write(json);
        }

        private List<Quiz> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<Quiz>();

            using (var reader = new StreamReader(_filePath))
            {
                var json = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(json))
                { return new List<Quiz>(); }
                else
                { return new List<Quiz>(); }
            }
        }
        

    }
}

