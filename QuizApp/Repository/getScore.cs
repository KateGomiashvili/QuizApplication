using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Repository
{
    internal class getScore
    {
        public int QuizScore(Quiz currentQuiz, byte[] userAnswers, User currentUser)
        {
            int score = 0;
            for (int i = 0; i < 5; i++)
            {
                if (currentQuiz.CorrectAnswers[i] == userAnswers[i])
                {
                    score += 20;
                }
                else
                {
                    score -= 20;
                }
            }
            currentUser.Score += score;
            return score;
        }
    }
}
