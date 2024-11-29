using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class User
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Score { get; set; } = 0;
        public User() { }
        public User( string username, string password)
        {
            UserName = username;
            Password = password;
            Score = Score;
        }
        
    }
}
