using FakeSteam.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        //[HttpGet]
        //public IEnumerable<AppUser> Get()
        //{
  
        //}

        // GET api/<UsersController>/5
        //[HttpPost("login")]
        //public IActionResult Login([FromBody] AppUser loginUser)
        //{
        //    // Validate that both Email and Password are provided
        //    if (string.IsNullOrWhiteSpace(loginUser.Email) || string.IsNullOrWhiteSpace(loginUser.Password))
        //    {
        //        return BadRequest(new { message = "Email and password are required." });
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        // Remove ModelState errors for fields not required for login
        //        ModelState.Remove("Name");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Find user by Email and Password
        //    var user = AppUser.UsersList.FirstOrDefault(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

        //    if (user != null)
        //    {
        //        return Ok(new { message = "Login successful!", userId = user.Id, name = user.Name });
        //    }

        //    return Unauthorized(new { message = "Invalid email or password." });
        //}


        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] AppUser newUser)
        {
           newUser.Insert();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
