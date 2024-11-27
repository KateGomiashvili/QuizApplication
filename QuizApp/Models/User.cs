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
        //public int Id { get; set; }
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Score { get; set; } = 0;
        public User( string username, string password)
        {
           // Id = id;
            UserName = username;
            Password = password;
            Score = Score;
        }
        
    }
    /*
    public class AccountEquilityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y) => x.UserName == y.UserName &&
                x.Password == y.Password;

        public int GetHashCode([DisallowNull] User obj) => obj.Id;
    }
    */
}
