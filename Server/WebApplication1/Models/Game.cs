using System.Text.Json;
using System.Text.Json.Serialization;

namespace FakeSteam.BL
{
    public class Game
    {
        [JsonPropertyName("AppID")]
        public int appid { get; set; }

        [JsonPropertyName("Name")]
        public string name{ get; set; }

        [JsonPropertyName("Release_date")]
        public DateTime releaseDate{ get; set; }

        [JsonPropertyName("Price")]
        public double price{ get; set; }

        [JsonPropertyName("description")]
        public string description{ get; set; }

        [JsonPropertyName("Full_audio_languages")]
        public string fullAudioLanguages { get; set; }

        [JsonPropertyName("Header_image")]
        public string headerImage{ get; set; }

        [JsonPropertyName("Website")]
        public string website { get; set; }

        [JsonPropertyName("Windows")]
        public Boolean windows { get; set; }

        [JsonPropertyName("Mac")]
        public Boolean mac { get; set; }

        [JsonPropertyName("Linux")]
        public Boolean linux { get; set; }

        [JsonPropertyName("scoreRank")]
        public int scoreRank { get; set; }

        [JsonPropertyName("Recommendations")]
        public string recommendations { get; set; }

        [JsonPropertyName("Developers")]
        public string developers { get; set; }

        [JsonPropertyName("Categories")]
        public string categories { get; set; }

        [JsonPropertyName("Genres")]
        public string genres { get; set; }

        [JsonPropertyName("Tags")]
        public string tags { get; set; }

        [JsonPropertyName("Screenshots")]
        public string screenshots { get; set; }



        public static List<Game> gamesList = new List<Game>();

        public Game(int appid, string name, DateTime releaseDate, double price, string description, string fullAudioLanguages, string headerImage, string website, bool windows, bool mac, bool linux, int scoreRank, string recommendations, string developers, string categories, string genres, string tags, string screenshots)
        {
            this.appid = appid;
            this.name = name;
            this.releaseDate = releaseDate;
            this.price = price;
            this.description = description;
            this.fullAudioLanguages = fullAudioLanguages;
            this.headerImage = headerImage;
            this.website = website;
            this.windows = windows;
            this.mac = mac;
            this.linux = linux;
            this.scoreRank = scoreRank;
            this.recommendations = recommendations;
            this.developers = developers;
            this.categories = categories;
            this.genres = genres;
            this.tags = tags;
            this.screenshots = screenshots;
        }

        public static bool Insert(Game NewGame)
        {
            if (gamesList.Any(game => game.appid == NewGame.appid || game.name == NewGame.name))
            {
                return false;
            }

            gamesList.Add(NewGame);
            return true;
        }

        public static bool DeleteById(int id)
        {

            for (int i = 0; i < gamesList.Count; i++)
            {
                if (gamesList[i].appid == id)
                {
                    gamesList.RemoveAt(i);
                    return true; 
                }
            }
            return false;
        }
        public static List<Game> Read()
        {
            return gamesList;
        }

        public List<Game> GetByPrice(float MinPrice)
        {
            List<Game> selectedGames = new List<Game>();
            foreach (Game game in gamesList)
            {
                if (game.price > MinPrice)
                {
                    selectedGames.Add(game);

                }
            }
            return selectedGames;
        }

        public List<Game> GetByRankScore(int Score_rank)
        {
            List<Game> selectedGames = new List<Game>();
            foreach (Game game in gamesList)
            {
                if (game.scoreRank > Score_rank)
                {
                    selectedGames.Add(game);

                }
            }
            return selectedGames;
        }
    }
    
}
