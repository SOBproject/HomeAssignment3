using FakeSteam.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<AppUser> Get()
        {
            return AppUser.Read();
        }

        //GET api/<UsersController>/5
        [HttpPost("Login/{Email}/{Password}")]
        public IActionResult Login(string Email,string Password)
        {
            AppUser user = new AppUser();
            if (user.Login(Email, Password))
            {
                return Ok(new { message = "Login successful!", userId = user.GetUserID(Email)});
            }

            return Unauthorized(new { message = "Invalid email or password." });
        }


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
