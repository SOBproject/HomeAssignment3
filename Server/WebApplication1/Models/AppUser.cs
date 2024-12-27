using System.Text.Json.Serialization;

namespace FakeSteam.BL
{
    public class AppUser
    {
        private int id;

       
        public string name;

        [JsonPropertyName("email")]
        public string email;

        [JsonPropertyName("password")]
        public string password;

        public static List<AppUser> UsersList = new List<AppUser>();
        private static int currentId = 1;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        public AppUser(string name, string email, string password)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "" : name;
            Email = email;
            Password = password;
        }
        public static bool Insert(AppUser newUser)
        {

            if (UsersList.Any(user => user.email == newUser.email))
            {
                return false;
            }

            newUser.id = currentId++;
            UsersList.Add(newUser);
            return true;
        }
        public List<AppUser> Read() 
        { 
            return UsersList;
        }
    }
}
