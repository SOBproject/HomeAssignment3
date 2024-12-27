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
        public IEnumerable <Game> Get()
        {
            return Game.Read();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var game = Game.gamesList.FirstOrDefault(g => g.appid == id);

            return game != null
                ? Ok(game)
                : NotFound($"Game with ID {id} could not be found");
        }

        [HttpGet("minPrice")]
        public List<Game> GetByPrice(int minPrice)
        {
            List<Game> pricedList = new List<Game>();

            foreach (var game in Game.gamesList)
            {
                if (game.price >= minPrice)
                    pricedList.Add(game);
            }

            return pricedList;
        }

        [HttpGet("scoreRank/{scoreRank}")]
        public List<Game> GetByScoreRank(int scoreRank)
        {
            List<Game> rankedList = new List<Game>();

            foreach (var game in Game.gamesList)
            {
                if (game.scoreRank >= scoreRank)
                    rankedList.Add(game);
            }

            return rankedList;
        }



        // POST api/<GamesController>
        [HttpPost]
      public IActionResult Post([FromBody] Game game)
        {
            bool x = Game.Insert(game);
            if (x)
            {
                return Ok(new
                {
                    message = "Game added successfully",
                    gameCount = Game.gamesList.Count
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Use the route parameter directly instead of from body
            bool x = Game.DeleteById(id);
            if (x)
            {
                return Ok(new
                {
                    message = "Game deleted successfully",
                    gameCount = Game.gamesList.Count
                });
            }
            return BadRequest("Failed deleting the game");
        }
    }
}
