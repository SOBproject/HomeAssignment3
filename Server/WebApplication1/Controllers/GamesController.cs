using System;
using FakeSteam.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return Game.Read();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var game = Game.Read().FirstOrDefault(g => g.appid == id);

            return game != null
                ? Ok(game)
                : NotFound($"Game with ID {id} could not be found");
        }


        [HttpGet("userID/{userID}")]
        public List<Game> GetUserGames(int userID)
        {
            return Game.Read(userID);
        }

        [HttpGet("filterPrice/{userID}/{minPrice}")]
        public List<Game> GetPricedList(int userID,float minPrice)
        {
            return Game.GetPricedList(userID,minPrice);
        }

        [HttpGet("filterRank/{userID}/{minRank}")]
        public List<Game> GetRankedList(int userID,int minRank)
        {
            return Game.GetRankedList(userID, minRank);
        }

        [HttpGet("filterBoth/{userID}/{minPrice}/{minRank}")]
        public List<Game> GetPricedAndRankedList(int userID,float minPrice,int minRank)
        {
            return Game.GetPricedAndRankedList(userID,minPrice,minRank);
        }





        // POST api/<GamesController>
        [HttpPost("AddGameToMyList")]
      public IActionResult AddGameToMyList(int userID, int appID)
        {
            int numEffected = Game.Insert(userID,appID);
            if (numEffected==2)
            {
                return Ok(new
                {
                    message = "Game added successfully",
                });
            }
            if (numEffected == -1)
            {
                return BadRequest(new
                {
                    message = "Game is already in your list!",
                });
            }
            return BadRequest("Adding the game has failed. Please try again.");
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Game game)
        {
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("RemoveGameFromList")]
        public IActionResult DeleteGameFromList(int userID, int appID)
        {
            int numEffected = Game.Delete(userID, appID);
            if (numEffected == 2)
            {
                return Ok(new
                {
                    message = "Game removed successfully",
                });
            }
            return BadRequest("Removing the game has failed. Please try again.");
        }
    }
}
