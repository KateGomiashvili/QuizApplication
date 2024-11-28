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

        
    }
}
