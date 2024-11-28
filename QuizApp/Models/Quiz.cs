using System;
using System.Collections.Generic;
using System.Text;

namespace QuizApp.Models
{
    public class Quiz
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }


        public Quiz(string author, string name, List<Question> questions)
        {
            //if (questions.Count != 5)
            //{
            //    throw new ArgumentException("A quiz must have exactly 5 questions.");
            //}

            foreach (var question in questions)
            {
                if (question.Answers.Length != 4)
                {
                    throw new ArgumentException("Each question must have exactly 4 possible answers.");
                }
            }
            Author = author;
            Name = name;
            Questions = questions;
        }
        public class Question
        {
            public string Text { get; set; }
            public string[] Answers { get; set; }
            public byte CorrectAnswer { get; set; }

            public Question(string text, string[] answers, byte correctAnswer)
            {
                if (answers.Length != 4)
                {
                    throw new ArgumentException("Each question must have exactly 4 possible answers.");
                }

                Text = text;
                Answers = answers;
                CorrectAnswer = correctAnswer;
            }
        }

        // Method to validate an answer for a specific questio

        // Override ToString for display
        //public override string ToString()
        //{
        //    var quizDetails = new StringBuilder();
        //    quizDetails.AppendLine($"Quiz: {Name} by {Author}");

        //    for (int i = 0; i < Questions.Count; i++)
        //    {
        //        quizDetails.AppendLine($"Question {i + 1}: {Questions[i].Text}");
        //        for (int j = 0; j < Questions[i].Answers.Length; j++)
        //        {
        //            quizDetails.AppendLine($"{j + 1}: {Questions[i].Answers[j]}");
        //        }
        //    }

        //    return quizDetails.ToString();
        //}
        
    }
}
