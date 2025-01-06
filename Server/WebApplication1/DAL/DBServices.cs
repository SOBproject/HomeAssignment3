using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using FakeSteam.BL;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace SteamTaskServer.DAL
{
    public class DBServices
    {
//Basic connections
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("igroup16_test2");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedureUser(String spName, SqlConnection con, AppUser user)
        {

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con; 

            cmd.CommandText = spName;  

            cmd.CommandTimeout = 10; 

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", user.Id);

            cmd.Parameters.AddWithValue("@Name", user.Name);

            cmd.Parameters.AddWithValue("@Email", user.Email);

            cmd.Parameters.AddWithValue("@Password", user.Password);


            return cmd;
        }

        //User functions
        public int Insert(AppUser user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("igroup16_test2"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Name", user.Name);
        paramDic.Add("@Email", user.Email);
        paramDic.Add("@Password", user.Password);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UserRegistration", con, paramDic);          // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

        public int Update(AppUser user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedureUser("SP_UpdateUserDetails ", con, user);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public List<AppUser> ReadUsers()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<AppUser> users = new List<AppUser>();

            cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadUsers", con, null );          // create the command

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                AppUser u = new AppUser();
                u.Id = Convert.ToInt32(dataReader["UserID"]);
                u.Name = dataReader["Name"].ToString();
                u.Email = dataReader["Email"].ToString();
                u.Password = dataReader["Password"].ToString();
                users.Add(u);
            }
            return users;
        }

//Game functions
        public List<Game> ReadGames()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGames", con, null);          // create the command

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();
                g.appid = Convert.ToInt32(dataReader["AppID"]);
                g.name = dataReader["Name"].ToString();
                g.releaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                g.price = Convert.ToDouble(dataReader["Price"]);
                g.description = dataReader["description"].ToString();
                g.fullAudioLanguages = dataReader["Full_audio_languages"].ToString();
                g.headerImage = dataReader["Header_image"].ToString();
                g.website = dataReader["Website"].ToString();
                g.windows = Convert.ToBoolean(dataReader["Windows"]);
                g.mac = Convert.ToBoolean(dataReader["Mac"]);
                g.linux = Convert.ToBoolean(dataReader["Linux"]);
                g.scoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                g.recommendations = dataReader["Recommendations"].ToString();
                g.developers = dataReader["Developers"].ToString();
                g.categories = dataReader["Categories"].ToString();
                g.genres = dataReader["Genres"].ToString();
                g.tags = dataReader["Tags"].ToString();
                g.screenshots = dataReader["Screenshots"].ToString();
                g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                games.Add(g);
            }
            return games;
        }

        public List<Game> GetUserGameList(int userID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> userGameList = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetUserGames", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.appid = Convert.ToInt32(dataReader["AppID"]);
                    g.name = dataReader["Name"].ToString();
                    g.releaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.price = Convert.ToDouble(dataReader["Price"]);
                    g.description = dataReader["description"].ToString();
                    g.fullAudioLanguages = dataReader["Full_audio_languages"].ToString();
                    g.headerImage = dataReader["Header_image"].ToString();
                    g.website = dataReader["Website"].ToString();
                    g.windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.scoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.recommendations = dataReader["Recommendations"].ToString();
                    g.developers = dataReader["Developers"].ToString();
                    g.categories = dataReader["Categories"].ToString();
                    g.genres = dataReader["Genres"].ToString();
                    g.tags = dataReader["Tags"].ToString();
                    g.screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    userGameList.Add(g);
                }
                return userGameList;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public List<Game> GetPricedAndRankedList(int userID,float minPrice,int minRank)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> filteredGameList = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);
            paramDic.Add("@minPrice", minPrice);
            paramDic.Add("@minRank", minRank);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetPricedAndRankedList", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.appid = Convert.ToInt32(dataReader["AppID"]);
                    g.name = dataReader["Name"].ToString();
                    g.releaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.price = Convert.ToDouble(dataReader["Price"]);
                    g.description = dataReader["description"].ToString();
                    g.fullAudioLanguages = dataReader["Full_audio_languages"].ToString();
                    g.headerImage = dataReader["Header_image"].ToString();
                    g.website = dataReader["Website"].ToString();
                    g.windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.scoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.recommendations = dataReader["Recommendations"].ToString();
                    g.developers = dataReader["Developers"].ToString();
                    g.categories = dataReader["Categories"].ToString();
                    g.genres = dataReader["Genres"].ToString();
                    g.tags = dataReader["Tags"].ToString();
                    g.screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    filteredGameList.Add(g);
                }
                return filteredGameList;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        public List<Game> GetPricedList(int userID, float minPrice)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> filteredGameList = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);
            paramDic.Add("@minPrice", minPrice);
           

            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetPricedAndRankedList", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.appid = Convert.ToInt32(dataReader["AppID"]);
                    g.name = dataReader["Name"].ToString();
                    g.releaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.price = Convert.ToDouble(dataReader["Price"]);
                    g.description = dataReader["description"].ToString();
                    g.fullAudioLanguages = dataReader["Full_audio_languages"].ToString();
                    g.headerImage = dataReader["Header_image"].ToString();
                    g.website = dataReader["Website"].ToString();
                    g.windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.scoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.recommendations = dataReader["Recommendations"].ToString();
                    g.developers = dataReader["Developers"].ToString();
                    g.categories = dataReader["Categories"].ToString();
                    g.genres = dataReader["Genres"].ToString();
                    g.tags = dataReader["Tags"].ToString();
                    g.screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    filteredGameList.Add(g);
                }
                return filteredGameList;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        public List<Game> GetRankedList(int userID, int minRank)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> filteredGameList = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);
            paramDic.Add("@minRank", minRank);


            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetPricedAndRankedList", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.appid = Convert.ToInt32(dataReader["AppID"]);
                    g.name = dataReader["Name"].ToString();
                    g.releaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.price = Convert.ToDouble(dataReader["Price"]);
                    g.description = dataReader["description"].ToString();
                    g.fullAudioLanguages = dataReader["Full_audio_languages"].ToString();
                    g.headerImage = dataReader["Header_image"].ToString();
                    g.website = dataReader["Website"].ToString();
                    g.windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.scoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.recommendations = dataReader["Recommendations"].ToString();
                    g.developers = dataReader["Developers"].ToString();
                    g.categories = dataReader["Categories"].ToString();
                    g.genres = dataReader["Genres"].ToString();
                    g.tags = dataReader["Tags"].ToString();
                    g.screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases= Convert.ToInt32(dataReader["numberOfPurchases"]);
                    filteredGameList.Add(g);
                }
                return filteredGameList;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public int AddGameToList(int userID,int appID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2"); 
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);
            paramDic.Add("@GameID", appID);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_BuyGame", con, paramDic);          // create the command

            try
            {
               List<Game> userGames = Game.Read(userID);
                if(userGames.Any(g=> g.appid==appID))
                {
                    return -1;

                }

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        public int DeleteGameFromList(int userID, int appID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup16_test2");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userID", userID);
            paramDic.Add("@GameID", appID);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_DeleteGame", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
    }
}   
