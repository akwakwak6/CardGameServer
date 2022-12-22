

using API.SSE;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardGameServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private UserService _UsrServ;

        public UserController(UserService usrServ) {
            _UsrServ = usrServ;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel u ) {

            try {
                return Ok(_UsrServ.Login(u));
            } catch(Exception ex) {
                return BadRequest("Login error");
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel u) {

            try {
                return Ok(_UsrServ.Register(u));
            } catch (Exception ex) {
                return BadRequest("Register error");
            }
        }
    }
}
