﻿

using API.Infrastructure;
using API.Mappers;
using API.Models;
using API.SSE;
using BLL.Models;
using BLL.Services;
using Entities;
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
                return Ok(getApiUser(_UsrServ.Login(u)));
            } catch(Exception ex) {//TODO redo catch
                return BadRequest(ex);
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel u) {

            try {
                return Ok(getApiUser(_UsrServ.Register(u)));
            } catch (Exception ex) {//TODO redo catch
                return BadRequest(ex);
            }
        }
    }
}
