

using API.SSE;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardGameServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel u ) {

            //TODO guest ;-)
            if (u.pseudo != "a" || u.Pwd != "a")
                return BadRequest("Login error");

            UserConnectedModel uc = new UserConnectedModel{
                Id = 1,
                Pseudo = u.pseudo,
                Token = "ABCDEF" 
            };

            

            return Ok(uc);
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel u) {

            //TODO guest ;-)
            UserConnectedModel uc = new UserConnectedModel {
                Id = 1,
                Pseudo = u.pseudo,
                Token = "ABCDEF"
            };

            return Ok(uc);
        }
    }
}
