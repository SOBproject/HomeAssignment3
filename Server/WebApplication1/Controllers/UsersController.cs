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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = AppUser.Read().FirstOrDefault(u => u.Id == id);

            return user != null
                ? Ok(user)
                : NotFound($"User with ID {id} could not be found");
        }

        //GET api/<UsersController>/5
        [HttpPost("Login/{Email}/{Password}")]
        public IActionResult Login(string Email,string Password)
        {
            AppUser user = new AppUser();
            if (user.Login(Email, Password))
            {
                return Ok(new { message = "Login successful!", userId = user.GetUserID(Email), userName = user.GetUserName(Email)});
            }

            return Unauthorized(new { message = "Invalid email or password." });
        }


        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] AppUser newUser)
        {
            try
            {
                int result = newUser.Insert();

                if (result > 0)
                {
                    return Ok(new
                    {
                        message = "User registered successfully!",
                        userId = newUser.GetUserID(newUser.Email),
                        name = newUser.GetUserName(newUser.Email)
                    });
                }
                else
                {
                    return BadRequest(new { message = "Failed to register user." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred during registration.",
                    details = ex.Message
                });
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] AppUser user)
        {
            user.Id = id;
            user.Update();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
