using API.SSE;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PresiController : ControllerBase {

        private PresiService _PresiSrv;
        private readonly ServerSentEventsService _SseService;
        private readonly ServerSentEventsManager _sseTeamServ;

        public PresiController (PresiService presiSrv, ServerSentEventsService sseService, ServerSentEventsManager sseTeamServ) {
            _PresiSrv = presiSrv;
            _SseService = sseService;
            _sseTeamServ = sseTeamServ;
        }

        [Authorize]
        [HttpPost("createTable")]
        public IActionResult CreateTable() {

            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if( identity == null ) return BadRequest("no token ?");       

            try {
                string psd = identity.FindFirst("Pseudo").Value;
                int USerId = int.Parse(identity.FindFirst("UserId").Value);
                _PresiSrv.CreateTable(USerId, psd);
                _SseService.SendObjectAsync( new UserLoginModel() { pseudo = "OKAY" }  );
                return Ok();
            } catch (Exception ex) {//TODO redo catch
                return BadRequest(ex);
            }
        }

    }
}
