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

        public AppUser()
        {
           
        }
        public AppUser(string name, string email, string password)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "" : name;
            Email = email;
            Password = password;
        }

        public bool Login(string email,string password)
        {
            DBServices dbs = new DBServices();
           
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            foreach (AppUser u in dbs.ReadUsers())
            {
                if(u.Email == email && u.Password == password)
                { return true; }
            }
            return false;
        }
        
        public int GetUserID(string email)
        {
            DBServices dbs = new DBServices();
            foreach (AppUser u in dbs.ReadUsers())
            {
                if (u.Email == email)
                { return u.Id; }
            }
            return 0; 
        }
        public int Insert()
        {
            DBServices dbs= new DBServices();
            return dbs.Insert(this);
        }
    
        public int Update()
        {
            DBServices dbs = new DBServices();
            return dbs.Update(this);

        }
        static public List<AppUser> Read()
        {
            DBServices dbs = new DBServices();
            return dbs.ReadUsers();
        }
    }
}
