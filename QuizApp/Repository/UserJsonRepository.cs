using System.Text.Json;
using System.Xml.Serialization;
using QuizApp.Models;

namespace QuizApp.Repository
{

    public class UserJsonRepository
    {
        private readonly string _filePath;
        private readonly List<User> _users;

        public UserJsonRepository(string filePath)
        {
            _filePath = filePath;
            _users = LoadData();
        }
        public List<User> GetUsers() => _users;
        public User GetUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.UserName==username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
            }
            return user;
        }
        public bool UserExists(string userName)
        {
            if (_users.Any(x => x.UserName == userName))
            {
                return true;
            } else { return false; }
        }

        public bool SignIn(string userName, string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username and password cannot be empty.");
                return false;
            }

            // Fetch the user
            User currentUser = _users.FirstOrDefault(acc => acc.UserName == userName);

            if (currentUser == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            // Check password
            if (currentUser.Password != password)
            {
                Console.WriteLine("Incorrect Password.");
                return false;
            }

            // Successful sign-in
            Console.WriteLine($"Welcome {userName}!");
            return true;
        }

        public void RegisterNewUser(User user)
        {
            if (!_users.Any(x => x.UserName == user.UserName))
            {
                _users.Add(user);
                SaveData();
            }
            else Console.WriteLine($"This username already exists.");
        }
        public void SaveData()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });

            using (var writer = new StreamWriter(_filePath, false)) // Overwrite the file
            {
                writer.Write(json);
            }
        }


        private List<User> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    var json = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return new List<User>();
                    }
                    else
                        return JsonSerializer.Deserialize<List<User>>(json);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error loading JSON data: {ex.Message}");
                return new List<User>();
            }
        }

    }
}

