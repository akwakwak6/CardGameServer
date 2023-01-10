

using API.Infrastructure;
using API.Mappers;
using API.Models;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardGameServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        private UserService _UsrServ;
        private TokenManager _TknServ;

        public UserController(UserService usrServ, TokenManager tknServ) {
            _UsrServ = usrServ;
            _TknServ = tknServ;
        }

        private ConnectedUserModel getApiUser(UserConnectedDalModel u) {
            return u.ToAPIuser(_TknServ.GenerateToken(u));
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel u ) {

            try {
                UserConnectedDalModel usr = _UsrServ.Login(u);
                return Ok(getApiUser(usr));
            } catch(Exception ex) {
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel u) {

            try {
                return Ok(getApiUser(_UsrServ.Register(u)));
            } catch (Exception ex) {
                return BadRequest();
            }
        }
    }
}
