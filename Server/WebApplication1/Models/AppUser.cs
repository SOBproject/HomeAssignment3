using System.Text.Json.Serialization;
using SteamTaskServer.DAL;

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
        public int Insert()
        {
            DBServices dbs= new DBServices();
            return dbs.Insert(this);
        }
    
    }
}
